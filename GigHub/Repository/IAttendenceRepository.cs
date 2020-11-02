using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repository
{
    public interface IAttendenceRepository
    {
        IEnumerable<Attendence> FutureAttendences(string user);
    }
}