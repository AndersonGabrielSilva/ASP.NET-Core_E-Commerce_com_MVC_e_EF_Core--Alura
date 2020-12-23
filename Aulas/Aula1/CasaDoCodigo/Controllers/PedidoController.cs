using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private IProdutoRepository produtoRepository;
        private IPedidoRepository pedidoRepository;
        public PedidoController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
        }


        public IActionResult Carrossel()
        {
            //Injetando os Livros dentro da View
            return View(produtoRepository.GetProdutos());
        }

        public IActionResult Carrinho(string codigo)
        {
            if (!String.IsNullOrEmpty(codigo))
            {
                pedidoRepository.AddItem(codigo);
            }

            Pedido pedido = pedidoRepository.GetPedido();
    
            return View(pedido.Itens);
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Resumo()
        {
            Pedido pedido = pedidoRepository.GetPedido();

            return View(pedido);
        }

        [HttpPost]
        public void UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {

        }
    }
}
