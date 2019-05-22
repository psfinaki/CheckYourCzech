module ArticleParser 

open Article

let tryGetNoun = 
    tryGetContent
    >> Option.bind (tryGetChildPart "čeština")
    >> Option.bind (tryGetChildPart "podstatné jméno")

let tryGetAdjective =
    tryGetContent
    >> Option.bind (tryGetChildPart "čeština")
    >> Option.bind (tryGetChildPart "přídavné jméno")

let tryGetVerb =
    tryGetContent
    >> Option.bind (tryGetChildPart "čeština")
    >> Option.bind (tryGetChildPart "sloveso")
