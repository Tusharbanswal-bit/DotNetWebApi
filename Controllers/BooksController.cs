using Microsoft.AspNetCore.Mvc;
using webAPIApp.Services;
using webAPIApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace webAPIApp.Controllers {
    
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase {
        private readonly BookService _bookService;
        private readonly UserService _userService;

        public BooksController(BookService bookService, UserService userService) {
            _bookService = bookService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<Book>> Get() => await _bookService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id) {
            var book = await _bookService.GetAsync(id);
            if(book is null) {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook){
            string? userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            newBook.CreatedByUserId = userId;
            newBook.ModifiedByUserId = userId;
            newBook.CreatedOn = DateTime.Now;
            newBook.ModifiedOn = DateTime.Now;
            await _bookService.CreateAsync(newBook);
            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        } 

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Book updateBook) {
            var book = await _bookService.GetAsync(id);
            if(book is null) {
                return NotFound();
            }
            updateBook.Id = book.Id.ToString();
            updateBook.CreatedByUserId = book.CreatedByUserId;
            updateBook.ModifiedByUserId = book.ModifiedByUserId;
            updateBook.CreatedOn = book.CreatedOn;
            updateBook.ModifiedOn = DateTime.Now;
            await _bookService.UpdateAsync(id, updateBook);
            return CreatedAtAction(nameof(Get), new { id = updateBook.Id }, updateBook);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id) {
            await _bookService.DeleteAsync(id);
            return NoContent(); 
        }
    }
}