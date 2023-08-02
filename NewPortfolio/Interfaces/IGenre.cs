using NewPortfolio.Models;

namespace NewPortfolio.Interfaces
{
    public interface IGenre
    {
        void Create(Genre obj);

        IEnumerable<Genre> GetAll();
    }
}
