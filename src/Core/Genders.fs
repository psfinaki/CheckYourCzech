module Genders

// http://www.fssnip.net/9h/title/Bi-directional-map
type BiMap<'a,'b when 'a : comparison and 'b : comparison>(item1s:'a list, item2s:'b list) =
    let item1IsKey = List.zip item1s item2s |> Map.ofList
    let item2IsKey = List.zip item2s item1s |> Map.ofList
  
    member __.findBy1 key = Map.find key item1IsKey
    member __.findBy2 key = Map.find key item2IsKey 
    member __.tryFindBy1 key = Map.tryFind key item1IsKey 
    member __.tryFindBy2 key = Map.tryFind key item2IsKey 

type Gender =
    | MasculineAnimate
    | MasculineInanimate
    | Feminine
    | Neuter

let genderTranslations = 
    BiMap([   MasculineAnimate  ;   MasculineInanimate  ;   Feminine  ;    Neuter     ],
          [ "rod mužský životný"; "rod mužský neživotný"; "rod ženský"; "rod střední" ])
  
type Gender with
    static member ToString      = genderTranslations.findBy1  
    static member FromString    = genderTranslations.findBy2
    static member TryFromString = genderTranslations.tryFindBy2
