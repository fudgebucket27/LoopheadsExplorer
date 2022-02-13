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

        public async Task<List<LoopheadNameClientVotesSqlData>> CheckIfVoteExists(string _clientUUID, string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { ClientUUID = _clientUUID, LoopheadName = _loopheadName.ToUpper(), LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadNameClientVotesSqlData>
                    ("select clientuuid, loopheadnumber, loopheadname, count(loopheadname) as votes from Votes where loopheadnumber = @LoopheadNumber and upper(loopheadname) = @LoopheadName and clientuuid = @ClientUUID group by clientuuid, loopheadnumber,loopheadname;",
                    parameters);
                return result.ToList();
            }
        }

        public async Task<List<LoopheadNameVotesSqlData>> CheckIfClientAddedNameAlready(string _clientUUID,  int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var parameters = new { ClientUUID = _clientUUID, LoopheadNumber = _loopheadNumber };
                var result = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
                    ("select * from names where clientuuid = @ClientUUID and loopheadnumber = @LoopheadNumber",
                    parameters);
                return result.ToList();
            }
        }


        public async Task AddName(string _clientUUID, string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var insertParameters = new
                {
                    ClientUUID = _clientUUID,
                    LoopheadName = _loopheadName,
                    LoopheadNumber = _loopheadNumber
                };
                var selectNameExistsParameters = new
                {
                    LoopheadName = _loopheadName.ToUpper(),
                    LoopheadNumber = _loopheadNumber
                };
                var nameExists = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
                    ("select * from Names where upper(loopheadname) = @LoopheadName and loopheadNumber = @LoopheadNumber",
                    selectNameExistsParameters);
                var nameExistsResult = nameExists.ToList();
                if (nameExistsResult.Count == 0)
                {
                    await db.ExecuteAsync("INSERT INTO Names VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName, GETDATE())", insertParameters);
                    await db.ExecuteAsync("INSERT INTO Votes VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName)", insertParameters);
                }
            }
        }

        public async Task AddVote(string _clientUUID, string _loopheadName, int _loopheadNumber)
        {
            using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString("DB")))
            {
                db.Open();
                var insertParameters = new
                {
                    ClientUUID = _clientUUID,
                    LoopheadName = _loopheadName,
                    LoopheadNumber = _loopheadNumber
                };

                var selectVoteExistsParameters = 
                    new { ClientUUID = _clientUUID, 
                        LoopheadName = _loopheadName.ToUpper(), 
                        LoopheadNumber = _loopheadNumber };
                var selectVoteExists = await db
                    .QueryAsync<LoopheadNameClientVotesSqlData>
                    ("select clientuuid, loopheadnumber, loopheadname, count(loopheadname) as votes from Votes where loopheadnumber = @LoopheadNumber and upper(loopheadname) = @LoopheadName and clientuuid = @ClientUUID group by clientuuid, loopheadnumber,loopheadname;",
                    selectVoteExistsParameters);
                var selectVoteExistsResult = selectVoteExists.ToList();

                var selectIfClientAddedNameParameters = new { ClientUUID = _clientUUID, LoopheadNumber = _loopheadNumber };
                var selectIfClientAddedNameExists = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
                    ("select * from names where clientuuid = @ClientUUID and loopheadnumber = @LoopheadNumber",
                    selectIfClientAddedNameParameters);
                var selectIfClientAddedNameResult = selectIfClientAddedNameExists.ToList();

                var selectNameExistsParameters = new
                {
                    LoopheadName = _loopheadName.ToUpper(),
                    LoopheadNumber = _loopheadNumber
                };
                var selectNameExists = await db
                    .QueryAsync<LoopheadNameVotesSqlData>
                    ("select * from Names where upper(loopheadname) = @LoopheadName and loopheadNumber = @LoopheadNumber",
                     selectNameExistsParameters);
                var selectNameExistsResult = selectNameExists.ToList();

                if (selectVoteExistsResult.Count == 0 && selectNameExistsResult.Count > 0)
                {
                    await db.ExecuteAsync("INSERT INTO Votes VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName)", insertParameters);
                }
                else if (selectVoteExistsResult.Count == 0 && selectIfClientAddedNameResult.Count == 0 && selectNameExistsResult.Count == 0)
                {
                    await db.ExecuteAsync("INSERT INTO Names VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName, GETDATE())", insertParameters);
                    await db.ExecuteAsync("INSERT INTO Votes VALUES (@ClientUUID, @LoopheadNumber, @LoopheadName)", insertParameters);
                }
            }
        }

        public async Task RemoveVote(string _clientUUID, string _loopheadName, int _loopheadNumber)
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
                await db.ExecuteAsync("delete from Votes where clientUUID = @ClientUUID and loopheadNumber = @LoopheadNumber and loopheadName =  @LoopheadName", parameters);
                var queryVotesResult = await db
                      .QueryAsync<LoopheadNameVotesSqlData>
                      ("select * from votes where loopheadname = @LoopheadName and loopheadnumber = @LoopheadNumber",
                      parameters);
                if(queryVotesResult.ToList().Count == 0)
                {
                    await db.ExecuteAsync("delete from names where loopheadname = @LoopheadName and loopheadnumber = @LoopheadNumber", parameters);
                }
            }
        }
    }
}

