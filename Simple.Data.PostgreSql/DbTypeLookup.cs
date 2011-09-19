using System.Collections.Generic;
using System.Data;
using NpgsqlTypes;

namespace Simple.Data.PostgreSql
{
  public class DbTypeLookup
  {
    public static NpgsqlDbType GetDbType(string typeName)
    {
      return DbTypeMap[typeName];
    }

    private static readonly Dictionary<string, NpgsqlDbType> DbTypeMap = new Dictionary<string, NpgsqlDbType>
                                                                     {
                                                                       {"smallint", NpgsqlDbType.Smallint},
                                                                       {"integer", NpgsqlDbType.Integer},
                                                                       {"bigint", NpgsqlDbType.Bigint},
                                                                       {"decimal", NpgsqlDbType.Numeric},
                                                                       {"numeric", NpgsqlDbType.Numeric},
                                                                       {"real", NpgsqlDbType.Real},
                                                                       {"double precision", NpgsqlDbType.Double},
                                                                       {"money", NpgsqlDbType.Money},
                                                                       {"character varying", NpgsqlDbType.Varchar},
                                                                       {"varchar", NpgsqlDbType.Varchar},
                                                                       {"character", NpgsqlDbType.Char},
                                                                       {"char", NpgsqlDbType.Char},
                                                                       {"text", NpgsqlDbType.Text},
                                                                       {"bytea", NpgsqlDbType.Bytea},
                                                                       {"timestamp", NpgsqlDbType.Timestamp},
                                                                       {"timestamp without time zone", NpgsqlDbType.Timestamp},
                                                                       {"timestamp with time zone", NpgsqlDbType.TimestampTZ},
                                                                       {"time", NpgsqlDbType.Time},
                                                                       {"time without time zone", NpgsqlDbType.Time},
                                                                       {"time with time zone", NpgsqlDbType.TimeTZ},
                                                                       {"interval", NpgsqlDbType.Interval},
                                                                       {"boolean", NpgsqlDbType.Boolean},
                                                                       {"point", NpgsqlDbType.Point},
                                                                       {"line", NpgsqlDbType.Line},
                                                                       {"lseg", NpgsqlDbType.LSeg},
                                                                       {"box", NpgsqlDbType.Box},
                                                                       {"path", NpgsqlDbType.Path},
                                                                       {"polygon", NpgsqlDbType.Polygon},
                                                                       {"Circle", NpgsqlDbType.Circle},
                                                                       {"cidr", NpgsqlDbType.Inet},
                                                                       {"inet", NpgsqlDbType.Inet},
                                                                       {"macaddr", NpgsqlDbType.Bytea},  // TODO: May be a better type
                                                                       {"bit", NpgsqlDbType.Bit},
                                                                       {"bit varying", NpgsqlDbType.Bit},
                                                                       {"uuid", NpgsqlDbType.Uuid},
                                                                       {"oid", NpgsqlDbType.Oidvector} // TODO: not sure about this one
                                                                       // TODO: arrays?
                                                                     };
  }
}