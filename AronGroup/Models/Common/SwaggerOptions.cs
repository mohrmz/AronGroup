namespace AronGroup.Models.Common;

public class SwaggerOption
{
    public bool Enabled { get; set; } = true;
    public SwaggerDocOption SwaggerDoc { get; set; } = new();
}

public class SwaggerDocOption
{
    public string Version { get; set; } = "v1";
    public string Title { get; set; } = string.Empty;
    public string Name { get; set; } = "v1";
    public string URL { get; set; } = "/swagger/v1/swagger.json";
    public string DocumentationFile { get; set; } = "AronGroupApi.xml";
}

