using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2
{
    class Registro
    {
        public string nemonico;
        public int numero;
        public string direccion;

        public Registro(string nemonico,int numero)
        {
            this.nemonico = nemonico;
            this.numero = numero;
            this.direccion = "FFFF";
        }
    }
}
