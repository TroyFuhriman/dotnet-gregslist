using System.Collections.Generic;
using System.Data;
using Dapper;
using fullstack_gregslist.Models;

namespace fullstack_gregslist.Repositories
{
  public class HouseFavoritesRepository
  {
    private readonly IDbConnection _db;

    public HouseFavoritesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal DTOHouseFavorite GetById(int id)
    {
      string sql = "SELECT * FROM housefavorites WHERE id = @id";
      return _db.QueryFirstOrDefault<DTOHouseFavorite>(sql, new { id });
    }
    internal IEnumerable<ViewModelHouseFavorite> GetByUser(string user)
    {
      string sql = @"
            SELECT
            c.*,
            cf.id as favoriteId
            FROM housefavorites cf
            INNER JOIN houses c ON c.id = cf.houseId
            WHERE user = @user;";

      return _db.Query<ViewModelHouseFavorite>(sql, new { user });
    }

    internal bool hasRelationship(DTOHouseFavorite fav)
    {
      string sql = "SELECT * FROM housefavorites WHERE houseId = @HouseId AND user = @User";
      var found = _db.QueryFirstOrDefault<DTOHouseFavorite>(sql, fav);
      return found != null;
    }

    internal DTOHouseFavorite Create(DTOHouseFavorite fav)
    {
      string sql = @"
            INSERT INTO housefavorites
            (user, houseid)
            VALUES
            (@User, @HouseId);
            SELECT LAST_INSERT_ID();
            ";
      fav.Id = _db.ExecuteScalar<int>(sql, fav);
      return fav;
    }
    internal bool Delete(int id, string user)
    {
      string sql = "DELETE FROM housefavorites WHERE id = @id AND user = @user LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id, user });
      return affectedRows == 1;
    }
  }
}