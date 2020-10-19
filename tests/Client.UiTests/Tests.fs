module Client.UiTests.Tests

open canopy.classic
open canopy.types
open System
open Xunit

type Fixture() =
    do
        //start Chrome // Use this if you want to see your tests in the browser
        start ChromeHeadless
        resize (1280, 960)

let serverUrl = Environment.GetEnvironmentVariable("SERVER_URL")

let openApp() =
    url serverUrl
    waitForElement("#elmish-app")

type Tests() = 
    interface IClassFixture<Fixture>

    [<Fact>]
    member _.``Soundcheck - server is online``() =
        openApp()
        
    [<Fact>]
    member _.``Validate word is loaded``() =
        openApp()
        click (first ".link-block")
        
        let taskText = (element ".task-label").GetAttribute("innerText")
        
        Assert.NotNull taskText
