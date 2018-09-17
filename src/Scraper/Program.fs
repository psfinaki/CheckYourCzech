[<EntryPoint>]
let main argv =
    let iterator _ = WordGenerator.getRandomWord()

    iterator
    |> Seq.initInfinite
    |> Seq.iter Word.record

    0
