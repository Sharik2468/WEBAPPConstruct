/* eslint-disable max-len */
/* eslint-disable no-unused-vars */
/* eslint-disable camelcase */
import React, {useEffect, useState, useContext} from 'react';
import {useNavigate} from 'react-router-dom';
import {Card, List, Descriptions, Button, Col, InputNumber, Row, Slider, Statistic, notification, Modal} from 'antd';
import './Style.css';
import UserContext from '../Authorization/UserContext';
import CountUp from 'react-countup';
import {
  ExclamationCircleFilled,
} from '@ant-design/icons';

const {confirm} = Modal;

const formatter = (value) => <CountUp end={value} separator="," />;

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
  const [statusOrderItem, setStatusOrderItem] = useState(1);

  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getStatusOrderItems = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/OrderItem/GetAllOrderItemStatuses`, requestOptions);
        const data = await response.json();
        console.log('Data:', data);
        setStatusOrderItem(data);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getStatusOrderItems();
  }, [user]); // добавьте user в список зависимостей


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

  const handleStatusChangeButtonClick = async (orderCode) => {
    console.log(`/api/Order/PrepareOrder/${orderCode}`);

    const requestOptions = {
      method: 'PUT',
    };

    try {
      const response = await fetch(`/api/Order/PrepareOrder/${orderCode}`, requestOptions);
      if (response.ok) {
        // Откройте оповещение
        notification.success({
          message: 'Теперь вы можете оплатить заказ',
          description: `Ваш номер заказа: ${orderCode}. Предоставьте его у кассы, чтобы оплатить заказ.`,
        });

        // Создать новый массив, содержащий только элементы с statusOrderItemTableId, отличным от '1'
        const newOrderItems = OrderItems.filter((item) => String(item.statusOrderItemTableId) !== '1');

        // Обновить состояние OrderItems с новым массивом
        setOrderItems(newOrderItems);
      }
    } catch (error) {
      console.log(error);
      notification.error({
        message: 'Ошибка',
        description: `Не удалось оформить заказ: ${error.message}`,
      });
    }
  };


  const handleStatusOrderItemChangeButtonClick = async (orderItemCode, newStatus) => {
    console.log(`/api/OrderItem/PutNewStatusID/${orderItemCode}/${newStatus}`);

    const requestOptions = {
      method: 'PUT',
    };

    try {
      const response = await fetch(`/api/OrderItem/PutNewStatusID/${orderItemCode}/${newStatus}`, requestOptions);
      if (response.ok) {
        // Найти элемент в массиве OrderItems с соответствующим orderItemCode
        const index = OrderItems.findIndex((item) => item.orderItemCode === orderItemCode);

        if (index !== -1) {
          // Создать копию массива OrderItems, чтобы не изменять его напрямую
          const newOrderItems = [...OrderItems];

          // Обновить statusOrderItemTableId для элемента с индексом index
          newOrderItems[index].statusOrderItemTableId = newStatus;

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

  const getTotalCost = () => {
    return OrderItems.reduce((total, item) => {
      if (String(item.statusOrderItemTableId) === '1') {
        return total + item.orderSum;
      } else {
        return total;
      }
    }, 0);
  };

  const showConfirm = ({order_Item_Code}) => {
    confirm({
      title: 'Вы действительно хотите удалить товар из корзины?',
      icon: <ExclamationCircleFilled />,
      content: 'Действие необратимо.',
      async onOk() {
        try {
          await deleteOrderItem({order_Item_Code});
          console.log('OK');
        } catch (error) {
          console.error(error);
        }
      },
      onCancel() {
        console.log('Cancel');
      },
    });
  };


  return (
    <React.Fragment>
      <h3>Ваша корзина</h3>
      {OrderItems && OrderItems.length > 0 && user.userID!=-1 && user.userRole=='user' ? (
        <>
          <Row gutter={16}>
            <List
              grid={{
                gutter: 16,
                column: 1,
              }}
              dataSource={OrderItems}
              renderItem={({orderItemCode, amountOrderItem, productCodeNavigation, statusOrderItemTableId}) => (
                <List.Item key={orderItemCode}>
                  <Card
                    title={`Количество товара: ${amountOrderItem}, Статус товара: ${statusOrderItem && statusOrderItem[statusOrderItemTableId - 1] ? statusOrderItem[statusOrderItemTableId - 1].statusOrderItemTable1 : 'неизвестно'}`}
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


                        <Button style={{marginRight: '10px'}} onClick={() => handleButtonClick(productCodeNavigation.productCode)} type="primary">Подробнее</Button>
                        <Button
                          type="text"
                          style={{marginRight: '10px'}}
                          onClick={() => showConfirm({order_Item_Code: orderItemCode})}
                        >
  Удалить товар из корзины
                        </Button>


                        <IntegerStep
                          maxValue={productCodeNavigation.numberInStock}
                          onSliderChange={handleSliderChange}
                        />

                        <Button style={{marginRight: '10px'}} onClick={() => handleSliderButtonClick(orderItemCode)}>
                          Изменить количество товара</Button>
                        <Button style={{marginRight: '10px'}} onClick={() => handleStatusOrderItemChangeButtonClick(orderItemCode, statusOrderItemTableId==1?2:1)}>
                          Изменить состояние товара</Button>


                        <hr />
                      </div>
                    )}
                  </Card>
                </List.Item>
              )}
            />
          </Row>
          <Row gutter={16}>
            <Col span={6}>
              <Statistic
                title="Общая стоимость заказов"
                value={getTotalCost()}
                formatter={formatter}
              />
            </Col>
            <Col span={6}>
              <Button type="primary" onClick={() => handleStatusChangeButtonClick(OrderItems[0].orderCode)}>
                          Оформить заказ</Button>
            </Col>
          </Row>
        </>
      ) : (
        <p>Корзина пуста...</p>
      )}
    </React.Fragment>
  );
};

export default OrderItem;
