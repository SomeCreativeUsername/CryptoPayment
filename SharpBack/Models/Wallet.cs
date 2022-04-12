using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Wallet
    {
        [Key]
        public string Address { get; set; }
        [Required]
        public string PrivateKey { get; set; }

        public string InputAmount { get; set; }
        public string AmountGas { get; set; }
        public string Comission { get; set; }
        public string ComissionGas { get; set; }
        public string TransactionHash { get; set; }
    }
}
