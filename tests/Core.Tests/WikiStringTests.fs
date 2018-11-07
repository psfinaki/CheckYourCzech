module WikiStringTests

open WikiString
open Xunit

let equals (x: string[]) (y: string[]) = Assert.Equal<string []>(x, y)

[<Fact>]
let ``Gets forms - one form``() = 
    "pastila"
    |> getForms
    |> equals [|"pastila"|]

[<Fact>]
let ``Gets forms - multiple forms``() = 
    "Edáčci/Edáčkové"
    |> getForms
    |> equals [|"Edáčci"; "Edáčkové"|]

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
