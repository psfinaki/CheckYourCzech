module ImprovedInput.State

open Elmish
open Types

(*
    function handleInputValueChange(e) {
 
        var cursorStart = e.target.selectionStart,
            cursorEnd = e.target.selectionEnd;
     
            // value manipulations...
     
        e.target.setSelectionRange(cursorStart, cursorEnd);
    }
*)

let init () = { 
    Value = "" 
    CursorStart = 1
    CursorEnd = 1
}

let update msg model =
    match msg with
    | _ -> model, Cmd.none // trigger a cmd when value changed
