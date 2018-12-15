module Verb

open StringHelper

let isArchaic verb = 
    verb |> ends "ti" || 
    verb |> ends "ci"
