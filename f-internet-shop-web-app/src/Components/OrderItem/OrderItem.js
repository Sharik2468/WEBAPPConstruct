import React, {useEffect} from 'react';
import './Style.css';

// eslint-disable-next-line react/prop-types
const OrderItem = ({user, OrderItems, setOrderItems, removeOrderItem}) => {
  useEffect(() => {
    const getOrderItems = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch('https://localhost:7194/api/OrderItem/', requestOptions);
        const data = await response.json();
        console.log('Data:', data);
        setOrderItems(data);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getOrderItems();
  }, [setOrderItems]);

  // eslint-disable-next-line camelcase
  const deleteOrderItem = async ({order_Item_Code}) => {
    const requestOptions = {
      method: 'DELETE',
    };
    // eslint-disable-next-line camelcase
    return await fetch(`https://localhost:7194/api/OrderItem/${order_Item_Code}`,
        requestOptions)

        .then((response) => {
          if (response.ok) {
            removeOrderItem(order_Item_Code);
          }
        },
        (error) => console.log(error),
        );
  };

  return (
    <React.Fragment>
      <h3>Список строк заказа</h3>
      {OrderItems && OrderItems.length > 0 ? (
        // eslint-disable-next-line camelcase, max-len
        OrderItems.map(
            ({
              order_Item_Code,
              order_Sum,
              amount_Order_Item,
              product_Code,
              order_Code,
              status_Order_Item_Table_ID,
              products,
            }) => (
            // eslint-disable-next-line camelcase
              <div
                className="OrderItem"
                key={order_Item_Code}
                id={order_Item_Code}
              >
                <strong>
                  {order_Item_Code}: {amount_Order_Item}
                </strong>

                {user.isAuthenticated ? (
                <button onClick={() => deleteItem({blogId})}>Удалить</button>
              ) : (
                ''
              )}

                {products &&
                products.map(
                    ({
                      product_Code,
                      orderItem_Code,
                      numberInStock,
                      categoryID,
                      dateOfManufacture,
                      description,
                      purchasePrice,
                      marketPrice,
                      bestBeforeDate,
                      name,
                      productOrderItem,
                    }) => (
                      <div
                        className="OrderItemText"
                        key={product_Code}
                        id={product_Code}
                      >
                        {name} <br />
                        {description}
                        <hr />
                      </div>
                    ),
                )}
              </div>
            ),
        )
      ) : (
        <p>Загрузка данных...</p>
      )}
    </React.Fragment>
  );
};

export default OrderItem;
