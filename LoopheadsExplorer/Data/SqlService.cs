using System.Data;
using System.Data.SqlClient;
using Dapper;
using LoopheadsExplorer.Models;

namespace LoopheadsExplorer.Data
{
    public class SqlService
    {
        private IConfiguration Configuration;

        public SqlService(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public async Task<List<LoopheadSqlData>> GetLoopheadNames(int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadSqlData>
                    ("select * from Names where loopheadnumber = @LoopheadNumber",
                    parameters);
                return result.ToList();
            }
        }

        public async Task<List<LoopheadSqlData>> CheckIfLoopheadNameExists(string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { LoopheadName = _loopheadName.ToUpper(), LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadSqlData>
                    ("select * from Names where upper(loopheadname) = @LoopheadName and loopheadNumber = @LoopheadNumber",
                    parameters);
                return result.ToList();
            }
        }

        public async Task<List<LoopheadSqlData>> CheckIfAddedNameToday(string _clientUUID,  int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { ClientUUID = _clientUUID, LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadSqlData>
                    ("select * from names where clientuuid = @ClientUUID and loopheadnumber = @LoopheadNumber and datesubmitted = CAST(GETDATE() as DATE)",
                    parameters);
                return result.ToList();
            }
        }


        public async Task AddName(string _clientUUID, string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new
                {
                    ClientUUID = _clientUUID,
                    LoopheadName = _loopheadName,
                    LoopheadNumber = _loopheadNumber
                };
                await db.ExecuteAsync("INSERT INTO Names VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName, GETDATE())", parameters);
                await db.ExecuteAsync("INSERT INTO Votes VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName)", parameters);
            }
        }
    }
}

