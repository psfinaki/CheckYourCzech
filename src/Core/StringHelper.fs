module StringHelper

let trim (s: string) = s.Trim()

let split (separators: char[]) (s: string) = s.Split separators

let starts (value: string) (s: string) = s.StartsWith value

let remove (value: string) (s: string) = s.Replace(value, "")

let removeMany (values: string[]) (s: string) = 
    let folder acc value = acc |> remove value
    values |> Array.fold folder s
