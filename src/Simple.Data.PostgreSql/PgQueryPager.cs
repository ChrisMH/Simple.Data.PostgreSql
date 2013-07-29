using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Simple.Data.Ado;

namespace Simple.Data.PostgreSql
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
      builder.AppendFormat(" LIMIT {0}, {1}", skip, take);

      yield return builder.ToString();
    }
  }
}
