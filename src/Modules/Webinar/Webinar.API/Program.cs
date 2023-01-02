var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

public partial class Program
{
    public static readonly string Namespace = typeof(Program).Namespace;

    public static readonly string AppName =
        Namespace[(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1)..];
}