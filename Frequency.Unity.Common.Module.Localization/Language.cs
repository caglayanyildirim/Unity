using System;
using System.Collections.Generic;
using System.Linq;
using Frequency.Framework;
using Frequency.Framework.Caching;
using Frequency.Framework.DataAccess;
using Frequency.Unity.Common.Module.SharedData;

namespace Frequency.Unity.Common.Module.Localization
{
    public class Language : ICached, ICacheKeyProvider
    {
        #region Constructor

        private readonly IRepository<Language> repository;
        private readonly IModuleContext context;
        private readonly IApplicationCache applicationCache;

        protected Language() { }
        public Language(IRepository<Language> repository, IModuleContext context, IApplicationCache applicationCache)
        {
            this.repository = repository;
            this.context = context;
            this.applicationCache = applicationCache;
        }

        #endregion

        #region Properties

        public virtual int Id { get; protected set; }
        public virtual string LanguageCode { get; protected set; }
        public virtual string LanguageName { get; protected set; }
        public virtual bool IsActive { get; protected set; }

        #endregion

        protected internal virtual Language With(string code, string name)
        {
            if (context.Query<Languages>().SingleByLanguageCode(code) != null)
                throw new SharedDataException.RecordAlreadyExists(code);

            LanguageCode = code;
            LanguageName = name;
            IsActive = true;

            repository.Insert(this);

            return this;
        }

        protected internal virtual List<LocalizationText> GetLocalizationTexts()
        {
            return context.Query<LocalizationTexts>().ByLanguage(this);
        }

        private const string DICTIONARY = "Dictionary";
        private Dictionary<string, string> Dictionary
        {
            get
            {
                return applicationCache
                    .Get(this, DICTIONARY)
                    .Using(() => GetLocalizationTexts().ToDictionary(lt => lt.TextKey, lt => lt.Value));
            }
        }

        protected internal virtual string GetLocalizedText(string key)
        {
            string result;

            Dictionary.TryGetValue(key, out result);

            return result ?? key;
        }

        protected internal virtual void SetLocalizedText(string key, string text)
        {
            var localizationText = context.Query<LocalizationTexts>().SingleBy(this, key) ??
                                   context.New<LocalizationText>().With(key, text, LanguageCode);

            localizationText.UpdateText(text);

            applicationCache.Remove(this, DICTIONARY);

            GetLocalizedText(key); //set işlemi sonunda tüm cache'i tekrar ayağa kaldırması için eklendi
        }

        #region Api Mappings

        #region Caching

        string ICacheKeyProvider.CacheKey { get { return LanguageCode; } }

        #endregion

        #endregion

    }

    public class Languages : CachedQuery<Language>
    {
        public Languages(IModuleContext context)
        : base(context)
        { }

        internal Language SingleByLanguageCode(string languageCode)
        {
            return context.Request.Cache
                .Get(this, "SingleByLanguageCode", languageCode)
                .Using(() => All().SingleOrDefault(l => l.LanguageCode == languageCode));
        }

        public new List<Language> All()
        {
            return context.Request.Cache
                .Get(this, "All")
                .Using(() => CachedAll(CacheRegion.LongTerm));
        }

        [Internal]
        public virtual Language Current()
        {
            return SingleByLanguageCode(context.System.CurrentLanguageCode());
        }
    }
    public static class LanguageExtensions
    {
        public static string CurrentLanguageCode(this ISystem system)
        {
            return system.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
        }
    }
}
