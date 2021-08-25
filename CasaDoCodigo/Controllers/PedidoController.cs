using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository produtoRepository;
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;
        private readonly ICategoriaRepository categoriaRepository;

        public PedidoController(IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository, IItemPedidoRepository itemPedidoRepository, ICategoriaRepository categoriaRepository)
        {
            this.produtoRepository = produtoRepository;
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
            this.categoriaRepository = categoriaRepository;
        }
        public async Task<IActionResult> Carrossel()
        {
            return View(await produtoRepository.GetProdutos());
        }
        public IActionResult Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                pedidoRepository.AddItem(codigo);
            }
            //Pedido pedido = pedidoRepository.GetPedido().Itens;
            List<ItemPedido> itens = pedidoRepository.GetPedido().Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }
        public IActionResult Cadastro()
        {
            var pedido = pedidoRepository.GetPedido();
            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }
            return View(pedido.Cadastro);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                return View(pedidoRepository.UpdateCadastro(cadastro));

            }
            return RedirectToAction("Cadastro");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public UpdateQuantidadeResponse UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {
            return pedidoRepository.UpdateQuantidade(itemPedido);
        }

        //Overload no método GetProdutos para aceitar uma string de pesquisa
        public async Task<IActionResult> BuscaDeProdutos(string pesquisa)
        {
            //Adaptando a action de busca de produtos para aceitar um parâmetro de pesquisa
            //Senão estiver passando dados para a pesquisa
            if (string.IsNullOrWhiteSpace(pesquisa))
            {
                var viewPesquisaEmBranco = new PesquisaViewModel(await produtoRepository.GetProdutos(), true);
                return View(viewPesquisaEmBranco);
            }

            //Retorna a view em branco senão encontrar dados
            if (produtoRepository.GetProdutos(pesquisa).Result.Count == 0)
            {
                var viewEmBranco = new PesquisaViewModel(false);
                return View(viewEmBranco);
            }

            //Realizar a busca por texto
            var viewPesquisaPorTexto = new PesquisaViewModel(await produtoRepository.GetProdutos(pesquisa), true);

            return View(viewPesquisaPorTexto);
        }
    }
}
