using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;
using Simple.Data.Ado;

namespace Simple.Data.PostgreSql
{
  [Export(typeof(IBulkInserter))]
  public class PgBulkInserter : IBulkInserter
  {
    public IEnumerable<IDictionary<string, object>> Insert(AdoAdapter adapter, string tableName, IEnumerable<IDictionary<string, object>> data, IDbTransaction transaction)
    {
      throw new NotImplementedException();
    }
  }
}
