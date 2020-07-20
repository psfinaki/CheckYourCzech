module Client.Utils

open Thoth
open Thoth.Fetch
open Thoth.Json

open Client.Widgets

let buildFetchTask url = 
    let decoder = Decode.Auto.generateDecoder<Task.Task option>()
    fun _ -> Fetch.fetchAs(url, decoder)
