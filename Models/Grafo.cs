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
                Aresta a = new Aresta(verifiedOrigem.nome, verifiedDestino.nome, peso);
                verifiedOrigem.AdicionaAresta(a);
                Console.WriteLine($"Aresta de {origem} para {destino} adicionada com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao adicionar a aresta de {origem} para {destino}!");
            }
        }

        public List<Aresta> caminhoMinimoDijkstra(string origem, string destino)
        {
            if (vertices.TryGetValue(origem, out Vertice verifiedOrigem) && vertices.TryGetValue(destino, out Vertice verifiedDestino))
            {
                PriorityQueue<Vertice, double> naoVisitados = new PriorityQueue<Vertice, double>();

                naoVisitados.Enqueue(verifiedOrigem, 0);

                while (naoVisitados.TryPeek(out Vertice peekedVertice, out double peekedDistance) && peekedDistance != double.MaxValue)
                {
                    naoVisitados.TryDequeue(out Vertice currentVertice, out double currentDistance);
                    vertices[currentVertice.nome].visitado = true;
                    foreach (Aresta aresta in currentVertice.arestas)
                    {
                        string currentDestino = aresta.destino;
                        if (!vertices[currentDestino].visitado)
                        {
                            if (currentDistance + aresta.peso < vertices[currentDestino].prioridade)
                            {
                                vertices[currentDestino].prioridade = currentDistance + aresta.peso;
                                vertices[currentDestino].path.Clear();
                                vertices[currentDestino].path.AddRange(vertices[aresta.origem].path);
                                vertices[currentDestino].path.Add(aresta);
                                naoVisitados.Enqueue(vertices[currentDestino], vertices[currentDestino].prioridade);
                            }
                        }
                    }
                }
                return vertices[destino].path;
            }
            else
            {
                return [];
            }
        }
    }
}