
namespace DMC.DAL.Script
{
    public interface IErpDataCache
    {
        /// <summary>
        /// 獲取緩存的Invoice
        /// </summary>
        string GetCacheInvoice { get; }
        /// <summary>
        /// 獲取緩存的Customer
        /// </summary>
        string GetCacheCustomer { get; }
    }
}
