module Scraper.App

open System.IO
open System.Net.Http

open Logging
open Common
open Word

let getRandomWord() = File.ReadLines "dictionary.txt"

let run log =
    let client = new HttpClient()
    let recordWithLog word = 
        try
            log (Trace ("Processing word: " + word))
            record client word
            log (Trace ("Processed word: " + word))
        with e ->
            log (Exception (e, ("word", word)))
            reraise()

    getRandomWord()
    |> Seq.randomize
    |> Seq.iter recordWithLog

