# H's HTTP Gateway Playground play-http-gateway

A playground repo for implementeing and use cases of an HTTP Gateway


## Purpose

The purpose of this experiment is to create a replication system for services that communicate over HTTP. E.g.: Azure Storage Services (Tables, Blobs)

## Remarks for Table Storage

The hosting host name must have a subdomain with the TS AccountName. E.g.: `http://pgdanhsa.dev.localhost:5066`.

That's because the TS client works with that internally and it expects such a specific format, otherwise it crashes.

When testing locally, the TS Client cannot resolve such a host (gawd knows why, because a browser can), which means that you need to create an entry in the `hosts` file (`C:\Windows\System32\drivers\etc\hosts`) for it to work.

> 127.0.0.1       pgdanhsa.dev.localhost



## Usage

[WiP]

### Implement a gate action

1. Create an assembly/project or use an existing one
1. Make sure that it's referenced as a dependency for **H.HttpGate.Runtime.Host.AspNetCore**
1. Make sure that the given project references **H.HttpGate.Contracts.Public** and the `Microsoft.Extensions.DependencyInjection.Abstractions` NuGet
1. Create a `class` that implements `ImAnHsHttpGateAction`
1. Register the class within the service collection of the **H.HttpGate.Runtime.Host.AspNetCore** ASP.NET Host (`Program.cs` -> `builder.Services`).
1. Done, your action will now be invoked for every HTTP request and you'll have access to the Request and Response data.

---
