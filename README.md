# Check Your Czech - a service to practice Czech grammar

## See it in action

The service is deployed here: https://check-your-czech.azurewebsites.net/

Three exercises are available now: 
1. [Plurals](https://check-your-czech.azurewebsites.net/#plurals) - make plural number for nouns
1. [Accusatives](https://check-your-czech.azurewebsites.net/#accusatives) - make accusative case for nouns
2. [Comparatives](https://check-your-czech.azurewebsites.net/#comparatives) - make comparative degree for adjectives
3. [Imperatives](https://check-your-czech.azurewebsites.net/#imperatives) - make imperative mood for verbs
4. [Participles](https://check-your-czech.azurewebsites.net/#participles) - make active participle for verbs

## Concept

The idea of the service is to have a nice app that will help to selectively train some Czech grammar rules. 

*Selectively* is a key word as language services usually check the language knowledge in full. Whereas in reality some people learn new language rule by rule, case by case, aspect by aspect - so that they can not only lamely communicate but actually speak properly.

This service is supposed to give exercises like *"practice all the [exceptions](http://prirucka.ujc.cas.cz/?id=227) of the second case plural for the masculine animate gender"*.

## Features

- For plurals, the gender can be selected
- For comparatives and participles, the regularity can be selected
- Semi automated deployment
- Alerts about requests and exceptions

## Tech stack

This app uses the [SAFE-stack](https://safe-stack.github.io/). Everything is F# driven: 
- [Saturn](https://saturnframework.org/docs/) server framework
- [Fable](https://fable.io/) compiler for JS
- [Elmish](https://elmish.github.io/elmish/) architecture for front end
- [Paket](https://fsprojects.github.io/Paket/) dependency manager
- [Fake](https://fake.build/) build automation system

For hosting and metrics, Microsoft [Azure](https://azure.microsoft.com/en-us/) is used.

## Plan of development

There are a lot of [issues](https://github.com/psfinaki/CheckYourCzech/issues) to address, starting from handling rear language [quirks](https://github.com/psfinaki/CheckYourCzech/issues/173) and ending with conceptual [tasks](https://github.com/psfinaki/CheckYourCzech/issues/153). 

As for features, new exercises will be added. And maybe, at some point in the future, new languages.
