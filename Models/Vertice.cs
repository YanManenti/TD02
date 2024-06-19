using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TD02.Models
{
    public class Vertice
    {
        public int prioridade;
        public bool visitado;
        public List<Aresta> arestas;
        public string nome;
        public List<Vertice> path;

        public Vertice(string _nome)
        {
            nome = _nome;
            visitado = false;
            prioridade = int.MaxValue;
            arestas = [];
            path = [];
        }

        public void AdicionaAresta(Aresta a)
        {
            arestas.Add(a);
        }
    }
}