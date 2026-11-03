using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController(AppDbContext context) : ControllerBase
    {
        [HttpGet] // https://localhost:7276/api/members
        public ActionResult<IReadOnlyList<AppUser>> GetMembers()
        {
            return context.Users.ToList();
        }
        
        [HttpGet("{id}")] // https://localhost:7276/api/members/bob-id
        public ActionResult<AppUser> GetMember(string id)
        {
            var member = context.Users.Find(id);

            if (member == null) return NotFound();

            return member;
        }
    }
}
