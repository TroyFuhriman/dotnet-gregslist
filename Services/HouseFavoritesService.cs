using System;
using System.Collections.Generic;
using fullstack_gregslist.Models;
using fullstack_gregslist.Repositories;

namespace fullstack_gregslist.Services
{
  public class HouseFavoritesService
  {
    private readonly HouseFavoritesRepository _repo;

    public HouseFavoritesService(HouseFavoritesRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<ViewModelHouseFavorite> Get(string user)
    {
      return _repo.GetByUser(user);
    }

    internal DTOHouseFavorite Create(DTOHouseFavorite fav)
    {
      if (_repo.hasRelationship(fav))
      {
        throw new Exception("you already have that fav");
      }
      return _repo.Create(fav);
    }
    private DTOHouseFavorite GetById(int id)
    {
      var found = _repo.GetById(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

    internal DTOHouseFavorite Delete(string user, int id)
    {
      var found = GetById(id);
      if (found.User != user)
      {
        throw new UnauthorizedAccessException("This is not your favorite!");
      }
      if (_repo.Delete(id, user))
      {
        return found;
      }
      throw new Exception("Somethin bad happened");
    }

  }
}