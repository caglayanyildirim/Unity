using System;
using System.Globalization;
using Frequency.Framework.Service;

namespace Frequency.Unity.Common.Module.SharedData
{
    public static class SharedDataException
    {
        public class Fatal : ServiceException { public Fatal() : base(ResultCodeBlocks.Framework.Fatal()) { } }
        public class ParameterHeaderNotFound : ServiceException { public ParameterHeaderNotFound() : base(ResultCodes.Common_SharedData.Err(0)) { } }
        public class CannotMakeCrossExchange : ServiceException { public CannotMakeCrossExchange() : base(ResultCodes.Common_SharedData.Err(1)) { } }
        public class RequiredParameterIsMissing : ServiceException { public RequiredParameterIsMissing(string parameterName) : base(ResultCodes.Common_SharedData.Err(2), parameterName) { } }
        public class RecordNotFound : ServiceException
        {
            public RecordNotFound(int id) : this(id.ToString(CultureInfo.InvariantCulture)) { }
            public RecordNotFound(Guid recordIdentifier) : this(recordIdentifier.ToString()) { }
            public RecordNotFound(string recordIdentifier) : base(ResultCodes.Common_SharedData.Err(3), recordIdentifier) { }
        }
        public class DateShouldBeAfter : ServiceException
        {
            public DateShouldBeAfter(string parameterName, Date expectedMinDate) : this(parameterName, expectedMinDate.ToDateTime()) { }
            public DateShouldBeAfter(string parameterName, DateTime expectedMinDateTime) : base(ResultCodes.Common_SharedData.Err(4), parameterName, expectedMinDateTime) { }
        }
        public class DateRangeShouldBeLessThan : ServiceException { public DateRangeShouldBeLessThan(string parameterName, int days) : base(ResultCodes.Common_SharedData.Err(5), parameterName, days) { } }
        public class StringLengthShouldBeMoreThan : ServiceException { public StringLengthShouldBeMoreThan(string parameterName, int length) : base(ResultCodes.Common_SharedData.Err(6), parameterName, length) { } }
        public class StringLengthShouldBe : ServiceException { public StringLengthShouldBe(string parameterName, int length) : base(ResultCodes.Common_SharedData.Err(7), parameterName, length) { } }
        public class ExchangeRateNotFound : ServiceException { public ExchangeRateNotFound(CurrencyCode currency) : base(ResultCodes.Common_SharedData.Err(8), currency) { } }
        public class RecordAlreadyExists : ServiceException { public RecordAlreadyExists(string recordIdentifier) : base(ResultCodes.Common_SharedData.Err(9), recordIdentifier) { } }
        public class ListCountShouldBeLessThan : ServiceException { public ListCountShouldBeLessThan(string parameterName, int length) : base(ResultCodes.Common_SharedData.Err(10), parameterName, length) { } }
        public class InvalidData : ServiceException { public InvalidData(string parameterName, string value) : base(ResultCodes.Common_SharedData.Err(11), parameterName, value) { } }
        public class DuplicateDataFound : ServiceException { public DuplicateDataFound(string parameterName, string parameterValue) : base(ResultCodes.Common_SharedData.Err(12), parameterName, parameterValue) { } }
        public class ResultCountShouldBeLessThan : ServiceException { public ResultCountShouldBeLessThan(int resultCount) : base(ResultCodes.Common_SharedData.Err(13), resultCount) { } }
        public class ImageFormatNotSupported : ServiceException { public ImageFormatNotSupported() : base(ResultCodes.Common_SharedData.Err(14)) { } }
        public class AmountMustBeGreaterThan : ServiceException { public AmountMustBeGreaterThan(Money amount) : base(ResultCodes.Common_SharedData.Err(15), amount.ToString(true)) { } }
        public class AmountMustBeLessThan : ServiceException { public AmountMustBeLessThan(Money amount) : base(ResultCodes.Common_SharedData.Err(16), amount.ToString(true)) { } }
        public class AnErrorOccurred : ServiceException { public AnErrorOccurred() : base(ResultCodes.Common_SharedData.Err(17)) { } }
        public class ProcessContinues : ServiceException { public ProcessContinues() : base(ResultCodes.Common_SharedData.Err(18)) { } }
        public class AlreadyEnabled : ServiceException { public AlreadyEnabled() : base(ResultCodes.Common_SharedData.Err(19)) { } }
        public class AlreadyInUse : ServiceException { public AlreadyInUse(string recordIdentifier) : base(ResultCodes.Common_SharedData.Err(20), recordIdentifier) { } }
        public class StringLengthShouldBeLessThan : ServiceException { public StringLengthShouldBeLessThan(string parameterName, int length) : base(ResultCodes.Common_SharedData.Err(21), parameterName, length) { } }
        public class PhotoCanNotBeLargerThan : ServiceException { public PhotoCanNotBeLargerThan(int mb) : base(ResultCodes.Common_SharedData.Err(22), mb) { } }
        public class ParameterShouldBeLargerThan : ServiceException { public ParameterShouldBeLargerThan(string parameterName, string parameterValue) : base(ResultCodes.Common_SharedData.Err(23), parameterName, parameterValue) { } }
        public class AmountMustBe : ServiceException { public AmountMustBe(string parameterName, Money amount) : base(ResultCodes.Common_SharedData.Err(24), parameterName, amount.ToString(true)) { } }
        public class MissMatch : ServiceException { public MissMatch(string firstParameterName, string secondParamterName) : base(ResultCodes.Common_SharedData.Err(25), firstParameterName, secondParamterName) { } }

        ////TODO REFACTOR - Yukaridakiler de validasyon hatasi, bu sinifa neden ihtiyac duyulduysa duruma spesifik hata(lar) kullanilmali veya olusturulmali - cihan
        ////TODO RENAME - Error ya da Exception suffix'leri vermeye gerek yok, burada zaten yalnizca hata siniflari var. - cihan
        /// TODO Suan icin sadece MerchantRegistration uzerinde with ve save methodlarinda kullanilacak hale getirildi. ValidationManager kurgusu geldiginde silinebilir. - ceyhun
        public class ValidationError : ServiceException { public ValidationError(string errorMessage) : base(ResultCodes.Common_SharedData.Err(26), errorMessage) { } }
        public class DateShouldBeBefore : ServiceException
        {
            public DateShouldBeBefore(string parameterName, Date expectedMaxDate) : this(parameterName, expectedMaxDate.ToDateTime()) { }
            public DateShouldBeBefore(string parameterName, DateTime expectedMaxDateTime) : base(ResultCodes.Common_SharedData.Err(27), parameterName, expectedMaxDateTime) { }
        }

        public class ValueShouldBeGreaterThan : ServiceException { public ValueShouldBeGreaterThan(string parameterName, int value) : base(ResultCodes.Common_SharedData.Err(28), parameterName, value) { } }
        public class NoChangeToUpdate : ServiceException { public NoChangeToUpdate() : base(ResultCodes.Common_SharedData.Err(29)) { } }
        public class InvalidRecordStatus : ServiceException { public InvalidRecordStatus(string status) : base(ResultCodes.Common_SharedData.Err(30), status) { } }
        public class PaymentNotFound : ServiceException { public PaymentNotFound(string clientReferenceId) : base(ResultCodes.Common_SharedData.Err(31), clientReferenceId) { } }
        public class InvalidPhoneNumber : ServiceException { public InvalidPhoneNumber(string phoneNumber) : base(ResultCodes.Common_SharedData.Err(32), phoneNumber) { } }
        public class AtLeastOneParameterIsRequired : ServiceException { public AtLeastOneParameterIsRequired() : base(ResultCodes.Common_SharedData.Err(33)) { } }
        public class AmountMustBeWithCurrency : ServiceException { public AmountMustBeWithCurrency(CurrencyCode currency) : base(ResultCodes.Common_SharedData.Err(34), currency) { } }

    }
}