using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Amount
    {
        public string InputAmount { get; set; }
        public string AmountGas { get; set; }
        public string Comission { get; set; }
        public string ComissionGas { get; set; }
    }
}