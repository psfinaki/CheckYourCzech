module Storage.Reflection

open System
open System.Reflection

open Common

let hasAttribute (attributeType: Type) (prop: PropertyInfo) = 
    prop.GetCustomAttributes(attributeType) |> Seq.any

let hasAttributes (prop: PropertyInfo) = 
    prop.GetCustomAttributes() |> Seq.any

let getAttributes (prop: PropertyInfo) = 
    prop.GetCustomAttributes() |> Seq.map (fun a -> a.GetType())

let getAttribute prop = prop |> getAttributes |> Seq.exactlyOne

let getValue (prop: PropertyInfo) entity = prop.GetValue entity

let setValue (prop: PropertyInfo) value entity = prop.SetValue(entity, value)
