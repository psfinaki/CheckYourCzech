/// Login web part and functions for API web part request authorisation with JWT.
module ServerCode.Auth

open Giraffe
open RequestErrors
open Microsoft.AspNetCore.Http

let createUserData (login : Domain.Login) =
    {
        UserName = "test"
        Token    =
            ServerCode.JsonWebToken.encode (
                { UserName = "test" } : ServerTypes.UserRights
            )
    } : Domain.UserData

let private missingToken = RequestErrors.BAD_REQUEST "Request doesn't contain a JSON Web Token"
let private invalidToken = RequestErrors.FORBIDDEN "Accessing this API is not allowed"

/// Checks if the HTTP request has a valid JWT token.
/// On success it will invoke the given `f` function by passing in the valid token.
let requiresJwtToken f : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        (match ctx.TryGetRequestHeader "Authorization" with
        | Some authHeader ->
            let jwt = authHeader.Replace("Bearer ", "")
            match JsonWebToken.isValid jwt with
            | Some token -> f token
            | None -> invalidToken
        | None -> missingToken) next ctx