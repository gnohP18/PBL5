using Volo.Abp.Settings;

namespace PBL5.Settings;

public class PBL5SettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(PBL5Settings.MySetting1));
    }
}
