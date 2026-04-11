using Microsoft.AspNetCore.Mvc;
using RabbitShowcase.MessageBus.Example.BookStore.Services;
using RabbitShowcase.MessageBus.Example.SDK.Abstraction.Model;

namespace RabbitShowcase.MessageBus.Example.BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController(
        BookService _bookService) : ControllerBase
    {
        [HttpGet]
        public async Task<Book[]> GetAll()
        {
            return await _bookService.GetAll();
        }
    }
}
