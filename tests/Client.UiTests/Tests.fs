module Client.UiTests.Tests

open canopy.classic
open System
open Xunit

open Fixture
open WebElementHelper

let private serverUrl = Environment.GetEnvironmentVariable "SERVER_URL"

let private app = "#elmish-app"
let private buttonsContainer = ".task-buttons-container"
let private taskLabel = ".task-label"
let private answerBox = ".input"
let private icon = ".icon.is-right.is-small"

let private getButtons() = element buttonsContainer |> findByTag "button"
let private getShowButton = getButtons >> Seq.head
let private getCheckButton = getButtons >> Seq.last
let private getNextButton = getButtons >> Seq.last

let private getTask() = element taskLabel |> getAttribute "innerText"
let private getAnswer() = element answerBox |> getAttribute "value"
let private writeAnswer text = element answerBox << text
let private getAnswerIcon() = element icon |> findByTag "i" |> Seq.exactlyOne

let private openApp() =
    url serverUrl
    waitForElement app

let private openExercise() =
    openApp()
    click (first ".link-block")
    waitFor(fun () -> getTask() <> "")

type Tests() = 
    interface IClassFixture<Fixture>

    [<Fact>]
    member _.``Soundcheck - Server is online``() =
        openApp()
        
    [<Fact>]
    member _.``Exercise - When opened - Task is loaded``() =
        openExercise()

        let task = getTask()
        
        Assert.NotEmpty task
    
    [<Fact>]
    member _.``Exercise - When no answer yet - Check button is disabled``() =
        openExercise()

        let isEnabled = getCheckButton() |> isEnabled

        Assert.False isEnabled

    [<Fact>]
    member _.``Exercise - When answer entered - Check button is enabled``() =
        openExercise()

        writeAnswer "test"

        let isEnabled = getCheckButton() |> isEnabled

        Assert.True isEnabled

    [<Fact>]
    member _.``Exercise - When click Show - Fills answer``() =
        openExercise()

        click (getShowButton())

        let answer = getAnswer()

        Assert.NotEmpty answer

    [<Fact>]
    member _.``Exercise - When click Next - Resets answer``() =
        openExercise()

        click (getShowButton())
        click (getNextButton())

        let answer = getAnswer()

        Assert.Empty answer

    [<Fact>]
    member _.``Exercise - When answer is not checked - Question mark``() =
        openExercise()

        writeAnswer "test"

        let iconClass = getAnswerIcon() |> getClass

        Assert.Contains("question", iconClass)
    
    [<Fact>]
    member _.``Exercise - When answer is wrong - Cross``() =
        openExercise()

        writeAnswer "test"
        click (getCheckButton())

        let iconClass = getAnswerIcon() |> getClass

        Assert.Contains("times", iconClass)
    
    [<Fact>]
    member _.``Exercise - When answer is correct - Tick``() =
        openExercise()

        click (getShowButton())
        let correctAnswer = getAnswer()

        writeAnswer "test" // to make "Check" button available again
        writeAnswer correctAnswer
        click (getCheckButton())

        let iconClass = getAnswerIcon() |> getClass

        Assert.Contains("check", iconClass)
