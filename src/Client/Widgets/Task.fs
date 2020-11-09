module Client.Widgets.Task

open Browser.Types
open Elmish
open Fable.React
open Fable.React.Props
open Fable.Core
open Fable.Core.JsInterop
open Fable.FontAwesome
open Fulma
open System

open Client
open Client.Markup
open Client.Widgets.ImprovedInput.Types
open Client.Widgets
open Common.StringHelper

let loadingImage : string = import "*" "../images/loading.gif"

[<Emit("/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)")>]
let isMobile : bool = jsNative
[<Literal>] 
let DefaultWord = ""
let DefaultAnswers = [||]
[<Literal>]
let UpArrowSymbol = '↑'
[<Literal>]
let DownArrowSymbol = '↓'
let SpecialSymbols = ['á'; 'č'; 'ď'; 'é'; 'ě'; 'í'; 'ň'; 'ó'; 'ř'; 'š'; 'ť'; 'ú'; 'ů'; 'ý'; 'ž']
let SpecialSymbolsUpper = List.map (fun c -> Char.ToUpper(c)) SpecialSymbols
[<Literal>]
let InputElementId = "task-input-element"

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
    UpperCase: bool
    State: State
}

type Msg = 
    | ImprovedInput of ImprovedInput.Types.Msg
    | InputUpdated
    | ShowAnswer
    | CheckAnswer
    | NextTask
    | ChangeSymbolCase
    | FetchedTask of Task
    | FetchError of exn

type LeftButtonState = Show | Next

let log taskName task answer result = 
    let message = sprintf "Task name:%s; Task:%s; Answer:%s; Result:%b" taskName task answer result
    Logger.log message

let loadTaskCmd getTask =
    Cmd.OfPromise.either getTask [] FetchedTask FetchError

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
                let answer = model.Input.Value |> trim
                let result = t.Answers |> Array.contains answer
                log model.TaskName t.Word model.Input.Value result
                let nextState = if result then Right else Wrong
                { model with State = InputProvided (t, nextState) }

            match inpState with
            | Shown -> model
            | _ -> checkAnswer' ()

let init taskName getTask =
    let input = ImprovedInput.State.init InputElementId
    { Input = input
      TaskName = taskName
      UpperCase = false
      State = Fetching },
      loadTaskCmd getTask

let update msg model getTask =
    match msg with
    | FetchedTask task -> 
        { model with 
            State = InputProvided ({ 
                        Word = task.Word 
                        Answers = task.Answers
                    }, NoInput)
        }, Cmd.none
    | FetchError _ ->
        model, Cmd.none
    | ImprovedInput msg' ->
        let input, cmd = ImprovedInput.State.update msg' model.Input
        let triggeredCmd = 
            match msg' with
            | ChangeInput _ | AddSymbol _ -> Cmd.ofMsg InputUpdated
            | _ -> Cmd.none
        { model with Input = input }, Cmd.batch [ Cmd.map ImprovedInput cmd; triggeredCmd ]
    | InputUpdated ->
        match model.State with
        | Fetching -> model, Cmd.none
        | InputProvided (task, oldState) ->
            let input = model.Input.Value
            let newState = 
                if input <> "" then Unknown
                else NoInput
            { model with State = InputProvided (task, newState) }, Cmd.none
    | ShowAnswer ->
        let task = getStateTask model.State
        let answer = task.Answers |> Array.tryHead |> Option.defaultValue DefaultWord
        { model with State = InputProvided (task, Shown) }, answer |> (SetInput >> ImprovedInput >> Cmd.ofMsg)
    | CheckAnswer -> 
        checkAnswer model, Cmd.none
    | NextTask ->
        { model with State = Fetching; UpperCase = false }, Cmd.batch[ (ImprovedInput Reset |> Cmd.ofMsg); loadTaskCmd getTask ]
    | ChangeSymbolCase ->
        { model with UpperCase = not model.UpperCase}, Cmd.none

type InputViewState = {
    Word : ReactElement
    InputClass : string
    InputText : string
}

let inputView model dispatch handleKeyDown =
    let defaultInputClass = "task-input-none"
    let defaultInputText = ""

    let inputViewState = 
        match model.State with
        | Fetching -> 
            let imageSource = loadingImage
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

let symbolButtonsView model dispatch = 
    let arrowButtonSymbol = if model.UpperCase then DownArrowSymbol else UpArrowSymbol
    let specialSymbols = if model.UpperCase then SpecialSymbolsUpper else SpecialSymbols
    let handleButtonOnFocus _ = dispatch (ImprovedInput FocusInput)
    let disabled = 
        match model.State with
        | Fetching -> true
        | _ -> false
    let createSymbolButton h s = 
        Button.button [ 
            Button.Color IsLight
            Button.Disabled disabled 
            Button.Props [
                OnClick h
                OnFocus handleButtonOnFocus
            ]
        ] [ str <| string s ]
    let createCzechSymbolButton s =
        createSymbolButton (fun _ -> (ImprovedInput >> dispatch) <| AddSymbol s) s
    let createArrowButton =
        createSymbolButton (fun _ -> dispatch ChangeSymbolCase) arrowButtonSymbol
    Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3); Columns.CustomClass "symbol-buttons-columns" ]
            [
                Column.column [ ] [ ]
                Column.column [ ] [
                    div [ClassName "symbol-buttons"] 
                        (createArrowButton :: (List.map createCzechSymbolButton specialSymbols))
                ]        
            ]

type ButtonViewState = {
    NextButtonDisabled : bool
    CheckButtonDisabled : bool
    ShowButtonDisabled : bool
}

let buttonView model dispatch nextButtonDisplayed checkButtonDisabled =
    let handleShowAnswerClick _ = dispatch ShowAnswer
    let handleUpdateClick _ = dispatch NextTask
    let handleCheckClick _ = dispatch CheckAnswer
    let handleButtonOnFocus _ = dispatch (ImprovedInput FocusInput)

    let buttonViewState =
        match model.State with
        | Fetching -> 
            { NextButtonDisabled = true; CheckButtonDisabled = true; ShowButtonDisabled = true; }
        | InputProvided _ ->
            { NextButtonDisabled = false; CheckButtonDisabled = checkButtonDisabled; ShowButtonDisabled = false; }

    let taskButton color handler text disabled = 
        let options = 
            [
                Button.Props 
                    [ 
                        OnClick handler; 
                        OnFocus handleButtonOnFocus
                    ]
                Button.Size IsMedium
                Button.Color color
                Button.Disabled disabled
                Button.CustomClass "task-button"
              ]
        Button.button options [ str text ]
    let nextButtonText = if isMobile then "Next" else "Next (⏎)"
    let checkButtonText = if isMobile then "Check" else "Check (⏎)"
    let showButtonText = if isMobile then "Show" else "Show (⇧ + ⏎)"

    let rightButton = 
        match nextButtonDisplayed with
        | true -> taskButton NoColor handleUpdateClick nextButtonText buttonViewState.NextButtonDisabled
        | false -> taskButton IsSuccess handleCheckClick checkButtonText buttonViewState.CheckButtonDisabled

    div [ ClassName "task-buttons-container" ]
        [
            taskButton IsLight handleShowAnswerClick showButtonText buttonViewState.ShowButtonDisabled
            rightButton
        ]

let view model dispatch =
    let nextButtonDisplayed, checkButtonDisabled = 
        match model.State with
        | InputProvided (_, inpState) ->
            match inpState with
            | Shown | Right -> true, true
            | Unknown | Wrong -> false, false
            | NoInput -> false, true
        | _ ->
            false, true

    let handleKeyDown (event: KeyboardEvent) =
        match model.State with
        | Fetching -> ()
        | _ ->
            match event.key with
            | "Enter" ->
                match event.shiftKey with
                | false -> 
                    match nextButtonDisplayed with
                    | true -> dispatch NextTask
                    | false -> if checkButtonDisabled then () 
                               else dispatch CheckAnswer
                | true  -> dispatch ShowAnswer
            | _ -> 
                ()
    div []
        [
            inputView model dispatch handleKeyDown
            symbolButtonsView model dispatch
            buttonView model dispatch nextButtonDisplayed checkButtonDisabled
        ]
