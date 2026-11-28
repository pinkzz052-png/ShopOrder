using ShopOrder.DAL.Interface;

namespace ShopOrder.DAL;

public class  UnitOfWork : IUnitOfWork
{
    private readonly IDbSession _dbSession;

    public UnitOfWork(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        await _dbSession.EnsureOpenAsync(cancellationToken);
        _dbSession.Transaction = await _dbSession.Connection.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_dbSession.Transaction is null) return;

        try
        {
            await _dbSession.Transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _dbSession.Transaction.DisposeAsync();
            _dbSession.Transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_dbSession.Transaction is null) return;

        try
        {
            await _dbSession.Transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _dbSession.Transaction.DisposeAsync();
            _dbSession.Transaction = null;
        }
    }
}