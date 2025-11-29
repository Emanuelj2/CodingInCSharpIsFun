using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Person
{
   public string Name { get; set; }
    public int Id { get; set; }

    //constructor
    protected Person(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public abstract void DisplayInfo(); //must be implemented by derived class
    public virtual string GetRole() //can be overriden since it is virtual
    {
        return "Person";
    }
}

public class Member : Person
{
    //
    private List<Book> borrowedBooks;
    private DateTime membershipDate;

    //
    public List<Book> BorrowedBooks => borrowedBooks;
    public DateTime MembershipDate => membershipDate;

    public Member(string name, int id) : base(name, id)
    {
        borrowedBooks = new List<Book>();
        membershipDate = DateTime.Now;
    }

    // POLYMORPHISM: Overriding abstract method
    public override void DisplayInfo()
    {
        Console.WriteLine($"Member: {Name} (ID: {Id})");
        Console.WriteLine($"Membership Date: {MembershipDate.ToShortDateString()}");
        Console.WriteLine($"Books Borrowed: {borrowedBooks.Count}");
    }

    public override string GetRole()
    {
        return "Library Member";
    }

    public void BorrowBook(Book book)
    {
        //check if th user can checkout a book
        if(borrowedBooks.Count >= 5)
        {
            Console.WriteLine($"{Name} has reached the borrowing limit!");
            return;
        }
        
        //see if the book is avaliable
        if(book.IsAvailable)
        {
            borrowedBooks.Add(book);
            book.BorrowBook(this);
            Console.WriteLine($"{Name} borrowed '{book.Title}'");
        }
        else
        {
            Console.WriteLine($"'{book.Title}' is not available");
        }

    }

    public void RetrunBook(Book book)
    {
        if(borrowedBooks.Contains(book))
        {
            borrowedBooks.Remove(book);
            Console.WriteLine($"{Name} returned '{book.Title}'");
        }
    }
}

public class Librarian : Person
{
    private string department;

    public Department => department;

    public Librarian(string name, int id, string department) base(name, id)
    {
        this.department = department;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Librarian: {Name} (ID: {Id})");
        Console.WriteLine($"Department: {department}");
    }

    public override string GetRole()
    {
        return "Librarian";
    }

    public void AddBookToLibrary(Library library, Book book)
    {
    library.AddBook(book);
    Console.WriteLine($"Librarian {Name} added '{book.Title}' to the library");
    }
}

public class Book
{
    // Fields
    private string title;
    private string author;
    private string isbn;
    private bool isAvailable;
    private Member borrowedBy;

    // Properties
    public string Title => title;
    public string Author => author;
    public string Isbn => isbn;
    public bool IsAvailable => isAvailable;
    public Member BorrowedBy => borrowedBy;

    // Constructor
    public Book(string title, string author, string isbn)
    {
        this.title = title;
        this.author = author;   
        this.isbn = isbn;
        this.isAvailable = true;
        this.borrowedBy = null;
    }

    public void BorrowBook(Member member)
    {
        if(isAvailable)
        {
            isAvailable = false;
            borrowedBy = member;
        }
        else
        {
            throw new InvalidOperationException("Book is already borrowed.");
        }
    }

    public void ReturnBook()
    {
        isAvailable = true;
        borrowedBy = null;
    }

    public void DisplayInfo()
    {
        Console.Writeline($"{title} by {author}, ISBN: {isbn}");
        Console.WriteLine($"Status: {(isAvailable ? "Available" : $"Borrowed by {currentBorrower?.Name}")}
    }
}

public interface ISearchable
{
    List<Book> SearchByTitle(string title);
    List<Book> SearchById(string author);
}

public class Library : ISearchable
{
    private string name;
    private List<Book> books;
    private List<Member> members;
    private List<Librarian> librarians;

    public string Name => name;

    //constructor
    public Library(string name)
    {
        this.name = name;
        books = new List<Book>();
        members = new List<Member>();
        librarians = new List<Librarian>();
    }

    public void AddBook(Book book)
    {
        books.Add(book);
    }

    public void RegisterMember(Member member)
    {
        members.Add(member);
        Console.WriteLine($"Member {member.Name} registered successfully!");
    }

    public void RegisterLibrarian(Librarian librarian)
    {
        librarians.Add(librarian);
        Console.WriteLine($"Librarian {librarian.Name} registered successfully!");
    }

    //interface implementation
    public List<Book> SearchByTitle(string title)
    {
        return books.Where(b => b.Title.Contains(Title, stringConparison.OrdinalIgnoreCase)).ToList();
    }
    public List<Book> SearchByAuthor(string author)
    {
        return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public void DisplayAllBooks()
    {
        Console.WriteLine($"\n=== Books in {name} ===");
        foreach (var book in books)
        {
            book.DisplayInfo();
            Console.WriteLine();
        }
    }

    public void DisplayStatistics()
    {
        Console.WriteLine($"\n=== {name} Statistics ===");
        Console.WriteLine($"Total Books: {books.Count}");
        Console.WriteLine($"Available Books: {books.Count(b => b.IsAvailable)}");
        Console.WriteLine($"Borrowed Books: {books.Count(b => !b.IsAvailable)}");
        Console.WriteLine($"Total Members: {members.Count}");
        Console.WriteLine($"Total Librarians: {librarians.Count}");
    }

}


class Program
{
    static void Main(string[] args) 
    {
    Console.WriteLine("=== Library Management System - OOP Tutorial ===\n");

    // Create library instance
    Library cityLibrary = new Library("City Central Library");

    // Create books
    Book book1 = new Book("C# Programming", "John Smith", "ISBN001");
    Book book2 = new Book("Design Patterns", "Gang of Four", "ISBN002");
    Book book3 = new Book("Clean Code", "Robert Martin", "ISBN003");

    // Create librarian (demonstrating inheritance)
    Librarian librarian = new Librarian("Alice Johnson", 101, "Technical Books");
    cityLibrary.RegisterLibrarian(librarian);

    // Librarian adds books (demonstrating method interaction)
    librarian.AddBookToLibrary(cityLibrary, book1);
    librarian.AddBookToLibrary(cityLibrary, book2);
    librarian.AddBookToLibrary(cityLibrary, book3);

    // Create members (demonstrating inheritance)
    Member member1 = new Member("Bob Williams", 201);
    Member member2 = new Member("Carol Davis", 202);
    cityLibrary.RegisterMember(member1);
    cityLibrary.RegisterMember(member2);

    Console.WriteLine("\n--- Initial Library State ---");
    cityLibrary.DisplayAllBooks();

    // Demonstrate polymorphism
    Console.WriteLine("\n--- Demonstrating Polymorphism ---");
    List<Person> people = new List<Person> { librarian, member1, member2 };
    foreach (var person in people)
    {
        Console.WriteLine($"{person.Name} is a {person.GetRole()}");
        person.DisplayInfo();
        Console.WriteLine();
    }

    // Members borrow books
    Console.WriteLine("\n--- Borrowing Books ---");
    member1.BorrowBook(book1);
    member1.BorrowBook(book2);
    member2.BorrowBook(book3);

    // Display statistics
    cityLibrary.DisplayStatistics();

    // Demonstrate interface usage (searching)
    Console.WriteLine("\n--- Searching Books (Interface Implementation) ---");
    var searchResults = cityLibrary.SearchByTitle("code");
    Console.WriteLine($"Search results for 'code':");
    foreach (var book in searchResults)
    {
        book.DisplayInfo();
    }

    // Member returns a book
    Console.WriteLine("\n--- Returning Books ---");
    member1.ReturnBook(book1);

    // Final statistics
    cityLibrary.DisplayStatistics();

    Console.WriteLine("\n=== OOP Concepts Demonstrated ===");
    Console.WriteLine("1. ENCAPSULATION: Private fields with public properties");
    Console.WriteLine("2. INHERITANCE: Member and Librarian inherit from Person");
    Console.WriteLine("3. POLYMORPHISM: Override methods (DisplayInfo, GetRole)");
    Console.WriteLine("4. ABSTRACTION: Abstract Person class with abstract methods");
    Console.WriteLine("5. COMPOSITION: Library contains Books, Members, Librarians");
    Console.WriteLine("6. INTERFACES: ISearchable interface implementation");

    Console.ReadLine();
    }
}