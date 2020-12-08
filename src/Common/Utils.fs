module Common.Utils

open Microsoft.FSharp.Collections
open Microsoft.FSharp.Reflection

// functions here have to be inline to be used in Fable:
// https://fable.io/docs/dotnet/compatibility.html#Reflection-and-Generics

let inline private makeUnionCase<'a> case = FSharpValue.MakeUnion(case,[||]) :?> 'a

let inline getRandomUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases 
    |> Seq.random
    |> makeUnionCase<'a>

let inline getAllUnion<'a> =
    typeof<'a> 
    |> FSharpType.GetUnionCases
    |> Seq.map makeUnionCase<'a>

let inline parseUnionCase<'a> name =
    typeof<'a>
    |> FSharpType.GetUnionCases
    |> Seq.filter (fun case -> case.Name = name)
    |> Seq.exactlyOne
    |> makeUnionCase<'a>
