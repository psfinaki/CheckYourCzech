module ImprovedInput.Types
open Fable.Import.React

type Model = { 
    Value: string
    CursorStart: int
    CursorEnd: int
}

type Msg = 
    | ChangeInput of FormEvent
    | SetInput of string
    | AddSymbol of char
    | Reset