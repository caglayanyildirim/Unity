using System.Collections.Generic;
using System.Linq;
using Frequency.Framework;
using Frequency.Framework.Caching;
using Frequency.Framework.DataAccess;
using Frequency.Unity.Common.Module.SharedData;


namespace Frequency.Unity.Common.Module.Localization
{
    public class LocalizationText : IAuditable, ICached
    {
        #region Constructor

        private readonly IRepository<LocalizationText> repository;
        private readonly IModuleContext context;
        protected LocalizationText() { }
        public LocalizationText(IRepository<LocalizationText> repository, IModuleContext context)
        {
            this.repository = repository;
            this.context = context;
        }
        #endregion

        #region Properties

        public virtual int Id { get; protected set; }
        public virtual string TextKey { get; protected set; }
        public virtual string Value { get; protected set; }
        public virtual Language Language { get; protected set; }
        public virtual bool IsPassive { get; protected set; }
        public virtual bool IsDeleted { get; protected set; }
        public virtual AuditInfo AuditInfo { get; protected set; }

        #endregion

        protected internal virtual LocalizationText With(string textKey, string value, string languageCode)
        {
            if (string.IsNullOrEmpty(textKey)) { throw new SharedDataException.RequiredParameterIsMissing("textKey"); }
            if (string.IsNullOrEmpty(value)) { throw new SharedDataException.RequiredParameterIsMissing("value"); }

            var language = context.Query<Languages>().SingleByLanguageCode(languageCode);
            if (language == null) { throw new SharedDataException.RecordNotFound("languageCode"); }

            Language = language;
            TextKey = textKey;
            Value = value;
            IsDeleted = false;

            repository.Insert(this);

            return this;
        }

        protected internal virtual void UpdateText(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return TextKey;
        }
    }

    public class LocalizationTexts : CachedQuery<LocalizationText>
    {
        public LocalizationTexts(IModuleContext context)
            : base(context) { }

        internal List<LocalizationText> ByLanguage(Language language)
        {
            return CachedBy(CacheRegion.LongTerm, lt => lt.Language == language);
        }

        internal LocalizationText SingleBy(Language language, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) { return null; }

            return SingleBy(lt => lt.Language == language && lt.TextKey == key);
        }

        public List<LocalizationText> UnsetByLanguage(Language language)
        {
            return By(lt => lt.Language == language && lt.TextKey == lt.Value);
        }

        public List<LocalizationText> ByValue(string value)
        {
            return Lookup.List(DEFAULT_FETCH_EAGER).Where(lt => lt.Value.StartsWith(value)).Take(20).ToList();
        }
    }
}
