namespace FluyAdmin.Infrastructure.Provisioning;

public class FluyServiceProvisioningOptions
{
    public const string SectionName = "Provisioning";

    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
