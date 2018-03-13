module ServerCode.Storage.AzureTable

open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table
open ServerCode.Domain
open System
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

type AzureConnection =
    | AzureConnection of string

let getBooksTable (AzureConnection connectionString) = task {
    let client = (CloudStorageAccount.Parse connectionString).CreateCloudTableClient()
    let table = client.GetTableReference "book"

    // Azure will temporarily lock table names after deleting and can take some time before the table name is made available again.
    let rec createTableSafe() = task {
        try
        let! _ = table.CreateIfNotExistsAsync()
        ()
        with _ ->
            do! Task.Delay 5000
            return! createTableSafe() }

    do! createTableSafe()
    return table }

/// Load from the database
let getWishListFromDB connectionString userName = task {
    let! results = task {
        let! table = getBooksTable connectionString
        let query = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userName)
        return! table.ExecuteQuerySegmentedAsync(TableQuery(FilterString = query), null)  }
    return
        { UserName = userName
          Books =
            [ for result in results ->
                { Title = result.Properties.["Title"].StringValue
                  Authors = string result.Properties.["Authors"].StringValue
                  Link = string result.Properties.["Link"].StringValue } ] } }

module private StateManagement =
    let getStateBlob (AzureConnection connectionString) name = task {
        let client = (CloudStorageAccount.Parse connectionString).CreateCloudBlobClient()
        let state = client.GetContainerReference "state"
        let! _ = state.CreateIfNotExistsAsync()
        return state.GetBlockBlobReference name }

    let resetTimeBlob connectionString = getStateBlob connectionString "resetTime"

    let storeResetTime connectionString = task {
        let! blob = resetTimeBlob connectionString
        return! blob.UploadTextAsync "" }

let getLastResetTime connectionString = task {
    let! blob = StateManagement.resetTimeBlob connectionString
    do! blob.FetchAttributesAsync()
    return blob.Properties.LastModified |> Option.ofNullable |> Option.map (fun d -> d.UtcDateTime)
}

/// Clears all Wishlists and records the time that it occurred at.
let clearWishLists connectionString = task {
    let! table = getBooksTable connectionString
    let! _ = table.DeleteIfExistsAsync()

    do! StateManagement.storeResetTime connectionString }