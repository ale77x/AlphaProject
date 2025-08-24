using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaProject.Shared.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "L'ID del cliente è obbligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "L'ID del cliente deve essere positivo")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "La data dell'ordine è obbligatoria")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "L'importo totale è obbligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "L'importo totale deve essere maggiore di 0")]
        public decimal TotalAmount { get; set; }

        // Navigation property - lista degli articoli dell'ordine
        public List<OrderItemDto>? OrderItems { get; set; }
    }
}
