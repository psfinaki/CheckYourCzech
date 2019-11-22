module Common.Utils

open System
open GrammarCategories
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Reflection

let toString (x:'a) = 
    match FSharpValue.GetUnionFields(x, typeof<'a>) with
    | case, _ -> case.Name

let fromString<'a> (s:string) =
    match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name = s) with
    |[|case|] -> Some(FSharpValue.MakeUnion(case,[||]) :?> 'a)
    |_ -> None

let shuffleR (r : Random) = Seq.sortBy (fun _ -> r.Next())
let getRandomItem (xs: seq<'a>) = xs |> shuffleR (Random ()) |> Seq.head
let getRandomUnion<'a> =
    typeof<'a> |>
    (FSharpType.GetUnionCases 
    >> getRandomItem 
    >> (fun case -> case.Name)
    >> fromString<'a>)

let getRandomPerson () = 
     getRandomUnion<Person>

let getRandomNumber () = 
     getRandomUnion<Number>