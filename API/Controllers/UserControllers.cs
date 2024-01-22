using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // https..../api/users
public class UsersController : ControllerBase
{
private readonly DataContext _context;

    //ctor
    public UsersController(DataContext context)
    {
        context = _context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers(){
        var users = _context.Users.ToList();
        return users;
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int id)
    {
        return _context.Users.Find(id);
    }
}