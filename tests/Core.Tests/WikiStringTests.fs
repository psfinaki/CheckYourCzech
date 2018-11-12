module WikiStringTests

open WikiString
open Xunit

let equals expected actual = Assert.Equal(expected, actual)
let equalsMany (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let ``Gets forms - one form``() = 
    "pastila"
    |> getForms
    |> equalsMany [|"pastila"|]

[<Theory>]
[<InlineData("Edáčci/Edáčkové")>]
[<InlineData("Edáčci,Edáčkové")>]
let ``Gets forms - multiple forms`` s = 
    s
    |> getForms
    |> equalsMany [|"Edáčci"; "Edáčkové"|]

[<Theory>]
[<InlineData("")>]
[<InlineData("—")>]
let ``Detects blank string`` s = 
    s
    |> isBlank
    |> Assert.True

[<Fact>]
let ``Detects word``() = 
    "Brussels"
    |> isBlank
    |> Assert.False

[<Fact>]
let ``Detects archaic form``() = 
    "(zastarale) pec"
    |> isArchaic
    |> Assert.True
    
[<Fact>]
let ``Detects modern form``() = 
    "peč"
    |> isArchaic
    |> Assert.False

[<Fact>]
let ``Detects colloquial form``() = 
    "(hovorově) slz"
    |> isColloquial
    |> Assert.True
    
[<Fact>]
let ``Detects literary form``() = 
    "slzej"
    |> isColloquial
    |> Assert.False

[<Fact>]
let ``Removes allowed labels - rearer``() =
    "(řidč.) pohaněj"
    |> removeLabels
    |> equals "pohaněj"
    
[<Fact>]
let ``Removes allowed labels - bookish``() =
    "(knižně) prohrej"
    |> removeLabels
    |> equals "prohrej"

[<Theory>]
[<InlineData("rci[1]")>]
[<InlineData("rci[2]")>]
[<InlineData("rci[12]")>]
[<InlineData("rci[1][2]")>]
let ``Removes references`` s =
    s
    |> removeReferences
    |> equals "rci"
