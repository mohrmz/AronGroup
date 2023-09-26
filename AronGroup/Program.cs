using AronGroup.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.ConfigureServices().MigrateDatabase().ConfigurePipeline();
app.Run();
