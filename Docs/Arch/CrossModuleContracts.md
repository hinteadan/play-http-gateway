# Cross Modules Operations and Data Contracts

Relevant `interfaces` and `DTOs` that are known globally across all modules and assemblies.

## Data Contracts

---

### 🗃️📦 Modules:
- 🗃️ **[`H.HttpGate.Contracts.Public`](/Src/H.Play.HttpGate/H.HttpGate.Contracts.Public)** - Contracts for HTTP Gate
- 🗃️ **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)** - Contracts for Data Replication
- 📦 [`H.Necessaire` _(NuGet)_ ↗️](https://www.nuget.org/packages/H.Necessaire)

---

### 📦 `EphemeralTypeBase` implements `IEphemeralType`

- **Purpose**: canonical base model `abstract class` used for decorating payloads with ephemeral timing info
    - `DateTime CreatedAt`
    - `DateTime AsOf`
    - `DateTime ValidFrom`
    - `TimeSpan? ValidFor`
    - `DateTime? ExpiresAt`
- Owned by `H.Necessaire` NuGet
    - **Source Code** URL: [`EphemeralTypeBase` Source Code ↗️](https://github.com/hinteadan/H.Necessaire/blob/master/Src/H.Necessaire/H.Necessaire/Models/EphemeralTypeBase.cs)
    - **NuGet** URL: [H.Necessaire NuGet Page ↗️](https://www.nuget.org/packages/H.Necessaire)
    - **GitHub** Repo URL: [H.Necessaire GitHub Page ↗️](https://github.com/hinteadan/H.Necessaire)

### 📦 `OperationResult`
- **Purpose**: canonical model `class` used as main building block for **resilient** flow implementations.
    - as in _"don't crash and throw exception, by default"_, but rather _"notify consumer of the failure and let it decide"_.
    - `bool IsSuccessful`
    - `string Reason`
    - `string[] Comments`
    - `string[] Warnings`
    - `string[] ReasonsToDisplay`
- `H.Necessaire` also comes with a lot of extension methods and resiliency execution methods built around this model.
- Owned by `H.Necessaire` NuGet
    - **Source Code** URL: [`OperationResult` Source Code ↗️](https://github.com/hinteadan/H.Necessaire/blob/master/Src/H.Necessaire/H.Necessaire/Models/OperationResult.cs)
    - **NuGet** URL: [H.Necessaire NuGet Page ↗️](https://www.nuget.org/packages/H.Necessaire)
    - **GitHub** Repo URL: [H.Necessaire GitHub Page ↗️](https://github.com/hinteadan/H.Necessaire)

### ✨💫 `HHttpGateResponse` _(+ linked `HHttpGateRequest`)_
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.HttpGate.Contracts.Public/DataContracts/HHttpGateResponse.cs)
- 🗃️ Module: **[`H.HttpGate.Contracts.Public`](/Src/H.Play.HttpGate/H.HttpGate.Contracts.Public)**
- **`HHttpGateRequest`** is a property of `HHttpGateResponse` that holds the corresponding request for the given response
- **Purpose**: **Exchange** the **HTTP Request and Response** with the **HTTP Gate Actions**.

### ✨💫 `HReplicationRequest : EphemeralTypeBase`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/DataContracts/HReplicationRequest.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- **Purpose**
    - **Exchange** replication **payload, source, type and validity** with the concrete replicators
    - Part of each replication registry entry

### ✨💫 `HReplicationRegistryEntry : EphemeralTypeBase`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/DataContracts/HReplicationRegistryEntry.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- **Purpose**
    - **Hold** the **replication state** of a **replication request**
    - Serves as an **audit** entry for **replication**
    - **Browse replication history** for any entity
        - Needed to determine if a replication request is obsolete due to a newer one, already completed
            - Thus enabling **out-of-order processing**

### ✨💫 `HReplicationProcessingQueueEntry : EphemeralTypeBase`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/DataContracts/HReplicationProcessingQueueEntry.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- **Purpose**
    - **The model of the processing queue message body**
    - **Replication Request** trigger payload
    - Reference to the `HReplicationRegistryEntry` that needs to be processed



## Operations

---

### 🗃️ Modules:
- 🗃️ **[`H.HttpGate.Contracts.Public`](/Src/H.Play.HttpGate/H.HttpGate.Contracts.Public)** - HTTP Gate API
- 🗃️ **[`H.HttpGate.Actions.AzureTableStorageReplication`](/Src/H.Play.HttpGate/AzureTableStorageReplication/H.HttpGate.Actions.AzureTableStorageReplication)** - HTTP Gate Azure TS Actions API
- 🗃️ **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)** - Replication API
- 🗃️ **[`H.Replication.Core`](/Src/H.Play.HttpGate/H.Replication.Core)** - Core Replication Concrete Implementations

---

### 🛖 Runtime Hosts
- 🛖👑 **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)** - HTTP Gate Host
- 🛖👑 **[`H.Replication.DataCopy.Host.AzureFunctions`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AzureFunctions)** - PROD Replication Host
- 🛖⚙️ **[`H.Replication.DataCopy.Host.AspNetCore`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AspNetCore)** - Alternative PROD Replication Host
- 🛖⚙️ **[`H.Replication.DataCopy.Host.Console`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.Console)** - DEBUG Replication Host

> ❕📝  All 3 `H.Replication.DataCopy.Host.***` runtime hosts are very shallow and use the same logic from **[`H.Replication.DataCopy.Processor`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Processor)**

---

### ✨🛠️ `ImAnHsHttpGateAction`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.HttpGate.Contracts.Public/ImAnHsHttpGateAction.cs)
- 🗃️ Module: **[`H.HttpGate.Contracts.Public`](/Src/H.Play.HttpGate/H.HttpGate.Contracts.Public)**
- 🛖 Hosted by: **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)**
- **Purpose**
    - Defines an **action** to be **invoked** by the **HTTP Gate** on **request interception**.
    - The Azure TS Replication is such an action
    - Multiple such actions can be defined, they'll all be invoked
    - **NOTE** that concrete implementations **MUST be very light** to not add overhead to the HTTP request
        - So no direct I/O or other heavy operations. Queue in-memory and defer execution.

### ✨🛠️ `AzureTableStorageReplicationAction : ImAnHsHttpGateAction`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/AzureTableStorageReplication/H.HttpGate.Actions.AzureTableStorageReplication/AzureTableStorageReplicationAction.cs)
- 🗃️ Module: **[`H.HttpGate.Actions.AzureTableStorageReplication`](/Src/H.Play.HttpGate/AzureTableStorageReplication/H.HttpGate.Actions.AzureTableStorageReplication)**
- 🛖 Hosted by: **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)**
- **Purpose** 
    - Concrete implementation of an `ImAnHsHttpGateAction`
    - 🌟 **Initiates** the Azure TS replication flow

### ✨🛠️ `ImAnHReplicator`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/ImAnHReplicator.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- 🛖 Hosted by: **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)**
- **Purpose**
    - **Orchestrate** replication process **initialization** _(the part that happens internally within the HTTP Gate private processing daemon)_
        - **Append** a **pending entry** in the **Replication Registry**
        - **Queue** processing **request** in the **Persistent Processing Queue**

### ✨🛠️ `HReplicator : ImAnHReplicator`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Core/HReplicator.cs)
- 🗃️ Module: **[`H.Replication.Core`](/Src/H.Play.HttpGate/H.Replication.Core)**
- 🛖 Hosted by: **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)**
- **Purpose**: Concrete implementation of the `ImAnHReplicator`, doing the described purpose
- 🤔 **NOTE TO SELF _(food for thought)_**: Perhaps this can be removed as a public abstraction since it might be just an internal detail of the HTTP Gate

### ✨🛠️ `ImAnHReplicationRegistry`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/ImAnHReplicationRegistry.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- 🛖 Hosted by:
    - **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)**
    - **[`H.Replication.DataCopy.Processor`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Processor)**
        - 👑 **[`H.Replication.DataCopy.Host.AzureFunctions`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AzureFunctions)**
        - ⚙️ **[`H.Replication.DataCopy.Host.AspNetCore`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AspNetCore)**
        - ⚙️ **[`H.Replication.DataCopy.Host.Console`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.Console)**
- **Purpose**: The **Replication Registry API**

### ✨🛠️ `ImAnHReplicationProcessingQueuePusher`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/ImAnHReplicationProcessingQueuePusher.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- 🛖 Hosted by: **[`H.HttpGate.Runtime.Host.AspNetCore`](/Src/H.Play.HttpGate/H.HttpGate.Runtime.Host.AspNetCore)**
- **Purpose**: The Replication Processing (_Persistent_) Queue **Submit Message** API

### ✨🛠️ `ImAnHReplicationProcessingQueueProcessor`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/ImAnHReplicationProcessingQueueProcessor.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- 🛖 Hosted by:
    - **[`H.Replication.DataCopy.Processor`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Processor)**
        - 👑 **[`H.Replication.DataCopy.Host.AzureFunctions`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AzureFunctions)**
        - ⚙️ **[`H.Replication.DataCopy.Host.AspNetCore`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AspNetCore)**
        - ⚙️ **[`H.Replication.DataCopy.Host.Console`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.Console)**
- **Purpose**: The Replication Processing (_Persistent_) Queue **Handle Message** API

### ✨🛠️ `ImAnHReplicationRequestProcessor`
- [Jump to Code ↗️](/Src/H.Play.HttpGate/H.Replication.Contracts/ImAnHReplicationRequestProcessor.cs)
- 🗃️ Module: **[`H.Replication.Contracts`](/Src/H.Play.HttpGate/H.Replication.Contracts)**
- 🛖 Hosted by:
    - **[`H.Replication.DataCopy.Processor`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Processor)**
        - 👑 **[`H.Replication.DataCopy.Host.AzureFunctions`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AzureFunctions)**
        - ⚙️ **[`H.Replication.DataCopy.Host.AspNetCore`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.AspNetCore)**
        - ⚙️ **[`H.Replication.DataCopy.Host.Console`](/Src/H.Play.HttpGate/H.Replication.DataCopy.Host.Console)**
- **Purpose**:
    - The **actual** Replication **Processing** API
    - After the `ImAnHReplicationProcessingQueueProcessor` unwraps the message received from the Processing (_Persistent_) Queue and validates it, it calls this API for the actual processing

---

## Concrete Implementations

Concrete implementations of these APIs are described in the following doc:

[Most Relevant Pieces ↗️](/Docs/Arch/MostRelevantPieces.md)