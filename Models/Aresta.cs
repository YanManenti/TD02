using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TD02.Models
{
    public class Aresta
    {
        public string origem;
        public string destino;
        public double peso;

        public Aresta(string _origem, string _destino, double _peso)
        {
            origem = _origem;
            destino = _destino;
            peso = _peso;
        }

    }
}