module ImprovedInput.Types
open Fable.Import.React

type Model = { 
    Value: string
    CursorStart: int
    CursorEnd: int
}

type Msg = 
    | SetInput of FormEvent
    | AddSymbol of char