using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace InternetShopWebApp.Services
{
    public class OrderServices
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public List<OrderItemTable> GetAllActiveOrderItemsByClientId(int ClientId)
        {
            var order = GetActiveOrderByClientId(ClientId);
            var allOrderItems = _unitOfWork.OrderItemRepository.Get();
            var needOrderItems = order == null ? null : from ord in allOrderItems where ord.OrderCode == order.OrderCode select ord;
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

        public bool AddNewOrderItem(int ClientId, int ProductId)
        {
            try
            {
                var currentOrder = GetActiveOrderByClientId(ClientId);
                if (currentOrder == null)
                {
                    OrderTable newOrderTable = new OrderTable();
                    newOrderTable.OrderFullfillment = null;
                    newOrderTable.OrderDate = DateTime.Now;
                    newOrderTable.ClientCode = ClientId;
                    newOrderTable.SalesmanCode = ClientId;
                    newOrderTable.StatusCode = 1;
                    newOrderTable.StatusCodeNavigation = _unitOfWork.StatusOrderRepository.GetByID(1);
                    var table = _unitOfWork.OrderRepository.Get().LastOrDefault();
                    newOrderTable.OrderCode = table == null ? 0 :
                                              table.OrderCode + 1;

                    _unitOfWork.OrderRepository.Insert(newOrderTable);
                    currentOrder = newOrderTable;
                    _unitOfWork.Save();
                }
                var currentProduct = _unitOfWork.ProductRepository.Get().Where(a => a.ProductCode == ProductId).FirstOrDefault();

                var createdOrderItems = GetAllOrderItemByOrderId(currentOrder.OrderCode);
                var repeatProduct = createdOrderItems.Where(a => a.ProductCode == currentProduct.ProductCode).FirstOrDefault();
                if (createdOrderItems == null || repeatProduct == null)
                {
                    var newOrderItem = new OrderItemTable();
                    newOrderItem.AmountOrderItem = 1;
                    newOrderItem.OrderCode = currentOrder.OrderCode;
                    newOrderItem.OrderSum = currentProduct.MarketPriceProduct;
                    newOrderItem.ProductCode = currentProduct.ProductCode;
                    newOrderItem.StatusOrderItemTableId = 1;
                    newOrderItem.OrderCodeNavigation = currentOrder;
                    newOrderItem.ProductCodeNavigation = currentProduct;
                    newOrderItem.StatusOrderItemTable = _unitOfWork.StatusOrderItemRepository.GetByID(1);
                    var tab = _unitOfWork.OrderItemRepository.Get().LastOrDefault();
                    newOrderItem.OrderItemCode = tab == null ? 0 :
                                                 tab.OrderItemCode + 1;

                    _unitOfWork.OrderItemRepository.Insert(newOrderItem);
                    _unitOfWork.Save();
                }
                else
                {
                    int amount = (int)repeatProduct.AmountOrderItem;
                    repeatProduct.AmountOrderItem = amount + 1;
                    repeatProduct.OrderSum = repeatProduct.AmountOrderItem * currentProduct.MarketPriceProduct;

                    _unitOfWork.OrderItemRepository.Update(repeatProduct);
                    _unitOfWork.Save();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
