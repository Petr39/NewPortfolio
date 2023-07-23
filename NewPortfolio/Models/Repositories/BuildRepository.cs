using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewPortfolio.Data;
using NewPortfolio.Interfaces;

namespace NewPortfolio.Models.Repositories
{
   
    public class BuildRepository : IBuild
    {

        private readonly ApplicationDbContext context;


        public BuildRepository(ApplicationDbContext context)
        {
                this.context = context; 
        }


        public void Create(Build build)
        {
            context.Add(build);
            
        }

        public void Delete(int id)
        {
            context.Remove(id);
        }

        public void Details(int id)
        {
            context.Builds.FindAsync(id);
        }

        public void Save()
        {
            context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Build>> ShowAll(int id)
        {
            var applicationDbContext = context.Builds.Where(c => c.BuildPostId == id);
            return await applicationDbContext.ToListAsync();
        }

       

        public void Update(Build build)
        {
            context.Update(build);
        }
    }
}
