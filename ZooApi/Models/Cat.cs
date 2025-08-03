namespace ZooApi.Models;

public class Cat : Animal
{
    public Cat(string name) : base(name) { }

    public override string Speak() => $"{Name} says: Meow!";
}