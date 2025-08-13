using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaProject.Shared.Dtos
{
    
        public class ProductDto
        {
            public int Id { get; set; }       // Id del prodotto
            public string Name { get; set; } = "";
            public decimal Price { get; set; }
            // niente Stock o CreatedAt se non ti servono
        }
    

}
