module StringHelper

let trim (s: string) = s.Trim()

let split (separator: char) (s: string) = s.Split separator