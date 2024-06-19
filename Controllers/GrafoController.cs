using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TD02.Models;

namespace TD02.Controllers;

[ApiController]
[Route("[controller]")]
public class GrafoController : ControllerBase
{
    private readonly ILogger<GrafoController> _logger;
    private Grafo grafo;

    public GrafoController(ILogger<GrafoController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetGrafo")]
    public async Task<IActionResult> Get()
    {
        List<Dictionary<string, Capital>> capitais = await JsonFileReader.ReadAsync<List<Dictionary<string, Capital>>>("Data\\capitais.json");
        return Ok(await Task.FromResult(capitais));
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
