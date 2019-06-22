module Storage

open System
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json

type CloudTable with
    member this.ExecuteQuery (query : TableQuery<'T>) =
        this.ExecuteQuerySegmentedAsync(query, null) 
        |> Async.AwaitTask 
        |> Async.RunSynchronously 
        |> fun segment -> segment.Results

    member this.Execute operation =
        this.ExecuteAsync operation 
        |> Async.AwaitTask 
        |> Async.RunSynchronously 
        |> ignore

    member this.CreateIfNotExists() =
        this.CreateIfNotExistsAsync() 
        |> Async.AwaitTask 
        |> Async.RunSynchronously 
        |> ignore

type QueryCondition =
    | Is
    | IsNot
    | Bool
    | Int
    | String

let mapSafeIntOption    (mapping: 'a -> int option)    = Option.ofObj >> Option.map mapping >> Option.flatten >> Option.map string >> Option.defaultValue ""
let mapSafeStringOption (mapping: 'a -> string option) = Option.ofObj >> Option.map mapping >> Option.flatten >> Option.map string >> Option.defaultValue ""
let mapSafeObject        mapping                       = Option.ofObj >> Option.map mapping >> Option.map string >> Option.defaultValue ""
let mapSafeString mapping = Option.ofObj >> Option.map mapping >> Option.toObj >> JsonConvert.SerializeObject
let mapSafeBool mapping   = Option.ofObj >> Option.map mapping >> Option.defaultValue false
let mapSafeInt mapping    = Option.ofObj >> Option.map mapping >> Option.defaultValue 0

let getAs<'T> = JsonConvert.DeserializeObject<'T>

let getTable name =
    let connectionString = Environment.GetEnvironmentVariable "STORAGE_CONNECTIONSTRING"
    let account = CloudStorageAccount.Parse connectionString
    let client = account.CreateCloudTableClient() 
    let table = client.GetTableReference name
    table.CreateIfNotExists()
    table

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

let tryGetRandom<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> tableName azureFilters postFilters =
    let table = getTable tableName

    azureFilters
    |> buildQuery<'T>
    |> table.ExecuteQuery
    |> Seq.tryRandomIf postFilters

let getSingle<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> tableName filters = 
    let table = getTable tableName

    filters
    |> buildQuery<'T>
    |> table.ExecuteQuery
    |> Seq.exactlyOne

let upsert tableName entity =
    let table = getTable tableName
    let operation = TableOperation.InsertOrReplace entity
    table.Execute operation
