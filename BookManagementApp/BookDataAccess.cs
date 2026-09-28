using System;
using System.Collections.Generic;
using System.IO;

namespace BookManagementApp;

public class BookDataAccess
{
    private const string FilePath = "books.txt";
    private const string BackupPath = "books_backup.txt";
    public void AddBook(Book book)
    {
        using (FileStream fs = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            writer.WriteLine($"{book.Id}, {book.Title}, {book.Author}, {book.Price}");
        }
    }
    public List<Book> GetAllBooks()
    {
        List<Book> books = new List<Book>();

        if (!File.Exists(FilePath))
        {
            return books;
        }

        using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        using (StreamReader reader = new StreamReader(fs))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    Book book = new Book
                    {
                        Id = int.Parse(parts[0].Trim()),
                        Title = parts[1].Trim(),
                        Author = parts[2].Trim(),
                        Price = double.Parse(parts[3].Trim())
                    };
                    books.Add(book);
                }
            }
        }

        return books;
    }
    public Book FindBookById(int id)
    {
        List<Book> books = GetAllBooks();
        foreach (Book book in books)
        {
            if (book.Id == id)
            {
                return book;
            }
        }
        return null;
    }
    public bool CreateBackup()
    {
        if (!File.Exists(FilePath))
        {
            return false;
        }
        using (FileStream sourceStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
        using (FileStream destinationStream = new FileStream(BackupPath, FileMode.Create, FileAccess.Write))
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }
        }

        return true;
    }
}

