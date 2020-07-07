module WikiParsing.NounWikiDeclension

open FSharp.Data

open Common.WikiArticles

type private EditableArticle1Declension = HtmlProvider<"https://cs.wiktionary.org/wiki/panda">
type private EditableArticle2Declensions = HtmlProvider<"https://cs.wiktionary.org/wiki/čtvrt">
type private LockedArticle = HtmlProvider<"https://cs.wiktionary.org/wiki/debil">

type WikiDeclension = 
    {
        SingularNominative: string
        SingularGenitive: string
        SingularDative: string
        SingularAccusative: string
        SingularVocative: string
        SingularLocative: string
        SingularInstrumental: string
        PluralNominative: string
        PluralGenitive: string
        PluralDative: string
        PluralAccusative: string
        PluralVocative: string
        PluralLocative: string
        PluralInstrumental: string
    }

let getEditable1Declension (NounArticle { Text = text }) =
    let data = EditableArticle1Declension.Parse text
    {
        SingularNominative =   data.Tables.``Skloňování[editovat]``.Rows.[0].singulár
        SingularGenitive =     data.Tables.``Skloňování[editovat]``.Rows.[1].singulár
        SingularDative =       data.Tables.``Skloňování[editovat]``.Rows.[2].singulár
        SingularVocative =     data.Tables.``Skloňování[editovat]``.Rows.[4].singulár
        SingularAccusative =   data.Tables.``Skloňování[editovat]``.Rows.[3].singulár
        SingularLocative =     data.Tables.``Skloňování[editovat]``.Rows.[5].singulár
        SingularInstrumental = data.Tables.``Skloňování[editovat]``.Rows.[6].singulár
        PluralNominative =     data.Tables.``Skloňování[editovat]``.Rows.[0].plurál  
        PluralGenitive =       data.Tables.``Skloňování[editovat]``.Rows.[1].plurál  
        PluralDative =         data.Tables.``Skloňování[editovat]``.Rows.[2].plurál  
        PluralAccusative =     data.Tables.``Skloňování[editovat]``.Rows.[3].plurál  
        PluralVocative =       data.Tables.``Skloňování[editovat]``.Rows.[4].plurál  
        PluralLocative =       data.Tables.``Skloňování[editovat]``.Rows.[5].plurál  
        PluralInstrumental =   data.Tables.``Skloňování[editovat]``.Rows.[6].plurál  
    }

let getEditable2Declensions (NounArticle { Text = text }) =
    let data = EditableArticle2Declensions.Parse text
    [
        {
            SingularNominative =   data.Tables.``Skloňování (1)[editovat]``.Rows.[0].singulár
            SingularGenitive =     data.Tables.``Skloňování (1)[editovat]``.Rows.[1].singulár
            SingularDative =       data.Tables.``Skloňování (1)[editovat]``.Rows.[2].singulár
            SingularVocative =     data.Tables.``Skloňování (1)[editovat]``.Rows.[4].singulár
            SingularAccusative =   data.Tables.``Skloňování (1)[editovat]``.Rows.[3].singulár
            SingularLocative =     data.Tables.``Skloňování (1)[editovat]``.Rows.[5].singulár
            SingularInstrumental = data.Tables.``Skloňování (1)[editovat]``.Rows.[6].singulár
            PluralNominative =     data.Tables.``Skloňování (1)[editovat]``.Rows.[0].plurál  
            PluralGenitive =       data.Tables.``Skloňování (1)[editovat]``.Rows.[1].plurál  
            PluralDative =         data.Tables.``Skloňování (1)[editovat]``.Rows.[2].plurál  
            PluralAccusative =     data.Tables.``Skloňování (1)[editovat]``.Rows.[3].plurál  
            PluralVocative =       data.Tables.``Skloňování (1)[editovat]``.Rows.[4].plurál  
            PluralLocative =       data.Tables.``Skloňování (1)[editovat]``.Rows.[5].plurál  
            PluralInstrumental =   data.Tables.``Skloňování (1)[editovat]``.Rows.[6].plurál  
        }
        {
            SingularNominative =   data.Tables.``Skloňování (2)[editovat]``.Rows.[0].singulár
            SingularGenitive =     data.Tables.``Skloňování (2)[editovat]``.Rows.[1].singulár
            SingularDative =       data.Tables.``Skloňování (2)[editovat]``.Rows.[2].singulár
            SingularVocative =     data.Tables.``Skloňování (2)[editovat]``.Rows.[4].singulár
            SingularAccusative =   data.Tables.``Skloňování (2)[editovat]``.Rows.[3].singulár
            SingularLocative =     data.Tables.``Skloňování (2)[editovat]``.Rows.[5].singulár
            SingularInstrumental = data.Tables.``Skloňování (2)[editovat]``.Rows.[6].singulár
            PluralNominative =     data.Tables.``Skloňování (2)[editovat]``.Rows.[0].plurál  
            PluralGenitive =       data.Tables.``Skloňování (2)[editovat]``.Rows.[1].plurál  
            PluralDative =         data.Tables.``Skloňování (2)[editovat]``.Rows.[2].plurál  
            PluralAccusative =     data.Tables.``Skloňování (2)[editovat]``.Rows.[3].plurál  
            PluralVocative =       data.Tables.``Skloňování (2)[editovat]``.Rows.[4].plurál  
            PluralLocative =       data.Tables.``Skloňování (2)[editovat]``.Rows.[5].plurál  
            PluralInstrumental =   data.Tables.``Skloňování (2)[editovat]``.Rows.[6].plurál  
        }
    ]

let getLocked (NounArticle { Text = text }) =
    let data = LockedArticle.Parse text
    {
        SingularNominative =   data.Tables.``Skloňování``.Rows.[0].singulár
        SingularGenitive =     data.Tables.``Skloňování``.Rows.[1].singulár
        SingularDative =       data.Tables.``Skloňování``.Rows.[2].singulár
        SingularVocative =     data.Tables.``Skloňování``.Rows.[4].singulár
        SingularAccusative =   data.Tables.``Skloňování``.Rows.[3].singulár
        SingularLocative =     data.Tables.``Skloňování``.Rows.[5].singulár
        SingularInstrumental = data.Tables.``Skloňování``.Rows.[6].singulár
        PluralNominative =     data.Tables.``Skloňování``.Rows.[0].plurál  
        PluralGenitive =       data.Tables.``Skloňování``.Rows.[1].plurál  
        PluralDative =         data.Tables.``Skloňování``.Rows.[2].plurál  
        PluralAccusative =     data.Tables.``Skloňování``.Rows.[3].plurál  
        PluralVocative =       data.Tables.``Skloňování``.Rows.[4].plurál  
        PluralLocative =       data.Tables.``Skloňování``.Rows.[5].plurál  
        PluralInstrumental =   data.Tables.``Skloňování``.Rows.[6].plurál  
    }
