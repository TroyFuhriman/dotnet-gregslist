using System.Collections.Generic;
using System.Security.Claims;
using fullstack_gregslist.Models;
using fullstack_gregslist.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fullstack_gregslist.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class HouseFavoritesController : ControllerBase
  {
    private readonly HouseFavoritesService _hfs;

    public HouseFavoritesController(HouseFavoritesService hfs)
    {
      _hfs = hfs;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ViewModelHouseFavorite>> Get()
    {
      try
      {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hfs.Get(userId));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPost]
    public ActionResult<DTOHouseFavorite> Create([FromBody] DTOHouseFavorite fav)
    {
      try
      {
        fav.User = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hfs.Create(fav));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpDelete("{Id}")]
    public ActionResult<DTOHouseFavorite> Delete(int Id)
    {
      try
      {
        string user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hfs.Delete(user, Id));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}