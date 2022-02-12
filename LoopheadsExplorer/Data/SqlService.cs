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
        public async Task<List<LoopheadNameVotesSqlData>> GetLoopheadVotes(int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
                    ("select loopheadnumber, loopheadname, count(loopheadname) as votes from Votes where loopheadnumber = @LoopheadNumber group by loopheadnumber,loopheadname order by count(loopheadname) desc",
                    parameters);
                return result.ToList();
            }
        }

        public async Task<List<LoopheadNameVotesSqlData>> CheckIfLoopheadNameExists(string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { LoopheadName = _loopheadName.ToUpper(), LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
                    ("select * from Names where upper(loopheadname) = @LoopheadName and loopheadNumber = @LoopheadNumber",
                    parameters);
                return result.ToList();
            }
        }

        public async Task<List<LoopheadNameVotesSqlData>> CheckIfAddedNameToday(string _clientUUID,  int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { ClientUUID = _clientUUID, LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
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

