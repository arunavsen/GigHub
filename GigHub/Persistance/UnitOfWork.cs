using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using GigHub.Models;
using GigHub.Repository;

namespace GigHub.Persistance
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public GigRepository Gigs { get; private set; }

        public AttendenceRepository Attendence { get; private set; }

        public GenreRepository Genre { get; private set; }

        public FollowRepository Follow { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(context);
        }
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}