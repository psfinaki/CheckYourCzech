module Conjugation

open System
open System.Collections.Generic

open GrammarCategories

type Pronoun =
    | Já | Ty | To | My | Vy | Oni

let getPronoun num pers =
    match num, pers with
    | Singular, First  -> Já
    | Singular, Second -> Ty
    | Singular, Third  -> To
    | Plural,   First  -> My
    | Plural,   Second -> Vy
    | Plural,   Third  -> Oni

let pronounToString pronoun =
    match pronoun with
    | Já  -> "já"
    | Ty  -> "ty"
    | To  -> "to"
    | My  -> "my"
    | Vy  -> "vy"
    | Oni -> "oni"

let getPronounString num pers =
    getPronoun num pers |> pronounToString

type ConjugationMapping = IDictionary<Tuple<Number, Person>, string []>