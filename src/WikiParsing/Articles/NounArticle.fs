module WikiParsing.Articles.NounArticle

open FSharp.Data

open WikiParsing.WikiString
open Article
open Common
open Common.GrammarCategories
open Common.Utils
open Common.GenderTranslations
open Common.WikiArticles

type EditableArticleOneDeclension = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type EditableArticleTwoDeclensions = HtmlProvider<"https://cs.wiktionary.org/wiki/čtvrt">
type LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

let caseColumns = 
    dict [ Case.Nominative, 0
           Case.Genitive, 1
           Case.Dative, 2
           Case.Accusative, 3
           Case.Vocative, 4
           Case.Locative, 5
           Case.Instrumental, 6 ]

let getNumberOfDeclensions (NounArticle article) =
    article
    |> matches [
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Seq.length

let getEditable (case: Case) number article =
    let numberOfDeclensions = getNumberOfDeclensions article
    let (NounArticle {Title = word; Text = text}) = article

    let caseColumn = caseColumns.[case]
    match numberOfDeclensions with
    | 1 ->
        let data = EditableArticleOneDeclension.Parse text
        match number with
        | Singular ->
            [ data.Tables.``Skloňování[editovat]``.Rows.[caseColumn].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování[editovat]``.Rows.[caseColumn].plurál ]
    | 2 ->
        let data = EditableArticleTwoDeclensions.Parse text
        match number with
        | Singular ->
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[caseColumn].singulár 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[caseColumn].singulár ]
        | Plural -> 
            [ data.Tables.``Skloňování (1)[editovat]``.Rows.[caseColumn].plurál 
              data.Tables.``Skloňování (2)[editovat]``.Rows.[caseColumn].plurál ]
    | _ ->
        invalidOp ("Odd word: " + word)

let getLocked (case: Case) number (NounArticle { Text = text }) =
    let data = LockedArticle.Parse text
    let caseColumn = caseColumns.[case]
    match number with
    | Singular ->
        [ data.Tables.Skloňování.Rows.[caseColumn].singulár ]
    | Plural -> 
        [ data.Tables.Skloňování.Rows.[caseColumn].plurál ]

let getDeclinability (NounArticle article) =
    let hasIndeclinabilityMarkInNounSection = 
        ``match`` [
            Is "podstatné jméno"
        ]
        >> Option.exists (hasInfo (Is "nesklonné"))

    let hasIndeclinabilityMarkInDeclensionSections =
        matches [
            Is "podstatné jméno"
            Starts "skloňování"
        ]
        >> Seq.exists (hasInfo (Is "nesklonné"))

    if
        article |> hasIndeclinabilityMarkInNounSection || 
        article |> hasIndeclinabilityMarkInDeclensionSections
    then Indeclinable
    else Declinable

let getDeclensionWiki (case: Case) number nounArticle =
    let (NounArticle article) = nounArticle
    let word = article.Title

    match word with
    | _ when getDeclinability nounArticle = Indeclinable ->
        [ word ]
    | _ when article |> isEditable ->
        getEditable case number nounArticle
    | _ when article |> isLocked ->
        getLocked case number nounArticle
    | word -> 
        invalidOp ("Odd word: " + word)

let private getPartialDeclension case number = 
    getDeclensionWiki case number 
    >> Seq.collect getForms
    >> Seq.distinct

let getDeclension article = 
    {
        SingularNominative = article |> getPartialDeclension Case.Nominative Number.Singular
        SingularGenitive = article |> getPartialDeclension Case.Genitive Number.Singular
        SingularDative = article |> getPartialDeclension Case.Dative Number.Singular
        SingularAccusative = article |> getPartialDeclension Case.Accusative Number.Singular
        SingularVocative = article |> getPartialDeclension Case.Vocative Number.Singular
        SingularLocative = article |> getPartialDeclension Case.Locative Number.Singular
        SingularInstrumental = article |> getPartialDeclension Case.Instrumental Number.Singular
        PluralNominative = article |> getPartialDeclension Case.Nominative Number.Plural
        PluralGenitive = article |> getPartialDeclension Case.Genitive Number.Plural
        PluralDative = article |> getPartialDeclension Case.Dative Number.Plural
        PluralAccusative = article |> getPartialDeclension Case.Accusative Number.Plural
        PluralVocative = article |> getPartialDeclension Case.Vocative Number.Plural
        PluralLocative = article |> getPartialDeclension Case.Locative Number.Plural
        PluralInstrumental = article |> getPartialDeclension Case.Instrumental Number.Plural
    }

let getGender (NounArticle article) =
    article
    |> ``match`` [
        Is "podstatné jméno"
    ] 
    |> Option.map (getInfos (OneOf (getAllUnion<Gender> |> Seq.map toString)))
    |> Option.filter Seq.hasOneElement
    |> Option.map Seq.exactlyOne

let hasRequiredInfoGender (NounArticle article) = 
    article
    |> ``match`` [
        Is "podstatné jméno"
    ] 
    |> Option.exists (hasInfo (OneOf (getAllUnion<Gender> |> Seq.map toString)))

let hasRequiredInfoDeclension (NounArticle article) =
    article
    |> isMatch [
        Is "podstatné jméno"
        Starts "skloňování"
    ]

let parseNounArticle article = 
    let (NounArticle { Title = title }) = article
    {
        CanonicalForm = title
        Declinability = article |> getDeclinability
        Gender = 
            if article |> hasRequiredInfoGender
            then article |> getGender |> Option.map fromString
            else None
        Declension = 
            if article |> hasRequiredInfoDeclension
            then article |> getDeclension |> Some
            else None
    }
