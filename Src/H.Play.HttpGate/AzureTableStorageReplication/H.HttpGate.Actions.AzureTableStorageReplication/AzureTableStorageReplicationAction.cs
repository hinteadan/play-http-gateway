using H.HttpGate.Actions.AzureTableStorageReplication.BLL;
using H.HttpGate.Contracts.Public;
using H.HttpGate.Contracts.Public.DataContracts;
using H.Necessaire;

namespace H.HttpGate.Actions.AzureTableStorageReplication
{
    internal class AzureTableStorageReplicationAction : ImAnHsHttpGateAction
    {
        #region Construct
        readonly AzureTableStorageReplicator azureTableStorageReplicator;
        readonly ImALogger log;
        public AzureTableStorageReplicationAction(AzureTableStorageReplicator azureTableStorageReplicator)
        {
            this.azureTableStorageReplicator = azureTableStorageReplicator;
            this.log = HApp.Deps.GetLogger<AzureTableStorageReplicationAction>("H.HttpGate.Actions.AzureTableStorageReplication");
        }
        #endregion

        public async Task<OperationResult> Run(HHttpGateResponse response)
        {
            if (!(await response.ToReplicationRequest().LogError(log, "HHttpGateResponse.ToReplicationRequest")).Ref(out var mapRes, out var replicationRequest))
                return mapRes;

            return await azureTableStorageReplicator.QueueReplication(replicationRequest);
        }
    }
}
