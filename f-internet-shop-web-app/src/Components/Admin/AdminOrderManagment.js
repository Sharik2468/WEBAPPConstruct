/* eslint-disable max-len */
import React, {useEffect, useState, useContext} from 'react';
import {Card, List, Descriptions, Button, Col, Row, Statistic} from 'antd';
import {useNavigate} from 'react-router-dom';
import UserContext from '../Authorization/UserContext';
import CountUp from 'react-countup';

const ManagmentOrder = () => {
  const {user} = useContext(UserContext);
  const [Orders, setOrders] = useState(1);
  const [Users, setUsers] = useState(1);
  const navigate = useNavigate(); // используйте useNavigate вместо useHistory
  const formatter = (value) => <CountUp end={value} separator="," />;

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getOrder = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Order/GetAllPreparedOrder`, requestOptions);
        const data1 = await response.json();
        console.log('Data:', data1);
        setOrders(data1);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getOrder();
  }, [setOrders, user]);

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getUser = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/account/getusers`, requestOptions);
        const usersData = await response.json();

        // Создаем словарь, который сопоставляет normalCode с пользователем
        const usersDict = {};
        for (const user of usersData) {
          usersDict[user.id] = user;
        }

        console.log('Users:', usersDict);
        setUsers(usersDict);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getUser();
  }, [setUsers, user]);


  const handleButtonClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  const handleStatusOrderChangeButtonClick = async (orderCode, newStatus) => {
    if (newStatus==3) {
      console.log(`/api/Order/PaidOrder/${orderCode}/${user.userID}`);

      const requestOptions = {
        method: 'PUT',
      };

      try {
        const response = await fetch(`/api/Order/PaidOrder/${orderCode}/${user.userID}`, requestOptions);
        if (response.ok) {
        // Найти элемент в массиве OrderItems с соответствующим orderCode
          const index = Orders.findIndex((item) => item.orderCode === orderCode);

          if (index !== -1) {
          // Создать копию массива OrderItems без элемента с индексом index
            const newOrders = [...Orders.slice(0, index), ...Orders.slice(index + 1)];

            // Обновить состояние OrderItems с новым массивом
            setOrders(newOrders);
          }
        }
      } catch (error) {
        console.log(error);
      }
    } else {
      console.log(`/api/Order/DeleteOrder/${orderCode}`);

      const requestOptions = {
        method: 'PUT',
      };

      try {
        const response = await fetch(`/api/Order/DeleteOrder/${orderCode}`, requestOptions);
        if (response.ok) {
        // Найти элемент в массиве OrderItems с соответствующим orderCode
          const index = Orders.findIndex((item) => item.orderCode === orderCode);

          if (index !== -1) {
          // Создать копию массива OrderItems без элемента с индексом index
            const newOrders = [...Orders.slice(0, index), ...Orders.slice(index + 1)];

            // Обновить состояние OrderItems с новым массивом
            setOrders(newOrders);
          }
        }
      } catch (error) {
        console.log(error);
      }
    }
  };

  return (
    <React.Fragment>
      <h3>Неоплаченные заказы</h3>
      {Orders && Orders.length > 0 && user.userID !== -1 && user.userRole === 'admin' ? (
        <List
          grid={{
            gutter: 16,
            column: 1,
          }}
          dataSource={Orders}
          renderItem={({orderCode, clientCode, orderItemTables}) => {
            const totalOrderSum = orderItemTables.reduce((sum, item) => sum + item.orderSum, 0);
            return (
              <List.Item key={orderCode}>
                <Card title={`Код заказа: ${orderCode}, 
                Почта клиента: ${Users[clientCode]?.userName || 'loading...'}`}>
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
                  <Row gutter={16}>
                    <Col>
                      <Button onClick={() => handleStatusOrderChangeButtonClick(orderCode, 3)} type="primary">
                          Заказ оплачен</Button>
                    </Col>
                    <Col>
                      <Button onClick={() => handleStatusOrderChangeButtonClick(orderCode, 4)} type="primary">
                          Отменить заказ</Button>
                    </Col>
                  </Row>
                </Card>
              </List.Item>
            );
          }}
        />
      ) : (
        <p>Все заказы оплачены...</p>
      )}
    </React.Fragment>
  );
};

export default ManagmentOrder;
