namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            //Print("Mahmoud");
            //Print(123);
            //Print(true);
            //Console.ReadKey();
            ///****///
            var nums = new Any<int>();
            nums.Add(1);
            nums.Add(2);
            nums.Add(3);
            nums.Add(4);
            nums.Add(5);
            nums.Display();
            Console.WriteLine($"Length : {nums.Count} Item");
            Console.WriteLine($"Empty  : {nums.IsEmpty} ");
            nums.RemoveAt(2);
            nums.Display();
            /////
            var persons = new Any<Person>();
            persons.Add(new Person("Mahmoud", "Hany"));
            persons.Add(new Person("Mohamed", "Hany"));
            persons.Add(new Person("Attia", "Hany"));
            Console.WriteLine($"Length : {persons.Count} Item");
            Console.WriteLine($"Empty  : {persons.IsEmpty} ");
            persons.Display();
            ///
            var person2 = new List<Person>();


            Console.ReadKey();


        }
        static void Print<T>(T s)
        {
            Console.WriteLine($"DateType : {typeof(T).Name}");
            Console.WriteLine(s);
        }
    }
    class Person
    {
        private string FName { get; set; }
        private string LName { get; set; }
        public Person(string fName, string lName)
        {
            FName = fName;
            LName = lName;
        }
        public override string ToString()
        {
            return $"'{FName} {LName}'";
        }
    }
    class Any<T> /*where T : class  */
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
                for (int i = 0; i < length; i++)
                {
                    dest[i] = _items[i];
                }
                dest[length] = item;
                _items = dest;
            }
        }
        public void RemoveAt(int position)
        {
            if (position < 0 || position > _items.Length - 1)
                return;
            int index = 0;
            var dest = new T[_items.Length - 1];
            for (int i = 0; i < _items.Length; i++)
            {
                if (position == i)
                    continue;
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
}
