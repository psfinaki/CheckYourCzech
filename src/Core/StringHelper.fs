module StringHelper

open System.Text.RegularExpressions

let trim (s: string) = s.Trim()

let split (separators: char[]) (s: string) = s.Split separators

let starts (value: string) (s: string) = s.StartsWith value

let ends (value: string) (s: string) = s.EndsWith value

let remove (value: string) (s: string) = s.Replace(value, "")

let removeLast n (s: string) = s.Remove(s.Length - n)

let append (value: string) (s: string) = s + value

let removeMany (values: seq<string>) (s: string) = 
    let folder acc value = acc |> remove value
    values |> Seq.fold folder s

let removePattern pattern s = Regex.Replace(s, pattern, "")

let containsAny (values: seq<string>) (s: string) = 
    let containsOne = s.Contains
    values |> Seq.exists containsOne
