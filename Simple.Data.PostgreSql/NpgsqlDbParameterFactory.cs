using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;
using Simple.Data.PostgreSql;

namespace Simple.Data.PostgreSql
{
  [Export(typeof (IDbParameterFactory))]
  public class NpgsqlDbParameterFactory : IDbParameterFactory
  {
    public IDbDataParameter CreateParameter(string name, Column column)
    {
      var npgsqlColumn = (NpgsqlColumn) column;
      return new NpgsqlParameter(name, npgsqlColumn.NpgsqlDbType, column.MaxLength, column.ActualName);
    }

    public IDbDataParameter CreateParameter(string name, DbType dbType, int maxLength)
    {
      return new NpgsqlParameter(name, dbType, maxLength);
    }
  }
}