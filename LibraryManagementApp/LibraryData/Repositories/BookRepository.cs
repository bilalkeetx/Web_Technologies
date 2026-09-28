using LibraryData.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
namespace LibraryData.Repositories
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddBook(Book book)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Books (Title, Author, Stock) VALUES (@Title, @Author, @Stock)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT BookId, Title, Author, Stock FROM Books";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            BookId = Convert.ToInt32(reader["BookId"]),
                            Title = reader["Title"].ToString(),
                            Author = reader["Author"].ToString(),
                            Stock = Convert.ToInt32(reader["Stock"])
                        });
                    }
                }
            }
            return books;
        }

        public void UpdateStock(int bookId, int newStock)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Books SET Stock = @Stock WHERE BookId = @BookId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Stock", newStock);
                cmd.Parameters.AddWithValue("@BookId", bookId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveBook(int bookId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Books WHERE BookId = @BookId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookId", bookId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}