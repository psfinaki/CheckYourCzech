module Word

open Article
open Gender

let isForTaskPlurals =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "podstatné jméno")
    >> Option.bind (tryGetPart "skloňování")
    >> Option.isSome

let isForTaskPluralsWithGender word =
    word
    |> isForTaskPlurals
    
    &&

    word
    |> tryGetContent
    |> Option.bind (tryGetPart "čeština")
    |> Option.bind (tryGetPart "podstatné jméno")
    |> Option.bind (tryGetInfo "rod")
    |> Option.bind Gender.TryFromString
    |> Option.isSome

let isForTaskComparatives =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "přídavné jméno")
    >> Option.bind (tryGetPart "stupňování")
    >> Option.isSome

let isForTaskImperatives =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
    >> Option.bind (tryGetPart "časování")
    >> Option.isSome