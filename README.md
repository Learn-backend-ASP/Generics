# Generics in C# 

- Intro
- Before Generics — overloading
- Before Generics — using base `object`
- Generics: What and Why
- Generic Methods
- Generic Classes
- Generic Constraints
- Built-in .NET Generics and Collections

---

## Intro

Generics let you write flexible, type-safe code that works with any data type while preserving compile-time type checking. With generics you avoid repetitive code, boxing/unboxing, and unsafe casts.

This repo contains a small set of examples in `Program.cs` demonstrating both a generic method (`Print<T>`) and a custom generic collection `Any<T>`.

## Before Generics — overloading

Before generics, a common approach to support multiple types was to write overloaded methods for each type you need. That quickly becomes verbose.

Example (pre-generics overloading):

```csharp
// BEFORE generics: multiple overloads
static void Print(string s) => Console.WriteLine(s);
static void Print(int i) => Console.WriteLine(i);
static void Print(bool b) => Console.WriteLine(b);
```

Problems:
- Repetition of the same logic for each type.
- Hard to maintain if you need many types.

## Before Generics — base class `object`

Another approach is to accept `object` and rely on boxing/unboxing and casting:

```csharp
static void Print(object o)
{
    Console.WriteLine(o);
}

Print(123);        // boxing for value types
Print("hello");

// To get the original type back, you must cast and risk runtime errors
var s = (string)someObj; // possible InvalidCastException
```

Problems:
- Boxing/unboxing for value types hurts performance.
- Compile-time safety is lost (casts may fail at runtime).

## Generics — What and Why

Generics let you parameterize types with type parameters. Rather than writing separate code for each type or using `object`, you write a single, type-safe implementation.

Benefits:
- Type safety at compile time.
- No boxing for value types.
- Reuse and cleaner APIs.

## Generic Methods

The repo contains a generic method `Print<T>` in `Program.cs`:

```csharp
static void Print<T>(T s)
{
    Console.WriteLine($"DateType : {typeof(T).Name}");
    Console.WriteLine(s);
}
```

Usage examples from `Program.cs` (calls are commented out but show intent):

```csharp
// Print("Mahmoud");
// Print(123);
// Print(true);
```

This single method handles any type while preserving the compile-time type information (e.g., `T` is `int`, `string`, etc.).

### Contract
- Input: a single value of type `T`.
- Output: prints type name and value. No return value.
- Error modes: none specific; printing `null` will print nothing or `null` depending on type.

## Generic Classes

`Program.cs` includes a custom generic collection class `Any<T>` that demonstrates a simple array-backed collection.

Key parts (extracted from the repo):

```csharp
class Any<T> /* where T : class */
{
    private T[] _items;

    public void Add(T item)
    {
        if (_items is null)
        {
            _items = new T[] { item };
        }
        else
        {
            var length = _items.Length;
            var dest = new T[length + 1];
            for (int i = 0; i < length; i++) dest[i] = _items[i];
            dest[length] = item;
            _items = dest;
        }
    }

    public void RemoveAt(int position)
    {
        if (position < 0 || position > _items.Length - 1) return;
        int index = 0;
        var dest = new T[_items.Length - 1];
        for (int i = 0; i < _items.Length; i++)
        {
            if (position == i) continue;
            dest[index++] = _items[i];
        }
        _items = dest;
    }

    public bool IsEmpty => _items.Length == 0 || _items is null;
    public int Count => _items is null ? 0 : _items.Length;

    public void Display()
    {
        Console.Write("[");
        for (int i = 0; i < _items.Length; i++)
        {
            Console.Write(_items[i]);
            if (i < _items.Length - 1) Console.Write(',');
        }
        Console.WriteLine("]");
    }
}
```

Usage in `Main` (from `Program.cs`):

```csharp
var nums = new Any<int>();
nums.Add(1);
nums.Add(2);
nums.Add(3);
nums.Display();
Console.WriteLine($"Length : {nums.Count} Item");
nums.RemoveAt(2);
nums.Display();

var persons = new Any<Person>();
persons.Add(new Person("Mahmoud", "Hany"));
persons.Add(new Person("Mohamed", "Hany"));
persons.Display();
```

`Person` class (simple example from repo):

```csharp
class Person
{
    private string FName { get; set; }
    private string LName { get; set; }
    public Person(string fName, string lName)
    {
        FName = fName;
        LName = lName;
    }
    public override string ToString() => $"'{FName} {LName}'";
}
```

This demonstrates using a single `Any<T>` type to store both `int` and `Person` values with compile-time safety.

## Generics Constraints

You can restrict what types are allowed for type parameters using `where` clauses. `Program.cs` has a commented constraint hint on `Any<T>`:

```csharp
class Any<T> /* where T : class */ { ... }
```

Common constraints:
- where T : class — reference types only
- where T : struct — value types only (non-nullable)
- where T : new() — must have public parameterless constructor
- where T : BaseClass — must inherit from BaseClass
- where T : ISomeInterface — must implement interface
- where T : unmanaged — unmanaged types (C# 7.3+)

Example usage:

```csharp
// Only reference types
class AnyReference<T> where T : class { }

// Constrain to types that implement IDisposable
class ResourceHolder<T> where T : IDisposable
{
    public void Use(T resource) { resource.Dispose(); }
}
```

Constraints let you use members or constructors of the constrained type safely.

## Built-in .NET Generics and Collections

The BCL provides many generic types; the most commonly used are in `System.Collections.Generic`:

- List<T> — dynamic array
- Dictionary<TKey, TValue> — hash table
- Queue<T>, Stack<T> — FIFO/LIFO
- HashSet<T> — set semantics
- LinkedList<T> — doubly linked list

Interfaces:
- IEnumerable<T>, ICollection<T>, IReadOnlyList<T>, IList<T>

Other generic types:
- Nullable<T> (T?) — for value types
- Tuple<T1, T2>
- ConcurrentQueue<T>, ConcurrentDictionary<TKey,TValue>

Small examples:

```csharp
var list = new List<int> { 1, 2, 3 };
list.Add(4);
int first = list[0]; // compile-time typed as int

var map = new Dictionary<string, int>();
map["one"] = 1;

IEnumerable<Person> people = new List<Person>();
```

**Author:** Mahmoud Hany    
**Language:** C#  
**Topic:** Generics 
