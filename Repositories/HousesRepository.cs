using System.Collections.Generic;
using System.Data;
using Dapper;
using fullstack_gregslist.Models;

namespace fullstack_gregslist.Repositories
{
  public class HousesRepository
  {
    private readonly IDbConnection _db;

    public HousesRepository(IDbConnection db)
    {
      _db = db;
    }
    internal IEnumerable<House> GetHousesByUserId(string userId)
    {
      string sql = "SELECT * FROM houses WHERE userId = @userId";
      return _db.Query<House>(sql, new { userId });
    }

    internal House GetById(int id)
    {
      string sql = "SELECT * FROM houses WHERE id = @id";
      return _db.QueryFirstOrDefault<House>(sql, new { id });
    }

    internal IEnumerable<House> GetAll()
    {
      string sql = "SELECT * FROM houses";
      return _db.Query<House>(sql);
    }

    internal House Create(House newHouse)
    {
      string sql = @"
            INSERT INTO houses
            (userId, year, price, body, imgUrl, name)
            VALUES
            (@UserId, @Year, @Price, @Body, @ImgUrl, @Name);
            SELECT LAST_INSERT_ID()";
      newHouse.Id = _db.ExecuteScalar<int>(sql, newHouse);
      return newHouse;
    }

    internal bool BidOnHouse(House houseToBidOn)
    {
      string sql = @"
            UPDATE houses
            SET
            price = @Price
            WHERE id = @Id";
      int affectedRows = _db.Execute(sql, houseToBidOn);
      return affectedRows == 1;
    }

    internal bool Edit(House houseToUpdate, string userId)
    {
      houseToUpdate.UserId = userId;
      string sql = @"
            UPDATE houses
            SET
                price = @Price,
                name = @Name,
                imgUrl = @ImgUrl,
                year = @Year,
                body = @Body
            WHERE id = @Id
            AND userId = @UserId";
      int affectedRows = _db.Execute(sql, houseToUpdate);
      return affectedRows == 1;
    }

    internal bool Delete(int id, string userId)
    {
      string sql = "DELETE FROM houses WHERE id = @id AND userId = @userId LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id, userId });
      return affectedRows == 1;
    }
  }
}