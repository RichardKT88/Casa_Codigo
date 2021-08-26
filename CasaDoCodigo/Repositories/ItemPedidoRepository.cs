using CasaDoCodigo.Models;
using System.Linq;

namespace CasaDoCodigo.Repositories
{

    public class ItemPedidoRepository : BaseRepository<ItemPedido>, IItemPedidoRepository
    {
        public ItemPedidoRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public ItemPedido GetItemPedido(int itemPedidoId)
        {   
            return
            dbSet
               .Where(ip => ip.Id == itemPedidoId)
               .SingleOrDefault();
        }

        public void RemoveItemPedido(int itemPedidoId)
        {
            dbSet.Remove(GetItemPedido(itemPedidoId));
        }
    }
}
