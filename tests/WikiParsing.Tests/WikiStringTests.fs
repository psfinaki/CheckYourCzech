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
[<InlineData("-")>]
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
let ``Detects official form``() = 
    "poslouchat"
    |> isOfficial
    |> Assert.True

[<Theory>]
[<InlineData "(zastarale) pec">]
[<InlineData "(archaicky) sloul">]
let ``Detects unofficial form - archaic`` form = 
    form
    |> isOfficial
    |> Assert.False
    
[<Fact>]
let ``Detects unofficial form - colloquial``() = 
    "(hovorově) slz"
    |> isOfficial
    |> Assert.False

[<Fact>]
let ``Detects unofficial form - informal``() = 
    "(v obecném jazyce) zab"
    |> isOfficial
    |> Assert.False

[<Fact>]
let ``Detects unofficial form - poetic``() = 
    "(básnicky) Zéva"
    |> isOfficial
    |> Assert.False

[<Fact>]
let ``Detects unofficial form - not used``() = 
    "znalečná*"
    |> isOfficial
    |> Assert.False

[<Fact>]
let ``Detects unofficial form - dialect``() = 
    "(nářečně) noce"
    |> isOfficial
    |> Assert.False

[<Theory>]
[<InlineData ("(řidč.) pohaněj", "pohaněj")>]
[<InlineData ("(zřídka) plazové", "plazové")>]
let ``Removes allowed labels - rarer`` labeledForm form =
    labeledForm
    |> removeLabels
    |> equals form
    
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
let ``Removes references - numbers`` s =
    s
    |> removeReferences
    |> equals "rci"

[<Fact>]
let ``Removes references - letter 'p'``() =
    "budiž[p 3]"
    |> removeReferences
    |> equals "budiž"
