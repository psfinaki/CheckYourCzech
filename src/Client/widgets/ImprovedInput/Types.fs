module ImprovedInput.Types
open Fable.Import.React

type Model = { 
    Value: string
    CursorStart: int
    CursorEnd: int
    InputId: string
}

type Msg = 
    | ChangeInput of FormEvent
    | SetInput of string
    | AddSymbol of char
    | FocusInput
    | Reset