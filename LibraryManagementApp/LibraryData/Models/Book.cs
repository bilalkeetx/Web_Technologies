using System;
using System.Collections.Generic;
using System.Text;
namespace LibraryData.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
