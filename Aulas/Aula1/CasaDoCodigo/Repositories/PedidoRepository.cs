using CasaDoCodigo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IPedidoRepository
    {
        Pedido GetPedido();
        void AddItem(string codigo);
    }

    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;

        public PedidoRepository(ApplicationContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.contextAccessor = httpContextAccessor;
        }

        public void AddItem(string codigo)
        {
            var produto = context.Set<Produto>()
                                 .Where(p => p.Codigo == codigo)
                                 .SingleOrDefault();

            if (produto == null)
                throw new ArgumentException("Produto não encontrado!");

            var pedido = GetPedido();

            var itemPedido = context.Set<ItemPedido>()
                                    .Where(ip => ip.Produto.Codigo == codigo
                                    && ip.Pedido.Id == pedido.Id)
                                    .SingleOrDefault();

            if(itemPedido == null)
            {
                context.Set<ItemPedido>().Add(new ItemPedido(pedido, produto, 1, produto.Preco));

                context.SaveChanges();
            }
        }

        public Pedido GetPedido()
        {
            var pedidoId = GetPedidoId();
            var pedido = dbSet
                              .Include(p => p.Itens)
                              .ThenInclude(i => i.Produto)
                              .Where(x => x.Id == pedidoId)
                              .SingleOrDefault();

            if(pedido == null)
            {
                pedido = new Pedido();
                dbSet.Add(pedido);
                context.SaveChanges();
                SetPedidoId(pedido.Id);
            }

            return pedido;
        }

        public int? GetPedidoId()
        {
            return contextAccessor.HttpContext.Session.GetInt32("pedidoId");
        }

        private void SetPedidoId(int pedidoId)
        {
            contextAccessor.HttpContext.Session.SetInt32("pedidoId", pedidoId);
        }
    }
}
