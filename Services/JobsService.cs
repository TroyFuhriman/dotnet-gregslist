using System;
using System.Collections.Generic;
using fullstack_gregslist.Models;
using fullstack_gregslist.Repositories;

namespace fullstack_gregslist.Services
{
  public class JobsService
  {
    private readonly JobsRepository _repo;

    public JobsService(JobsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Job> GetAll()
    {
      return _repo.GetAll();
    }

    internal IEnumerable<Job> GetByUserId(string userId)
    {
      return _repo.GetJobsByUserId(userId);
    }

    public Job GetById(int id)
    {
      Job foundJob = _repo.GetById(id);
      if (foundJob == null)
      {
        throw new Exception("Invalid Id");
      }
      return foundJob;
    }

    internal Job Create(Job newJob)
    {
      return _repo.Create(newJob);
    }

    internal Job Edit(Job jobToUpdate, string userId)
    {
      Job foundJob = GetById(jobToUpdate.Id);
      // NOTE Check if not the owner, and price is increasing
      if (foundJob.UserId != userId && foundJob.Price < jobToUpdate.Price)
      {
        if (_repo.BidOnJob(jobToUpdate))
        {
          foundJob.Price = jobToUpdate.Price;
          return foundJob;
        }
        throw new Exception("Could not bid on that job");
      }
      if (foundJob.UserId == userId && _repo.Edit(jobToUpdate, userId))
      {
        return jobToUpdate;
      }
      throw new Exception("You cant edit that, it is not yo job!");
    }

    internal string Delete(int id, string userId)
    {
      Job foundJob = GetById(id);
      if (foundJob.UserId != userId)
      {
        throw new Exception("This is not your job!");
      }
      if (_repo.Delete(id, userId))
      {
        return "Sucessfully delorted.";
      }
      throw new Exception("Somethin bad happened");
    }
  }
}