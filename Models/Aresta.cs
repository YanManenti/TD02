using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TD02.Models
{
    public class Aresta
    {
        public Vertice origem;
        public Vertice destino;
        public int peso;

        public Aresta(Vertice _origem, Vertice _destino, int _peso)
        {
            origem = _origem;
            destino = _destino;
            peso = _peso;
        }

    }
}