# Extension Points

Some possible extension scenarios and how to approach them

---

## Replicate the other way around, from Mongo to TS

1. Instead of the HTTP Gate, use Mongo's Change Stream to trigger the replication
    1. https://www.mongodb.com/docs/drivers/csharp/current/logging-and-monitoring/change-streams/
1. Reuse all other components with specific adaptions
    1. `HReplicationRequest`
        1. would have `Source` set to `MongoDB`
        1. `Payload**` props populated from Mongo's BsonDocument
    1. Implement a concrete `TSReplicationRequestProcessor : ImAnHReplicationRequestProcessor`
        1. Insipred by `MongoDbReplicationRequestProcessor`
        1. Would do the adaption the other way around
            1. Convert Mongo's Bson to TS JSON
            1. Save/Update/Delete in TS
    1. Implement a `ReplicationRequestProcessorLocator`
        1. Will select the proper `ImAnHReplicationRequestProcessor` instance based on the `Source`
        1. Required since we'd have two implementations (Mongo and TS)
    1. Wireup the necessary dependencies

---

## Use as Data Migration Tool

---

## Replication Health Checks

As a safety net, for resiliency, a periodic replication health check could be implemented.

1. Iterate/Stream through the `ImAnHReplicationRegistry`
    1. Looking for entries that are sketch
        1. `Pending` for a long time
        1. `Failed` without an obvious reason
        1. Etc.
    1. Try to reprocess them if needed
1. [OPTIONAL/HEAVY] Compare the 2 sources for inconsistencies, somehow, if possible
    1. Stream everything from Source (TS) and validate its replication on the destination (Mongo)

---

## Run some additional logic on TS change

The HTTP Gate with this implementation, basically acts as a Change Stream API for TS.

Therefore if any additional action needs to taken, apart from the current replication, it can be done easily by:

1. Implementing a new `ImAnHsHttpGateAction`
1. Registering it in the Service Collection of the HTTP Gate Host (`H.HttpGate.Runtime.Host.AspNetCore`)

---

## Adding a new replication destination

1. Implement a new `ImAnHReplicationRequestProcessor`
1. Change `HReplicationProcessingQueueProcessor` implementation
    1. Inject multiple `ImAnHReplicationRequestProcessor`
        1. `IEnumerable<ImAnHReplicationRegistry>` instead of `ImAnHReplicationRegistry`
    1. In `async Task<OperationResult> ProcessReplicationRequest(HReplicationRequest replicationRequest)`
        1. Iterate `IEnumerable<ImAnHReplicationRegistry>`
        1. Call `Process` on each one

---

## Changing the underlying tech of some components

### Persistent ProcessingQueue

Say we want to switch to RabbitMQ instead of Azure SB

1. Create a new assembly/ClassLibrary Project
1. Reference `H.Replication.Contracts`, `H.Replication.DataCopy.Processor` and `Microsoft.Extensions.Hosting.Abstractions` NuGet
1. Implement `ImAnHReplicationProcessingQueuePusher`
    1. Using RabbitMQ's .NET NuGet to push messages
1. Implement a `IHostedService` that
    1. Acts as the RabbitMQ subscriber/listener
        1. Just like `AzureServiceBusReplicationProcessingQueueDaemon : IHostedService`
    1. Calls `ImAnHReplicationProcessingQueueProcessor.Process`
        1. To handle received messages

### Replication Destination

Say we want to switch from Mongo to Cosmos

1. Create a new assembly/ClassLibrary Project
1. Reference `H.Replication.Contracts`
1. Implement `ImAnHReplicationRegistry`
    1. Just like `MongoDbReplicationRegistry` but with Cosmos, using the Cosmos C# Client NuGet
1. Implement `ImAnHReplicationRequestProcessor`
    1. Just like `MongoDbReplicationRequestProcessor` but with Cosmos, using the Cosmos C# Client NuGet

---

## [Home ↗️](/README.md)


## Relevant Cross Module Operation and Data Contracts

These are the relevant `interfaces` and `DTOs` that are known _(globally used)_ by almost all the modules.

[Cross Module Contracts ↗️](/Docs/Arch/CrossModuleContracts.md)


## Most Relevant Pieces

This part presents the most relevant parts of the solution.

At a higher level at first without their internal details, and further down with internal details as well.

[Most Relevant Pieces ↗️](/Docs/Arch/MostRelevantPieces.md)


## Modules Tree

This part presents the entire modules tree.

Highlights the most relevant pieces and presents the enitre dependency tree

[Modules Tree ↗️](/Docs/Arch/ModulesTree.md)


---
