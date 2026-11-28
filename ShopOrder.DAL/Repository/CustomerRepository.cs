using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;

namespace ShopOrder.DAL.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbSession _dbSession;

    public CustomerRepository(IDbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        const string sql = @"
                SELECT Id, FullName, Phone, Email, Address
                FROM Customers";

        return await _dbSession.Connection.QueryAsync<Customer>(sql, transaction: _dbSession.Transaction);
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        const string sql = @"
                SELECT Id, FullName, Phone, Email, Address FROM Customers
                WHERE Id = @Id";

        return await _dbSession.Connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Id = id }, transaction: _dbSession.Transaction);
    }

    public async Task<Customer?> CreateAsync(Customer customer)
    {
        const string sql = @"
                INSERT INTO Customers (FullName, Phone, Email, Address)
                OUTPUT INSERTED.Id, INSERTED.FullName, INSERTED.Phone, INSERTED.Email, INSERTED.Address
                VALUES (@FullName, @Phone, @Email, @Address)";

        return await _dbSession.Connection.QuerySingleOrDefaultAsync<Customer>(sql, customer, transaction: _dbSession.Transaction);
    }

    public async Task<Customer?> UpdateAsync(int id, Customer customer)
    {
        const string sql = @"
                UPDATE Customers
                SET FullName = @FullName, Phone = @Phone, Email = @Email, Address = @Address
                OUTPUT INSERTED.Id, INSERTED.FullName, INSERTED.Phone, INSERTED.Email, INSERTED.Address
                WHERE Id = @Id";

        var rowAffected = await _dbSession.Connection.ExecuteAsync(sql, customer, transaction: _dbSession.Transaction);
        if (rowAffected == 0) return null;
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return false;
    }
}