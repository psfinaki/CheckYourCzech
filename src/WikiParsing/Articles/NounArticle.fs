module NounArticle

open FSharp.Data
open GrammarCategories
open WikiString
open Article

type EditableArticleOneDeclension = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type EditableArticleTwoDeclensions = HtmlProvider<"https://cs.wiktionary.org/wiki/čtvrt">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let getNumberOfDeclensions =
    matches [
        Is "čeština"
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    >> Seq.length

let getEditable case number word =
    let numberOfDeclensions = getNumberOfDeclensions word
    let url = getUrl word
    match numberOfDeclensions with
    | 1 ->
        let data = EditableArticleOneDeclension.Load url
        match number with
        | Singular ->
            [ data.Tables.``Skloňování[editovat]``.Rows.[case].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování[editovat]``.Rows.[case].plurál ]
    | 2 ->
        let data = EditableArticleTwoDeclensions.Load url
        match number with
        | Singular ->
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[case].singulár 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[case].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[case].plurál 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[case].plurál ]
    | _ ->
        invalidOp ("Odd word: " + word)

let getLocked case number word =
    let data = word |> getUrl |> LockedArticle.Load
    match number with
    | Singular ->
        [ data.Tables.Skloňování.Rows.[case].singulár ]
    | Plural -> 
        [ data.Tables.Skloňování.Rows.[case].plurál ]

let getDeclinability word = 
    let hasIndeclinabilityMarkInNounSection = 
        ``match`` [
            Is "čeština"
            Is "podstatné jméno"
        ] 
        >> Option.exists (hasInfo (Is "nesklonné"))

    let hasIndeclinabilityMarkInDeclensionSections =
        matches [
            Is "čeština"
            Is "podstatné jméno"
            Starts "skloňování"
        ]
        >> Seq.exists (hasInfo (Is "nesklonné"))

    if
        word |> hasIndeclinabilityMarkInNounSection || 
        word |> hasIndeclinabilityMarkInDeclensionSections
    then Indeclinable
    else Declinable

let getDeclensionWiki (case: Case) number word = 
    match word with
    | _ when getDeclinability word = Indeclinable ->
        [ word ]
    | _ when word |> isEditable ->
        getEditable (int case) number word
    | _ when word |> isLocked ->
        getLocked (int case) number word
    | word -> 
        invalidOp ("Odd word: " + word)

let getDeclension case number = 
    getDeclensionWiki case number 
    >> Seq.collect getForms
    >> Seq.distinct

let getGender =
    ``match`` [
        Is "čeština"
        Is "podstatné jméno"
    ] 
    >> Option.map (getInfos (Starts "rod "))
    >> Option.filter Seq.hasOneElement
    >> Option.map Seq.exactlyOne
    // TODO:
    // There is a problem here: we couple this place
    // with the fact that this is called after the noun validation
    // where we check the existance of gender in the article.
    // So logically we know that the gender exists
    // but code-wise we don't. This should be fixed.
    >> Option.defaultWith (fun () -> failwith "odd word")
