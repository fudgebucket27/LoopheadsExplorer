using System.Data;
using System.Data.SqlClient;
using Dapper;
using LoopheadsExplorer.Models;

namespace LoopheadsExplorer.Data
{
    public class SqlService
    {
        public async Task<List<LoopheadSqlData>> GetLoopheadNames(int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DB", EnvironmentVariableTarget.Machine)))
            {
                db.Open();
                var parameters = new { LoopheadNumber = _loopheadNumber  };
                var result = await db
                    .QueryAsync<LoopheadSqlData>
                    ("select * from Names where loopheadnumber = @LoopheadNumber",
                    parameters);
                return result.ToList();
            }
        }
    }
}
