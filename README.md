# H's HTTP Gateway Playground play-http-gateway

A playground repo for implementeing and use cases of an HTTP Gateway


## Purpose

The purpose of this experiment is to create a replication system for services that communicate over HTTP. E.g.: Azure Storage Services (Tables, Blobs)


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