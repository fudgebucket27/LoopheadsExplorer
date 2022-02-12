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
                    ("select * from LoopheadNames where loopheadnumber = @LoopheadNumber",
                    parameters);
                return result.ToList();
            }
        }

        public async Task AddName(string _clientUUID, string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_DB", EnvironmentVariableTarget.Machine)))
            {
                db.Open();
                var parameters = new
                {
                    ClientUUID = _clientUUID,
                    LoopheadName = _loopheadName,
                    LoopheadNumber = _loopheadNumber
                };
                await db.ExecuteAsync("INSERT INTO LoopheadNames VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName, GETDATE())", parameters);
                await db.ExecuteAsync("INSERT INTO LoopheadVotes VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName)", parameters);
            }
        }
    }
}
