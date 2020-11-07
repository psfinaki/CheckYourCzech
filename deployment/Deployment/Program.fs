module Deployment.Program

open Farmer

[<EntryPoint>]
let main args =
    args
    |> Seq.exactlyOne
    |> Infrastructure.createDeployment
    |> Writer.quickWrite "template"

    0

