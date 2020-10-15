using System.Linq;
using System.Web.Http;
using GigHub.DTOs;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class FollowingsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public FollowingsController()
        {
            _context = new ApplicationDbContext();
        }

        

        [HttpPost]
        public IHttpActionResult Follow(FollowingDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exist = _context.Followings.Any(u => u.FollowerId == userId && u.FolloweeId == dto.FolloweeId);
            if (exist)
            {
                return BadRequest("Already Followed.");
            }
            var follow = new Following()
            {
                FollowerId = userId,
                FolloweeId = dto.FolloweeId
            };

            _context.Followings.Add(follow);
            _context.SaveChanges();
            return Ok();
        }
    }
}
