using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    // A palavra reservada Where realiza um filtro na qual informa para o compilador que tipo de classe que ela irá receber, como generica.
    // Ou seja :
    //A BaseRepository ira receber somente classes na qual herdem da BaseModel
    public class BaseRepository<T> where T : BaseModel
    {
        protected readonly ApplicationContext context;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }
    }
}
