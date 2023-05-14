/* eslint-disable max-len */
import React, {useEffect, useState, useContext} from 'react';
import {Card, List, Descriptions, Button, Col, Row, Statistic} from 'antd';
import {useNavigate} from 'react-router-dom';
import UserContext from '../Authorization/UserContext';
import CountUp from 'react-countup';

const UserHistoryOrder = () => {
  const {user} = useContext(UserContext);
  const [Orders, setOrders] = useState(1);
  const [Statuses, setStatuses] = useState(1);
  const navigate = useNavigate(); // используйте useNavigate вместо useHistory
  const formatter = (value) => <CountUp end={value} separator="," />;

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
        const response = await fetch(`/api/Order/GetAllUserOrder/${user.userID}`, requestOptions);
        const data1 = await response.json();
        console.log('Data:', data1);
        setOrders(data1);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getOrder();
  }, [setOrders, user]);

  const handleButtonClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  return (
    <React.Fragment>
      <h3>История заказов</h3>
      {Orders && Orders.length > 0 && user.userID !== -1 && user.userRole === 'user' ? (
        <List
          grid={{
            gutter: 16,
            column: 1,
          }}
          dataSource={Orders}
          renderItem={({orderCode, clientCode, orderItemTables, statusCode}) => {
            const totalOrderSum = orderItemTables.reduce((sum, item) => sum + item.orderSum, 0);
            return (
              <List.Item key={orderCode}>
                <Card title={`Код заказа: ${orderCode}, Статус заказа: ${Statuses[statusCode]?.orderStatus || 'Загрузка...'}`}>
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
      ) : (
        <p>Нет активных заказов...</p>
      )}
    </React.Fragment>
  );
};

export default UserHistoryOrder;
