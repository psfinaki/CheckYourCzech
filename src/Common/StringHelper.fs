module Common.StringHelper

open System.Text.RegularExpressions

let trim (s: string) = s.Trim()

let split (separators: char[]) (s: string) = s.Split separators |> Seq.toList

let splitWords = split [|' '|] >> Seq.map trim

let starts (value: string) (s: string) = s.StartsWith value

let ends (value: string) (s: string) = s.EndsWith value

let endsOneOf (values: seq<string>) (s: string) = 
    values |> Seq.exists s.EndsWith

let remove (value: string) (s: string) = s.Replace(value, "")

let removeLast n (s: string) = s.Remove(s.Length - n)

let append (value: string) (s: string) = $"{s}{value}"

let appendAfterSpace (value: string) (s: string) = $"{s} {value}"

let prepend (value: string) (s: string) = $"{value}{s}"

let prependBeforeSpace (value: string) (s: string) = $"{value} {s}"

let toLower (s: string) = s.ToLower()

let removeMany (values: seq<string>) (s: string) = 
    let folder acc value = acc |> remove value
    values |> Seq.fold folder s

let removePattern pattern s = Regex.Replace(s, pattern, "")

let containsAny (values: seq<string>) (s: string) = 
    values |> Seq.exists s.Contains

let contains (s: string) = containsAny [s]

let takeFirst n (s: string) = s.Substring(0, n)

let takeLast n (s: string) = s.Substring (s.Length - n)

let takeFrom n (s: string) = s.Substring n

let isMatch pattern string = Regex.IsMatch(string, pattern)

let ``match`` pattern string = Regex.Match(string, pattern)
