using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    public partial class Startup
    {
        class DataService : IDataService
        {
            private readonly ApplicationContext context;
            private readonly IProdutoRepository produtoRepository;

            public DataService(ApplicationContext context, IProdutoRepository produtoRepository)
            {
                this.context = context;
                this.produtoRepository = produtoRepository;
            }

            public void InicializaDB()
            {
                context.Database.EnsureCreated();

                List<Livro>  livros = GetLivros();

                produtoRepository.SaveProdutos(livros);
            }                      

            private static List<Livro> GetLivros()
            {
                var json = File.ReadAllText("livros.json");

                return JsonConvert.DeserializeObject<List<Livro>>(json);
            }
        }

      
    }
}
