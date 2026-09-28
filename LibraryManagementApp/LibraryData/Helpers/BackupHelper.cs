using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LibraryData.Helpers
{
    public class BackupHelper
    {
        public static void ExportBooksToJson(List<Book> books, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(books, options);
            File.WriteAllText(filePath, jsonString);
        }

        public static List<Book> ImportBooksFromJson(string filePath)
        {
            if (!File.Exists(filePath)) return new List<Book>();

            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<Book>>(jsonString);
        }
    }
}
