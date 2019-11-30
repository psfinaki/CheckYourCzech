module Common.Utils

open Microsoft.FSharp.Collections
open Microsoft.FSharp.Reflection

let private getUnionCase<'a> case = FSharpValue.MakeUnion(case,[||]) :?> 'a

let getRandomUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases 
    |> Seq.random
    |> getUnionCase<'a>

let getAllUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases
    |> Seq.map getUnionCase<'a>
