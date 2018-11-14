module Seq 

open System

let behead seq = 
    seq |> Seq.head, 
    seq |> Seq.tail

// https://stackoverflow.com/a/6737659/3232646
let splitBy func input =
    let i = ref 0
    let projection x = 
        if func x then incr i
        !i

    input
    |> Seq.groupBy projection
    |> Seq.map snd

let random (seq: seq<'T>) = 
    seq
    |> Seq.sortBy (fun _ -> Guid.NewGuid())
    |> Seq.take 1
    |> Seq.exactlyOne

let tryRandom = function
    | seq when seq |> Seq.isEmpty ->
        None
    | seq ->
        seq
        |> random
        |> Some

let trySingle = function
    | seq when seq |> Seq.isEmpty ->
        None
    | seq ->
        seq
        |> Seq.exactlyOne
        |> Some
