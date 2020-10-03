﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GigHub.DTOs;
using GigHub.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace GigHub.Controllers
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
    }
}
