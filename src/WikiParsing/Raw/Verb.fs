module WikiParsing.Raw.Verb

open FSharp.Data

open Common.WikiArticles
open WikiParsing.ConcreteArticles

type private WikiVerbMusit = HtmlProvider<"https://cs.wiktionary.org/wiki/musit">
type private WikiVerbMyslet = HtmlProvider<"https://cs.wiktionary.org/wiki/myslet">

type WikiConjugation = 
    {
        FirstSingular: string
        SecondSingular: string
        ThirdSingular: string
        FirstPlural: string
        SecondPlural: string
        ThirdPlural: string
    }

type WikiImperative = 
    {
        Singular: string
        FirstPlural: string
        SecondPlural: string
    }

type WikiParticiple = 
    {
        SingularMasculineAnimate: string
        SingularMasculineInanimate: string
        SingularFeminine: string
        SingularNeuter: string
        PluralMasculineAnimate: string
        PluralMasculineInanimate: string
        PluralFeminine: string
        PluralNeuter: string
    }

let getConjugation (VerbArticleWithConjugation (VerbArticle { Text = text })) = 
    let data = WikiVerbMyslet.Parse text
    let conjugationTable = data.Tables.``Časování[editovat]``
    {
        FirstSingular  = conjugationTable.Rows.[0].``Číslo jednotné - 1.``
        SecondSingular = conjugationTable.Rows.[0].``Číslo jednotné - 2.``
        ThirdSingular  = conjugationTable.Rows.[0].``Číslo jednotné - 3.``
        FirstPlural    = conjugationTable.Rows.[0].``Číslo množné - 1.``
        SecondPlural   = conjugationTable.Rows.[0].``Číslo množné - 2.``
        ThirdPlural    = conjugationTable.Rows.[0].``Číslo množné - 3.``
    }

let getImperative (VerbArticleWithImperative (VerbArticle { Text = text })) = 
    let data = WikiVerbMyslet.Parse text
    let imperativeTable = data.Tables.``Časování[editovat]2``
    {
        Singular     = imperativeTable.Rows.[0].``Číslo jednotné - 2.``
        FirstPlural  = imperativeTable.Rows.[0].``Číslo množné - 1.``
        SecondPlural = imperativeTable.Rows.[0].``Číslo množné - 2.``
    }

let getParticipleWhenImperativeAbsent (VerbArticleWithParticiple (VerbArticle { Text = text })) = 
    let data = WikiVerbMusit.Parse text
    let participleTable = data.Tables.``Časování[editovat]2``
    {
        SingularMasculineAnimate   = participleTable.Rows.[0].``Číslo jednotné - mužský životný i neživotný``
        SingularMasculineInanimate = participleTable.Rows.[0].``Číslo jednotné - mužský životný i neživotný``
        SingularFeminine           = participleTable.Rows.[0].``Číslo jednotné - ženský``
        SingularNeuter             = participleTable.Rows.[0].``Číslo jednotné - střední``
        PluralMasculineAnimate     = participleTable.Rows.[0].``Číslo množné - mužský životný``
        PluralMasculineInanimate   = participleTable.Rows.[0].``Číslo množné - mužský neživotný a ženský``
        PluralFeminine             = participleTable.Rows.[0].``Číslo množné - mužský neživotný a ženský``
        PluralNeuter               = participleTable.Rows.[0].``Číslo množné - střední``
    }

let getParticipleWhenImperativePresent (VerbArticleWithParticiple (VerbArticle { Text = text })) =
    let data = WikiVerbMyslet.Parse text
    let participleTable = data.Tables.``Časování[editovat]3``
    {
        SingularMasculineAnimate   = participleTable.Rows.[0].``Číslo jednotné - mužský životný i neživotný``
        SingularMasculineInanimate = participleTable.Rows.[0].``Číslo jednotné - mužský životný i neživotný``
        SingularFeminine           = participleTable.Rows.[0].``Číslo jednotné - ženský``
        SingularNeuter             = participleTable.Rows.[0].``Číslo jednotné - střední``
        PluralMasculineAnimate     = participleTable.Rows.[0].``Číslo množné - mužský životný``
        PluralMasculineInanimate   = participleTable.Rows.[0].``Číslo množné - mužský neživotný a ženský``
        PluralFeminine             = participleTable.Rows.[0].``Číslo množné - mužský neživotný a ženský``
        PluralNeuter               = participleTable.Rows.[0].``Číslo množné - střední``
    }
