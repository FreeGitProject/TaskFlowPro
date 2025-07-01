var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TaskFlowPro_Api>("taskflowpro-api");

builder.Build().Run();
