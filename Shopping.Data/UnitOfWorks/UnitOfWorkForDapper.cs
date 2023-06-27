using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Shopping.Core.UnitOfWorks;

namespace Shopping.Data.UnitOfWorks
{
    public class UnitOfWorkForDapper : IUnitOfWorkForDapper
    {
        private readonly string _connectionString;
        private bool disposed = false;

        private SqlTransaction sqlTransaction;
        private SqlConnection sqlConnection;

        public UnitOfWorkForDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            sqlConnection = new SqlConnection(_connectionString);
        }

        public SqlTransaction BeginTransaction()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Open)
            {
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
            }

            return sqlTransaction;
        }

        public SqlConnection GetConnection()
        {
            return sqlConnection;
        }

        public SqlTransaction GetTransaction()
        {
            return sqlTransaction;
        }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    sqlTransaction = null;
                }

                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                disposed = true;
            }
        }

        public void SaveChanges()
        {
            sqlTransaction.Commit();
            sqlConnection.Close();
            sqlTransaction = null;
        }

        ~UnitOfWorkForDapper() { Dispose(false); }
    }
}
