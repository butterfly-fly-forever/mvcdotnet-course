using System.Data;

namespace BookSale.Management.Domain.Abstract
{
    public interface ISQLQueryHandler
    {
        Task ExecuteNonReturnAsync(string query, object parammeters = null, IDbTransaction dbTransaction = null);
        Task<T> ExecuteReturnSingleRowAsync<T>(string query, object parammeters = null, IDbTransaction dbTransaction = null);
        Task<T?> ExecuteReturnSingleValueScalarAsync<T>(string query, object parammeters = null, IDbTransaction dbTransaction = null);
        Task<IEnumerable<T>> ExecuteStoreProdecureReturnListAsync<T>(string storeName, object parammeters = null, IDbTransaction dbTransaction = null);
    }
}