[<AutoOpen>]
module Core.Tests.Helper

open Xunit

let equals (expected: 'T) (actual: 'T) = Assert.Equal<'T>(expected, actual)
