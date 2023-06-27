namespace NewPortfolio.Models
{
    //Třída pro posílání kreditů mezi uživateli
    public class TransferViewModel
    {

        //Id uživatele, co posílá kredit
        public string SourceUserId { get; set; }
        //Id uživatele, co přijímá kredit
        public string TargetUserId { get; set; }
        //Částka k poslání
        public int Amount { get; set; }
    }
}
