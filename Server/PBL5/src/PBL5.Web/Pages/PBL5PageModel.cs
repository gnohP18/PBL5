using PBL5.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace PBL5.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class PBL5PageModel : AbpPageModel
{
    protected PBL5PageModel()
    {
        LocalizationResourceType = typeof(PBL5Resource);
    }
}
