module Task

open Elmish
open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.Import.React
open Fable.Core
open Fable.Core.JsInterop
open Fable.FontAwesome
open Fable.FontAwesome.Free

open Fulma
open System
open Markup

[<Literal>] 
let DefaultWord = ""
let DefaultAnswers = [||]
let SpecialSymbols = ['á'; 'č'; 'ď'; 'é'; 'ě'; 'í'; 'ň'; 'ó'; 'ř'; 'š'; 'ť'; 'ú'; 'ů'; 'ý'; 'ž']

type Task = { 
    Word : string 
    Answers : string[]
}

type InputState = 
    | NoInput
    | Unknown
    | Wrong
    | Right
    | Shown

type State = 
    | Fetching
    | InputProvided of Task * InputState

type Model = {
    TaskName: string
    Input: ImprovedInput.Types.Model
    State: State
}

type Msg = 
    | ImprovedInput of ImprovedInput.Types.Msg
    | InputUpdated
    | ShowAnswer
    | CheckAnswer
    | NextTask
    | FetchedTask of Task option
    | FetchError of exn

type LeftButtonState = Show | Next

let log taskName task answer result = 
    let message = sprintf "Task name:%s; Task:%s; Answer:%s; Result:%b" taskName task answer result
    Logger.log message

let loadTaskCmd getTask =
    Cmd.ofPromise getTask [] FetchedTask FetchError

let getStateTask s = 
    match s with 
    | Fetching -> { Word = DefaultWord; Answers = DefaultAnswers}
    | InputProvided (t, _) -> t

let getTaskIcon c = 
    match c with 
    | "task-input-correct"   -> Fa.Solid.CheckCircle
    | "task-input-incorrect" -> Fa.Solid.TimesCircle
    | "task-input-none"      -> Fa.Solid.QuestionCircle
    | _                      -> invalidArg "c" "Only certain type of icons could be mapped"

let checkAnswer model = 
    match model.State with 
        | Fetching -> model
        | InputProvided (t, inpState) -> 
            let checkAnswer' () = 
                let result = t.Answers |> Array.contains model.Input.Value
                log model.TaskName t.Word model.Input.Value result
                let nextState = if result then Right else Wrong
                { model with State = InputProvided (t, nextState) }

            match inpState with
            | Shown -> model
            | _ -> checkAnswer' ()

let init taskName getTask =
    let input = ImprovedInput.State.init()
    { Input = input
      TaskName = taskName
      State = Fetching },
      loadTaskCmd getTask

let update msg model getTask =
    match msg with
    | FetchedTask task -> 
        { model with 
            State = InputProvided ({ 
                        Word = task |> Option.map (fun t -> t.Word) |> Option.defaultValue DefaultWord; 
                        Answers = task |> Option.map (fun t -> t.Answers) |> Option.defaultValue DefaultAnswers
                    }, NoInput)
        }, Cmd.none
    | FetchError _ ->
        model, Cmd.none
    | ImprovedInput msg' ->
        let input, cmd = ImprovedInput.State.update msg' model.Input
        let triggeredCmd = 
            match msg' with
            | ImprovedInput.Types.ChangeInput _ -> Cmd.ofMsg InputUpdated
            | _ -> Cmd.none
        { model with Input = input }, Cmd.batch [ cmd; triggeredCmd ]
    | InputUpdated ->
        let input = model.Input.Value
        let newState = 
            if input <> "" then Unknown
            else NoInput
        { model with State = InputProvided (getStateTask model.State, newState) }, Cmd.none
    | ShowAnswer ->
        let task = getStateTask model.State
        let answer = task.Answers |> Array.tryHead |> Option.defaultValue DefaultWord
        { model with State = InputProvided (task, Shown) }, answer |> (ImprovedInput.Types.SetInput >> ImprovedInput >> Cmd.ofMsg)
    | CheckAnswer -> 
        checkAnswer model, Cmd.none
    | NextTask ->
        { model with State = Fetching }, Cmd.batch[ Cmd.ofMsg (ImprovedInput ImprovedInput.Types.Reset); loadTaskCmd getTask ]

type InputViewState = {
    Word : ReactElement
    InputClass : string
    InputText : string
}

let inputView model dispatch handleKeyDown inputElementId =
    let defaultInputClass = "task-input-none"
    let defaultInputText = ""

    let inputViewState = 
        match model.State with
        | Fetching -> 
            let imageSource = "images/loading.gif"
            let altText = "Loading..."
            let wordDisplay = icon imageSource 25 altText
            { Word = wordDisplay; InputClass = defaultInputClass; InputText = defaultInputText }
        | InputProvided (task, inputState) ->
            let inputClass =  
                match inputState with
                | Right -> "task-input-correct"
                | Wrong -> "task-input-incorrect"
                | _ -> defaultInputClass
            { Word = str task.Word; InputClass = inputClass; InputText = model.Input.Value }
    
    let inputIcon = getTaskIcon inputViewState.InputClass

    let inputProps : ImprovedInput.View.Props = {
        InputId = inputElementId
        InputSize = IsLarge
        OnKeyDownHandler = handleKeyDown
        AutoCapitalize = "none"
        AutoFocus = true
        AutoComplete = "off"
    }
    Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3) ]
        [
            Column.column [ ] [Tag.tag [ Tag.Color IsLight ; Tag.CustomClass "task-label"; ] [inputViewState.Word] ]
            Column.column [ ] [
                div [ClassName ("control has-icons-right " + inputViewState.InputClass)]
                    [
                        ImprovedInput.View.root inputProps model.Input (ImprovedInput >> dispatch)
                        Icon.icon [ Icon.Size IsSmall; Icon.IsRight ]
                            [ Fa.i [ inputIcon ] [] ]
                    ]                     
            ]        
        ]

let symbolButtonsView model dispatch inputElementId = 
    let disabled = 
        match model.State with
        | Fetching -> true
        | _ -> false
    let createSymbolButton s = 
        Button.button [ 
            Button.Color IsLight
            Button.Disabled disabled 
            Button.Props [
                OnClick (fun _ -> (ImprovedInput >> dispatch) <| ImprovedInput.Types.AddSymbol s)
                OnFocus (fun (e) -> Fable.Import.Browser.document.getElementById(inputElementId).focus()) 
            ]
        ] [ str <| string (Char.ToUpper(s)) ]
    Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3) ]
            [
                Column.column [ ] [ ]
                Column.column [ ] [
                    div [ClassName "symbol-buttons"] 
                        (List.map createSymbolButton SpecialSymbols)
                ]        
            ]

type ButtonViewState = {
    NextButtonDisabled : bool
    CheckButtonDisabled : bool
    ShowButtonDisabled : bool
}

let buttonView model dispatch nextButtonDisplayed inputElementId =
    let handleShowAnswerClick _ = dispatch ShowAnswer
    let handleUpdateClick _ = dispatch NextTask
    let handleCheckClick _ = dispatch CheckAnswer

    let buttonViewState =
        match model.State with
        | Fetching -> 
            { NextButtonDisabled = true; CheckButtonDisabled = true; ShowButtonDisabled = true; }
        | InputProvided (_, inputState) ->
            let checkButtonDisabled = 
                match inputState with
                | NoInput -> true
                | _ -> false
            { NextButtonDisabled = false; CheckButtonDisabled = checkButtonDisabled; ShowButtonDisabled = false; }

    let taskButton color handler text disabled = 
        let options = 
            [
                Button.Props 
                    [ 
                        OnClick handler; 
                        OnFocus (fun (e) -> Fable.Import.Browser.document.getElementById(inputElementId).focus()) 
                    ]
                Button.Size IsMedium
                Button.Color color
                Button.Disabled disabled
                Button.CustomClass "task-button"
              ]
        Button.button options [ str text ]

    let rightButton = 
        match nextButtonDisplayed with
        | true -> taskButton NoColor handleUpdateClick "Next (⏎)" buttonViewState.NextButtonDisabled
        | false -> taskButton IsSuccess handleCheckClick "Check (⏎)" buttonViewState.CheckButtonDisabled

    div [ ClassName "task-buttons-container" ]
        [
            taskButton IsLight handleShowAnswerClick "Show (⇧ + ⏎)" buttonViewState.ShowButtonDisabled
            rightButton
        ]

let view model dispatch =
    let inputElementId = "task-input-element"
    let nextButtonDisplayed = 
        match model.State with
        | InputProvided (_, inpState) 
            when inpState = Shown || inpState = Right ->
            true
        | _ ->
            false

    let handleKeyDown (event: KeyboardEvent) =
        match model.State with
        | Fetching -> ()
        | _ ->
            match event.keyCode with
            | Keyboard.Codes.enter ->
                match event.shiftKey with
                | false -> 
                    match nextButtonDisplayed with
                    | true -> dispatch NextTask
                    | false -> dispatch CheckAnswer
                | true  -> dispatch ShowAnswer
            | _ -> 
                ()
    div []
        [
            inputView model dispatch handleKeyDown inputElementId
            symbolButtonsView model dispatch inputElementId
            buttonView model dispatch nextButtonDisplayed inputElementId
        ]
