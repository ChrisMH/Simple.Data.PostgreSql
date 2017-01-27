using System;
using System.Data;
using NpgsqlTypes;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  public class PgColumn : Column
  {
    protected PgColumn(string actualName, Table table)
      : base(actualName, table)
    {
    }

    protected PgColumn(string actualName, Table table, NpgsqlDbType npgsqlDbType)
      : base(actualName, table)
    {
    }

    protected PgColumn(string actualName, Table table, bool isIdentity) : base(actualName, table, isIdentity)
    {
    }

    public PgColumn(string actualName, Table table, bool isIdentity, DbType dbType, int maxLength, Type clrType)
      : base(actualName, table, isIdentity, dbType, maxLength)
    {
      ClrType = clrType;
    }
    
    public override bool IsBinary
    {
      get { return ClrType.Equals(typeof (Byte[])); }
    }

    public Type ClrType { get; private set; }

  }
}