
using System.Data;
using System.Data.Common;
using ShopOrder.DAL.Interface;

namespace ShopOrder.DAL;

public class DbSession : IDbSession
{
    private readonly DbConnection _connection;
    public DbSession(IDbConnectionFactory dbConnectionFactory)
    {
        _connection = dbConnectionFactory.CreateConnection();
    }
    
    public DbConnection Connection => _connection;
    public DbTransaction? Transaction { get; set; }

    public async Task EnsureOpenAsync(CancellationToken cancellationToken = default)
    {
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(cancellationToken);
        }
    }

    public void Dispose()
    {
        Transaction?.Dispose();
        _connection.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (Transaction is not null) await Transaction.DisposeAsync();
        await _connection.DisposeAsync();
    }
}