using System.Linq;
using System.Web.Http;
using GigHub.DTOs;
using GigHub.Models;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendencesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendencesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendenceDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exist = _context.Attendences.Any(u => u.AttendeeId == userId && u.GigId == dto.GigId);
            if (exist)
            {
                return BadRequest("The attendance is already exist.");
            }
            var attendee = new Attendence()
            {
                GigId = dto.GigId,
                AttendeeId = userId
            };

            _context.Attendences.Add(attendee);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteAttendence(int id)
        {
            var user = User.Identity.GetUserId();

            var attendences = _context.Attendences
                .SingleOrDefault(m => m.AttendeeId == user && m.GigId == id);

            if (attendences == null)
            {
                return NotFound();
            }

            _context.Attendences.Remove(attendences);
            _context.SaveChanges();

            return Ok();
        }
    }
}
