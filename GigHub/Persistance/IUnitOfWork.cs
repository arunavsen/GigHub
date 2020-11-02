using GigHub.Repository;

namespace GigHub.Persistance
{
    public interface IUnitOfWork
    {
        IGigRepository Gigs { get; }
        IAttendenceRepository Attendence { get; }
        IGenreRepository Genre { get; }
        IFollowRepository Follow { get; }
        void Complete();
    }
}