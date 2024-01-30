using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICACORE.Models
{
    public class Cliente
    {
        public string Empresa { get; set; }
        public string Contacto { get; set; }
        public string Cargo { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
    }

    public class Pedido
    {
        public string CodigoPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string FormaEnvio { get; set; }
        public decimal Importe { get; set; }
    }
}
