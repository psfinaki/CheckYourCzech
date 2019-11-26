module Common.Utils

open Microsoft.FSharp.Collections
open Microsoft.FSharp.Reflection
open Conjugation

let fromString<'a> (s:string) =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
    |[|case|] -> Some(FSharpValue.MakeUnion(case,[||]) :?> 'a)
    |_ -> None

let getRandomUnion<'a> =
    typeof<'a> |>
    (FSharpType.GetUnionCases 
    >> Seq.random 
    >> (fun case -> case.Name)
    >> fromString<'a>)

let getRandomPronoun () = 
     getRandomUnion<Pronoun>