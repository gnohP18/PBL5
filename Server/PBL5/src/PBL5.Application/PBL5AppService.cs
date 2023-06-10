using System;
using System.Collections.Generic;
using System.Text;
using PBL5.Localization;
using Volo.Abp.Application.Services;

namespace PBL5;

/* Inherit your application services from this class.
 */
public abstract class PBL5AppService : ApplicationService
{
    protected PBL5AppService()
    {
        LocalizationResource = typeof(PBL5Resource);
    }
}
