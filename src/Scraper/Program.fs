[<EntryPoint>]
let main argv =
    let iterator _ = WordGenerator.getRandomWord()

    let recordWithLog word = 
        try
            Logger.logMessage ("Processing word: " + word)
            Word.record word
            Logger.logMessage ("Processed word: " + word)
        with e ->
            Logger.logError(e, ("word", word))
            reraise()

    iterator
    |> Seq.initInfinite
    |> Seq.iter recordWithLog

    0
