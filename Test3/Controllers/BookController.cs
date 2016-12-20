using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Test3.Models;
using System.Web.Helpers;
using System.Diagnostics;
using System;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;

namespace Test3.Controllers
{
    [System.Web.Http.RoutePrefix("api/book")]
    public class BookController : ApiController
    {
        [System.Web.Http.Route("catalog")]
        public IEnumerable<Book> GetAllBooks()
        {
            var bookData = new BookData();
            return bookData.GetBooks();
        }

        [System.Web.Http.Route("add")]
        public IEnumerable<Book> AddBook()
        {
            var data = Request.Content.ReadAsStringAsync();
            var newBook = JObject.Parse(data.Result);
            var bookData = new BookData();

            var books = bookData.GetBooks();
            var book = new Book();
            book.Author = newBook["author"].ToString();
            book.Title = newBook["title"].ToString();
            var isAdd = true;
            if (books.Exists(b => b.Author.Contains(book.Author) && b.Title.Contains(book.Title)))
            {
                isAdd = false;
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            }
            books.Add(book);
            bookData.Save(books);

            return bookData.GetBooks();
        }

        //public JsonResult AddBook()
        //{
        //    var data = Request.Content.ReadAsStringAsync();
        //    var newBook = JObject.Parse(data.Result);
        //    var bookData = new BookData();

        //    var books = bookData.GetBooks();
        //    var book = new Book();
        //    book.Author = newBook["author"].ToString();
        //    book.Title = newBook["title"].ToString();
        //    var isAdd = true;
        //    var mess = "";
        //    if (books.Exists(b => b.Author.Contains(book.Author) && b.Title.Contains(book.Title)))
        //    {
        //        isAdd = false;
        //        mess = "book exists";

        //    }
        //    books.Add(book);
        //    bookData.Save(books);
        //    var jsonO = new
        //    {
        //        isAdded = isAdd,
        //        message = mess,
        //        bookList = books
        //    };



        //return  Json(jsonO,JsonRequestBehavior.AllowGet);
        //}

        [System.Web.Http.Route("edit")]
        public IEnumerable<Book> EditBook()
        {
            var data = Request.Content.ReadAsStringAsync();
            var newBook = JObject.Parse(data.Result);
            var bookData = new BookData();
            var books = bookData.GetBooks();            
            var a = newBook["author"].ToString();
            var t = newBook["title"].ToString();
            var isEd = true;
            if (!books.Exists(b => b.Author.Contains(a) && b.Title.Contains(t)))
            {
                isEd = false;
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var booktoEdit = books.Find(b => b.Title.Equals(t) && b.Author.Equals(a));
            booktoEdit.Price = newBook["price"].ToString();
            booktoEdit.Date = newBook["date"].ToString();
            booktoEdit.Genre = newBook["genre"].ToString();
            booktoEdit.Description = newBook["description"].ToString();
            bookData.Save(books);

            return bookData.GetBooks();
        }
        [System.Web.Http.Route("sort")]
        public IEnumerable<Book> SortBook()
        {
            var bookData = new BookData();
            return bookData.SortBooks(bookData.GetBooks());
        }
        [System.Web.Http.Route("delete")]
        public  IEnumerable<Book> RemoveBook()
        {
            var data = Request.Content.ReadAsStringAsync();
            var newBook = JObject.Parse(data.Result);
            var bookData = new BookData();
            var books = bookData.GetBooks();
            var a = newBook["author"].ToString().Trim();
            var t = newBook["title"].ToString().Trim();
            if (!books.Exists(b => b.Author.Contains(a) && b.Title.Contains(t)))
            {
                
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var book = books.Find(x => x.Author.Equals(a) && x.Title.Equals(t));
            books.Remove(book);
            bookData.Save(books);
            return bookData.GetBooks();

        }
        

        [System.Web.Http.Route("groupby")]
        public IEnumerable<Book> GroupBy()
        {
            var bookData = new BookData();
            return bookData.GroupByG(bookData.GetBooks());
            
        }
    }
}