using Microsoft.AspNetCore.Mvc;
using NewPortfolio.Models;

namespace NewPortfolio.Repositories
{
    public interface IArticlePost
    {
       public Task<IEnumerable<Article>> Index();      

        public Task<IActionResult> Details(int? id);

        public Task<IActionResult> Create([Bind("Id,Content,Title,Description")] CreatePostVM article);

        public Task<IActionResult> Edit(int? id);

        public Task<IActionResult> Edit(int id, [Bind("Id,Content,Title,Description,AppUserId, NickName")] Article article);

        public Task<IActionResult> Delete(int? id);

        public Task<IActionResult> DeleteConfirmed(int id);

    }
}
