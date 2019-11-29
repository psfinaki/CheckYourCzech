module Common.Utils

open Microsoft.FSharp.Collections
open Microsoft.FSharp.Reflection

let getRandomUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases 
    |> Seq.random
    |> fun case -> FSharpValue.MakeUnion(case,[||]) :?> 'a