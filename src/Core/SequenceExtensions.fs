module Seq 

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
