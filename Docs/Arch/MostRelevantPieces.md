# Most Relevant Pieces

Relevant **concrete** implementations of the solution.

---

## 🛖 Runtime Hosts


### The HTTP Gate

🛖👑 **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)** - HTTP Gate Host


### The Data Copy Processor

🛖👑 **[`H.Replication.DataCopy.Host.AzureFunctions`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AzureFunctions)** - PROD Replication Host

🛖⚙️ **[`H.Replication.DataCopy.Host.AspNetCore`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AspNetCore)** - Alternative PROD Replication Host

🛖⚙️ **[`H.Replication.DataCopy.Host.Console`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.Console)** - DEBUG Replication Host

> ❕📝  All 3 `H.Replication.DataCopy.Host.***` runtime hosts are very shallow and use the same logic from **[`H.Replication.DataCopy.Processor`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Processor)**

---


## ✨🛠️ Concrete Pieces


### 🛖 HTTP Gate Host Layer

```mermaid
---
title: "Phase 1 : Azure TS HTTP Request/Response Interception"
config:
  theme: dark
  look: handDrawn
---
classDiagram
    direction LR

note for AzureTableStorageReplicator "Simple, in-memory queue, at this point"

ImAnHsHttpGateAction ().. AzureTableStorageReplicationAction

namespace H.HttpGate.Actions.AzureTableStorageReplication {
    class MappingXtnx {
        ToReplicationRequest(HHttpGateResponse)
    }

    class AzureTableStorageReplicationAction {
        Run(HHttpGateResponse)
    }
    
    
    class AzureTableStorageReplicator {
        $ConcurrentQueue replicationQueue
        QueueReplication()
    }
}

AzureTableStorageReplicationAction <--> MappingXtnx
AzureTableStorageReplicationAction --> AzureTableStorageReplicator
```

```mermaid
---
title: "Phase 2 : In-Memory Queue Processing"
config:
  theme: dark
  look: handDrawn
---
classDiagram
    direction TD

note for HReplicator "Persistent processing queue, at this point: Azure SB"

IHostedService ().. AzureTableStorageReplicationProcessingDaemon

namespace H.HttpGate.Actions.AzureTableStorageReplication {
    class AzureTableStorageReplicationProcessingDaemon {
        RunProcessingSession()
    }

    class MappingXtnx {
        ToHReplicationRequest()
    }

    class AzureTableStorageReplicator {
        $ConcurrentQueue replicationQueue
        RunReplicationSession()
    }
}

namespace H.Replication.Core {
    class HReplicator {
        Enqueue(HReplicationRequest)
    }
}

HReplicator ..() ImAnHReplicator
AzureTableStorageReplicationProcessingDaemon --> AzureTableStorageReplicator:every 5 seconds
AzureTableStorageReplicator <--> MappingXtnx
AzureTableStorageReplicator --> HReplicator:while replicationQueue.TryDequeue
```

```mermaid
---
title: "Phase 3 : HReplicator"
config:
  theme: dark
  look: handDrawn
---
classDiagram
    direction TD

note for ImAnHReplicationProcessingQueuePusher "Persistent processing queue, at this point: Azure SB"

namespace H.Replication.Core {
    class HReplicator {
        Enqueue(HReplicationRequest)
    }
}

HReplicator ..() ImAnHReplicator

namespace H.Replication.Contracts {
    
    class ImAnHReplicationRegistry { <<interface>>
        Append(HReplicationRequest)
    }
    class ImAnHReplicationProcessingQueuePusher { <<interface>>
        Enqueue(HReplicationRegistryEntry)
    }
}

namespace H.Replication.AzureServiceBus {
    class AzureServiceBusReplicationProcessingQueuePusher {
        Enqueue(HReplicationRegistryEntry)
    }
}

namespace H.Replication.MongoDB {
    class MongoDbReplicationRegistry {
        Append(HReplicationRequest)
    }
}

ImAnHReplicationRegistry <|-- MongoDbReplicationRegistry
ImAnHReplicationProcessingQueuePusher <|-- AzureServiceBusReplicationProcessingQueuePusher

HReplicator <--> ImAnHReplicationRegistry
HReplicator --> ImAnHReplicationProcessingQueuePusher

```

---


### 🛖 Data Copy Host Layer

🛖👑 **[`H.Replication.DataCopy.Host.AzureFunctions`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AzureFunctions)** - PROD Replication Host

🛖⚙️ **[`H.Replication.DataCopy.Host.AspNetCore`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AspNetCore)** - Alternative PROD Replication Host

🛖⚙️ **[`H.Replication.DataCopy.Host.Console`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.Console)** - DEBUG Replication Host

> ❕📝  All 3 `H.Replication.DataCopy.Host.***` runtime hosts are very shallow and use the same logic from **[`H.Replication.DataCopy.Processor`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Processor)**



```mermaid
---
title: "Data Copy Processor"
config:
  theme: dark
  look: handDrawn
---
classDiagram
    direction TD

note for HReplicationProcessingQueueProcessor "Hosted as Azure Functions (alt. as AppService or Console)"

ImAnHReplicationProcessingQueueProcessor ().. HReplicationProcessingQueueProcessor

class AzureSBQueue { <<Service>>
    Trigger(SBMessage)
}

namespace H.Replication.DataCopy.Processor {
    class HReplicationProcessingQueueProcessor {
        Process(HReplicationProcessingQueueEntry)
    }
}

namespace H.Replication.Contracts {
    
    class ImAnHReplicationRegistry { <<interface>>
        LoadEntry(Guid)
        Update(HReplicationRegistryEntry)
    }
    class ImAnHReplicationRequestProcessor { <<interface>>
        Process(HReplicationRequest)
    }
}

namespace H.Replication.MongoDB {
    class MongoDbReplicationRegistry {
        LoadEntry(Guid)
        Update(HReplicationRegistryEntry)
    }

    class MongoDbReplicationRequestProcessor {
        Process(HReplicationRequest)
    }
}

ImAnHReplicationRegistry <|-- MongoDbReplicationRegistry
ImAnHReplicationRequestProcessor <|-- MongoDbReplicationRequestProcessor

AzureSBQueue ..|> HReplicationProcessingQueueProcessor:Triggers
HReplicationProcessingQueueProcessor <--> ImAnHReplicationRegistry:LoadEntry
HReplicationProcessingQueueProcessor <--> ImAnHReplicationRequestProcessor:Process
HReplicationProcessingQueueProcessor <--> ImAnHReplicationRegistry:Update

```

---

## [Home ↗️](/README.md)


## Relevant Cross Module Operation and Data Contracts

These are the relevant `interfaces` and `DTOs` that are known _(globally used)_ by almost all the modules.

[Cross Module Contracts ↗️](/Docs/Arch/CrossModuleContracts.md)



## Extension Points

This part presents the extension points of the solution.

Such as:
- Run healthchecks on replications
- Rerun defunct or failed replications
- Adding a new action when TS data change occurs
- Adding a new replication destination
- Changing the underlying tech of some components 
    - E.g.: Use RabbitMQ
    - Or use another destination storage such as Cosmos, Raven, Mongo or even another TS
    - Use another storage for the replication registry
    - Implement a data mapping/transformation pipeline
    - Etc.

[Extension Points ↗️](/Docs/Arch/ExtensionPoints.md)

---