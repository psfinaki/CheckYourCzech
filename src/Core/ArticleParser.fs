module ArticleParser 

open Article

let tryGetNoun = 
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "podstatné jméno")

let tryGetAdjective =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "přídavné jméno")

let tryGetVerb =
    tryGetContent
    >> Option.bind (tryGetPart "čeština")
    >> Option.bind (tryGetPart "sloveso")
