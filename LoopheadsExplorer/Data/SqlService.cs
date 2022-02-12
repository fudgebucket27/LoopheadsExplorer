using System.Data;
using System.Data.SqlClient;
using Dapper;
using LoopheadsExplorer.Models;

namespace LoopheadsExplorer.Data
{
    public class SqlService
    {
        public async Task<List<LoopheadSqlData>> GetLoopheadNames(int loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DB")))
            {
                db.Open();
                var result = await db
                    .QueryAsync<LoopheadSqlData>
                    ("select * from names");
                return result.ToList();
            }
        }
    }
}
