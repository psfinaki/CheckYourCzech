module Storage

open System
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json

type CloudTable with
    member this.ExecuteQuery (query : TableQuery<'T>) =
        async {
            let! segment = 
                this.ExecuteQuerySegmentedAsync(query, null) 
                |> Async.AwaitTask

            return segment.Results        
        }

    member this.Execute =
        this.ExecuteAsync 
        >> Async.AwaitTask 
        >> Async.Ignore

    member this.CreateIfNotExists() =
        this.CreateIfNotExistsAsync() 
        |> Async.AwaitTask 
        |> Async.Ignore 

type QueryCondition =
    | Is
    | IsNot
    | Bool
    | Int
    | String

type SerializeObject() = inherit Attribute()
type SerializeString() = inherit Attribute()
type SerializeOption() = inherit Attribute()

let getTable name =
    async {
        let connectionString = Environment.GetEnvironmentVariable "STORAGE_CONNECTIONSTRING"
        let account = CloudStorageAccount.Parse connectionString
        let client = account.CreateCloudTableClient() 
        let table = client.GetTableReference name
        do! table.CreateIfNotExists()
        return table
    }

let buildFilter (property, condition, value : obj) =
    let createStringFilter = TableQuery.GenerateFilterCondition
    let createBoolFilter   = TableQuery.GenerateFilterConditionForBool

    match condition with
    | Is     -> createStringFilter (property, QueryComparisons.Equal,    JsonConvert.SerializeObject value)
    | IsNot  -> createStringFilter (property, QueryComparisons.NotEqual, JsonConvert.SerializeObject value)
    | Bool   -> createBoolFilter   (property, QueryComparisons.Equal,    Convert.ToBoolean value)
    | Int    -> createStringFilter (property, QueryComparisons.Equal,    value.ToString())
    | String -> createStringFilter (property, QueryComparisons.Equal,    value.ToString())

let combineFilters f1 f2 = TableQuery.CombineFilters(f1, TableOperators.And, f2)

let buildQuery<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> filters = 
    match filters with
    | sequence when Seq.isEmpty sequence -> 
        TableQuery<'T>()
    | _ ->
        filters
        |> Seq.map buildFilter
        |> Seq.reduce combineFilters
        |> TableQuery<'T>().Where

let tryGetRandomWithFilters<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> tableName azureFilters postFilters =
    async {
        let! table = getTable tableName

        let! queryResults = 
            azureFilters
            |> buildQuery<'T>
            |> table.ExecuteQuery
        
        return queryResults |> Seq.tryRandomIf postFilters    
    }

let tryGetRandom<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> tableName azureFilters = 
    tryGetRandomWithFilters<'T> tableName azureFilters []

let upsert tableName entity =
    async {
        let! table = getTable tableName
        let operation = TableOperation.InsertOrReplace entity
        return! table.Execute operation
    }
