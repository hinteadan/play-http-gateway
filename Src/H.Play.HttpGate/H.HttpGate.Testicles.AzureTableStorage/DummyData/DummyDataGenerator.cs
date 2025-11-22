using Bogus;
using H.HttpGate.Testicles.AzureTableStorage.DataContracts;

namespace H.HttpGate.Testicles.AzureTableStorage.DummyData
{
    internal class DummyDataGenerator
    {
        static readonly Faker bogus = new Faker();

        public HGateTableStorageTestData NewHGateTableStorageTestData()
        {
            return
                new HGateTableStorageTestData
                {
                    DisplayName = bogus.Company.CompanyName(),
                    Description = bogus.Lorem.Paragraph(),
                }
                ;
        }
    }
}
