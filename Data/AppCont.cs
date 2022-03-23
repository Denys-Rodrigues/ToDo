using Microsoft.EntityFrameworkCore;
using toDo.Models;

namespace toDo.Data
{
    public class AppCont : DbContext
    {
        public  AppCont(DbContextOptions<AppCont> options) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}