namespace NewPortfolio.Models
{
    //Třída pro posílání kreditů mezi uživateli
    public class TransferViewModel
    {

        /// <summary>
        /// Id uživatele, co posílá kredit
        /// </summary>
        public string SourceUserId { get; set; }
        /// <summary>
        /// Id uživatele, co přijímá kredit
        /// </summary>
        public string TargetUserId { get; set; }
        /// <summary>
        /// Částka k poslání
        /// </summary>
        public int Amount { get; set; }
    }
}
