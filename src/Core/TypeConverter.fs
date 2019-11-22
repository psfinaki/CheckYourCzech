module TypeConverter

open System
open System.ComponentModel
open Common.Utils

type TupleConverter<'T1, 'T2> () =  
    inherit TypeConverter()

    override this.CanConvertFrom (context, sourceType) =
        sourceType = typeof<string> || base.CanConvertFrom(context, sourceType)

    override this.ConvertFrom(context, culture, value) =
        let elements = 
            Convert
                .ToString(value)
                .Trim('(').Trim(')')
                .Split([|','|], StringSplitOptions.RemoveEmptyEntries)
        match (elements.[0].Trim() |> fromString<'T1>, elements.[1].Trim() |> fromString<'T2>) with
        | Some e1, Some e2 -> (e1, e2) :> obj
        | _ -> failwith "failed to parse tuple type"