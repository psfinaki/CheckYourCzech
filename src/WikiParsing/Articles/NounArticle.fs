module WikiParsing.Articles.NounArticle

open WikiParsing.WikiString
open WikiParsing.NounWikiDeclension
open Article
open Common
open Common.Declension
open Common.GrammarCategories
open Common.Utils
open Common.GenderTranslations
open Common.WikiArticles

let getNumberOfDeclensions (NounArticle article) =
    article
    |> matches [
        Is "podstatné jméno"
        Starts "skloňování"
    ]
    |> Seq.length

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

let private getDeclensionWiki nounArticle = 
    let (NounArticle article) = nounArticle
    let word = article.Title
    let numberOfDeclensions = nounArticle |> getNumberOfDeclensions

    match word with
    | _ when article |> isEditable && numberOfDeclensions = 1 ->
        [ getEditable1Declension nounArticle ]
    | _ when article |> isEditable && numberOfDeclensions = 2 ->
          getEditable2Declensions nounArticle
    | _ when article |> isLocked ->
        [ getLocked nounArticle ]
    | _ -> 
        invalidOp ("Odd word: " + word)

let private getDeclensionForDeclinable article = 
    let declensions = getDeclensionWiki article
    let getForms getCase = Seq.map getCase >> Seq.collect getForms >> Seq.distinct

    {
        SingularNominative =   declensions |> getForms (fun d -> d.SingularNominative)
        SingularGenitive =     declensions |> getForms (fun d -> d.SingularGenitive)
        SingularDative =       declensions |> getForms (fun d -> d.SingularDative)
        SingularAccusative =   declensions |> getForms (fun d -> d.SingularAccusative)
        SingularVocative =     declensions |> getForms (fun d -> d.SingularVocative)
        SingularLocative =     declensions |> getForms (fun d -> d.SingularLocative)
        SingularInstrumental = declensions |> getForms (fun d -> d.SingularInstrumental)
        PluralNominative =     declensions |> getForms (fun d -> d.PluralNominative)
        PluralGenitive =       declensions |> getForms (fun d -> d.PluralGenitive)
        PluralDative =         declensions |> getForms (fun d -> d.PluralDative)
        PluralAccusative =     declensions |> getForms (fun d -> d.PluralAccusative)
        PluralVocative =       declensions |> getForms (fun d -> d.PluralVocative)
        PluralLocative =       declensions |> getForms (fun d -> d.PluralLocative)
        PluralInstrumental =   declensions |> getForms (fun d -> d.PluralInstrumental)
    }

let private getDeclensionForIndeclinable (NounArticle { Title = word }) = 
    {
        SingularNominative =   seq { word }
        SingularGenitive =     seq { word }
        SingularDative =       seq { word }
        SingularVocative =     seq { word }
        SingularAccusative =   seq { word }
        SingularLocative =     seq { word }
        SingularInstrumental = seq { word }
        PluralNominative =     seq { word }
        PluralGenitive =       seq { word }
        PluralDative =         seq { word }
        PluralAccusative =     seq { word }
        PluralVocative =       seq { word }
        PluralLocative =       seq { word }
        PluralInstrumental =   seq { word }
    }

let getDeclension article =
    match article |> getDeclinability with
    | Declinability.Declinable -> getDeclensionForDeclinable article
    | Declinability.Indeclinable -> getDeclensionForIndeclinable article

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
