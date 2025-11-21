using H.HttpGate.Ocelot;

namespace H.HttpGate.Runtime.Host.AspNetCore
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder
                = WebApplication
                .CreateBuilder(args)
                .AddHHttpGateOcelot()
                ;

            var app
                = await builder
                .Build()
                .UseHHttpGateOcelot()
                ;

            await app.RunAsync();
        }
    }
}
