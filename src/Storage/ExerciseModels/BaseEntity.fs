module Storage.ExerciseModels.BaseEntity

open Microsoft.WindowsAzure.Storage.Table
open Newtonsoft.Json
open System.Reflection

open Common.StringHelper
open Storage.Storage
open Storage.Reflection

let private serializeObject = JsonConvert.SerializeObject
let private serializeString = string
let private serializeOption = function 
    | null -> ""
    | value -> value.ToString() |> removeMany ["Some(" ; ")"]

let private serializationMap = 
    dict [ (typeof<SerializeObject>, serializeObject) 
           (typeof<SerializeString>, serializeString)
           (typeof<SerializeOption>, serializeOption) ] 

type BaseEntity(key) = 

    inherit TableEntity(key, key)

    override this.WriteEntity(context) = 
        let entityProperties = base.WriteEntity(context)
        let typeProperties = this.GetType().GetProperties()

        let serialize prop =
            let attribute = getAttribute prop
            let rawValue = this |> getValue prop
            serializationMap.[attribute] rawValue

        typeProperties
        |> Seq.where hasAttributes
        |> Seq.map (fun prop -> prop.Name, serialize prop)
        |> Seq.iter (fun (name, value) -> entityProperties.[name] <- EntityProperty(value))

        entityProperties

    override this.ReadEntity(entityProperties, context) =
        base.ReadEntity(entityProperties, context)
        let typeProperties = this.GetType().GetProperties()

        let deserialize (property: PropertyInfo) = 
            let stringValue = entityProperties.[property.Name].StringValue
            let ``type`` = property.PropertyType
            JsonConvert.DeserializeObject(stringValue, ``type``)

        typeProperties
        |> Seq.where (hasAttribute typeof<SerializeObject>)
        |> Seq.map (fun prop -> prop, deserialize prop)
        |> Seq.iter (fun (prop, value) -> this |> setValue prop value)
