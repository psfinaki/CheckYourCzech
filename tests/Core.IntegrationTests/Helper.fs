[<AutoOpen>]
module Helper

open Xunit

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)
