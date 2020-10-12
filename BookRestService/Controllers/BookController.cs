using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookClassLibrary;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookRestService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private static List<Book> bList = new List<Book>()
        {
            new Book {Author = "Kurt Vonnegut", PageNumber = 302, Isbn13 = "0-385-28089-0", Title = "Breakfast of Champions"},
            new Book {Author = "Kurt Vonnegut", PageNumber = 215, Isbn13 = "0-385-31208-3", Title = "Slaughterhouse-Five"},
            new Book {Author = "Kurt Vonnegut", PageNumber = 304, Isbn13 = "0-385-33348-X", Title = "Cat's Cradle"},
            new Book {Author = "Frank Herbert", PageNumber = 412, Isbn13 = "978-044117271", Title = "Dune"},
            new Book {Author = "George Orwell", PageNumber = 328, Isbn13 = "978-045152493", Title = "1984"},
            new Book {Author = "Johnathan Franzen", PageNumber = 576, Isbn13 = "0-374-15846-0", Title = "Freedom"}
        };


        // GET: api/<BookController>
        [HttpGet]
        public List<Book> Get()
        {
            return bList;
        }

        // GET api/<BookController>/5
        [HttpGet("{isbn13}")]
        public IActionResult Get(string isbn13)
        {
            var book = GetBook(isbn13);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }
            return Ok(book);
        }

        // POST api/<BookController>
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            if (!BookExists(book.Isbn13))
            {
                bList.Add(book);
                return CreatedAtAction("Get", new { id = book.Isbn13 }, book);
            }
            else
            {
                return NotFound(new { message = "Isbn is duplicated" });
            }
        }

        // PUT api/<BookController>/5
        [HttpPut("{isbn13}")]
        public IActionResult Put(string isbn13, [FromBody] Book newBook)
        {
            if (isbn13 != newBook.Isbn13)
            {
                return BadRequest();
            }
            var currentBook = GetBook(isbn13);

            if (currentBook != null)
            {
                currentBook.Isbn13 = newBook.Isbn13;
                currentBook.Author = newBook.Author;
                currentBook.Title = newBook.Title;
                currentBook.PageNumber = newBook.PageNumber;
            }
            else
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE api/<BookController>/5
        [HttpDelete("{isbn13}")]
        public IActionResult Delete(string isbn13)
        {
            var book = GetBook(isbn13);

            if (book != null)
            {
                bList.Remove(book);
            }
            else
            {
                return NotFound();
            }
            return Ok(book);
        }

        public Book GetBook(string isbn13)
        {
            var book = bList.FirstOrDefault(e => e.Isbn13 == isbn13);
            return book;
        }
        private bool BookExists(string isbn13)
        {
            return bList.Any(e => e.Isbn13 == isbn13);
        }
    }
}
