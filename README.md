# 🦁 CSharp-OOP-Tutorial

A beginner-friendly C# project that teaches the **four pillars of Object-Oriented Programming (OOP)** through a fun Zoo API example. You'll learn how to model real-world things (animals!) using classes, inheritance, interfaces, and more — all bundled into a working ASP.NET Core Web API.

---

## 📚 Table of Contents

- [What is OOP?](#what-is-oop)
- [Project Structure](#project-structure)
- [OOP Concepts in This Project](#oop-concepts-in-this-project)
  - [1. Classes and Objects](#1-classes-and-objects)
  - [2. Encapsulation](#2-encapsulation)
  - [3. Inheritance](#3-inheritance)
  - [4. Abstraction](#4-abstraction)
  - [5. Polymorphism](#5-polymorphism)
  - [6. Interfaces](#6-interfaces)
- [How It All Fits Together](#how-it-all-fits-together)
- [Running the Project](#running-the-project)
- [API Endpoints](#api-endpoints)

---

## What is OOP?

**Object-Oriented Programming (OOP)** is a way of writing programs by modeling them around **objects** — just like real life things. Instead of writing a long list of instructions, you define "blueprints" (called **classes**) that describe what something *is* and what it can *do*.

Think about a `Dog`:
- It **has** a name → that's a **property**
- It can **bark** → that's a **method**

OOP has **four main pillars**:

| Pillar | Simple Meaning |
|---|---|
| **Encapsulation** | Bundling data (properties) and behavior (methods) together inside a class |
| **Inheritance** | A class can "inherit" features from another class (like a child from a parent) |
| **Abstraction** | Hiding complex details and showing only what's necessary |
| **Polymorphism** | One method name, many different behaviors depending on the object |

---

## Project Structure

```
ZooApi/
├── Models/
│   ├── Animal.cs          # Abstract base class (the "blueprint of all animals")
│   ├── Dog.cs             # Concrete class that inherits from Animal
│   └── Cat.cs             # Concrete class that inherits from Animal
├── Services/
│   ├── IAnimalService.cs  # Interface (a "contract" for what the service must do)
│   └── AnimalService.cs   # Implementation of the interface
├── Controllers/
│   └── AnimalsController.cs  # REST API controller (handles HTTP requests)
└── Program.cs             # App startup and Dependency Injection setup
```

---

## OOP Concepts in This Project

### 1. Classes and Objects

A **class** is a blueprint. An **object** is a real instance created from that blueprint.

```csharp
// "Dog" is a class (the blueprint)
public class Dog : Animal
{
    public Dog(string name) : base(name) { }
    public override string Speak() => $"{Name} says: Woof!";
}

// Creating objects (instances) from the blueprint:
var dog1 = new Dog("Rex");
var dog2 = new Dog("Buddy");
```

`dog1` and `dog2` are two **different objects**, but they were both created from the same `Dog` class.

---

### 2. Encapsulation

**Encapsulation** means keeping the internal details of an object safe and controlled. You expose only what's needed.

In `Animal.cs`, the `Name` property is **public** (anyone can read it), but the constructor is **protected** (only the class and its children can call it):

```csharp
public abstract class Animal
{
    public string Name { get; set; }  // Public: accessible from anywhere

    protected Animal(string name)     // Protected: only Animal and its subclasses can use this
    {
        Name = name;
    }
}
```

In `AnimalService.cs`, the list of animals is **private** — no one outside the class can directly modify it:

```csharp
public class AnimalService : IAnimalService
{
    private readonly List<Animal> _animals;  // Private: hidden from the outside world
    ...
}
```

The underscore `_` prefix by convention signals that this is a **private field**.

---

### 3. Inheritance

**Inheritance** lets a class reuse properties and methods from another class. The child class gets everything the parent has, and can add or change behavior.

`Dog` and `Cat` both **inherit** from `Animal` (note the `: Animal` syntax):

```csharp
// Animal.cs — the PARENT class
public abstract class Animal
{
    public string Name { get; set; }

    protected Animal(string name)
    {
        Name = name;
    }
}

// Dog.cs — the CHILD class
public class Dog : Animal
{
    // ": base(name)" calls the Animal constructor to set the Name
    public Dog(string name) : base(name) { }

    public override string Speak() => $"{Name} says: Woof!";
}

// Cat.cs — another CHILD class
public class Cat : Animal
{
    public Cat(string name) : base(name) { }

    public override string Speak() => $"{Name} says: Meow!";
}
```

Both `Dog` and `Cat` **automatically have** the `Name` property because they inherit it from `Animal`. They don't need to redefine it.

---

### 4. Abstraction

**Abstraction** means defining *what* something should do, without specifying *how* it does it. This is done using `abstract` classes and `abstract` methods.

The `Animal` class is `abstract`, which means:
- You **cannot** create an `Animal` directly: `new Animal("Leo")` ❌
- Every subclass **must implement** abstract methods

```csharp
public abstract class Animal
{
    public string Name { get; set; }

    protected Animal(string name)
    {
        Name = name;
    }

    // Abstract method: every subclass MUST provide its own implementation
    public abstract string Speak();
}
```

The `Speak()` method is declared here but has no body. It's up to `Dog` and `Cat` to define how their version of `Speak()` works.

---

### 5. Polymorphism

**Polymorphism** (from Greek: "many forms") means that the same method call can behave differently depending on the actual type of the object.

The word `override` is the key:

```csharp
// In Dog.cs
public override string Speak() => $"{Name} says: Woof!";

// In Cat.cs
public override string Speak() => $"{Name} says: Meow!";
```

In action — the controller calls `Speak()` on each animal, without knowing (or caring) whether it's a `Dog` or a `Cat`:

```csharp
// AnimalsController.cs
var result = _animalService.GetAll()
    .Select(a => new { a.Name, Sound = a.Speak() });
//                              ^^^^^^^^
// The same method call, but returns "Woof!" for dogs and "Meow!" for cats!
```

This is polymorphism: one method name (`Speak`), many different results.

---

### 6. Interfaces

An **interface** is a "contract". It says: *"Any class that implements me must provide these methods."* It defines **what** a class must do, but not **how**.

```csharp
// IAnimalService.cs — The contract
public interface IAnimalService
{
    IEnumerable<Animal> GetAll();
    Animal? GetByName(string name);
}

// AnimalService.cs — A class that "signs the contract" with ": IAnimalService"
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
```

**Why use an interface?** The `AnimalsController` depends on `IAnimalService`, not `AnimalService` directly. This means tomorrow you could swap `AnimalService` for a `DatabaseAnimalService` that reads from a real database — and the controller wouldn't change at all!

```csharp
// AnimalsController.cs — depends on the INTERFACE, not the concrete class
public class AnimalsController : ControllerBase
{
    private readonly IAnimalService _animalService;

    public AnimalsController(IAnimalService animalService) // ← Injected automatically!
    {
        _animalService = animalService;
    }
    ...
}
```

This technique is called **Dependency Injection (DI)**, and it's wired up in `Program.cs`:

```csharp
// Program.cs
// "When someone asks for IAnimalService, give them an AnimalService"
builder.Services.AddSingleton<IAnimalService, AnimalService>();
```

---

## How It All Fits Together

Here's the complete flow when a user makes an HTTP request:

```
HTTP GET /api/animals
        │
        ▼
AnimalsController       ← Receives the request
        │  calls
        ▼
IAnimalService          ← Interface (the contract)
        │  implemented by
        ▼
AnimalService           ← Returns a List<Animal> containing Dog and Cat objects
        │  calls
        ▼
animal.Speak()          ← Polymorphism: "Woof!" or "Meow!" depending on the type
        │
        ▼
JSON Response           ← [{ "name": "Rex", "sound": "Rex says: Woof!" }, ...]
```

---

## Running the Project

**Prerequisites:** [.NET SDK 8+](https://dotnet.microsoft.com/download)

```bash
# 1. Navigate to the project folder
cd ZooApi

# 2. Run the project
dotnet run
```

Once running, open your browser and go to:

> **http://localhost:5XXX/swagger**

Swagger UI will open automatically and let you test the API endpoints visually.

---

## API Endpoints

| Method | URL | Description |
|--------|-----|-------------|
| `GET` | `/api/animals` | Returns all animals and their sounds |
| `GET` | `/api/animals/{name}` | Returns a specific animal by name |

### Example Responses

**GET /api/animals**
```json
[
  { "name": "Rex",      "sound": "Rex says: Woof!" },
  { "name": "Whiskers", "sound": "Whiskers says: Meow!" },
  { "name": "Buddy",    "sound": "Buddy says: Woof!" }
]
```

**GET /api/animals/whiskers**
```json
{ "name": "Whiskers", "sound": "Whiskers says: Meow!" }
```

**GET /api/animals/unknown** → `404 Not Found`
```
Animal with name 'unknown' not found.
```
