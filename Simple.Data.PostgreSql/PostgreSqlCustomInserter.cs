using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using Simple.Data.Ado;
using Simple.Data.Ado.Schema;

namespace Simple.Data.PostgreSql
{
  [Export(typeof(ICustomInserter))]
  public class PostgreSqlCustomInserter : ICustomInserter
  {
    public IDictionary<string, object> Insert(AdoAdapter adapter, string tableName, IDictionary<string, object> data, IDbTransaction transaction)
    {
      var table = DatabaseSchema.Get(adapter.ConnectionProvider, adapter.ProviderHelper).FindTable(tableName);
      if(table == null) throw new SimpleDataException(String.Format("Table '{0}' not found", tableName));


      var insertableData = data.Where(p => table.HasColumn(p.Key));


      return null;  

    }
  }
}
