module Common.Utils

open Microsoft.FSharp.Collections
open Microsoft.FSharp.Reflection

let private makeUnionCase<'a> case = FSharpValue.MakeUnion(case,[||]) :?> 'a

let getRandomUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases 
    |> Seq.random
    |> makeUnionCase<'a>

let getAllUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases
    |> Seq.map makeUnionCase<'a>
