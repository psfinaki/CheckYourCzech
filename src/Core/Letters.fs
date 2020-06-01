module Core.Letters 

let syllabicConsonants = ['l';'r']
let hardConsonants = ['d';'t';'n']
let softConsonants = ['ř';'č';'ž';'š';'ť';'ď';'ň']
let neutralHardConsonants = ['v';'m';'b';'p']
let neutralSoftConsonants = ['l';'z';'s';'c';'j';'x']

let longVowels = ['á';'é';'í';'ó';'ú';'ů']
let shortVowels = ['a';'e';'ě';'i';'o';'u';'y']
let vowels = longVowels @ shortVowels

let isVowel letter = vowels |> List.contains letter
let isConsonant = not << isVowel

let isSyllabicConsonant letter = syllabicConsonants |> List.contains letter
let isNotSyllabicConsonant = not << isSyllabicConsonant

let isConsonantGroup letters = letters |> Seq.forall isConsonant
