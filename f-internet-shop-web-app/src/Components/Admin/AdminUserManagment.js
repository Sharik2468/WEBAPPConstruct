/* eslint-disable max-len */
import React, {useEffect, useState, useCallback} from 'react';
import {ProList} from '@ant-design/pro-components';
import {Button, Space, Tag} from 'antd';

const ManagmentUser = () => {
  // Fetch users and store in state
  const [fetchedUsers, setFetchedUsers] = useState([]);

  const fetchUsers = useCallback(async () => {
    // Fetch users data here
    // Replace with your actual API URL
    const response = await fetch('api/account/getuserswithroles');
    const usersData = await response.json();
    console.log(usersData);
    setFetchedUsers(usersData);
  }, []);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const updateUserRole = useCallback(async (userId, newRole) => {
    await fetch(`api/account/updateUserRole/${userId}/${newRole}`, {
      method: 'PUT',
    });
    // After updating the role, refetch the users
    fetchUsers();
  }, [fetchUsers]);

  // Prepare dataSource for ProList from fetchedUsers
  const dataSource = fetchedUsers.map((user) => ({
    name: user.userName,
    email: user.email,
    id: user.id,
    role: user.roles[0],
  }));

  return (
    <React.Fragment>
      <h3>Пользователи</h3>
      <ProList
        rowKey="id"
        headerTitle="Список пользователей"
        tooltip="Конфигурация базового списка"
        dataSource={dataSource}
        metas={{
          title: {
            dataIndex: 'name',
          },
          description: {
            dataIndex: 'email',
          },
          subTitle: {
            render: (_, record) => {
              return (
                <Space size={0}>
                  <Tag color={record.role=='admin'?('#5BD8A6'):('blue')}>{record.role}</Tag>
                </Space>
              );
            },
          },
          actions: {
            render: (text, record) => [
              <Button
                onClick={() => updateUserRole(record.id, record.role === 'admin' ? 'user' : 'admin')}
                key="link"
              >
                Сменить роль
              </Button>,
            ],
          },
        }}
      />
    </React.Fragment>
  );
};

export default ManagmentUser;
