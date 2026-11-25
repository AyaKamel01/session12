
using System;
using System.Collections.Generic;
using System.Linq;

// ======================================================
// SECTION 01 - Book Class
// ======================================================

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string[] Authors { get; set; }
    public DateTime PublicationDate { get; set; }
    public decimal Price { get; set; }

    public Book(
        string _ISBN,
        string _Title,
        string[] _Authors,
        DateTime _PublicationDate,
        decimal _Price)
    {
        ISBN = _ISBN;
        Title = _Title;
        Authors = _Authors;
        PublicationDate = _PublicationDate;
        Price = _Price;
    }

    public override string ToString()
    {
        return $"ISBN: {ISBN}, Title: {Title}, " +
               $"Authors: {string.Join(", ", Authors)}, " +
               $"Publication Date: {PublicationDate.ToShortDateString()}, " +
               $"Price: {Price}";
    }
}

// ======================================================
// SECTION 01 - BookFunctions
// ======================================================

public class BookFunctions
{
    public static string GetTitle(Book B)
    {
        return B.Title;
    }

    public static string GetAuthors(Book B)
    {
        return string.Join(", ", B.Authors);
    }

    public static string GetPrice(Book B)
    {
        return B.Price.ToString();
    }
}

// User-defined delegate
public delegate string BookDelegate(Book B);

// ======================================================
// SECTION 01 - LibraryEngine
// ======================================================

public class LibraryEngine
{
    // A) User-defined Delegate
    public static void ProcessBooks(
        List<Book> bList,
        BookDelegate fPtr)
    {
        foreach (Book B in bList)
        {
            Console.WriteLine(fPtr(B));
        }
    }

    // B) Built-in Delegate
    public static void ProcessBooks(
        List<Book> bList,
        Func<Book, string> fPtr)
    {
        foreach (Book B in bList)
        {
            Console.WriteLine(fPtr(B));
        }
    }
}

// ======================================================
// SECTION 02 - Order Class
// ======================================================

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

// User-defined delegate
public delegate decimal PriceCalculator(Order order);

// ======================================================
// SECTION 02 - OrderService
// ======================================================

public class OrderService
{
    public event Action<Order> OrderProcessed;

    // Receives pricing behavior from outside
    public decimal CalculateOrderPrice(
        Order order,
        Func<Order, decimal> pricingStrategy)
    {
        return pricingStrategy(order);
    }

    // Process order and raise event
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Order {order.Id} is being processed.");

        Console.WriteLine("Order processed successfully.");

        OrderProcessed?.Invoke(order);
    }
}

// ======================================================
// PROGRAM
// ======================================================

class Program
{
    // ==================================================
    // SECTION 02 - Part 1
    // User-defined Delegate Methods
    // ==================================================

    static decimal CalculateTotal(Order order)
    {
        return order.Price * order.Quantity;
    }

    static decimal CalculateTotalWithDiscount(Order order)
    {
        return (order.Price * order.Quantity) - 50;
    }

    static decimal CalculateOrderPrice(
        Order order,
        PriceCalculator calculator)
    {
        return calculator(order);
    }

    // ==================================================
    // SECTION 02 - Part 3
    // Predicate
    // ==================================================

    static bool ValidateOrder(
        Order order,
        Predicate<Order> validationRule)
    {
        return validationRule(order);
    }

    // ==================================================
    // SECTION 02 - Part 4
    // Action
    // ==================================================

    static void ProcessOrder(
        Order order,
        Action<Order> action)
    {
        action(order);
    }

    // ==================================================
    // SECTION 02 - Part 5 & 6
    // Event Handlers
    // ==================================================

    static void Handler1(Order order)
    {
        Console.WriteLine(
            $"Handler 1: Order {order.Id} completed.");
    }

    static void Handler2(Order order)
    {
        Console.WriteLine(
            $"Handler 2: Confirmation sent to {order.CustomerName}.");
    }

    static void Handler3(Order order)
    {
        Console.WriteLine(
            $"Handler 3: Audit record created for Order {order.Id}.");
    }

    // ==================================================
    // MAIN
    // ==================================================

    static void Main()
    {
        // ==============================================
        // SECTION 01
        // ==============================================

        Console.WriteLine("========== SECTION 01 ==========");

        List<Book> books = new List<Book>
        {
            new Book(
                "111",
                "C# Basics",
                new string[] { "Ahmed Ali", "Sara Mohamed" },
                new DateTime(2024, 1, 10),
                200m
            ),

            new Book(
                "222",
                "Advanced C#",
                new string[] { "Mohamed Hassan" },
                new DateTime(2025, 3, 15),
                350m
            ),

            new Book(
                "333",
                "OOP Programming",
                new string[] { "Aya Kamel", "Ali Ahmed" },
                new DateTime(2023, 6, 20),
                150m
            )
        };

        // A) User-defined Delegate
        Console.WriteLine("\nA) User-defined Delegate - GetTitle");

        LibraryEngine.ProcessBooks(
            books,
            new BookDelegate(BookFunctions.GetTitle)
        );

        // B) Built-in Delegate
        Console.WriteLine("\nB) Built-in Delegate - GetAuthors");

        LibraryEngine.ProcessBooks(
            books,
            new Func<Book, string>(BookFunctions.GetAuthors)
        );

        // GetPrice
        Console.WriteLine("\nGet Price");

        LibraryEngine.ProcessBooks(
            books,
            new BookDelegate(BookFunctions.GetPrice)
        );

        // C) Anonymous Method - GetISBN
        Console.WriteLine("\nC) Anonymous Method - GetISBN");

        LibraryEngine.ProcessBooks(
            books,
            new Func<Book, string>(
                delegate (Book B)
                {
                    return B.ISBN;
                }
            )
        );

        // D) Lambda Expression - GetPublicationDate
        Console.WriteLine("\nD) Lambda Expression - GetPublicationDate");

        LibraryEngine.ProcessBooks(
            books,
            new Func<Book, string>(
                B => B.PublicationDate.ToShortDateString()
            )
        );

        // ToString
        Console.WriteLine("\nBook Information");

        foreach (Book B in books)
        {
            Console.WriteLine(B.ToString());
        }


        // ==============================================
        // SECTION 02
        // ==============================================

        Console.WriteLine("\n========== SECTION 02 ==========");

        Order order = new Order
        {
            Id = 101,
            CustomerName = "Aya",
            Price = 100m,
            Quantity = 5
        };


        // ==============================================
        // PART 1 - User-defined Delegate
        // ==============================================

        Console.WriteLine("\nPART 1 - User-defined Delegate");

        Console.WriteLine(
            "Normal Price: " +
            CalculateOrderPrice(order, CalculateTotal)
        );

        Console.WriteLine(
            "Discount Price: " +
            CalculateOrderPrice(order, CalculateTotalWithDiscount)
        );


        // ==============================================
        // PART 2 - Func<>
        // ==============================================

        Console.WriteLine("\nPART 2 - Func Delegate");

        Func<Order, decimal> normalPrice =
            x => x.Price * x.Quantity;

        Func<Order, decimal> discount10 =
            x => x.Price * x.Quantity * 0.90m;

        Console.WriteLine("Normal: " + normalPrice(order));

        Console.WriteLine("10% Discount: " + discount10(order));


        // ==============================================
        // PART 3 - Predicate<>
        // ==============================================

        Console.WriteLine("\nPART 3 - Predicate Delegate");

        Predicate<Order> validQuantity =
            x => x.Quantity > 0;

        Predicate<Order> validPrice =
            x => x.Price > 0;

        Predicate<Order> validCustomer =
            x => !string.IsNullOrWhiteSpace(x.CustomerName);

        Console.WriteLine(
            "Valid Quantity: " +
            ValidateOrder(order, validQuantity)
        );

        Console.WriteLine(
            "Valid Price: " +
            ValidateOrder(order, validPrice)
        );

        Console.WriteLine(
            "Valid Customer: " +
            ValidateOrder(order, validCustomer)
        );


        // ==============================================
        // PART 4 - Action<>
        // ==============================================

        Console.WriteLine("\nPART 4 - Action Delegate");

        Action<Order> printOrder = x =>
        {
            Console.WriteLine(
                $"Order ID: {x.Id}, Customer: {x.CustomerName}"
            );
        };

        Action<Order> sendConfirmation = x =>
        {
            Console.WriteLine(
                $"Confirmation sent to {x.CustomerName}"
            );
        };

        Action<Order> writeAudit = x =>
        {
            Console.WriteLine(
                $"Audit: Order {x.Id} was processed."
            );
        };

        ProcessOrder(order, printOrder);

        ProcessOrder(order, sendConfirmation);

        ProcessOrder(order, writeAudit);


        // ==============================================
        // PART 5 & 6 - Events and Subscription
        // ==============================================

        Console.WriteLine("\nPART 5 & 6 - Events");

        OrderService orderService = new OrderService();

        orderService.OrderProcessed += Handler1;
        orderService.OrderProcessed += Handler2;
        orderService.OrderProcessed += Handler3;

        Console.WriteLine("\nAll handlers subscribed:");

        orderService.ProcessOrder(order);

        Console.WriteLine("\nUnsubscribing Handler1:");

        orderService.OrderProcessed -= Handler1;

        orderService.ProcessOrder(order);


        // ==============================================
        // PART 7 - Final Application
        // ==============================================

        Console.WriteLine("\nPART 7 - Final Application");

        Predicate<Order>[] validationRules =
        {
            x => x.Quantity > 0,
            x => x.Price > 0,
            x => !string.IsNullOrWhiteSpace(x.CustomerName)
        };

        bool isValid = validationRules.All(
            rule => ValidateOrder(order, rule)
        );

        Console.WriteLine("Order Valid: " + isValid);

        Func<Order, decimal> pricingStrategy =
            x => x.Price * x.Quantity;

        decimal finalPrice =
            orderService.CalculateOrderPrice(
                order,
                pricingStrategy
            );

        Console.WriteLine("Final Price: " + finalPrice);

        orderService.ProcessOrder(order);


        // ==============================================
        // BONUS - Different Pricing Strategies
        // ==============================================

        Console.WriteLine("\nBONUS - Pricing Strategies");

        Func<Order, decimal> normalPricing =
            x => x.Price * x.Quantity;

        Func<Order, decimal> tenPercentDiscount =
            x => x.Price * x.Quantity * 0.90m;

        Func<Order, decimal> twentyPercentDiscount =
            x => x.Price * x.Quantity * 0.80m;

        Func<Order, decimal> vipDiscount =
            x => x.Price * x.Quantity * 0.70m;

        Console.WriteLine(
            "Normal Pricing: " +
            orderService.CalculateOrderPrice(order, normalPricing)
        );

        Console.WriteLine(
            "10% Discount: " +
            orderService.CalculateOrderPrice(order, tenPercentDiscount)
        );

        Console.WriteLine(
            "20% Discount: " +
            orderService.CalculateOrderPrice(order, twentyPercentDiscount)
        );

        Console.WriteLine(
            "VIP Discount: " +
            orderService.CalculateOrderPrice(order, vipDiscount)
        );

        Console.WriteLine("\nAssignment Completed!");
    }
}

/*
======================================================
QUESTIONS - SECTION 02
======================================================

Q1:
PriceCalculator is a user-defined delegate created for a specific purpose.
Func<Order, decimal> is a built-in generic delegate.
Both accept an Order and return a decimal.

Q2:
Action<Order> accepts an Order and returns void.
Func<Order, decimal> accepts an Order and returns decimal.

Q3:
Predicate<T> returns bool because it is designed to test a condition.
It is commonly used for filtering and validation.

Q4:
A delegate is a type-safe reference to a method.
An event is a notification mechanism based on delegates.
Events control how external code subscribes and unsubscribes.

Q5:
External code cannot normally invoke an event because only the class
that declares the event can raise it.

Q6:
When multiple handlers subscribe to the same event,
all subscribed handlers are invoked when the event is raised.

Q7:
orderService.OrderProcessed += HandleOrderProcessed;

OrderProcessed: The event name.
+: Adds a handler to the event.
=: Assignment operator used with +=.
HandleOrderProcessed: The method that executes when the event is raised.

Q8:
Action<Order> is a delegate that can be invoked directly.
event Action<Order> provides controlled subscription and unsubscription.
Only the declaring class can raise the event.
This protects the event from being invoked externally.
*/
