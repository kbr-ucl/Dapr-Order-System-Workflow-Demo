var builder = WebApplication.CreateBuilder(args);

// Configure the services
// Add services to the container.
//var daprHttpPort = DaprHttpPort();
//var daprGrpcPort = DaprGrpcPort();
//builder.Services.AddDaprClient(config => config
//    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
//    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

//var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500";
//var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "50001";
//builder.Services.AddDaprClient(config => config
//    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
//    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));
//builder.Services.AddDaprClient();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapGet("/hello", () => "Order: Hello World!").WithOpenApi();

app.Run();


//string DaprHttpPort()
//{
//    var result = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT");
//    if (!string.IsNullOrEmpty(result)) return result;

//    result = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORTS");
//    if (!string.IsNullOrEmpty(result)) return result;

//    return "8080";
//}

//string DaprGrpcPort()
//{
//    var result = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT");
//    if (!string.IsNullOrEmpty(result)) return result;
    
//    return "3800";
//}