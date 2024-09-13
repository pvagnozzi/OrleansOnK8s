var builder = DistributedApplication.CreateBuilder(args);

var redis = builder
    .AddRedis("redis");
var redisEndpoint = redis.GetEndpoint("tcp");

var apiService = builder
    .AddProject<Projects.OrleansOnK8s_ApiService>("apiservice");

builder
    .AddProject<Projects.OrleansOnK8s_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(redis)
    .WithReference(apiService);

builder.AddProject<Projects.OrleansOnK8s_Voting>("voting")
    .WithReference(redis)
    .WithEnvironment("redis", redisEndpoint);

await builder
    .Build()
    .RunAsync();
