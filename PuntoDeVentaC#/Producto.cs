using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoDeVenta
{
    class Producto
    {
        private int codigo;
        private string nombre;
        private int inventario;
        private int precio;
        private int descuento;

        public int Codigo { get => codigo; set => codigo = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int Inventario { get => inventario; set => inventario = value; }
        public int Precio { get => precio; set => precio = value; }
        public int Descuento { get => descuento; set => descuento = value; }
    }
}
