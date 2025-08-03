using ZooApi.Models;

namespace ZooApi.Services;

public interface IAnimalService
{
    IEnumerable<Animal> GetAll();
    Animal? GetByName(string name);
}