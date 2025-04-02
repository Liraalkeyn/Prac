
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Выберите задание (1-6):");
        int taskNumber = int.Parse(Console.ReadLine());

        switch (taskNumber)
        {
            case 1:
                Task1();
                break;
            case 2:
                Task2();
                break;
            case 3:
                Task3();
                break;
            case 4:
                Task4();
                break;
            case 5:
                Task5();
                break;
            case 6:
                Task6();
                break;
            default:
                Console.WriteLine("Неверный номер задания");
                break;
        }
    }

    static void Task1()
    {
        Console.WriteLine("Задание 1: Расчет времени ожидания в очереди");
        Console.Write("Введите кол-во пациентов: ");
        int patients = int.Parse(Console.ReadLine());
        
        int totalMinutes = patients * 10;
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;
        
        Console.WriteLine($"Вы должны отстоять в очереди {hours} часа(ов) и {minutes} минут.");
    }

    static void Task2()
    {
        Console.WriteLine("Задание 2: Сумма кратных 3 или 5");
        Console.Write("Введите число: ");
        int number = int.Parse(Console.ReadLine());
        
        if (number < 0)
        {
            Console.WriteLine(0);
            return;
        }
        
        int sum = 0;
        for (int i = 0; i < number; i++)
        {
            if (i % 3 == 0 || i % 5 == 0)
            {
                sum += i;
            }
        }
        
        Console.WriteLine($"Сумма кратных 3 или 5 ниже {number}: {sum}");
    }

    static void Task3()
    {
        Console.WriteLine("Задание 3: Форматирование номера телефона");
        Console.WriteLine("Введите 10 цифр номера телефона (через пробел или без разделителей):");
        string input = Console.ReadLine();
        
        string cleanedInput = input.Replace(" ", "");
        if (cleanedInput.Length != 10 || !cleanedInput.All(char.IsDigit))
        {
            Console.WriteLine("Нужно ввести ровно 10 цифр!");
            return;
        }
        
        int[] numbers = cleanedInput.Select(c => int.Parse(c.ToString())).ToArray();
        
        string phoneNumber = $"+7 ({numbers[0]}{numbers[1]}{numbers[2]}) {numbers[3]}{numbers[4]}{numbers[5]}-{numbers[6]}{numbers[7]}{numbers[8]}{numbers[9]}";
        Console.WriteLine(phoneNumber);
    }

    static void Task4()
    {
        Console.WriteLine("Задание 4: Вывод последовательности");
        int start = 5;
        int step = 7;
        int max = 103;
        
        for (int i = start; i <= max; i += step)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();
    }

    static void Task5()
    {
        Console.WriteLine("Задание 5: Система магазина");
        
        Good iPhone12 = new Good("IPhone 12");
        Good iPhone11 = new Good("IPhone 11");
        
        Warehouse warehouse = new Warehouse();
        Shop shop = new Shop(warehouse);
        
        warehouse.Delive(iPhone12, 10);
        warehouse.Delive(iPhone11, 1);
        
        Console.WriteLine("Товары на складе:");
        foreach (var item in warehouse.GetInventory())
        {
            Console.WriteLine($"{item.Key.Name}: {item.Value} шт.");
        }
        
        Cart cart = shop.Cart();
        try
        {
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); // Ошибка: нет нужного кол-ва
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        
        // Вывод всех товаров в корзине
        Console.WriteLine("Товары в корзине:");
        foreach (var item in cart.GetItems())
        {
            Console.WriteLine($"{item.Key.Name}: {item.Value} шт.");
        }
        
        try
        {
            Order order = cart.Order();
            Console.WriteLine($"Ссылка для оплаты: {order.Paylink}");
            
            cart.Add(iPhone12, 9); // Ошибка: после заказа нельзя ничего добавить
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void Task6()
    {
        Console.WriteLine("Задание 6: Расчет времени ожидания в очереди (усложненный)");
        Console.Write("Введите кол-во пациентов: ");
        int patients = int.Parse(Console.ReadLine());
        
        Console.Write("Введите время приема одного пациента (в минутах): ");
        int timePerPatient = int.Parse(Console.ReadLine());
        
        int totalMinutes = patients * timePerPatient;
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;
        
        Console.WriteLine($"Вы должны отстоять в очереди {hours} часа(ов) и {minutes} минут.");
    }
}

// Классы для задания 5
class Good
{
    public string Name { get; }
    
    public Good(string name)
    {
        Name = name;
    }
}

class Warehouse
{
    private Dictionary<Good, int> inventory = new Dictionary<Good, int>();
    
    public void Delive(Good good, int quantity)
    {
        if (inventory.ContainsKey(good))
        {
            inventory[good] += quantity;
        }
        else
        {
            inventory.Add(good, quantity);
        }
    }
    
    public bool Remove(Good good, int quantity)
    {
        if (inventory.ContainsKey(good) && inventory[good] >= quantity)
        {
            inventory[good] -= quantity;
            return true;
        }
        return false;
    }
    
    public Dictionary<Good, int> GetInventory()
    {
        return new Dictionary<Good, int>(inventory);
    }
    
    public int GetQuantity(Good good)
    {
        return inventory.ContainsKey(good) ? inventory[good] : 0;
    }
}

class Shop
{
    private Warehouse warehouse;
    
    public Shop(Warehouse warehouse)
    {
        this.warehouse = warehouse;
    }
    
    public Cart Cart()
    {
        return new Cart(warehouse);
    }
}

class Cart
{
    private Warehouse warehouse;
    private Dictionary<Good, int> items = new Dictionary<Good, int>();
    private bool ordered = false;
    
    public Cart(Warehouse warehouse)
    {
        this.warehouse = warehouse;
    }
    
    public void Add(Good good, int quantity)
    {
        if (ordered)
        {
            throw new InvalidOperationException("Невозможно добавить товар после оформления заказа");
        }
        
        if (warehouse.GetQuantity(good) < quantity)
        {
            throw new InvalidOperationException($"Недостаточно товара {good.Name} на складе");
        }
        
        if (items.ContainsKey(good))
        {
            items[good] += quantity;
        }
        else
        {
            items.Add(good, quantity);
        }
    }
    
    public Order Order()
    {
        foreach (var item in items)
        {
            if (!warehouse.Remove(item.Key, item.Value))
            {
                throw new InvalidOperationException($"Не удалось зарезервировать товар {item.Key.Name}");
            }
        }
        
        ordered = true;
        return new Order();
    }
    
    public Dictionary<Good, int> GetItems()
    {
        return new Dictionary<Good, int>(items);
    }
}

class Order
{
    public string Paylink { get; } = "https://pay.example.com/" + Guid.NewGuid().ToString();
}