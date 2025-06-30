
var builder = DistributedApplication.CreateBuilder(args);

// Add SQL Server with data volume and a named database
var sqlServer = builder.AddSqlServer("sql")
    .WithDataVolume() // Ensures data persists across container restarts
    .AddDatabase("sqldb");

// Add Redis (optional, used for caching/session etc.)
var redis = builder.AddRedis("redis");

// Add API service with dependency on SQL and development environment
var apiService = builder.AddProject<Projects.TaskFlowPro_Api>("api")
    .WithReference(sqlServer)       // Links the SQL dependency
    .WithReference(redis)           // Optional: if API uses Redis
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
.WithEndpoint(port: 5000, scheme: "http");


// Add Worker service (background processing, etc.)
var workerService = builder.AddProject<Projects.TaskFlowPro_Worker>("worker")
    .WithReference(sqlServer)
    .WithEnvironment("DOTNET_ENVIRONMENT", "Development");

// Add Client frontend, typically a web UI that talks to the API
var client = builder.AddProject<Projects.TaskFlowPro_Client>("client")
    .WithReference(apiService)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEndpoint(port: 5001, scheme: "http");


// Build and run the entire distributed application
builder.Build().Run();
