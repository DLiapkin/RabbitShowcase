
using RabbitMQShowcase.RemoteProcedureCall.BookStore.Services;
using RabbitMQShowcase.RemoteProcedureCall.SDK;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Models;
using RabbitMQShowcase.RemoteProcedureCall.SDK.Abstraction.Services;

namespace RabbitMQShowcase.RemoteProcedureCall.BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddTransient<IClient<Book[]>, Client<Book[]>>();
            builder.Services.AddTransient<BookService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
