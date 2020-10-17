module Client.UiTests.Tests

open canopy.classic
open Expecto
open System
open System.IO

let serverUrl = Environment.GetEnvironmentVariable("SERVER_URL")

type TypeInThisAssembly() =
    class end

let screenshotFolder = FileInfo(System.Reflection.Assembly.GetAssembly(typeof<TypeInThisAssembly>).Location).Directory.FullName

let testCase name f =
    testCase name (fun x ->
        try
            f x
        with
        | exn ->
            screenshot screenshotFolder (name  + "-" + System.DateTime.Now.ToString("MMM-d_HH-mm-ss")) |> ignore
            raise exn
    )

let startApp () =
    url serverUrl
    waitForElement("#elmish-app")

let tests =
    testList "client tests" [
        testCase "sound check - server is online" (fun () ->
            startApp ()
        )

        testCase "validate word is loaded" (fun () ->
            startApp ()

            click (first ".link-block")

            let taskText = (element ".task-label").GetAttribute("innerText")
            Expect.isNotNull taskText "should contain text"
        )
    ]
