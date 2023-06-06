using NewPortfolio.Migrations;

namespace NewPortfolio.Models
{
    public abstract class CountView : IcountView
    {
        public int Id { get; set; }

        public  DateTime DateOfView { get; set; } = DateTime.Now;

        public  int Count { get; set; } = 0;


        public int CountViewTest() 
        {
            Count++;
            return Count;
        }

    }
}
