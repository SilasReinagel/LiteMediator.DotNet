# LiteMediator

[![Build status](https://ci.appveyor.com/api/projects/status/27k3kat6xyhgq3tc?svg=true)](https://ci.appveyor.com/project/TheoConfidor/litemediator-dotnet)
[![Nuget](https://img.shields.io/nuget/vpre/litemediator.svg)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](./LICENSE)

Mediator pattern is a very useful way to build your API in a modular, decomposed way, and then aggregate your functionality into one common application interface. The senders and receivers of messages do not know about each other, instead they depend upon the message itself. 
It is one of the best ways of building [horizontally-organized systems](http://silasreinagel.com/2016/09/26/build-horizontally-not-vertically/).

The purpose of this project is to provide a very simple, no-fluff, single source file .NET Mediator implementation. This project is designed to offer maximum value with minimal setup/configuration/learning. It is unintrusive and avoids the complexity and lack of clarity provided by other .NET Mediator implementations such as [MediatR](https://github.com/jbogard/MediatR) and [Mediator.Net](https://github.com/mayuanyang/Mediator.Net).

----

## Usage

### Add to Project

This library contains a single source file with two variants: `AsyncMediator.cs` and `SyncMediator.cs`

Use `AsyncMediator.cs` in projects that are primary asynchonrous and use a lot of `async Task`.
Use `SyncMediator.cs` in all other projects.

Copy-paste the selected file directly into your project. 

If you are required to use NuGet for a company project, this library is also available on Nuget.org. 
`Install-Package LiteMediator`

### Request / Response

**Async Mediator**
```
var mediator = new AsyncMediator();

mediator.Register<CreateUserRequest, UserCreatedResponse>(
    async req => await _datasource.InsertUserAsync(req));

var response = await mediator.GetResponse<CreateUserRequest, UserCreatedResponse>(
    new CreateUserRequest(...));
```

**Sync Mediator**
```
var mediator = new SyncMediator();

mediator.Register<CreateUserRequest, UserCreatedResponse>(
   req => _datasource.InsertUser(req));

var response = mediator.GetResponse<CreateUserRequest, UserCreatedResponse>(
    new CreateUserRequest(...));
```

### Messaging

**Async Mediator**
```
var mediator = new AsyncMediator();

mediator.Register<NewProductAdded>(
    async msg => await _catalog.Update(msg));

await mediator.Publish(new NewProductAdded(...));
```

**Sync Mediator**
```
var mediator = new SyncMediator();

mediator.Register<NewProductAdded>(
    msg => _catalog.Update(msg));

mediator.Publish(new NewProductAdded(...));
```

----

## License

You may use this code in part or in full however you wish.  
No credit or attachments are required.
