using NewPortfolio.Data;
using NewPortfolio.Interfaces;
using NewPortfolio.Models;

namespace NewPortfolio.Repositories
{
    public class GenreRepository : IGenre
    {

        private readonly ApplicationDbContext _db;


        public GenreRepository(ApplicationDbContext db)
        {
            _db = db;   
        }
        public void Create(Genre obj)
        {
            _db.Genres.Add(obj);
            _db.SaveChanges();
        }

        public IEnumerable<Genre> GetAll()
        {
            return _db.Genres.ToList();
        }
    }
}
