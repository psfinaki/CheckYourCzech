module Server.Tasks.Utils

let getAzureFilter columnName queryCondition = function
    | Ok parameterValue -> Some (columnName, queryCondition, box parameterValue)
    | Error _ -> None

let getPostFilter filterCondition = function
    | Ok parameterValue -> Some (filterCondition parameterValue)
    | Error _ -> None

[<CLIMutable>]
type Task = {
    Word: string
    Answers: string[]
}
