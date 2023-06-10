using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace PBL5.Web;

[Dependency(ReplaceServices = true)]
public class PBL5BrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "PBL5";
}
