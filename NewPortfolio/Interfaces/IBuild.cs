using Microsoft.AspNetCore.Mvc;
using NewPortfolio.Models;


namespace NewPortfolio.Interfaces
{
    public interface IBuild
    {
        Task <IEnumerable<Build>> ShowAll(int id);

        void Create(Build build);

        void Update(Build build);

        void Details(int id);

        void Delete(int id);

        void Save();
    }
}
