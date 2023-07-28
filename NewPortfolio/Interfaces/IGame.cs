using NewPortfolio.Models;
using NewPortfolio.Repositories;
using System.Linq.Expressions;

namespace NewPortfolio.Interfaces
{
    public interface IGame
    {
        IEnumerable <Game> GetAll();

        Game Find(int id);

        Task Create(Game game);

        Task Update(Game game);

        void Delete(int id);

        void Save(Game game);

  
        
    }
}
