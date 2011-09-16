using System.Collections.Generic;
using System.Data;

namespace Simple.Data.PostgreSql
{
  public class DbTypeLookup
  {
    public static DbType GetDbType(string typeName)
    {
      return DbTypeMap[typeName];
    }

    private static readonly Dictionary<string, DbType> DbTypeMap = new Dictionary<string, DbType>
                                                                     {
                                                                       {"character varying", DbType.String},
                                                                       {"integer", DbType.Int32}
                                                                     };
  }
}