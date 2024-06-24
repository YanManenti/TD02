using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TD02.Models;

namespace TD02.Controllers;

[ApiController]
[Route("/api")]
public class GrafoController : ControllerBase
{
    private Grafo grafo = new Grafo();
    private List<Dictionary<string, Capital>> capitais = new List<Dictionary<string, Capital>>();
    private List<string> capitaisArray = new List<string>();

    public GrafoController()
    {
        capitais = LoadCapitais().Result;
        capitaisArray = LoadCapitalNamesArray();
    }

    [HttpGet("path", Name = "GetPath")]
    public IActionResult GetPath(string origem, string destino, double precoGasolina, double autonomiaCarro)
    {
        if (origem == null || destino == null || precoGasolina <= 0 || autonomiaCarro <= 0)
        {
            return BadRequest(JsonConvert.SerializeObject(new ResponseObject<string> { message = "Parâmetros inválidos!", data = [] }));
        }
        if (!capitaisArray.Contains(origem) || !capitaisArray.Contains(destino))
        {
            return BadRequest(JsonConvert.SerializeObject(new ResponseObject<string> { message = "Origem ou destino não encontrados!", data = [] }));
        }
        if (origem == destino)
        {
            return BadRequest(JsonConvert.SerializeObject(new ResponseObject<string> { message = "Origem e destino são iguais!", data = [origem] }));
        }

        grafo = new Grafo();
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
        List<Aresta> result = grafo.caminhoMinimoDijkstra(origem, destino);

        if (result.Count == 0)
        {
            return BadRequest(JsonConvert.SerializeObject(new ResponseObject<string> { message = "Não foi possível calcular o caminho mínimo!", data = [] }));
        }

        return Ok(JsonConvert.SerializeObject(new ResponseObject<Aresta> { message = "Caminho mínimo calculado com sucesso!", data = result }));
    }

    [HttpGet("filteredOptions", Name = "GetFilteredOptions")]
    public IActionResult GetFilteredOptions(string match)
    {
        List<string> filteredOptions = capitaisArray.Where(capital => capital.Contains(match, StringComparison.CurrentCultureIgnoreCase)).ToList();
        if (filteredOptions.Count == 0)
        {
            return BadRequest(JsonConvert.SerializeObject(new ResponseObject<string> { message = "Nenhuma capital encontrada!", data = [] }));
        }
        return Ok(JsonConvert.SerializeObject(new ResponseObject<string> { message = "Capitais filtradas com sucesso!", data = filteredOptions }));

    }

    [NonAction]
    public List<string> LoadCapitalNamesArray()
    {
        List<string> capitaisArray = new List<string>();
        foreach (Dictionary<string, Capital> capital in capitais)
        {
            foreach (KeyValuePair<string, Capital> pair in capital)
            {
                if (!capitaisArray.Contains(pair.Key))
                {
                    capitaisArray.Add(pair.Key);
                }
                foreach (KeyValuePair<string, int> neighbor in pair.Value.neighbors)
                {
                    if (!capitaisArray.Contains(neighbor.Key))
                    {
                        capitaisArray.Add(neighbor.Key);
                    }
                }
            }
        }
        capitaisArray.Sort();
        return capitaisArray;
    }

    [NonAction]
    public async Task<List<Dictionary<string, Capital>>> LoadCapitais()
    {
        return await JsonFileReader.ReadAsync<List<Dictionary<string, Capital>>>("Data\\capitais.json");
    }



    public static class JsonFileReader
    {
        public static async Task<T> ReadAsync<T>(string path)
        {
            using FileStream stream = System.IO.File.OpenRead(path);
            return await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
}
