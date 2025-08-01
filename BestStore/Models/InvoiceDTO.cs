using System.ComponentModel.DataAnnotations;

namespace BestStore.Models
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        [Required]
        public string Number { get; set; } = "";
        [Required]
        public string Status { get; set; } = "";
        public DateTime? IssueDate { get; set; }
        public DateTime? DueDate { get; set; }

        [Required]
        public string Service { get; set; } = "";
        [Required,Display(Name ="Unit Price")]
        public decimal UnitPrice { get; set; }  
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string ClientName { get; set; } = "";
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string Phone { get; set; } = "";
        [Required]
        public string Address { get; set; } = "";

    }
}
