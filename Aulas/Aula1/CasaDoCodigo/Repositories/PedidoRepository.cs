using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
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
        UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido);
        Pedido UpdateCadastro(Cadastro cadastro);

    }

    public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICadastroRepository cadastroRepository;

        public PedidoRepository(ApplicationContext context, 
            IHttpContextAccessor httpContextAccessor,
            IItemPedidoRepository itemPedidoRepository, 
            ICadastroRepository cadastroRepository) : base(context)
        {
            this.contextAccessor = httpContextAccessor;
            this.itemPedidoRepository = itemPedidoRepository;
            this.cadastroRepository = cadastroRepository;
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
                              .Include(X => X.Cadastro)
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

        public UpdateQuantidadeResponse UpdateQuantidade(ItemPedido itemPedido)
        {
            var itemPedidoDB = itemPedidoRepository.GetItemPedido(itemPedido.Id);

            if (itemPedidoDB == null)
                throw new ArgumentException("ItemPedido não encontrado!");

            itemPedidoDB.AtualizaQuantidade(itemPedido.Quantidade);

            if (itemPedidoDB.Quantidade == 0)
                itemPedidoRepository.RemoveItemPedido(itemPedidoDB.Id);

            context.SaveChanges();

            var carrinhoViewModel = new CarrinhoViewModel(GetPedido().Itens);

            return new UpdateQuantidadeResponse(itemPedidoDB, carrinhoViewModel);
        }

        public Pedido UpdateCadastro(Cadastro cadastro)
        {
            var pedido = GetPedido();
            cadastroRepository.Update(pedido.Cadastro.Id, cadastro);

            return pedido;
        }
    }
}

