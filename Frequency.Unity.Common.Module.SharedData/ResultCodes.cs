using Frequency.Framework.Service;

namespace Frequency.Unity.Common.Module.SharedData
{
    public class ResultCodes: ResultCodeBlocks
    {
        //0 - 1 framework tarafindan kullanilmaktadir. Bu bloklar rezervedir
        public static readonly ResultCodeBlock Security = CreateBlock(2, "Security");
        public static readonly ResultCodeBlock Common_Localization = CreateBlock(4, "Common.Localization");
        public static readonly ResultCodeBlock Common_Scheduling = CreateBlock(5, "Common.Scheduling");
        public static readonly ResultCodeBlock Common_Notification = CreateBlock(6, "Common.Notification");
        //7
        public static readonly ResultCodeBlock Common_SharedData = CreateBlock(8, "Common.SharedData");
        //9 - 20
        public static readonly ResultCodeBlock BizDev_ProductManagement = CreateBlock(21, "BizDev.ProductManagement");
        public static readonly ResultCodeBlock BizDev_Contact = CreateBlock(22, "BizDev.Contact");
        public static readonly ResultCodeBlock BizDev_Enterprise = CreateBlock(23, "BizDev.Enterprise");
        public static readonly ResultCodeBlock BizDev_Legal = CreateBlock(24, "BizDev.Legal");
        public static readonly ResultCodeBlock BizDev_Marketing = CreateBlock(25, "BizDev.Marketing");
        //25 - 40
        public static readonly ResultCodeBlock ProOps_OrderManagement = CreateBlock(41, "ProOps.OrderManagement");
        public static readonly ResultCodeBlock ProOps_Accounting = CreateBlock(42, "ProOps.Accounting");
        public static readonly ResultCodeBlock ProOps_Invoicing = CreateBlock(43, "ProOps.Invoicing");
        public static readonly ResultCodeBlock ProOps_Payment = CreateBlock(44, "ProOps.Payment");
        public static readonly ResultCodeBlock ProOps_Maintenance = CreateBlock(45, "ProOps.Maintenance");
    }
}