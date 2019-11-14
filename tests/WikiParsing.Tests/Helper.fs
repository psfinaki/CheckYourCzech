[<AutoOpen>]
module Helper

open Xunit

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)
let seqEquals (expected: 'T list) (actual: seq<'T>) = Assert.Equal<'T>(expected, Seq.toList actual)
