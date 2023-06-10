using PBL5.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PBL5.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class PBL5Controller : AbpControllerBase
{
    protected PBL5Controller()
    {
        LocalizationResource = typeof(PBL5Resource);
    }
}
