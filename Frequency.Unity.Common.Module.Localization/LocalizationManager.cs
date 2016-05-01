using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Frequency.Framework;
using Frequency.Framework.Localization;

namespace Frequency.Unity.Common.Module.Localization
{
    public class LocalizationManager : ILocalizer
    {
        private readonly IModuleContext context;

        public LocalizationManager(IModuleContext context)
        {
            this.context = context;
        }
        public string GetLocalizedText(string key)
        {
            return GetCurrentLanguage().GetLocalizedText(key);
        }

        public List<LocalizationText> GetLocalizationTexts()
        {
            return GetCurrentLanguage().GetLocalizationTexts();
        }
        public Language GetCurrentLanguage()
        {
            return context.Query<Languages>().Current();
        }

        #region Api Mappings

        #region Localization

        string ILocalizer.GetLocalizedText(string key) { return GetLocalizedText(key); }
        List<CultureInfo> ILocalizer.GetLanguages() { return context.Query<Languages>().All().Select(l => new CultureInfo(l.LanguageCode)).ToList(); }
        Dictionary<string, string> ILocalizer.GetLocalizedTexts() { return GetLocalizationTexts().ToDictionary(lt => lt.TextKey, lt => lt.Value); ; }

        #endregion

        #endregion
    }
}