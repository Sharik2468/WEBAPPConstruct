using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace InternetShopWebApp.Services
{
    public class OrderServices
    {
        private UnitOfWork _unitOfWork= new UnitOfWork();

        public List<OrderItemTable> GetAllActiveOrderItemsByClientId(int ClientId)
        {
            var order = GetActiveOrderByClientId(ClientId);
            var allOrderItems = _unitOfWork.OrderItemRepository.Get();
            var needOrderItems = order==null?null:from ord in allOrderItems where ord.OrderCode == order.OrderCode select ord;
            return needOrderItems.ToList();
        }

        public List<OrderItemTable> GetAllOrderItemByOrderId(int OrderId)
        {
            var allOrderItems = _unitOfWork.OrderItemRepository.Get();
            var needOrderItems = from ord in allOrderItems where ord.OrderCode == OrderId select ord;
            return needOrderItems.ToList();
        }

        public List<OrderTable> GetAllOrdersByClientId(int ClientId)
        {
            var c = _unitOfWork.OrderRepository.Get();
            var l = from ord in c where ord.ClientCode == ClientId select ord;
            return l.ToList();
        }
        public OrderTable GetActiveOrderByClientId(int ClientId)
        {
            var c = _unitOfWork.OrderRepository.Get();
            var l = from ord in c where ord.ClientCode == ClientId && ord.StatusCode == 1 select ord;
            return l.FirstOrDefault();
        }
    }
}
