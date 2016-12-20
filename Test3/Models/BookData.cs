using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

namespace Test3.Models
{
    public class BookData
    {
        private readonly string filename = "Test3.books.xml";
        Assembly _assembly = Assembly.GetExecutingAssembly();

        public BookData()
        {
            if (HttpContext.Current.Session["bookdata"] == null)
            {
                using (Stream stream = _assembly.GetManifestResourceStream(filename))
                {
                    var streamReader = new StreamReader(stream);
                    HttpContext.Current.Session["bookdata"] = streamReader.ReadToEnd();
                }
            }
        }

        public List<Book> GetBooks()
        {
            var bookdata = HttpContext.Current.Session["bookdata"].ToString();
            var books = new List<Book>();
            if (!string.IsNullOrEmpty(bookdata))
            {
                using (var xmlTextReader = new XmlTextReader(new StringReader(bookdata)))
                {
                    var nodeType = string.Empty;
                    Book currentEntry = null;
                    while (xmlTextReader.Read())
                    {
                        switch (xmlTextReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                nodeType = xmlTextReader.Name;
                                if (nodeType == "book")
                                {
                                    currentEntry = new Book();
                                    books.Add(currentEntry);
                                }
                                break;
                            case XmlNodeType.Text:
                                switch (nodeType)
                                {
                                    case "author":
                                        currentEntry.Author = xmlTextReader.Value;
                                        break;
                                    case "title":
                                        currentEntry.Title = xmlTextReader.Value;
                                        break;
                                }
                                break;
                            case XmlNodeType.EndElement:
                                nodeType = string.Empty;
                                break;
                        }
                    }
                }
            }
            return books;
        }


        public void Save(List<Book> books)
        {
            using (Stream stream = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stream))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("catalog");
                    books.ForEach(book =>
                    {
                        writer.WriteStartElement("book");
                        writer.WriteElementString("author", book.Author);
                        writer.WriteElementString("title", book.Title);
                        writer.WriteEndElement();
                    });
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                stream.Position = 0;
                HttpContext.Current.Session["bookdata"] = new StreamReader(stream).ReadToEnd();
            }

        }
        public List<Book> SortBooks(List<Book> books)
        {
            var sortedList = books.OrderBy(a => a.Author).ThenBy(t => t.Title).ToList();
            return sortedList;
        }
        public List<Book> GroupByG(List<Book> books)
        {
            var groupedList = books.GroupBy(b => b.Genre).SelectMany(grp => grp).ToList();
            Debug.WriteLine(groupedList.Count());
            return groupedList;
        }

    }
}