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

        let e1 = elements.[0].Trim() |> fromString<'T1>
        let e2 = elements.[1].Trim() |> fromString<'T2>

        match (e1, e2) with
        | Some v1, Some v2 -> (v1, v2) :> obj
        | _ -> failwith "failed to parse tuple type"