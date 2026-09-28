using System;
using System.Collections.Generic;
using LibraryData.Models;
using LibraryData.Repositories;
using LibraryData.Helpers;

namespace LibraryApp
{
    internal class Program
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LibraryDB;Integrated Security=True;";

        static void Main(string[] args)
        {
            BookRepository bookRepo = new BookRepository(ConnectionString);
            MemberRepository memberRepo = new MemberRepository(ConnectionString);
            IssueRepository issueRepo = new IssueRepository(ConnectionString);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== LIBRARY MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Add Book               2. View All Books");
                Console.WriteLine("3. Add Member             4. View All Members");
                Console.WriteLine("5. Issue Book (Tx)        6. Return Book (Tx)");
                Console.WriteLine("7. Export Books (JSON)    8. Import Books (JSON)");
                Console.WriteLine("9. Exit");
                Console.Write("Enter option (1-9): ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Title: ");
                        string title = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Author: ");
                        string author = Console.ReadLine() ?? string.Empty;
                        Console.Write("Enter Stock: ");
                        int.TryParse(Console.ReadLine(), out int stock);
                        bookRepo.AddBook(new Book { Title = title, Author = author, Stock = stock });
                        Console.WriteLine("Book added successfully.");
                        break;

                    case "2":
                        var books = bookRepo.GetAllBooks();
                        foreach (var b in books)
                            Console.WriteLine($"ID: {b.BookId} | Title: {b.Title} | Author: {b.Author} | Stock: {b.Stock}");
                        break;

                    case "3":
                        Console.Write("Enter Member Name: ");
                        string name = Console.ReadLine() ?? string.Empty;
                        memberRepo.AddMember(new Member { Name = name });
                        Console.WriteLine("Member added successfully.");
                        break;

                    case "4":
                        var members = memberRepo.GetAllMembers();
                        foreach (var m in members)
                            Console.WriteLine($"ID: {m.MemberId} | Name: {m.Name}");
                        break;

                    case "5":
                        Console.Write("Enter Book ID: ");
                        int.TryParse(Console.ReadLine(), out int bId);
                        Console.Write("Enter Member ID: ");
                        int.TryParse(Console.ReadLine(), out int mId);
                        if (issueRepo.IssueBook(bId, mId))
                            Console.WriteLine("Book issued successfully!");
                        else
                            Console.WriteLine("Failed to issue book (Out of stock or invalid ID).");
                        break;

                    case "6":
                        Console.Write("Enter Issue ID: ");
                        int.TryParse(Console.ReadLine(), out int issueId);
                        if (issueRepo.ReturnBook(issueId))
                            Console.WriteLine("Book returned successfully!");
                        else
                            Console.WriteLine("Failed to return book (Invalid ID or already returned).");
                        break;

                    case "7":
                        BackupHelper.ExportBooksToJson(bookRepo.GetAllBooks(), "books_backup.json");
                        Console.WriteLine("Exported to books_backup.json successfully.");
                        break;

                    case "8":
                        var importedBooks = BackupHelper.ImportBooksFromJson("books_backup.json");
                        foreach (var b in importedBooks)
                            Console.WriteLine($"Title: {b.Title} | Current Stock: {b.Stock}");
                        break;

                    case "9":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
