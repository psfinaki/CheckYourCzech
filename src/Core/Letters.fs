module Letters 

let syllabicConsonants = ['l';'r']
let longVowels = ['á';'é';'í';'ó';'ú';'ů']
let shortVowels = ['a';'e';'i';'o';'u';'y']
let vowels = longVowels @ shortVowels

let isVowel letter = vowels |> List.contains letter
let isConsonant = not << isVowel

let isSyllabicConsonant letter = syllabicConsonants |> List.contains letter
let isNotSyllabicConsonant = not << isSyllabicConsonant
