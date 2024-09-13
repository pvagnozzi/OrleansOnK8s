using OrleansOnK8s.Voting.Data;
using OrleansOnK8s.Voting.Helpers;
using StackExchange.Redis;
using PollService = OrleansOnK8s.Voting.Data.PollService;

var builder = WebApplication.CreateBuilder(args);
var redisEndPointValue = Environment.GetEnvironmentVariable("redis") ?? string.Empty;
var redisEndPoint = redisEndPointValue.ToDnsEndPoint(6379);

builder.Host.UseOrleans((ctx, orleansBuilder) =>
{
    var redisEndpoints = new EndPointCollection { redisEndPoint };
    orleansBuilder.UseRedisClustering(options =>
        options.ConfigurationOptions = new ConfigurationOptions
        {
            DefaultDatabase = 1,
            EndPoints = redisEndpoints
        });

    orleansBuilder.AddRedisGrainStorage(
        "votes",
        optionsBuilder => optionsBuilder.Configure(options =>
            options.ConfigurationOptions = new ConfigurationOptions
            {
                DefaultDatabase = 2,
                EndPoints = redisEndpoints
            }
        ));
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<PollService>();
builder.Services.AddScoped<DemoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
await app.RunAsync();