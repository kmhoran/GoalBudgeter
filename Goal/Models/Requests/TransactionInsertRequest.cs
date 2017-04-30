using Goal.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Goal.Models.Requests
{
    public class TransactionInsertRequest
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public TransactionType TypeId { get; set; }

        // UserId is added server-side
        public string UserId { get; set; }
    }
}