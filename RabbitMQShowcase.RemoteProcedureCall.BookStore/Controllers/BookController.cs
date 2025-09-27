using Microsoft.AspNetCore.Mvc;
using RabbitMQShowcase.RemoteProcedureCall.BookStore.Services;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Models;

namespace RabbitMQShowcase.RemoteProcedureCall.BookStore.Controllers
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
