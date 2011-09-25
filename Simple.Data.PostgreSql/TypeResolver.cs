using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Simple.Data.PostgreSql
{
  public class TypeResolver
  {
    public static NpgsqlDbType GetDbType(string typeName)
    {
      if (!TypeMap.ContainsKey(typeName))
      {
        return NpgsqlDbType.Bytea; // Typically a user-defined type
      }
      return TypeMap[typeName].DbType;
    }

    public static Type GetClrType(string typeName)
    {
      if (!TypeMap.ContainsKey(typeName))
      {
        return typeof(byte[]); // Typically a user-defined type
      }
      return TypeMap[typeName].ClrType;
    }

    private static readonly Dictionary<string, TypeEntry> TypeMap = new Dictionary<string, TypeEntry>
                                                                           {
                                                                             {"ARRAY", new TypeEntry(NpgsqlDbType.Array, null)},
                                                                             {"bigint", new TypeEntry(NpgsqlDbType.Bigint, typeof(long))},
                                                                             {"bit", new TypeEntry(NpgsqlDbType.Bit, null)},
                                                                             {"bit varying", new TypeEntry(NpgsqlDbType.Bit, null)},
                                                                             {"boolean", new TypeEntry(NpgsqlDbType.Boolean, typeof(bool))},
                                                                             {"box", new TypeEntry(NpgsqlDbType.Box, typeof(NpgsqlBox))}, // TODO: Not sure how these Npgsql types will work.  Write some tests.
                                                                             {"bytea", new TypeEntry(NpgsqlDbType.Bytea, typeof(byte[]))},
                                                                             {"char", new TypeEntry(NpgsqlDbType.Char, typeof(char))},
                                                                             {"character", new TypeEntry(NpgsqlDbType.Char, typeof(char))},
                                                                             {"character varying", new TypeEntry(NpgsqlDbType.Varchar, typeof(string))},
                                                                             {"cidr", new TypeEntry(NpgsqlDbType.Inet, typeof(string))},
                                                                             {"circle", new TypeEntry(NpgsqlDbType.Circle, typeof(NpgsqlCircle))},
                                                                             {"decimal", new TypeEntry(NpgsqlDbType.Numeric, typeof(double))},
                                                                             {"double precision", new TypeEntry(NpgsqlDbType.Double, typeof(double))},
                                                                             {"inet", new TypeEntry(NpgsqlDbType.Inet, typeof(string))},
                                                                             {"integer", new TypeEntry(NpgsqlDbType.Integer, typeof(int))},
                                                                             {"interval", new TypeEntry(NpgsqlDbType.Interval, typeof(TimeSpan))},
                                                                             {"line", new TypeEntry(NpgsqlDbType.Line, typeof(NpgsqlPath))},
                                                                             {"lseg", new TypeEntry(NpgsqlDbType.LSeg, typeof(NpgsqlLSeg))},
                                                                             {"macaddr", new TypeEntry(NpgsqlDbType.Bytea, typeof(byte[]))}, // TODO: May be a better type
                                                                             {"money", new TypeEntry(NpgsqlDbType.Money, typeof(string))},
                                                                             {"numeric", new TypeEntry(NpgsqlDbType.Numeric, typeof(double))},
                                                                             {"oid", new TypeEntry(NpgsqlDbType.Oidvector, typeof(int))}, // TODO: not sure about this one
                                                                             {"point", new TypeEntry(NpgsqlDbType.Point, typeof(NpgsqlPoint))},
                                                                             {"polygon", new TypeEntry(NpgsqlDbType.Polygon, typeof(NpgsqlPolygon))},
                                                                             {"path", new TypeEntry(NpgsqlDbType.Path, typeof(NpgsqlPath))},
                                                                             {"real", new TypeEntry(NpgsqlDbType.Real, typeof(float))},
                                                                             {"smallint", new TypeEntry(NpgsqlDbType.Smallint, typeof(short))},
                                                                             {"text", new TypeEntry(NpgsqlDbType.Text, typeof(string))},
                                                                             {"time", new TypeEntry(NpgsqlDbType.Time, typeof(DateTime))},
                                                                             {"time with time zone", new TypeEntry(NpgsqlDbType.TimeTZ, typeof(DateTime))},
                                                                             {"time without time zone", new TypeEntry(NpgsqlDbType.Time, typeof(DateTime))},
                                                                             {"timestamp", new TypeEntry(NpgsqlDbType.Timestamp, typeof(DateTime))},
                                                                             {"timestamp with time zone", new TypeEntry(NpgsqlDbType.TimestampTZ, typeof(DateTime))},
                                                                             {"timestamp without time zone", new TypeEntry(NpgsqlDbType.Timestamp, typeof(DateTime))},
                                                                             {"uuid", new TypeEntry(NpgsqlDbType.Uuid, typeof(Guid))},
                                                                             {"varchar", new TypeEntry(NpgsqlDbType.Varchar, typeof(string))}
                                                                           };

    private class TypeEntry
    {
      public TypeEntry(NpgsqlDbType dbType, Type clrType)
      {
        DbType = dbType;
        ClrType = clrType;
      }

      public NpgsqlDbType DbType { get; private set; }
      public Type ClrType { get; private set; }
    }
  }

  
}