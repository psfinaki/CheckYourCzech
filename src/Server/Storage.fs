module Storage

open System
open System.Collections.Generic
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table

module List = 
    let random (list : List<'T>) = 
        let maxValue = list.Count
        let randomIndex = Random().Next maxValue
        list.[randomIndex]

type CloudTable with
    member this.ExecuteQuery (query : TableQuery<'T>) =
        this.ExecuteQuerySegmentedAsync(query, null) 
        |> Async.AwaitTask 
        |> Async.RunSynchronously 
        |> fun segment -> segment.Results

let getTable name =
    let connectionString = Environment.GetEnvironmentVariable "STORAGE_CONNECTIONSTRING"
    let account = CloudStorageAccount.Parse connectionString
    let client = account.CreateCloudTableClient()
    let table = client.GetTableReference name
    table

let buildFilter (x, y, shouldBeEqual) = 
    match shouldBeEqual with
    | true  -> TableQuery.GenerateFilterCondition(x, QueryComparisons.Equal, y)
    | false -> TableQuery.GenerateFilterCondition(x, QueryComparisons.NotEqual, y)

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

let getRandom<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> tableName filters =
    let table = getTable tableName

    filters
    |> buildQuery<'T>
    |> table.ExecuteQuery
    |> List.random
    
let getSingle<'T when 'T : (new : unit -> 'T) and 'T :> ITableEntity> tableName filters = 
    let table = getTable tableName

    filters
    |> buildQuery<'T>
    |> table.ExecuteQuery
    |> Seq.exactlyOne
