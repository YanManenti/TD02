using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TD02.Models;

namespace TD02.Controllers;

[ApiController]
[Route("[controller]")]
public class GrafoController : ControllerBase
{
    private readonly ILogger<GrafoController> _logger;
    private Grafo grafo = new Grafo();

    public GrafoController(ILogger<GrafoController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetGrafo")]
    public async Task<IActionResult> Get(String origem, String destino, double precoGasolina, double autonomiaCarro)
    {
        List<Dictionary<string, Capital>> capitais = await JsonFileReader.ReadAsync<List<Dictionary<string, Capital>>>("Data\\capitais.json");
        foreach (Dictionary<string, Capital> capital in capitais)
        {
            foreach (KeyValuePair<string, Capital> pair in capital)
            {
                grafo.AdicionaVertice(pair.Key);

            }
        }
        foreach (Dictionary<string, Capital> capital in capitais)
        {
            foreach (KeyValuePair<string, Capital> pair in capital)
            {
                foreach (KeyValuePair<string, int> neighbor in pair.Value.neighbors)
                {
                    double peso = pair.Value.toll + neighbor.Value * precoGasolina / autonomiaCarro;
                    grafo.AdicionaAresta(pair.Key, neighbor.Key, peso);
                }
            }
        }

        return Ok(grafo.caminhoMinimoDijkstra(origem, destino));
    }



    public static class JsonFileReader
    {
        public static async Task<T> ReadAsync<T>(string path)
        {
            using FileStream stream = System.IO.File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
