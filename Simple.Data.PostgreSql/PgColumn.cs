using System.Data;
using NpgsqlTypes;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  public class PgColumn : Column
  {
    public PgColumn(string actualName, Table table)
      : base(actualName, table)
    {
    }

    public PgColumn(string actualName, Table table, NpgsqlDbType npgsqlDbType) : base(actualName, table)
    {
      NpgsqlDbType = npgsqlDbType;
    }

    public PgColumn(string actualName, Table table, bool isIdentity) : base(actualName, table, isIdentity)
    {
    }

    public PgColumn(string actualName, Table table, bool isIdentity, NpgsqlDbType npgsqlDbType, int maxLength)
      : base(actualName, table, isIdentity, default(DbType), maxLength)
    {
      NpgsqlDbType = npgsqlDbType;
    }


    public override bool IsBinary
    {
      get
      {
        return NpgsqlDbType == NpgsqlDbType.Bit ||
               NpgsqlDbType == NpgsqlDbType.Bytea;
      }
    }

    public NpgsqlDbType NpgsqlDbType { get; private set; }
  }
}