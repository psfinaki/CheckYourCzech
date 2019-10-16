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
open Markup

[<Literal>] 
let DefaultWord = ""
let DefaultAnswers = [||]
let SpecialSymbols = ["Á"; "Č"; "Ď"; "É"; "Ě"; "Í"; "Ň"; "Ó"; "Ř"; "Š"; "Ť"; "Ú"; "Ů"; "Ý"; "Ž"]

type Task = { 
    Word : string 
    Answers : string[]
}

type InputState = 
    | Unknown
    | Wrong
    | Right
    | Shown

type Input = string
type TaskWithInput = Task * Input * InputState

type State = 
    | Fetching
    | TaskProvided of Task
    | InputProvided of TaskWithInput

type Model = {
    TaskName: string
    State: State
}

type Msg = 
    | SetAnswer of string
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
    | TaskProvided t -> t
    | InputProvided (t, _, _) -> t

let getTaskIcon c = 
    match c with 
    | "task-input-correct"   -> Fa.Solid.CheckCircle
    | "task-input-incorrect" -> Fa.Solid.TimesCircle
    | "task-input-none"      -> Fa.Solid.QuestionCircle
    | _                      -> invalidArg "c" "Only certain type of icons could be mapped"

let checkAnswer model = 
    match model.State with 
        | Fetching -> model
        | TaskProvided _ -> model
        | InputProvided (t, inp, inpState) -> 
            let checkAnswer' () = 
                let result = t.Answers |> Array.contains inp
                log model.TaskName t.Word inp result
                let nextState = if result then Right else Wrong
                { model with State = InputProvided (t, inp, nextState) }

            match inpState with
            | Shown -> model
            | _ -> checkAnswer' ()

let init taskName getTask =
    { TaskName = taskName
      State = Fetching },
      loadTaskCmd getTask

let update msg model getTask =
    match msg with
    | FetchedTask task -> 
        { model with 
            State = TaskProvided { 
                        Word = task |> Option.map (fun t -> t.Word) |> Option.defaultValue DefaultWord; 
                        Answers = task |> Option.map (fun t -> t.Answers) |> Option.defaultValue DefaultAnswers
                    }
        }, Cmd.none
    | FetchError _ ->
        model, Cmd.none
    | SetAnswer input ->
        let newState = 
            if input <> "" then
                InputProvided (getStateTask model.State, input, Unknown)
            else
                TaskProvided (getStateTask model.State)
        { model with State = newState }, Cmd.none
    | ShowAnswer ->
        let task = getStateTask model.State
        let answer = task.Answers |> Array.tryHead |> Option.defaultValue DefaultWord
        { model with State = InputProvided (task, answer, Shown) }, Cmd.none
    | CheckAnswer -> 
        checkAnswer model, Cmd.none
    | NextTask ->
        { model with State = Fetching }, loadTaskCmd getTask

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
        | InputProvided (task, inputText, inputState) ->
            let inputClass =  
                match inputState with
                | Right -> "task-input-correct"
                | Wrong -> "task-input-incorrect"
                | _ -> defaultInputClass
            { Word = str task.Word; InputClass = inputClass; InputText = inputText }
        | TaskProvided task ->
            { Word = str task.Word; InputClass = defaultInputClass; InputText = defaultInputText }
    
    let inputIcon = getTaskIcon inputViewState.InputClass

    let handleChangeAnswer (event: FormEvent) =
        dispatch (SetAnswer !!event.target?value)
               
    Columns.columns [ Columns.IsGap (Screen.All, Columns.Is3) ]
        [
            Column.column [ ] [Tag.tag [ Tag.Color IsLight ; Tag.CustomClass "task-label"; ] [inputViewState.Word] ]
            Column.column [ ] [
                div [ClassName ("control has-icons-right " + inputViewState.InputClass)]
                    [
                        Input.text
                            [
                                Input.Id inputElementId
                                Input.Props [OnChange handleChangeAnswer; OnKeyDown handleKeyDown; AutoCapitalize "none"; AutoFocus true] 
                                Input.Value inputViewState.InputText
                                Input.Size Size.IsLarge
                            ]
                        Icon.icon [ Icon.Size IsSmall; Icon.IsRight ]
                            [ Fa.i [ inputIcon ] [] ]
                    ]                     
            ]        
        ]

let symbolButtonsView model dispatch = 
    let createSymbolButton s = Button.button [ ] [ str s ]
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
        | InputProvided _ ->
            { NextButtonDisabled = false; CheckButtonDisabled = false; ShowButtonDisabled = false; }
        | _ -> 
            { NextButtonDisabled = false; CheckButtonDisabled = true; ShowButtonDisabled = false; }

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
        | InputProvided (_, _, inpState) 
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
            symbolButtonsView model dispatch
            buttonView model dispatch nextButtonDisplayed inputElementId
        ]
