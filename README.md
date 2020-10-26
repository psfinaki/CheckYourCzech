# Check Your Czech - a service to practice Czech grammar

[![Build Status](https://psfinaki.visualstudio.com/Check%20Your%20Czech/_apis/build/status/Check%20Your%20Czech%20X?branchName=master)](https://psfinaki.visualstudio.com/Check%20Your%20Czech/_build/latest?definitionId=2&branchName=master)

## See it in action

The service is deployed here: https://check-your-czech.azurewebsites.net/

Seven exercises are available now:
1. [Noun Declension](https://check-your-czech.azurewebsites.net/#nouns-declension)
2. [Adjective Declension](https://check-your-czech.azurewebsites.net/#adjectives-declension)
3. [Adjective Comparative](https://check-your-czech.azurewebsites.net/#adjecitves-comparatives)
4. [Verb Imperative](https://check-your-czech.azurewebsites.net/#verbs-imperatives)
5. [Verb Participle](https://check-your-czech.azurewebsites.net/#verbs-participles)
6. [Verb Conjugation](https://check-your-czech.azurewebsites.net/#verbs-conjugation)
7. [Numeral Cardinals](https://check-your-czech.azurewebsites.net/#numerals-cardinals)

## Concept

The idea of the service is to have a nice app that will help to selectively train some Czech grammar rules. 

*Selectively* is a key word as language services usually check the language knowledge in full. Whereas in reality some people learn new language rule by rule, case by case, aspect by aspect - so that they can not only lamely communicate but actually speak properly.

This service is supposed to give exercises like *"practice all the [exceptions](http://prirucka.ujc.cas.cz/?id=227) of the second case plural for the masculine animate gender"*.

## Features

- For declension, case and number can be selected, also gender and pattern for nouns
- For verb imperatives, verb class and verb pattern can be selected
- For comparatives and participles, regularity can be selected
- Automated deployment via Azure Devops
- Alerts about requests and exceptions

## Tech stack

This app uses the [SAFE-stack](https://safe-stack.github.io/). Everything is F# driven: 
- [Saturn](https://saturnframework.org/docs/) server framework
- [Fable](https://fable.io/) compiler for JS
- [Elmish](https://elmish.github.io/elmish/) architecture for front end
- [Paket](https://fsprojects.github.io/Paket/) dependency manager
- [Fake](https://fake.build/) build automation system

For hosting and metrics, Microsoft [Azure](https://azure.microsoft.com/en-us/) is used.

## Local testing

Developing and testing locally is possible both for Windows and Unix. Full stack debugging is supported in VS Code as described [here](https://safe-stack.github.io/docs/feature-debugging/).

### Installation for Windows
1. Install [.NET Core SDK](https://dotnet.microsoft.com/download)
2. Install [node.js](https://nodejs.org/)
3. Install [yarn](https://yarnpkg.com/en/)
4. Install [Microsoft SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-2016-express-localdb?view=sql-server-2017#try-it-out) either as a part of SQL Server or Visual Studio
5. Install and run [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409)

### Installation for MacOS/Linux
First three steps are same as for Windows.

For Azure storage emulator use [azurite](https://github.com/azure/azurite). Use V2 because the latest version supports Blobs only.

To install azurite use: `npm install -g azurite@2.7.0`
To run Azure storage emulator use: `azurite`

### Running the App
1. Open PowerShell, go to the repo root
2. Run Scraper: `dotnet fake build target runscraper`
3. Let it run for a a few minutes to generate some local word database
4. Stop Scraper
5. Run web app: `dotnet fake build target runweb`

**To upgrade paket dependencies**

```bash
dotnet tool run paket update
```

**Testing**

```bash
dotnet test
```

**UI Testing**
```bash
fake build target rune2etests
```
libgdiplus is required for Linux/Mac OS: https://github.com/dotnet/core/issues/2746 (https://formulae.brew.sh/formula/mono-libgdiplus)
`sudo ln -s /usr/local/lib/libgdiplus.dylib /usr/local/share/dotnet/shared/Microsoft.NETCore.App/3.1.6`

## Plan of development

There are a lot of [issues](https://github.com/psfinaki/CheckYourCzech/issues) to address, starting from handling rear language [quirks](https://github.com/psfinaki/CheckYourCzech/issues/173) and ending with conceptual [tasks](https://github.com/psfinaki/CheckYourCzech/issues/153). 

As for features, current exercises will be extended, new exercises will be added. 
And maybe, at some point in the future, new languages.
 
