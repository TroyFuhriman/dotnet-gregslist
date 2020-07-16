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
  public class HouseController : ControllerBase
  {
    private readonly HousesService _hs;

    public HouseController(HousesService hs)
    {
      _hs = hs;
    }
    // NOTE path is https://localhost:5001/api/house
    [HttpGet]
    public ActionResult<IEnumerable<House>> GetAll()
    {
      try
      {
        return Ok(_hs.GetAll());
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    //NOTE path does not follow standards https://localhost:5001/api/house/user
    [HttpGet("user")]
    [Authorize]
    public ActionResult<IEnumerable<House>> GetHouseByUser()
    {
      try
      {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hs.GetByUserId(userId));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    // NOTE path is https://localhost:5001/api/house/id
    [HttpGet("{id}")]
    public ActionResult<House> GetById(int id)
    {
      try
      {
        return Ok(_hs.GetById(id));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public ActionResult<House> Create([FromBody] House newHouse)
    {
      try
      {
        newHouse.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hs.Create(newHouse));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult<House> Edit(int id, [FromBody] House houseToUpdate)
    {
      try
      {
        houseToUpdate.Id = id;
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hs.Edit(houseToUpdate, userId));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }



    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult<string> Delete(int id)
    {
      try
      {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_hs.Delete(id, userId));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}