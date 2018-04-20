# LiteMediator

[![Build status](https://ci.appveyor.com/api/projects/status/27k3kat6xyhgq3tc?svg=true)](https://ci.appveyor.com/project/TheoConfidor/litemediator-dotnet)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE)

The purpose of this project is to provide a very simple, no-fluff, single source file .NET Mediator implementation. 

This project is designed to offer maximum value with minimal setup/configuration/learning. It is unintrusive and avoid the complexity and lack of clarity provided by other .NET Mediator implementations. 

## Usage

#### Add to Project

This library contains a single source file with two variants: `AsyncMediator.cs` and `SyncMediator.cs`

Use `AsyncMediator.cs` in projects that are primary asynchonrous and use a lot of `async Task`.
Use `SyncMediator.cs` in all other projects.

Copy-paste the selected file directly into your project. 

#### Request / Response

// TODO

#### Messaging

// TODO

## Credits

Silas Reinagel

## License

You may use this code in part or in full however you wish.  
No credit or attachments are required.