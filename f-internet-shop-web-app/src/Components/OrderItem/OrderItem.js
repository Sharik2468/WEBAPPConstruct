/* eslint-disable max-len */
/* eslint-disable no-unused-vars */
/* eslint-disable camelcase */
import React, {useEffect, useState, useContext} from 'react';
import {useNavigate} from 'react-router-dom';
import {Card, List, Descriptions, Image, Button, Col, InputNumber, Row, Slider, Space} from 'antd';
import './Style.css';
import UserContext from '../Authorization/UserContext';

const IntegerStep = ({maxValue, onSliderChange}) => {
  const [inputValue, setInputValue] = useState(1);
  const onChange = (newValue) => {
    setInputValue(newValue);
    onSliderChange(newValue);
  };
  return (
    <Row>
      <Col span={12}>
        <Slider
          min={1}
          max={maxValue}
          onChange={onChange}
          value={typeof inputValue === 'number' ? inputValue : 0}
        />
      </Col>
      <Col span={4}>
        <InputNumber
          min={1}
          max={maxValue}
          style={{
            margin: '0 16px',
          }}
          value={inputValue}
          onChange={onChange}
        />
      </Col>
    </Row>
  );
};

// eslint-disable-next-line react/prop-types
const OrderItem = ({OrderItems, setOrderItems, removeOrderItem}) => {
  const {user} = useContext(UserContext);
  const [sliderValue, setSliderValue] = useState(1);

  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getOrderItems = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/OrderItem/${user.userID}`, requestOptions);
        const data1 = await response.json();
        console.log('Data:', data1);
        setOrderItems(data1);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getOrderItems();
  }, [setOrderItems, user]);

  // eslint-disable-next-line camelcase
  const deleteOrderItem = async ({order_Item_Code}) => {
    const requestOptions = {
      method: 'DELETE',
    };
    console.log(`/api/OrderItem/${order_Item_Code}`);
    // eslint-disable-next-line camelcase
    return await fetch(`/api/OrderItem/${order_Item_Code}`,
        requestOptions)
        .then((response) => {
          if (response.ok) {
            removeOrderItem(order_Item_Code);
            setOrderItems((prevOrderItems) =>
              prevOrderItems.filter((item) => item.orderItemCode !== order_Item_Code),
            );
          }
        },
        (error) => console.log(error),
        );
  };

  const handleSliderChange = (newValue) => {
    setSliderValue(newValue);
  };

  const handleSliderButtonClick = async (orderItemCode) => {
    console.log(`/api/OrderItem/${orderItemCode}/${sliderValue}`);

    const requestOptions = {
      method: 'PUT',
    };

    try {
      const response = await fetch(`/api/OrderItem/${orderItemCode}/${sliderValue}`, requestOptions);
      if (response.ok) {
        // Найти элемент в массиве OrderItems с соответствующим orderItemCode
        const index = OrderItems.findIndex((item) => item.orderItemCode === orderItemCode);

        if (index !== -1) {
        // Создать копию массива OrderItems, чтобы не изменять его напрямую
          const newOrderItems = [...OrderItems];

          // Обновить amountOrderItem и orderSum для элемента с индексом index
          newOrderItems[index].amountOrderItem = sliderValue;
          newOrderItems[index].orderSum = sliderValue*OrderItems[index].productCodeNavigation.marketPriceProduct;

          // Обновить состояние OrderItems с новым массивом
          setOrderItems(newOrderItems);
        }
      }
    } catch (error) {
      console.log(error);
    }
  };

  const handleButtonClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  return (
    <React.Fragment>
      <h3>Ваша корзина</h3>
      {OrderItems && OrderItems.length > 0 && user.userID!=-1 && user.userRole=='user' ? (
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
                        {productCodeNavigation.marketPriceProduct*amountOrderItem} Рублей
                      </Descriptions.Item>
                    </Descriptions>
                    <Button onClick={() => handleButtonClick(productCodeNavigation.productCode)} type="primary">Подробнее</Button>
                    {user && user.isAuthenticated && user.userRole === 'user' && (
                      <>
                        <Button
                          type="text"
                          onClick={() => deleteOrderItem({order_Item_Code: orderItemCode})}
                        >
                      Удалить товар из корзины
                        </Button>
                        <IntegerStep
                          maxValue={productCodeNavigation.numberInStock}
                          onSliderChange={handleSliderChange}
                        />
                        <Button onClick={() => handleSliderButtonClick(orderItemCode)}>
                          Изменить количество товара</Button>
                      </>
                    )}
                    <hr />
                  </div>
                )}
              </Card>
            </List.Item>
          )}
        />
      ) : (
        <p>Корзина пуста...</p>
      )}
    </React.Fragment>
  );
};

export default OrderItem;
