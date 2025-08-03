namespace ZooApi.Models;

public abstract class Animal
{
    public string Name { get; set; }

    protected Animal(string name)
    {
        Name = name;
    }

    public abstract string Speak();
}