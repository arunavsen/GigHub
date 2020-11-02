using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using GigHub.Models;
using GigHub.Repository;

namespace GigHub.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGigRepository Gigs { get; private set; }

        public IAttendenceRepository Attendence { get; private set; }

        public IGenreRepository Genre { get; private set; }

        public IFollowRepository Follow { get; private set; }

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