using Microsoft.EntityFrameworkCore;
using NewPortfolio.Data;
using NewPortfolio.Interfaces;
using NewPortfolio.Models;
using System.Linq.Expressions;

namespace NewPortfolio.Repositories
{
    public class GameRepository : IGame
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<object> _db;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
               _db=_context.Set<object>();
        }

        public async Task Create(Game game)
        {
            await _context.AddAsync(game);
            
        }

        public void Delete(int id)
        {
            var game= Find(id);
            _context.Remove(game);
            _context.SaveChanges();
        }

        public Game Find(int id)
        {
            var game = _context.Games.FirstOrDefault(g=>g.Id==id);
            if(game!=null)
                return game;
            return null;
        }

        public IEnumerable<Game> GetAll()
        {
            var gamesList=_context.Games.ToList();
            return gamesList;
        }

        public void Save(Game game)
        {
            _context.SaveChanges();
        }

      

        public async Task Update(Game game)
        {
            _context.Update(game);
            await _context.SaveChangesAsync();
          
        }

        
    }
}
