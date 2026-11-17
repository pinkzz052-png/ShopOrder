using Microsoft.Data.SqlClient;
using ShopOrder.DAL.Interface;
using ShopOrder.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace ShopOrder.DAL.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        public CustomerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            const string sql = @"
                SELECT Id, FullName, Phone, Email, Address
                FROM Customers";

            var customers = new List<Customer>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);

            await using var reader = await command.ExecuteReaderAsync();

            var idCol = reader.GetOrdinal("Id");
            var fullNameCol = reader.GetOrdinal("FullName");
            var phoneCol = reader.GetOrdinal("Phone");
            var emailCol = reader.GetOrdinal("Email");
            var addressCol = reader.GetOrdinal("Address");

            while (await reader.ReadAsync())
            {
                customers.Add(new Customer

                {
                    Id = reader.GetInt32(idCol),
                    FullName = reader.GetString(fullNameCol),
                    Phone = reader.GetString(phoneCol),
                    Email = reader.IsDBNull(emailCol) ? null : reader.GetString(emailCol),
                    Address = reader.IsDBNull(addressCol) ? null : reader.GetString(addressCol)
                });
            }
            return customers;
        }
        public async Task<Customer?> GetByIdAsync(int id)
        {
            const string sql = @"
                SELECT Id, FullName, Phone, Email, Address FROM Customers
                WHERE Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapToCustomer(reader);
            }
            return null;
        }
        public async Task<Customer> CreateAsync(Customer customer)
        {
            const string sql = @"
                INSERT INTO Customers (FullName, Phone, Email, Address)
                OUTPUT INSERTED.Id, INSERTED.FullName, INSERTED.Phone, INSERTED.Email, INSERTED.Address
                VALUES (@FullName, @Phone, @Email, @Address)";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value = customer.FullName;
            command.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = customer.Phone;
            command.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = (object?)customer.Email ?? DBNull.Value;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = (object?)customer.Address ?? DBNull.Value;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException("Can not create customer");
            }
            return MapToCustomer(reader);
        }
        public async Task<Customer> UpdateAsync(int id, Customer customer) 
        {
            const string sql = @"
                UPDATE Customers
                SET FullName = @FullName, Phone = @Phone, Email = @Email, Address = @Address
                OUTPUT INSERTED.Id, INSERTED.FullName, INSERTED.Phone, INSERTED.Email, INSERTED.Address
                WHERE Id = @Id";

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var command = new SqlCommand(sql, connection);
            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value = customer.FullName;
            command.Parameters.Add("@Phone", SqlDbType.VarChar, 20).Value = customer.Phone;
            command.Parameters.Add("@Email", SqlDbType.VarChar, 150).Value = (object?)customer.Email ?? DBNull.Value;
            command.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = (object?)customer.Address ?? DBNull.Value;

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException("Can not update customer");
            }
            return MapToCustomer(reader);
        } 
        public async Task<bool> DeleteAsync(int id)
        {
            return false;
        }

        public static Customer MapToCustomer(SqlDataReader reader) => new Customer
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            FullName = reader.GetString(reader.GetOrdinal("FullName")),
            Phone = reader.GetString(reader.GetOrdinal("Phone")),
            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address"))
        };
    }
}
