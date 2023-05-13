using InternetShopWebApp.Models;
using InternetShopWebApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InternetShopWebApp.Services
{
    public class OrderServices
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        /// <summary>
        /// Возвращает строки заказа(товары из корзины) по активному заказу.
        /// </summary>
        /// <param name="ClientId">Идентификатор клиента. Конкретно NormalCode!</param>
        /// <returns></returns>
        public List<OrderItemTable> GetAllActiveOrderItemsByClientId(int ClientId)
        {
            var order = GetActiveOrderByClientId(ClientId);
            var allOrderItems = _unitOfWork.OrderItemRepository
                .Get(includeProperties: "ProductCodeNavigation");
            var needOrderItems = order == null ? null : from ord in allOrderItems where ord.OrderCode == order.OrderCode select ord;
            return needOrderItems.ToList();
        }

        public List<OrderItemTable> GetAllOrderItemByOrderId(int OrderId)
        {
            var allOrderItems = _unitOfWork.OrderItemRepository
                .Get();
            var needOrderItems = from ord in allOrderItems where ord.OrderCode == OrderId select ord;
            return needOrderItems.ToList();
        }

        public OrderTable GetOrderByOrderItemID(int OrderItemId)
        {
            var orderItems = _unitOfWork.OrderItemRepository
                .Get(includeProperties: "OrderCodeNavigation");
            var currentOrderItem = orderItems.Where(a => a.OrderItemCode == OrderItemId).FirstOrDefault();
            return currentOrderItem.OrderCodeNavigation;
        }
        /// <summary>
        /// Функция удалит строку заказа. Вернёт true если количество товара удалось уменьшить. И false удалит строку заказа
        /// </summary>
        /// <param name="OrderItemId"></param>
        /// <param name="NewAmount"></param>
        /// <returns></returns>
        public bool SetNewOrderItemAmount(int OrderItemId, int NewAmount)
        {
            var orderItem = _unitOfWork.OrderItemRepository.GetByID(OrderItemId);
            if (NewAmount > 0)
            {
                var currentProduct = _unitOfWork.ProductRepository.Get().Where(a => a.ProductCode == orderItem.ProductCode).FirstOrDefault();
                orderItem.AmountOrderItem = currentProduct.NumberInStock >= NewAmount ? NewAmount : currentProduct.NumberInStock;
                orderItem.OrderSum = orderItem.AmountOrderItem * currentProduct.MarketPriceProduct;

                _unitOfWork.OrderItemRepository.Update(orderItem);
                _unitOfWork.Save();

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<OrderTable> GetAllOrdersByClientId(int ClientId)
        {
            var c = _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables");
            var l = from ord in c where ord.ClientCode == ClientId select ord;
            return l.ToList();
        }
        public OrderTable GetActiveOrderByClientId(int ClientId)
        {
            var c = _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables");
            var l = from ord in c where ord.ClientCode == ClientId && ord.StatusCode == 1 select ord;
            return l.FirstOrDefault();
        }

        public OrderItemTable AddNewOrderItem(int ClientId, int ProductId)
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
                    var table = _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").LastOrDefault();
                    newOrderTable.OrderCode = table == null ? 0 :
                                              table.OrderCode + 1;

                    _unitOfWork.OrderRepository.Insert(newOrderTable);
                    currentOrder = newOrderTable;
                    _unitOfWork.Save();
                }
                var currentProduct = _unitOfWork.ProductRepository.Get().Where(a => a.ProductCode == ProductId).FirstOrDefault();

                var createdOrderItems = GetAllOrderItemByOrderId(currentOrder.OrderCode);
                var repeatProduct = createdOrderItems.Where(a => 
                a.ProductCode == currentProduct.ProductCode).FirstOrDefault();

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

                    return newOrderItem;
                }
                else
                {
                    int amount = (int)repeatProduct.AmountOrderItem;
                    repeatProduct.AmountOrderItem = currentProduct.NumberInStock >= amount+1 ? amount+1 : currentProduct.NumberInStock;
                    repeatProduct.OrderSum = repeatProduct.AmountOrderItem * currentProduct.MarketPriceProduct;

                    _unitOfWork.OrderItemRepository.Update(repeatProduct);
                    _unitOfWork.Save();

                    return repeatProduct;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        //Сервисы

        #region Services

        #region Orders
        public ActionResult<IEnumerable<OrderTable>> GetAllOrderService()
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").ToList();
        }

        public ActionResult<IEnumerable<StatusTable>> GetAllStatusServices()
        {
            return _unitOfWork.StatusOrderRepository.Get().ToList();
        }
        public StatusTable GetStatusByIDService(int id)
        {
            return _unitOfWork.StatusOrderRepository.GetByID(id);
        }
        public ActionResult<IEnumerable<OrderTable>> GetAllTreatmentOrderService()
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 1).ToList();
        }
        public ActionResult<IEnumerable<OrderTable>> GetAllPreparedOrderService()
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables")
                .Where(a => a.StatusCode == 2).ToList();
        }
        public ActionResult<IEnumerable<OrderTable>> GetAllPreparedUserOrderService(int clientid)
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables")
                .Where(a => a.StatusCode == 2 && a.ClientCode == clientid).ToList();
        }
        public ActionResult<IEnumerable<OrderTable>> GetAllUserOrderService(int clientid)
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables")
                .Where(a => a.ClientCode == clientid
                && (a.StatusCode == 3 || a.StatusCode == 4)).ToList();
        }
        public ActionResult<IEnumerable<OrderTable>> GetAllPaidOrderService()
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 3).ToList();
        }
        public ActionResult<IEnumerable<OrderTable>> GetAllCanceledOrderService()
        {
            return _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").Where(a => a.StatusCode == 4).ToList();
        }
        public OrderTable GetOrderByIDService(int id)
        {
            return _unitOfWork.OrderRepository.GetByID(id);
        }
        public void NewOrderService(OrderTable Order)
        {
            _unitOfWork.OrderRepository.Insert(Order);
            _unitOfWork.Save();
        }
        public bool PrepareOrderService(int id)
        {
            int currentStatus = 2;
            //_context.Entry(Order).State = EntityState.Modified;
            var currentOrder = _unitOfWork.OrderRepository.GetByID(id);

            bool isInsert = false;
            OrderTable newOrder = new OrderTable();
            newOrder.OrderFullfillment = currentOrder.OrderFullfillment;
            newOrder.OrderDate = currentOrder.OrderDate;
            newOrder.ClientCode = currentOrder.ClientCode;
            newOrder.SalesmanCode = currentOrder.SalesmanCode;
            newOrder.StatusCode = currentOrder.StatusCode;
            var table = _unitOfWork.OrderRepository.Get(includeProperties: "OrderItemTables").LastOrDefault();
            newOrder.OrderCode = table == null ? 0 :
                                      table.OrderCode + 1;

            var currentOrderRec = _unitOfWork.OrderRepository
                .Get(includeProperties: "OrderItemTables")
                .Where(a => a.OrderCode == id)
                .LastOrDefault();

            var currentOrderItemsRec = currentOrderRec.OrderItemTables.Where(a => a.StatusOrderItemTableId == 2);

            if (currentOrderRec.OrderItemTables.Count() == currentOrderItemsRec.ToList().Count())
                return false;

            List<OrderItemTable> OrderTableForRepalce = new List<OrderItemTable>();
            foreach (var item in currentOrderItemsRec)
            {
                var current = _unitOfWork.OrderItemRepository.GetByID(item.OrderItemCode);
                current.OrderCode = newOrder.OrderCode;
                OrderTableForRepalce.Add(current);
                isInsert = true;
            }

            if (isInsert)
            {
                _unitOfWork.OrderRepository.Insert(newOrder);
                foreach (var item in OrderTableForRepalce)
                {
                    item.OrderCodeNavigation = null;
                    _unitOfWork.OrderItemRepository.Update(item);
                }
                _unitOfWork.Save();
            }

            currentOrder.StatusCode = currentStatus;
            _unitOfWork.OrderRepository.Update(currentOrder);

            _unitOfWork.Save();

            return true;
        }
        public void PaidOrderService(int id, int clientid)
        {
            int currentStatus = 3;
            var currentOrder = _unitOfWork.OrderRepository.GetByID(id);

            var currentOrderRec = _unitOfWork.OrderRepository
                .Get(includeProperties: "OrderItemTables")
                .Where(a => a.OrderCode == id)
                .LastOrDefault();

            ProductTable product;
            foreach (var item in currentOrderRec.OrderItemTables)
            {
                product = _unitOfWork.ProductRepository.GetByID(item.ProductCode);
                product.NumberInStock -= item.AmountOrderItem;
                _unitOfWork.ProductRepository.Update(product);
            }

            currentOrder.SalesmanCode = clientid;
            currentOrder.OrderFullfillment = DateTime.Now;
            currentOrder.StatusCode = currentStatus;
            _unitOfWork.OrderRepository.Update(currentOrder);
            _unitOfWork.Save();
        }
        public void DeleteOrderService(int id)
        {
            int currentStatus = 4;
            //_context.Entry(Order).State = EntityState.Modified;
            var currentOrder = _unitOfWork.OrderRepository.GetByID(id);

            currentOrder.StatusCode = currentStatus;
            _unitOfWork.OrderRepository.Update(currentOrder);
            _unitOfWork.Save();
        }
        public void PutOrderService(OrderTable Order)
        {
            _unitOfWork.OrderRepository.Update(Order);
            _unitOfWork.Save();
        }
        public bool OrderExistService(int id)
        {
            return _unitOfWork.OrderRepository.GetByID(id) != null;
        }
        public bool FullDeleteOrderService(int id)
        {
            var Order = _unitOfWork.OrderRepository.GetByID(id);
            if (Order == null)
            {
                return false;
            }
            _unitOfWork.OrderRepository.Delete(Order);
            _unitOfWork.Save();
            return true;
        }
        #endregion

        #region OrderItems

        public ActionResult<IEnumerable<OrderItemTable>> GetAllOrderItemService()
        {
            return _unitOfWork.OrderItemRepository
                .Get(includeProperties: "ProductCodeNavigation,OrderCodeNavigation").ToList();
        }
        public ActionResult<IEnumerable<StatusOrderItemTable>> GetAllOrderItemStatusesService()
        {
            return _unitOfWork.StatusOrderItemRepository.Get().ToList();
        }
        public ActionResult<IEnumerable<OrderItemTable>> GetAllInStockOrderItemService()
        {
            return _unitOfWork.OrderItemRepository
                .Get(includeProperties: "ProductCodeNavigation,OrderCodeNavigation")
                .Where(a => a.StatusOrderItemTableId == 1).ToList();
        }
        public ActionResult<IEnumerable<OrderItemTable>> GetAllCanceledOrderItemService()
        {
            return _unitOfWork.OrderItemRepository
                .Get(includeProperties: "ProductCodeNavigation,OrderCodeNavigation")
                .Where(a => a.StatusOrderItemTableId == 2).ToList();
        }
        public StatusOrderItemTable GetOrderItemStatusByIDService(int id)
        {
            return _unitOfWork.StatusOrderItemRepository.GetByID(id);
        }
        public void PutClearOrderItemService(OrderItemTable OrderItem)
        {
            _unitOfWork.OrderItemRepository.Update(OrderItem);
            _unitOfWork.Save();
        }
        public void PutStatusIDForOrderItemService(int id, int status)
        {
            var OrderItem = _unitOfWork.OrderItemRepository.GetByID(id);
            OrderItem.StatusOrderItemTableId = status;
            _unitOfWork.OrderItemRepository.Update(OrderItem);
            _unitOfWork.Save();
        }
        public bool OrderItemExistService(int id)
        {
            return _unitOfWork.OrderItemRepository.Get().Any(e => e.OrderItemCode == id);
        }
        public bool DeleteOrderItemService(int id)
        {
            var order = GetOrderByOrderItemID(id);
            var orderitem = _unitOfWork.OrderItemRepository.GetByID(id);
            if (orderitem == null)
            {
                return false;
            }
            _unitOfWork.OrderItemRepository.Delete(orderitem);
            _unitOfWork.Save();

            if (order.OrderItemTables.Count() - 1 == 0)
            {
                order.OrderItemTables.Clear();
                _unitOfWork.OrderRepository.Delete(order);
                _unitOfWork.Save();
            }
            return true;
        }

        #endregion

        #endregion
    }
}
