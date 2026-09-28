using BookManagementApp;
using System;
using System.Collections.Generic;

namespace BookManagementApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BookDataAccess dal = new BookDataAccess();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== BOOK MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. View All Books");
                Console.WriteLine("3. Find Book by ID");
                Console.WriteLine("4. Create Backup");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice (1-5): ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddBookUI(dal);
                        break;
                    case "2":
                        ViewAllBooksUI(dal);
                        break;
                    case "3":
                        FindBookByIdUI(dal);
                        break;
                    case "4":
                        CreateBackupUI(dal);
                        break;
                    case "5":
                        running = false;
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose between 1 and 5.");
                        break;
                }
            }
        }

        private static void AddBookUI(BookDataAccess dal)
        {
            Console.Write("Enter Book ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.Write("Enter Author: ");
            string author = Console.ReadLine();

            Console.Write("Enter Price: ");
            double price = double.Parse(Console.ReadLine());

            // Instantiating via constructor
            Book newBook = new Book(id, title, author, price);

            dal.AddBook(newBook);
            Console.WriteLine("Book added successfully!");
        }

        private static void ViewAllBooksUI(BookDataAccess dal)
        {
            List<Book> books = dal.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                return;
            }

            Console.WriteLine("--- Registered Books ---");
            foreach (Book b in books)
            {
                b.DisplayInfo();
            }
        }

        private static void FindBookByIdUI(BookDataAccess dal)
        {
            Console.Write("Enter Book ID to search: ");
            int searchId = int.Parse(Console.ReadLine());

            Book book = dal.FindBookById(searchId);

            if (book != null)
            {
                Console.WriteLine("\n--- Book Found ---");
                book.DisplayInfo();
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
        }

        private static void CreateBackupUI(BookDataAccess dal)
        {
            bool success = dal.CreateBackup();
            if (success)
            {
                Console.WriteLine("Backup created successfully.");
            }
            else
            {
                Console.WriteLine("Backup failed: Source file does not exist or has no records.");
            }
        }
    }
}
