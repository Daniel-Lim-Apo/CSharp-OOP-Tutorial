using Microsoft.AspNetCore.Mvc;
using ZooApi.Services;

namespace ZooApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalsController(IAnimalService animalService)
    {
        _animalService = animalService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _animalService.GetAll()
            .Select(a => new { a.Name, Sound = a.Speak() });
        return Ok(result);
    }

    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        var animal = _animalService.GetByName(name);
        if (animal == null)
            return NotFound($"Animal with name '{name}' not found.");

        return Ok(new { animal.Name, Sound = animal.Speak() });
    }
}