using System;
using System.Collections.Generic;
using System.Data;
using NpgsqlTypes;

namespace Simple.Data.PostgreSql
{
  public class TypeMap
  {
    public static TypeEntry GetTypeEntry(string dataType, string elementType = null)
    {
      if (!PgTypeToTypeEntry.ContainsKey(dataType))throw new ArgumentException(String.Format("Unknown data type name: {0}", dataType));

      return PgTypeToTypeEntry[dataType];
    }

    private static readonly Dictionary<string, TypeEntry> PgTypeToTypeEntry =
      new Dictionary<string, TypeEntry>
        {
          {"smallint", new TypeEntry("smallint", DbType.Int16, NpgsqlDbType.Smallint, typeof (Int16))},
          {"integer", new TypeEntry("integer", DbType.Int32, NpgsqlDbType.Integer, typeof (Int32))},
          {"bigint", new TypeEntry("bigint", DbType.Int64, NpgsqlDbType.Bigint, typeof (Int64))},
          {"decimal", new TypeEntry("decimal", DbType.Decimal, NpgsqlDbType.Numeric, typeof (Decimal))},
          {"numeric", new TypeEntry("numeric", DbType.Decimal, NpgsqlDbType.Numeric, typeof (Decimal))},
          {"real", new TypeEntry("numeric", DbType.Single, NpgsqlDbType.Real, typeof (Single))},
          {"double precision", new TypeEntry("double precision", DbType.Double, NpgsqlDbType.Double, typeof (Double))},
          {"money", new TypeEntry("money", DbType.Currency, NpgsqlDbType.Money, typeof (Decimal))},
          {"character varying", new TypeEntry("character varying", DbType.String, NpgsqlDbType.Varchar, typeof (String))},
          {"character", new TypeEntry("character varying", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"text", new TypeEntry("text", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"bytea", new TypeEntry("bytea", DbType.Binary, NpgsqlDbType.Bytea, typeof (Byte[]))},
          {"timestamp without time zone", new TypeEntry("timestamp without time zone", DbType.DateTime, NpgsqlDbType.Timestamp, typeof (DateTime))},
          {"timestamp with time zone", new TypeEntry("timestamp with time zone", DbType.DateTime, NpgsqlDbType.TimestampTZ, typeof (DateTime))},
          {"date", new TypeEntry("date", DbType.Date, NpgsqlDbType.Date, typeof (DateTime))},
          {"time without time zone", new TypeEntry("time without time zone", DbType.Time, NpgsqlDbType.Time, typeof (DateTime))},
          {"time with time zone", new TypeEntry("time with time zone", DbType.Time, NpgsqlDbType.TimeTZ, typeof (DateTime))},
          {"interval", new TypeEntry("interval", DbType.Object, NpgsqlDbType.Interval, typeof (TimeSpan))},
          {"boolean", new TypeEntry("boolean", DbType.Boolean, NpgsqlDbType.Boolean, typeof (Boolean))},
          {"point", new TypeEntry("point", DbType.Object, NpgsqlDbType.Point, typeof (NpgsqlPoint))},
          {"lseg", new TypeEntry("lseg", DbType.Object, NpgsqlDbType.LSeg, typeof (NpgsqlLSeg))},
          {"box", new TypeEntry("box", DbType.Object, NpgsqlDbType.Box, typeof (NpgsqlBox))},
          {"path", new TypeEntry("path", DbType.Object, NpgsqlDbType.Path, typeof (NpgsqlPath))},
          {"polygon", new TypeEntry("polygon", DbType.Object, NpgsqlDbType.Polygon, typeof (NpgsqlPolygon))},
          {"circle", new TypeEntry("circle", DbType.Object, NpgsqlDbType.Circle, typeof (NpgsqlCircle))},
          {"cidr", new TypeEntry("cidr", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"inet", new TypeEntry("inet", DbType.Object, NpgsqlDbType.Inet, typeof (System.Net.IPAddress))},
          {"macaddr", new TypeEntry("macaddr", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"bit", new TypeEntry("bit", DbType.Object, NpgsqlDbType.Bit, typeof (BitString))},
          {"bit varying", new TypeEntry("bit varying", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"tsvector", new TypeEntry("tsvector", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"tsquery", new TypeEntry("tsquery", DbType.String, NpgsqlDbType.Text, typeof (String))},
          {"uuid", new TypeEntry("uuid", DbType.Guid, NpgsqlDbType.Uuid, typeof (Guid))},
          {"oid", new TypeEntry("oid", DbType.Int64, NpgsqlDbType.Bigint, typeof (Int64))},
          {"ARRAY", new TypeEntry("ARRAY", DbType.Object, NpgsqlDbType.Array, typeof (Object))},
          {"USER-DEFINED", new TypeEntry("USER-DEFINED", DbType.Object, NpgsqlDbType.Bytea, typeof (Object))},
        };
  }

  public class TypeEntry
  {
    public TypeEntry(string pgTypeName, DbType dbType, NpgsqlDbType npgsqlDbType, Type clrType)
    {
      PgTypeName = pgTypeName;
      DbType = dbType;
      NpgsqlDbType = npgsqlDbType;
      ClrType = clrType;
    }

    public string PgTypeName { get; private set; }
    public DbType DbType { get; private set; }
    private NpgsqlDbType NpgsqlDbType { get; set; }
    public Type ClrType { get; private set; }
  }
}