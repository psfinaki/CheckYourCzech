module Client.UiTests.Fixture

open canopy.classic
open canopy.types
open System

type Fixture() =
    do
        //start Chrome // Use this if you want to see your tests in the browser
        start ChromeHeadless
        resize (1280, 960)

    interface IDisposable with 
        member _.Dispose() =
            quit()
