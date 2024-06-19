using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TD02.Models
{
    public class Grafo
    {
        Dictionary<string, Vertice> vertices;

        public Grafo()
        {
            vertices = new Dictionary<string, Vertice>();
        }

        public void AdicionaVertice(string nome)
        {
            Vertice v = new Vertice(nome);
            if (vertices.TryAdd(nome, v))
            {
                Console.WriteLine($"Vertice {nome} adicionado com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao adicionar o vertice {nome}!");
            }
        }

        public void AdicionaAresta(string origem, string destino, double peso)
        {
            if (vertices.TryGetValue(origem, out Vertice verifiedOrigem) && vertices.TryGetValue(destino, out Vertice verifiedDestino))
            {
                Aresta a = new Aresta(verifiedOrigem, verifiedDestino, peso);
                verifiedOrigem.AdicionaAresta(a);
                Console.WriteLine($"Aresta de {origem} para {destino} adicionada com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao adicionar a aresta de {origem} para {destino}!");
            }
        }

        public string caminhoMinimoDijkstra(string origem, string destino)
        {
            if (vertices.TryGetValue(origem, out Vertice verifiedOrigem) && vertices.TryGetValue(destino, out Vertice verifiedDestino))
            {
                PriorityQueue<Vertice, double> naoVisitados = new PriorityQueue<Vertice, double>();

                vertices[destino].path.Add(vertices[origem]);
                naoVisitados.Enqueue(verifiedOrigem, 0);

                while (naoVisitados.TryPeek(out Vertice peekedVertice, out double peekedDistance) && peekedDistance != double.MaxValue)
                {
                    naoVisitados.TryDequeue(out Vertice currentVertice, out double currentDistance);
                    vertices[currentVertice.nome].visitado = true;
                    foreach (Aresta aresta in currentVertice.arestas)
                    {
                        Vertice currentDestino = aresta.destino;
                        if (!vertices[currentDestino.nome].visitado)
                        {
                            if (currentDistance + aresta.peso < vertices[currentDestino.nome].prioridade)
                            {
                                vertices[currentDestino.nome].prioridade = currentDistance + aresta.peso;
                                if (vertices[currentDestino.nome].path.Count > 0)
                                {
                                    vertices[currentDestino.nome].path.Clear();
                                    vertices[currentDestino.nome].path.AddRange(vertices[currentVertice.nome].path);
                                }
                                vertices[currentDestino.nome].path.Add(vertices[currentVertice.nome]);
                                naoVisitados.Enqueue(vertices[currentDestino.nome], vertices[currentDestino.nome].prioridade);
                            }
                        }
                    }
                }
                return ($"Caminho de {origem} para {destino}: {string.Join(" -> ", vertices[destino].path.Select(v => v.nome))} -> {vertices[destino].nome} com distancia {vertices[destino].prioridade}!");
            }
            else
            {
                return ($"Erro ao encontrar o caminho de {origem} para {destino}!");
            }
        }
    }
}