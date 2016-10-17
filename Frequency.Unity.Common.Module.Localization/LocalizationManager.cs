using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Frequency.Framework;
using Frequency.Framework.Localization;
using Frequency.Framework.Service;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.Localization
{
    public class LocalizationManager : ILocalizer
    {
        private readonly IModuleContext context;

        public LocalizationManager(IModuleContext context)
        {
            this.context = context;
        }

        public string GetResultMessage(int resultCode)
        {
            return GetLocalizedText(string.Format("ERR-{0}", resultCode));
        }

        public string GetErrorMessage(ServiceException soaException)
        {
            return GetResultMessage(soaException.ResultCode).FormatWith(soaException.MessageParameters);
        }

        public Language GetLanguage(string languageCode)
        {
            return context.Query<Languages>().SingleByLanguageCode(languageCode);
        }

        public Language GetCurrentLanguage()
        {
            return context.Query<Languages>().Current();
        }

        public Language AddLanguage(string code, string name)
        {
            return context.New<Language>().With(code, name);
        }

        public List<Language> GetLanguages()
        {
            return context.Query<Languages>().All();
        }

        public void SetErrorMessage(int errorCode, string errorMessage) { SetErrorMessage(errorCode, errorMessage, context.System.CurrentLanguageCode()); }
        public void SetErrorMessage(int errorCode, string errorMessage, string languageCode)
        {
            SetLocalizedText(string.Format("ERR-{0}", errorCode), errorMessage, languageCode);
        }

        public List<string> GetKeyOptionsForSetLocalizedText()
        {
            return GetCurrentLanguage().GetUnsetLocalizationTexts().Select(lt => lt.TextKey).ToList();
        }

        public void SetLocalizedText(string key, string text) { SetLocalizedText(key, text, context.System.CurrentLanguageCode()); }
        public void SetLocalizedText(string key, string text, string languageCode)
        {
            var language = context.Query<Languages>().SingleByLanguageCode(languageCode);
            if (language == null)
            {
                throw new SharedDataException.RecordNotFound(languageCode);
            }

            language.SetLocalizedText(key, text);
        }

        public void SetLocalizedText(LocalizationText oldText, string newText)
        {
            if (oldText == null) { throw new SharedDataException.RequiredParameterIsMissing("oldText"); }

            oldText.UpdateText(newText);
        }

        public string GetLocalizedText(string key) { return GetLocalizedText(key, new object[0]); }
        [Internal]
        public string GetLocalizedText(string key, params object[] args)
        {
            var result = GetCurrentLanguage().GetLocalizedText(key);

            if (args != null && args.Length > 0)
            {
                result = result.FormatWith(args);
            }

            return result;
        }

        public List<LocalizationText> GetLocalizationTexts()
        {
            return GetCurrentLanguage().GetLocalizationTexts();
        }

        #region Api Mappings

        #region Localization

        string ILocalizer.GetLocalizedText(string key, params object[] args) { return GetLocalizedText(key, args); }
        List<CultureInfo> ILocalizer.GetLanguages() { return context.Query<Languages>().All().Select(l => new CultureInfo(l.LanguageCode)).ToList(); }
        Dictionary<string, string> ILocalizer.GetLocalizedTexts() { return GetLocalizationTexts().ToDictionary(lt => lt.TextKey, lt => lt.Value); ; }

        #endregion

        #endregion
    }
}
