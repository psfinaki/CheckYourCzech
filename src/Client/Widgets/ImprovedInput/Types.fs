module Client.Widgets.ImprovedInput.Types

open Browser.Types

type Model = { 
    Value: string
    CursorStart: int
    CursorEnd: int
    InputId: string
}

type Msg = 
    | ChangeInput of Event
    | SetInput of string
    | AddSymbol of char
    | FocusInput
    | Reset
