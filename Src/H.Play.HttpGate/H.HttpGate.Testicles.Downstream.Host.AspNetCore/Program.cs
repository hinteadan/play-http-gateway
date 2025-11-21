namespace H.HttpGate.Testicles.Downstream.Host.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "GET Hello World!");
            app.MapPost("/", () => "POST Hello World!");

            app.Run();
        }
    }
}
