using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.Repository
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}