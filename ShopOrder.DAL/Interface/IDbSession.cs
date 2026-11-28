using System.Data.Common;

namespace ShopOrder.DAL.Interface;

public interface IDbSession : IDisposable, IAsyncDisposable
{
    DbConnection Connection { get; }
    DbTransaction? Transaction { get; set; }
    Task EnsureOpenAsync(CancellationToken cancellationToken = default);
}