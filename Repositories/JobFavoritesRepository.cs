using System.Collections.Generic;
using System.Data;
using Dapper;
using fullstack_gregslist.Models;

namespace fullstack_gregslist.Repositories
{
  public class JobFavoritesRepository
  {
    private readonly IDbConnection _db;

    public JobFavoritesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal DTOJobFavorite GetById(int id)
    {
      string sql = "SELECT * FROM jobfavorites WHERE id = @id";
      return _db.QueryFirstOrDefault<DTOJobFavorite>(sql, new { id });
    }
    internal IEnumerable<ViewModelJobFavorite> GetByUser(string user)
    {
      string sql = @"
            SELECT
            c.*,
            cf.id as favoriteId
            FROM jobfavorites cf
            INNER JOIN jobs c ON c.id = cf.jobId
            WHERE user = @user;";

      return _db.Query<ViewModelJobFavorite>(sql, new { user });
    }

    internal bool hasRelationship(DTOJobFavorite fav)
    {
      string sql = "SELECT * FROM jobfavorites WHERE jobId = @JobId AND user = @User";
      var found = _db.QueryFirstOrDefault<DTOJobFavorite>(sql, fav);
      return found != null;
    }

    internal DTOJobFavorite Create(DTOJobFavorite fav)
    {
      string sql = @"
            INSERT INTO jobfavorites
            (user, jobid)
            VALUES
            (@User, @JobId);
            SELECT LAST_INSERT_ID();
            ";
      fav.Id = _db.ExecuteScalar<int>(sql, fav);
      return fav;
    }
    internal bool Delete(int id, string user)
    {
      string sql = "DELETE FROM jobfavorites WHERE id = @id AND user = @user LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id, user });
      return affectedRows == 1;
    }
  }
}