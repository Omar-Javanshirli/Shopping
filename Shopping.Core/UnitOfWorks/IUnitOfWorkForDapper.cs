using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping.Core.UnitOfWorks
{
    public interface IUnitOfWorkForDapper : IDisposable
    {
        SqlTransaction BeginTransaction();
        SqlConnection GetConnection();
        SqlTransaction GetTransaction();
        void SaveChanges();
    }
}
