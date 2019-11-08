using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using ECommerce.Customers.Api.Model;

namespace ECommerce.Customers.Api.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public void Add(Customer prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO Customers (FirstName, LastName, UserName, Password)"
                    + " VALUES(@FirstName, @LastName, @UserName, @Password)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, prod);
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Customer>("SELECT * FROM Customers");
            }
        }

        public IEnumerable<Customer> GetAll(Customer query)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();

                var sql = new StringBuilder("SELECT * FROM Customers WHERE 1=1 ");

                if (!string.IsNullOrEmpty(query.UserName))
                {
                    sql.AppendFormat(" and UserName=@UserName ");
                }

                return dbConnection.Query<Customer>(sql.ToString(), new { UserName = query.UserName  });
            }
        }

        public Customer GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Customers"
                               + " WHERE CustomerId = @Id";
                dbConnection.Open();
                return dbConnection.Query<Customer>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM Customers"
                             + " WHERE CustomerId = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(Customer prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Customers SET FirstName = @FirstName,"
                               + " LastName = @LastName"
                               + " WHERE CustomerId = @CustomerId";
                dbConnection.Open();
                dbConnection.Query(sQuery, prod);
            }
        }
    }
}
