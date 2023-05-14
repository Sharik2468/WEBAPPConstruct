/* eslint-disable max-len */
import React, {useEffect, useState, useContext} from 'react';
import {Card, List, Descriptions, Button, Col, Row, Statistic} from 'antd';
import {ProCard} from '@ant-design/pro-components';
import {useNavigate} from 'react-router-dom';
import UserContext from '../Authorization/UserContext';
import CountUp from 'react-countup';

const AdminHistoryOrder = () => {
  const {user} = useContext(UserContext);
  const [OrdersHistory, setOrdersHistory] = useState([]);
  const [Statuses, setStatuses] = useState({});
  const [OrdersCount, setOrdersCount] = useState([]);
  const [Revenue, setRevenue] = useState([]);
  const [AllRevenue, setAllRevenue] = useState([]);
  const [selectedDate, setSelectedDate] = useState(null);
  const navigate = useNavigate();
  const formatter = (value) => <CountUp end={value} separator="," />;

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getAllRevenue = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Order/GetRevenue/${user.userID}`, requestOptions);
        const data1 = await response.json();
        setAllRevenue(data1);
        console.log('UniqueOrderCount:', data1);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getAllRevenue();
  }, [setAllRevenue, user]);

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getCountOrder = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Order/GetAllUniqueOrdersCount/${user.userID}`, requestOptions);
        const data1 = await response.json();
        setOrdersCount(data1);
        getRevenue(data1[0].item1);
        console.log('UniqueOrderCount:', OrdersCount);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getCountOrder();
  }, [setOrdersCount, user]);

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getStatus = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Order/GetAllStatuses`, requestOptions);
        const usersData = await response.json();

        // Создаем словарь, который сопоставляет normalCode с пользователем
        const usersDict = {};
        for (const user of usersData) {
          usersDict[user.statusCode] = user;
        }

        console.log('Users:', usersDict);
        setStatuses(usersDict);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getStatus();
  }, [setStatuses, user]);

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getOrder = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Order/GetAllAdminOrder/${user.userID}`, requestOptions);
        const data1 = await response.json();
        console.log('Data:', data1);
        setOrdersHistory(data1);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getOrder();
  }, [setOrdersHistory, user]);

  const handleButtonClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  const getRevenue = async (Date) => {
    const requestOptions = {
      method: 'GET',
    };

    try {
      const response = await fetch(`/api/Order/GetRevenue/${Date}/${user.userID}`, requestOptions);
      const data1 = await response.json();
      console.log('Value:', data1);

      setRevenue(data1);
    } catch (error) {
      console.log('Error:', error);
    }
  };

  return (
    <React.Fragment>
      <h3>Ваша история продаж</h3>
      {OrdersHistory && OrdersHistory.length > 0 && user.userID !== -1 && user.userRole === 'admin' ? (
        <>
          <Row gutter={[16, 16]}>
            <ProCard
              tabs={{
                onChange: (key) => {
                  if (key === 'all') {
                    setSelectedDate(null);
                  } else {
                    getRevenue(key);
                    setSelectedDate(key);
                  }
                },
                items: [
                  ...OrdersCount.map((item) => {
                    return {
                      key: item.item1,
                      style: {width: '100%'},
                      label: (
                        <Statistic
                          layout="vertical"
                          title={new Date(item.item1).toLocaleDateString()}
                          value={item.item2}
                          style={{
                            width: 120,
                            borderInlineEnd: '1px solid #f0f0f0',
                          }}
                        />
                      ),
                      children: (
                        <div
                          style={{
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            backgroundColor:
              '#fafafa',
                            height: 100,
                          }}
                        >
            Выручка за день {Revenue} рублей
                        </div>
                      ),
                    };
                  }),
                  {
                    key: 'all',
                    label: 'Все заказы',
                    children: (
                      <div style={{
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        backgroundColor:
          '#fafafa',
                        height: 100,
                      }}>
                        <p>Общая выручка со всех заказов: {AllRevenue} рублей</p>
                      </div>
                    ),
                  },
                ],
              }}
            />

          </Row>
          <p></p>
          <Row gutter={[16, 16]}>
            <List
              grid={{
                gutter: 16,
                column: 1,
              }}
              dataSource={OrdersHistory.filter((order) => {
                // If no date is selected, show all orders
                if (!selectedDate) return true;

                // If a date is selected, only show orders from that date
                const orderDate = new Date(order.orderFullfillment);
                const selectedOrderDate = new Date(selectedDate);

                return orderDate.getDate() === selectedOrderDate.getDate() &&
                       orderDate.getMonth() === selectedOrderDate.getMonth() &&
                       orderDate.getFullYear() === selectedOrderDate.getFullYear();
              })}
              renderItem={({orderCode, clientCode, orderItemTables, statusCode, orderFullfillment}) => {
                const totalOrderSum = orderItemTables.reduce((sum, item) => sum + item.orderSum, 0);

                // Переводим дату из строки в объект Date
                const date = new Date(orderFullfillment);

                // Форматируем дату
                const formattedDate = `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()}`;

                return (
                  <List.Item key={orderCode}>
                    <Card title={`Код заказа: ${orderCode}, Дата оплаты заказа: ${formattedDate} Статус заказа: ${Statuses[statusCode]?.orderStatus || 'Загрузка...'}`}>
                      {orderItemTables &&
                    orderItemTables.map((item) => (
                      <div className="OrderItemText" key={item.productCode} id={item.productCode}>
                        <Descriptions title={`Код товара: ${item.productCode}`} size="small">
                          <Descriptions.Item label="Количество товара">
                            {item.amountOrderItem} штук
                          </Descriptions.Item>
                          <Descriptions.Item label="Сумма заказа">{item.orderSum} рублей</Descriptions.Item>
                        </Descriptions>
                        <Button onClick={() => handleButtonClick(item.productCode)} type="primary">
                          Подробнее о товаре</Button>
                        <hr />
                      </div>
                    ))}
                      <Row gutter={16}>
                        <Col span={12}>
                          <Statistic title="Общая сумма заказа(руб)" value={totalOrderSum} formatter={formatter}/>
                        </Col>
                      </Row>
                    </Card>
                  </List.Item>
                );
              }}
            />
          </Row>
        </>

      ) : (
        <p>История пуста...</p>
      )}
    </React.Fragment>
  );
};

export default AdminHistoryOrder;
