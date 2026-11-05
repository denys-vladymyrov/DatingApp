using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MembersController(AppDbContext context) : BaseApiController
{
    [HttpGet] // https://localhost:5001/api/members
    public ActionResult<IReadOnlyList<AppUser>> GetMembers()
    {
        return context.Users.ToList();
    }
    
    [Authorize]
    [HttpGet("{id}")] // https://localhost:5001/api/members/bob-id
    public ActionResult<AppUser> GetMember(string id)
    {
        var member = context.Users.Find(id);

        if (member == null) return NotFound();

        return member;
    }
}

