using System.Collections.Generic;
using System.Data;
using Dapper;
using fullstack_gregslist.Models;

namespace fullstack_gregslist.Repositories
{
  public class JobsRepository
  {
    private readonly IDbConnection _db;

    public JobsRepository(IDbConnection db)
    {
      _db = db;
    }
    internal IEnumerable<Job> GetJobsByUserId(string userId)
    {
      string sql = "SELECT * FROM jobs WHERE userId = @userId";
      return _db.Query<Job>(sql, new { userId });
    }

    internal Job GetById(int id)
    {
      string sql = "SELECT * FROM jobs WHERE id = @id";
      return _db.QueryFirstOrDefault<Job>(sql, new { id });
    }

    internal IEnumerable<Job> GetAll()
    {
      string sql = "SELECT * FROM jobs";
      return _db.Query<Job>(sql);
    }

    internal Job Create(Job newJob)
    {
      string sql = @"
            INSERT INTO jobs
            (userId, price, body, title, price)
            VALUES
            (@UserId, @Price, @Body, @Title, @Price);
            SELECT LAST_INSERT_ID()";
      newJob.Id = _db.ExecuteScalar<int>(sql, newJob);
      return newJob;
    }

    internal bool BidOnJob(Job jobToBidOn)
    {
      string sql = @"
            UPDATE jobs
            SET
            price = @Price
            WHERE id = @Id";
      int affectedRows = _db.Execute(sql, jobToBidOn);
      return affectedRows == 1;
    }

    internal bool Edit(Job jobToUpdate, string userId)
    {
      jobToUpdate.UserId = userId;
      string sql = @"
            UPDATE jobs
            SET
                price = @Price,
                title = @Title
                body = @Body
            WHERE id = @Id
            AND userId = @UserId";
      int affectedRows = _db.Execute(sql, jobToUpdate);
      return affectedRows == 1;
    }

    internal bool Delete(int id, string userId)
    {
      string sql = "DELETE FROM jobs WHERE id = @id AND userId = @userId LIMIT 1";
      int affectedRows = _db.Execute(sql, new { id, userId });
      return affectedRows == 1;
    }
  }
}