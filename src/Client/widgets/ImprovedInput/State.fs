module ImprovedInput.State

open Elmish
open Types
open Fable.Core.JsInterop

(*
    function handleInputValueChange(e) {
 
        var cursorStart = e.target.selectionStart,
            cursorEnd = e.target.selectionEnd;
     
            // value manipulations...
     
        e.target.setSelectionRange(cursorStart, cursorEnd);
    }
*)
// let handleChangeAnswer (event: FormEvent) =
//     dispatch (SetAnswer !!event.target?value)
let init () = { 
    Value = "" 
    CursorStart = 1
    CursorEnd = 1
}

let update msg model =
    match msg with
    | ChangeInput e -> 
        { model with Value = e.target?value; CursorStart = e.target?selectionStart; CursorEnd = e.target?selectionEnd}, Cmd.none
    | SetInput s ->
        { model with Value = s}, Cmd.none
    | AddSymbol c ->
        { model with Value = model.Value + string c}, Cmd.none
    | Reset ->
        init(), Cmd.none
    | _ ->
        model, Cmd.none // trigger a cmd when value changed
