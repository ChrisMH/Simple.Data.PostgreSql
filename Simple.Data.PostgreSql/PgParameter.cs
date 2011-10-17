using System;
using System.Data;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  public class PgParameter : Parameter
  {
    protected PgParameter(string name, Type type, ParameterDirection direction)
      : base(name, type, direction)
    {
    }

    public PgParameter(string name, Type type, ParameterDirection direction, DbType dbtype, int size, int ordinalPosition)
      : base(name, type, direction, dbtype, size)
    {
      OrdinalPosition = ordinalPosition;
    }

    public int OrdinalPosition { get; private set; }
  }
}
