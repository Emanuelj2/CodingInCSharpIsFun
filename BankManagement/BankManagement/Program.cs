using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

class Program
{

    /*
     * static variables
     * Shared across all instances of a class.
     * Only one copy exists in memory
     * Belongs to the class, not any object.
     */
    static decimal accountBalance = 0;
    static List<string> toDo = new List<string>();
    static string? todoFilePath = "todoList.txt";

    static void Main(string[] args)
    {
        //LoadTodoList

        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            ShowMenu();
            string? choice = Console.ReadLine();
            //make a switch case to give user a choice
            switch (choice)
            {
                case "1":
                    BankingOperations();
                    break;
                case "2":
                    toDoList();
                    break;
                case "3":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;

            }
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("===Main Menu===");
        Console.WriteLine("1. Banking Operations");
        Console.WriteLine("2. To-Do List");
        Console.WriteLine("3. Exit");
        Console.WriteLine("Enter your choice");

    }

    static void BankingOperations()
    {
        bool isRunning = true;

        while (isRunning)
        {
            //clear the console
            Console.Clear();

            Console.WriteLine("===Banking Operations===");
            Console.WriteLine($"Current Balance: ${accountBalance}");
            Console.WriteLine("1. Deposite");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Main Menu");

            Console.WriteLine("Enter your choice");
            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Deposite();
                        break;
                    case "2":
                        Withdraw();
                        break;
                    case "3":
                        
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. press any key to try again ...");
                        Console.ReadKey();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.ReadKey();
            }
        }
    }

    static void Deposite()
    {
        Console.WriteLine("Enter the amount that you would like to deposite");
        string? input = Console.ReadLine();

        if (decimal.TryParse(input, out decimal deposit))
        {

            if(deposit < 0)
            {
                Console.WriteLine("Deposite must be greator than zero.");
                Console.ReadKey();
                return;
            }
            accountBalance += deposit;
            Console.WriteLine($"You deposited: {deposit:C}");
            Console.WriteLine($"Your new balance is {accountBalance}");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            
        }
        Console.WriteLine("Press any key to return to the Banking Menu...");
        Console.ReadKey();

    }

    static void Withdraw() 
    {
        Console.WriteLine("Enter the amount that you would like to withdraw");
        string?  input = Console.ReadLine();

        if (decimal.TryParse(input, out decimal withdraw))
        {
            if (withdraw > accountBalance)
            {
                Console.WriteLine($"Insuficient funds");
                Console.ReadKey();
                return;
            }
            if(withdraw == 0)
            {
                Console.WriteLine("Withdraw amount must be grator than zero");
                Console.ReadKey();
                return;
                
            }

            accountBalance -= withdraw;
            Console.WriteLine($"You withdrew: {withdraw:C}");
            Console.WriteLine($"Your balance is {accountBalance}");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            
        }
        Console.WriteLine("Press any key to return to the Banking Menu...");
        Console.ReadKey();
    }

    static void toDoList()
    {
        bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("===To-Do List===");
            Console.WriteLine("1. View To-Do List");
            Console.WriteLine("2. Add To-Do Item");
            Console.WriteLine("3. Remove To-Do Item");
            Console.WriteLine("4. Update To-Do List");
            Console.WriteLine("5. Main Menu");
            Console.WriteLine("Enter your choice");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewToDoList();
                    break;
                case "2":
                    AddToDoItem();
                    break;
                case "3":
                    RemoveToDoItem();
                    break;
                case "4":
                    UpdateToDoList();
                    break;
                case "5":
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void ViewToDoList()
    {
        Console.Clear();
        Console.WriteLine("===Your To-Do List===");
        if (toDo.Count == 0)
        {
            Console.WriteLine("Your to-do list is empty.");
        }
        else
        {
            for (int i = 0; i < toDo.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {toDo[i]}");
            }
        }
        Console.WriteLine("Press any key to return to the To-Do Menu...");
        Console.ReadKey();
    }

    static void AddToDoItem()
    {
        Console.WriteLine("Enter a new to-do item:");
        string? item = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(item))
        {
            toDo.Add(item);
            Console.WriteLine("Item added to the to-do list.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a non-empty item.");
        }
    }

    static void RemoveToDoItem()
    {
        Console.WriteLine("Enter the number of the item to remove:");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int index))
        {
            if (index > 0 && index <= toDo.Count)
            {
                toDo.RemoveAt(index - 1);
                Console.WriteLine("Item removed from the to-do list.");
            }
            else
            {
                Console.WriteLine("Invalid index. Please enter a valid item number.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }
    static void UpdateToDoList()
    {
        Console.WriteLine("Enter the number of the item to Update:");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int index))
        {
            if (index > 0 && index <= toDo.Count)
            {
                Console.WriteLine("Enter the updated to-do item:");
                string? updatedItem = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(updatedItem))
                {
                    toDo[index - 1] = updatedItem;
                    Console.WriteLine("Item updated in the to-do list.");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a non-empty item.");
                }
            }
            else
            {
                Console.WriteLine("Invalid index. Please enter a valid item number.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

}