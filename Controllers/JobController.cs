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
  public class JobController : ControllerBase
  {
    private readonly JobsService _js;

    public JobController(JobsService js)
    {
      _js = js;
    }
    // NOTE path is https://localhost:5001/api/jobs
    [HttpGet]
    public ActionResult<IEnumerable<Job>> GetAll()
    {
      try
      {
        return Ok(_js.GetAll());
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    //NOTE path does not follow standards https://localhost:5001/api/jobs/user
    [HttpGet("user")]
    [Authorize]
    public ActionResult<IEnumerable<Job>> GetJobByUser()
    {
      try
      {
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_js.GetByUserId(userId));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    // NOTE path is https://localhost:5001/api/jobs/id
    [HttpGet("{id}")]
    public ActionResult<Job> GetById(int id)
    {
      try
      {
        return Ok(_js.GetById(id));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public ActionResult<Job> Create([FromBody] Job newJob)
    {
      try
      {
        newJob.UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_js.Create(newJob));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult<Job> Edit(int id, [FromBody] Job jobToUpdate)
    {
      try
      {
        jobToUpdate.Id = id;
        string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        return Ok(_js.Edit(jobToUpdate, userId));
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
        return Ok(_js.Delete(id, userId));
      }
      catch (System.Exception err)
      {
        return BadRequest(err.Message);
      }
    }
  }
}