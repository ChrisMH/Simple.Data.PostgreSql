using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;

using NpgsqlTypes;

namespace Simple.Data.PostgreSql
{
  public class TypeMap
  {
    public static TypeEntry GetTypeEntry(string dataType, string elementType = null)
    {
      if (!PgTypeToTypeEntry.ContainsKey(dataType))throw new ArgumentException($"Unknown data type name: {dataType}");

      return PgTypeToTypeEntry[dataType];
    }

    private static readonly Dictionary<string, TypeEntry> PgTypeToTypeEntry =
      new Dictionary<string, TypeEntry>
        {
          {"smallint", new TypeEntry("smallint", DbType.Int16, NpgsqlDbType.Smallint, typeof (short))},
          {"integer", new TypeEntry("integer", DbType.Int32, NpgsqlDbType.Integer, typeof (int))},
          {"bigint", new TypeEntry("bigint", DbType.Int64, NpgsqlDbType.Bigint, typeof (long))},
          {"decimal", new TypeEntry("decimal", DbType.Decimal, NpgsqlDbType.Numeric, typeof (decimal))},
          {"numeric", new TypeEntry("numeric", DbType.Decimal, NpgsqlDbType.Numeric, typeof (decimal))},
          {"real", new TypeEntry("numeric", DbType.Single, NpgsqlDbType.Real, typeof (float))},
          {"double precision", new TypeEntry("double precision", DbType.Double, NpgsqlDbType.Double, typeof (double))},
          {"money", new TypeEntry("money", DbType.Currency, NpgsqlDbType.Money, typeof (decimal))},
          {"character varying", new TypeEntry("character varying", DbType.String, NpgsqlDbType.Varchar, typeof (string))},
          {"character", new TypeEntry("character varying", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"text", new TypeEntry("text", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"bytea", new TypeEntry("bytea", DbType.Binary, NpgsqlDbType.Bytea, typeof (byte[]))},
          {"timestamp without time zone", new TypeEntry("timestamp without time zone", DbType.DateTime, NpgsqlDbType.Timestamp, typeof (DateTime))},
          {"timestamp with time zone", new TypeEntry("timestamp with time zone", DbType.DateTime, NpgsqlDbType.TimestampTZ, typeof (DateTime))},
          {"date", new TypeEntry("date", DbType.Date, NpgsqlDbType.Date, typeof (DateTime))},
          {"time without time zone", new TypeEntry("time without time zone", DbType.Time, NpgsqlDbType.Time, typeof (DateTime))},
          {"time with time zone", new TypeEntry("time with time zone", DbType.Time, NpgsqlDbType.TimeTZ, typeof (DateTime))},
          {"interval", new TypeEntry("interval", DbType.Object, NpgsqlDbType.Interval, typeof (TimeSpan))},
          {"boolean", new TypeEntry("boolean", DbType.Boolean, NpgsqlDbType.Boolean, typeof (bool))},
          {"point", new TypeEntry("point", DbType.Object, NpgsqlDbType.Point, typeof (NpgsqlPoint))},
          {"lseg", new TypeEntry("lseg", DbType.Object, NpgsqlDbType.LSeg, typeof (NpgsqlLSeg))},
          {"box", new TypeEntry("box", DbType.Object, NpgsqlDbType.Box, typeof (NpgsqlBox))},
          {"path", new TypeEntry("path", DbType.Object, NpgsqlDbType.Path, typeof (NpgsqlPath))},
          {"polygon", new TypeEntry("polygon", DbType.Object, NpgsqlDbType.Polygon, typeof (NpgsqlPolygon))},
          {"circle", new TypeEntry("circle", DbType.Object, NpgsqlDbType.Circle, typeof (NpgsqlCircle))},
          {"cidr", new TypeEntry("cidr", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"inet", new TypeEntry("inet", DbType.Object, NpgsqlDbType.Inet, typeof (IPAddress))},
          {"macaddr", new TypeEntry("macaddr", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"bit", new TypeEntry("bit", DbType.Object, NpgsqlDbType.Bit, typeof (BitArray))},
          {"bit varying", new TypeEntry("bit varying", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"tsvector", new TypeEntry("tsvector", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"tsquery", new TypeEntry("tsquery", DbType.String, NpgsqlDbType.Text, typeof (string))},
          {"uuid", new TypeEntry("uuid", DbType.Guid, NpgsqlDbType.Uuid, typeof (Guid))},
          {"oid", new TypeEntry("oid", DbType.Int64, NpgsqlDbType.Bigint, typeof (long))},
          {"ARRAY", new TypeEntry("ARRAY", DbType.Object, NpgsqlDbType.Array, typeof (object))},
          {"USER-DEFINED", new TypeEntry("USER-DEFINED", DbType.Object, NpgsqlDbType.Bytea, typeof (object))},
          {"refcursor", new TypeEntry("refcursor", DbType.Object, NpgsqlDbType.Bytea, typeof (object))}
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