using ZooApi.Models;

namespace ZooApi.Services;

public class AnimalService : IAnimalService
{
    private readonly List<Animal> _animals;

    public AnimalService()
    {
        _animals = new List<Animal>
        {
            new Dog("Rex"),
            new Cat("Whiskers"),
            new Dog("Buddy"),
        };
    }

    public IEnumerable<Animal> GetAll() => _animals;

    public Animal? GetByName(string name) =>
        _animals.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}