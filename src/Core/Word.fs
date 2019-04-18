module Word

let isNoun = ArticleParser.tryGetNoun >> Option.isSome
let isAdjective = ArticleParser.tryGetAdjective >> Option.isSome
let isVerb = ArticleParser.tryGetVerb >> Option.isSome
