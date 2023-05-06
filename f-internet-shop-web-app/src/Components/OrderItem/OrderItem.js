/* eslint-disable max-len */
/* eslint-disable no-unused-vars */
/* eslint-disable camelcase */
import React, {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import {Card, List, Descriptions, Image, Button} from 'antd';
import './Style.css';

// eslint-disable-next-line react/prop-types
const OrderItem = ({user, OrderItems, setOrderItems, removeOrderItem}) => {
  const [selectedUser, setSelectedUser] = useState(null);

  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  useEffect(() => {
    const getUserId = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/account/getid/`, requestOptions);
        const data = await response.json();
        console.log('Data:', data);
        setSelectedUser(data.userCode);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getUserId();
  }, []);

  useEffect(() => {
    if (selectedUser === null) {
      return;
    }

    const getOrderItems = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/OrderItem/${selectedUser}`, requestOptions);
        const data1 = await response.json();
        console.log('Data:', data1);
        setOrderItems(data1);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getOrderItems();
  }, [setOrderItems, selectedUser]);

  // eslint-disable-next-line camelcase
  const deleteOrderItem = async ({order_Item_Code}) => {
    const requestOptions = {
      method: 'DELETE',
    };
    // eslint-disable-next-line camelcase
    return await fetch('/api/OrderItem/${order_Item_Code}',
        requestOptions)

        .then((response) => {
          if (response.ok) {
            removeOrderItem(order_Item_Code);
          }
        },
        (error) => console.log(error),
        );
  };

  const handleButtonClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  return (
    <React.Fragment>
      <h3>Ваша корзина</h3>
      {OrderItems && OrderItems.length > 0 ? (
        <List
          grid={{
            gutter: 16,
            column: 1,
          }}
          dataSource={OrderItems}
          renderItem={({orderItemCode, amountOrderItem, productCodeNavigation}) => (
            <List.Item key={orderItemCode}>
              <Card
                title={`Количество товара: ${amountOrderItem}`}
                extra={
                  user.isAuthenticated && user.userRole !== 'admin' ? (
                    <button
                      onClick={() => deleteOrderItem({orderItemCode})}
                    >
                      Удалить
                    </button>
                  ) : (
                    ''
                  )
                }
              >
                {productCodeNavigation && (
                  <div
                    className="OrderItemText"
                    key={productCodeNavigation.productCode}
                    id={productCodeNavigation.productCode}
                  >
                    {/* <Image src={`data:image/jpeg;base64,${productCodeNavigation.image}`} width={200} /> */}
                    <Descriptions title={productCodeNavigation.nameProduct} size="small">
                      <Descriptions.Item label="Описание">
                        {
                productCodeNavigation.description.length > 20 ?
                productCodeNavigation.description.substring(0, 20) + '...' :
                productCodeNavigation.description
                        }
                      </Descriptions.Item>
                      <Descriptions.Item label="Гарантия">
                        {productCodeNavigation.bestBeforeDateProduct} Лет
                      </Descriptions.Item>
                      <Descriptions.Item label="Цена">
                        {productCodeNavigation.marketPriceProduct} Рублей
                      </Descriptions.Item>
                    </Descriptions>
                    <Button onClick={() => handleButtonClick(productCodeNavigation.productCode)} type="primary">Подробнее</Button>
                    <hr />
                  </div>
                )}
              </Card>
            </List.Item>
          )}
        />
      ) : (
        <p>Загрузка данных...</p>
      )}
    </React.Fragment>
  );
};

export default OrderItem;
