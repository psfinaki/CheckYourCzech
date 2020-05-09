module WikiStringTests

open WikiString
open Xunit

[<Theory>]
[<InlineData "">]
[<InlineData "—">]
[<InlineData "-">]
let ``Ignores blank field`` s = 
    s
    |> getForms
    |> seqEquals []

[<Fact>]
let ``Ignores unused form``() = 
    "znalečná*"
    |> getForms
    |> seqEquals []

[<Fact>]
let ``Gets simple form``() = 
    "poslouchat"
    |> getForm
    |> equals "poslouchat"

[<Fact>]
let ``Gets simple form - with space``() = 
    "divím se"
    |> getForm
    |> equals "divím se"

[<Fact>]
let ``Gets simple form - with dash``() = 
    "Tchaj-wan"
    |> getForm
    |> equals "Tchaj-wan"

[<Theory>]
[<InlineData " poslouchat">]
[<InlineData "poslouchat ">]
[<InlineData " poslouchat ">]
let ``Gets form when spaced`` s = 
    s
    |> getForm
    |> equals "poslouchat"

[<Fact>]
let ``Gets form with reference - only number``() =
    "rci[1]"
    |> getForm
    |> equals "rci"

[<Fact>]
let ``Gets form with reference - number and letter 'p'``() =
    "budiž[p 3]"
    |> getForm
    |> equals "budiž"

[<Fact>]
let ``Gets form with appropriate label - bookish``() =
    "(knižně) prohrej"
    |> getForm
    |> equals "prohrej"

[<Fact>]
let ``Gets form with appropriate label - rarer - 1``() =
    "(řidč.) pohaněj"
    |> getForm
    |> equals "pohaněj"

[<Fact>]
let ``Gets form with appropriate label - rarer - 2``() =
    "(zřídka) plazové"
    |> getForm
    |> equals "plazové"

[<Theory>]
[<InlineData "(zastarale) pec">]
[<InlineData "(archaicky) sloul">]
[<InlineData "(hovorově) slz">]
[<InlineData "(zastarale) pec">]
[<InlineData "(v obecném jazyce) zab">]
[<InlineData "(básnicky) Zéva">]
[<InlineData "(nářečně) noce">]
let ``Ignores form with inappropriate label`` s =
    s
    |> getForms
    |> seqEquals []

[<Fact>]
let ``Gets form when second form is after colon``() =
    "abdikuji (hovorově: abdikuju)"
    |> getForm
    |> equals "abdikuji" 

[<Fact>]
let ``Gets single form when split``() =
    "produkuji / (hovorově) produkuju"
    |> getForm
    |> equals "produkuji"

[<Fact>]
let ``Gets multiple forms when split``() = 
    "Edáčci/Edáčkové"
    |> getForms
    |> seqEquals ["Edáčci"; "Edáčkové"]
