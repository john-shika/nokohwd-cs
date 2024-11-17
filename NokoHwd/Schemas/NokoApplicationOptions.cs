namespace NokoHwd.Schemas;

public class NokoApplicationOptions
{
    public bool Scan { get; set; }
    public bool Print { get; set; }
    public bool Test { get; set; }
    public string? Json { get; set; }
    public string? Serial { get; set; }
    public string? Name { get; set; }
}
