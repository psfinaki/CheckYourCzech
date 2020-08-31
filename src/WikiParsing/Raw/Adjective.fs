module WikiParsing.Raw.Adjective

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

let getFirstDeclension (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) =
    let data = WikiAdjectiveNový.Parse text
    let declensionTable = data.Tables.``Skloňování[editovat]``
    {
        SingularNominative =   declensionTable.Rows.[0].``singulár - mužský životný``
        SingularGenitive =     declensionTable.Rows.[1].``singulár - mužský životný`` 
        SingularDative =       declensionTable.Rows.[2].``singulár - mužský životný`` 
        SingularVocative =     declensionTable.Rows.[4].``singulár - mužský životný`` 
        SingularAccusative =   declensionTable.Rows.[3].``singulár - mužský životný`` 
        SingularLocative =     declensionTable.Rows.[5].``singulár - mužský životný`` 
        SingularInstrumental = declensionTable.Rows.[6].``singulár - mužský životný`` 
        PluralNominative =     declensionTable.Rows.[0].``plurál - mužský životný``  
        PluralGenitive =       declensionTable.Rows.[1].``plurál - mužský životný``  
        PluralDative =         declensionTable.Rows.[2].``plurál - mužský životný``  
        PluralAccusative =     declensionTable.Rows.[3].``plurál - mužský životný``  
        PluralVocative =       declensionTable.Rows.[4].``plurál - mužský životný``  
        PluralLocative =       declensionTable.Rows.[5].``plurál - mužský životný``  
        PluralInstrumental =   declensionTable.Rows.[6].``plurál - mužský životný``  
    }

let getSecondDeclension (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) =
    let data = WikiAdjectiveStarý.Parse text
    let declensionTable = data.Tables.``Skloňování[editovat]2``
    {
        SingularNominative =   declensionTable.Rows.[0].``singulár - mužský životný``
        SingularGenitive =     declensionTable.Rows.[1].``singulár - mužský životný`` 
        SingularDative =       declensionTable.Rows.[2].``singulár - mužský životný`` 
        SingularVocative =     declensionTable.Rows.[4].``singulár - mužský životný`` 
        SingularAccusative =   declensionTable.Rows.[3].``singulár - mužský životný`` 
        SingularLocative =     declensionTable.Rows.[5].``singulár - mužský životný`` 
        SingularInstrumental = declensionTable.Rows.[6].``singulár - mužský životný`` 
        PluralNominative =     declensionTable.Rows.[0].``plurál - mužský životný``  
        PluralGenitive =       declensionTable.Rows.[1].``plurál - mužský životný``  
        PluralDative =         declensionTable.Rows.[2].``plurál - mužský životný``  
        PluralAccusative =     declensionTable.Rows.[3].``plurál - mužský životný``  
        PluralVocative =       declensionTable.Rows.[4].``plurál - mužský životný``  
        PluralLocative =       declensionTable.Rows.[5].``plurál - mužský životný``  
        PluralInstrumental =   declensionTable.Rows.[6].``plurál - mužský životný``  
    }

let getThirdDeclension (AdjectiveArticleWithPlural (AdjectiveArticle { Text = text })) =
    let data = WikiAdjectiveVrchní.Parse text
    let declensionTable = data.Tables.``Skloňování[editovat]3``
    {
        SingularNominative =   declensionTable.Rows.[0].``singulár - mužský životný``
        SingularGenitive =     declensionTable.Rows.[1].``singulár - mužský životný`` 
        SingularDative =       declensionTable.Rows.[2].``singulár - mužský životný`` 
        SingularVocative =     declensionTable.Rows.[4].``singulár - mužský životný`` 
        SingularAccusative =   declensionTable.Rows.[3].``singulár - mužský životný`` 
        SingularLocative =     declensionTable.Rows.[5].``singulár - mužský životný`` 
        SingularInstrumental = declensionTable.Rows.[6].``singulár - mužský životný`` 
        PluralNominative =     declensionTable.Rows.[0].``plurál - mužský životný``  
        PluralGenitive =       declensionTable.Rows.[1].``plurál - mužský životný``  
        PluralDative =         declensionTable.Rows.[2].``plurál - mužský životný``  
        PluralAccusative =     declensionTable.Rows.[3].``plurál - mužský životný``  
        PluralVocative =       declensionTable.Rows.[4].``plurál - mužský životný``  
        PluralLocative =       declensionTable.Rows.[5].``plurál - mužský životný``  
        PluralInstrumental =   declensionTable.Rows.[6].``plurál - mužský životný``  
    }

let getComparison (AdjectiveArticleWithComparative (AdjectiveArticle { Text = text })) = 
    let data = WikiAdjectiveNový.Parse text
    let comparisonTable = data.Tables.``Stupňování[editovat]``
    {
        Positive =    comparisonTable.Rows.[0].tvar
        Comparative = comparisonTable.Rows.[1].tvar
        Superlative = comparisonTable.Rows.[2].tvar
    }

