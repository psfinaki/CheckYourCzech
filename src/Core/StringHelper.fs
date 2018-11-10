module StringHelper

let trim (s: string) = s.Trim()

let split (separators: char[]) (s: string) = s.Split separators