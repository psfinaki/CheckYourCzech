module WikiParsing.AdjectiveWikiParsing

open FSharp.Data

open Common.WikiArticles
open WikiParsing.ConcreteArticles

type private WikiAdjectiveNový = HtmlProvider<"https://cs.wiktionary.org/wiki/nový">
type private WikiAdjectiveStarý = HtmlProvider<"https://cs.wiktionary.org/wiki/starý">
type private WikiAdjectiveVrchní = HtmlProvider<"https://cs.wiktionary.org/wiki/vrchní">

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

type WikiComparison =
    {
        Positive: string
        Comparative: string
        Superlative: string
    }

let get1Declension (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) =
    let data = WikiAdjectiveNový.Parse text
    {
        SingularNominative =   data.Tables.``Skloňování[editovat]``.Rows.[0].``singulár - mužský životný``
        SingularGenitive =     data.Tables.``Skloňování[editovat]``.Rows.[1].``singulár - mužský životný`` 
        SingularDative =       data.Tables.``Skloňování[editovat]``.Rows.[2].``singulár - mužský životný`` 
        SingularVocative =     data.Tables.``Skloňování[editovat]``.Rows.[4].``singulár - mužský životný`` 
        SingularAccusative =   data.Tables.``Skloňování[editovat]``.Rows.[3].``singulár - mužský životný`` 
        SingularLocative =     data.Tables.``Skloňování[editovat]``.Rows.[5].``singulár - mužský životný`` 
        SingularInstrumental = data.Tables.``Skloňování[editovat]``.Rows.[6].``singulár - mužský životný`` 
        PluralNominative =     data.Tables.``Skloňování[editovat]``.Rows.[0].``plurál - mužský životný``  
        PluralGenitive =       data.Tables.``Skloňování[editovat]``.Rows.[1].``plurál - mužský životný``  
        PluralDative =         data.Tables.``Skloňování[editovat]``.Rows.[2].``plurál - mužský životný``  
        PluralAccusative =     data.Tables.``Skloňování[editovat]``.Rows.[3].``plurál - mužský životný``  
        PluralVocative =       data.Tables.``Skloňování[editovat]``.Rows.[4].``plurál - mužský životný``  
        PluralLocative =       data.Tables.``Skloňování[editovat]``.Rows.[5].``plurál - mužský životný``  
        PluralInstrumental =   data.Tables.``Skloňování[editovat]``.Rows.[6].``plurál - mužský životný``  
    }

let get2Declension (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) =
    let data = WikiAdjectiveStarý.Parse text
    {
        SingularNominative =   data.Tables.``Skloňování[editovat]2``.Rows.[0].``singulár - mužský životný``
        SingularGenitive =     data.Tables.``Skloňování[editovat]2``.Rows.[1].``singulár - mužský životný`` 
        SingularDative =       data.Tables.``Skloňování[editovat]2``.Rows.[2].``singulár - mužský životný`` 
        SingularVocative =     data.Tables.``Skloňování[editovat]2``.Rows.[4].``singulár - mužský životný`` 
        SingularAccusative =   data.Tables.``Skloňování[editovat]2``.Rows.[3].``singulár - mužský životný`` 
        SingularLocative =     data.Tables.``Skloňování[editovat]2``.Rows.[5].``singulár - mužský životný`` 
        SingularInstrumental = data.Tables.``Skloňování[editovat]2``.Rows.[6].``singulár - mužský životný`` 
        PluralNominative =     data.Tables.``Skloňování[editovat]2``.Rows.[0].``plurál - mužský životný``  
        PluralGenitive =       data.Tables.``Skloňování[editovat]2``.Rows.[1].``plurál - mužský životný``  
        PluralDative =         data.Tables.``Skloňování[editovat]2``.Rows.[2].``plurál - mužský životný``  
        PluralAccusative =     data.Tables.``Skloňování[editovat]2``.Rows.[3].``plurál - mužský životný``  
        PluralVocative =       data.Tables.``Skloňování[editovat]2``.Rows.[4].``plurál - mužský životný``  
        PluralLocative =       data.Tables.``Skloňování[editovat]2``.Rows.[5].``plurál - mužský životný``  
        PluralInstrumental =   data.Tables.``Skloňování[editovat]2``.Rows.[6].``plurál - mužský životný``  
    }

let get3Declension (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) =
    let data = WikiAdjectiveVrchní.Parse text
    {
        SingularNominative =   data.Tables.``Skloňování[editovat]3``.Rows.[0].``singulár - mužský životný``
        SingularGenitive =     data.Tables.``Skloňování[editovat]3``.Rows.[1].``singulár - mužský životný`` 
        SingularDative =       data.Tables.``Skloňování[editovat]3``.Rows.[2].``singulár - mužský životný`` 
        SingularVocative =     data.Tables.``Skloňování[editovat]3``.Rows.[4].``singulár - mužský životný`` 
        SingularAccusative =   data.Tables.``Skloňování[editovat]3``.Rows.[3].``singulár - mužský životný`` 
        SingularLocative =     data.Tables.``Skloňování[editovat]3``.Rows.[5].``singulár - mužský životný`` 
        SingularInstrumental = data.Tables.``Skloňování[editovat]3``.Rows.[6].``singulár - mužský životný`` 
        PluralNominative =     data.Tables.``Skloňování[editovat]3``.Rows.[0].``plurál - mužský životný``  
        PluralGenitive =       data.Tables.``Skloňování[editovat]3``.Rows.[1].``plurál - mužský životný``  
        PluralDative =         data.Tables.``Skloňování[editovat]3``.Rows.[2].``plurál - mužský životný``  
        PluralAccusative =     data.Tables.``Skloňování[editovat]3``.Rows.[3].``plurál - mužský životný``  
        PluralVocative =       data.Tables.``Skloňování[editovat]3``.Rows.[4].``plurál - mužský životný``  
        PluralLocative =       data.Tables.``Skloňování[editovat]3``.Rows.[5].``plurál - mužský životný``  
        PluralInstrumental =   data.Tables.``Skloňování[editovat]3``.Rows.[6].``plurál - mužský životný``  
    }

let getComparison (AdjectiveArticleWithComparative (AdjectiveArticle { Text = text })) = 
    let data = WikiAdjectiveNový.Parse text
    {
        Positive = data.Tables.``Stupňování[editovat]``.Rows.[0].tvar
        Comparative = data.Tables.``Stupňování[editovat]``.Rows.[1].tvar
        Superlative = data.Tables.``Stupňování[editovat]``.Rows.[2].tvar
    }

