module StringHelpers

let split (separators: char[]) (s: string) = s.Split separators

let trim (s: string) = s.Trim()

let remove (oldValue: string) (s: string) = s.Replace(oldValue, "")

let startsWith (value: string) (s: string) = s.StartsWith value