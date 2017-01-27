using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

using Simple.Data.Ado;

namespace Simple.Data.Npgsql
{
  [Export(typeof(IQueryPager))]
  public class PgQueryPager : IQueryPager
  {
    public IEnumerable<string> ApplyLimit(string sql, int take)
    {
      var builder = new StringBuilder(sql);
      builder.AppendFormat(" LIMIT {0}", take);

      yield return builder.ToString();
    }

    public IEnumerable<string> ApplyPaging(string sql, string[] keys, int skip, int take)
      {
      var builder = new StringBuilder(sql);
      builder.AppendFormat(" LIMIT {0} OFFSET {1}", take, skip);

      yield return builder.ToString();
    }
  }
}
