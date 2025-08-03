namespace ZooApi.Models;

public class Dog : Animal
{
    public Dog(string name) : base(name) { }

    public override string Speak() => $"{Name} says: Woof!";
}