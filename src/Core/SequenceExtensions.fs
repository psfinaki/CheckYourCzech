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

let any (seq: seq<'T>) =
    seq
    |> Seq.isEmpty
    |> not

let hasOneElement (seq: seq<'T>) = 
    seq
    |> Seq.length 
    |> (=) 1
        
let tryRandom = function
    | seq when seq |> Seq.isEmpty ->
        None
    | seq ->
        seq
        |> random
        |> Some

let tryRandomIf filters =
    let combinedFilter x = 
        filters 
        |> Seq.map (fun f -> f x) 
        |> Seq.fold (&&) true
    
    Seq.where combinedFilter 
    >> tryRandom

let tryExactlyOne = function
    | seq when not (seq |> Seq.length = 1) ->
        None
    | seq ->
        seq |> Seq.exactlyOne |> Some

let lastButOne seq =
    seq
    |> Seq.rev
    |> Seq.skip 1
    |> Seq.head

let isMultiple seq =
    seq 
    |> Seq.length 
    |> (<) 1
