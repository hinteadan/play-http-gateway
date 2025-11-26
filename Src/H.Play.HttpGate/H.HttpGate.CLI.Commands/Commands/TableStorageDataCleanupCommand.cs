using H.Necessaire;
using H.Necessaire.CLI.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace H.HttpGate.CLI.Commands.Commands
{
    internal class TableStorageDataCleanupCommand : CommandBase
    {
        public override async Task<OperationResult> Run()
        {
            string tsJson = "{\"ID\":\"HGateTableStorageTestData::070826e5-f78e-4d56-ab82-4257b8c1e991\",\"DisplayName\":\"Bailey - Koepp\",\"Description\":\"Quia ipsa repudiandae et quia nam. Autem ex consequatur est deserunt hic non eligendi aut. Temporibus voluptatum sint.\",\"PartitionKey\":\"HGateTableStorageTestData\",\"RowKey\":\"070826e5-f78e-4d56-ab82-4257b8c1e991\",\"CreatedAt\":\"2025-11-25T23:30:58.7327485Z\",\"CreatedAt@odata.type\":\"Edm.DateTime\",\"AsOf\":\"2025-11-25T23:30:58.7327485Z\",\"AsOf@odata.type\":\"Edm.DateTime\",\"AsOfTicks\":\"638997102587327485\",\"AsOfTicks@odata.type\":\"Edm.Int64\",\"ValidFrom\":\"2025-11-25T23:30:58.7327485Z\",\"ValidFrom@odata.type\":\"Edm.DateTime\",\"ValidFromTicks\":\"638997102587327485\",\"ValidFromTicks@odata.type\":\"Edm.Int64\",\"ValidFor\":\"P1D\",\"ValidForTicks\":\"864000000000\",\"ValidForTicks@odata.type\":\"Edm.Int64\",\"ExpiresAt\":\"2025-11-26T23:30:58.7327485Z\",\"ExpiresAt@odata.type\":\"Edm.DateTime\",\"ExpiresAtTicks\":\"638997966587327485\",\"ExpiresAtTicks@odata.type\":\"Edm.Int64\"}";

            //\"CreatedAt\":\"2025-11-25T23:30:58.7327485Z\",\"CreatedAt@odata.type\":\"Edm.DateTime\"

            Regex regex = new Regex(",\"[^\"]+@odata\\.type\":\"[^\"]+\"");

            var cleanup = regex.Replace(tsJson, string.Empty);

            return true;
        }
    }
}
