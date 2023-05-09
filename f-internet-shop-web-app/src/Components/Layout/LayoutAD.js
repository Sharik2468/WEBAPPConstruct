/* eslint-disable max-len */
import React, {useState, useContext} from 'react';
import {Outlet, Link} from 'react-router-dom';
import {Layout as LayoutAntd, Menu, Button} from 'antd';
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  HomeOutlined,
  LoginOutlined,
  ShopOutlined,
  UserAddOutlined,
  LogoutOutlined,
  ShoppingCartOutlined,
  FileAddOutlined,
  UserOutlined,
  ProfileOutlined,
} from '@ant-design/icons';
import './LayoutStyle.css';
import UserContext from '../Authorization/UserContext';

const {Header, Content, Footer, Sider} = LayoutAntd;
const {SubMenu} = Menu;

// const items = [
//   {
//     label: <Link to={'/'}>Главная</Link>,
//     key: '1',
//   },
//   {
//     label: <Link to={'/products'}>Товары</Link>,
//     key: '2',
//   },
//   {
//     label: <Link to={'/OrderItems'}>Строки заказа</Link>,
//     key: '3',
//   },
//   {
//     label: <Link to={'/login'}>Вход</Link>,
//     key: '4',
//   },
//   {
//     label: <Link to={'/logout'}>Выход</Link>,
//     key: '5',
//   },
//   {
//     label: <Link to={'/register'}>Регистрация</Link>,
//     key: '6',
//   },
// ];

const Layout = () => {
  const [collapsed, setCollapsed] = useState(false);
  const {user} = useContext(UserContext);

  return (
    <LayoutAntd>
      <Sider trigger={null} collapsible collapsed={collapsed} style={{height: 'auto'}}>
        <div className="logo" />
        <Menu theme="dark" mode="inline" defaultSelectedKeys={['1']}>
          <Menu.Item key="1" icon={<HomeOutlined />}>
            <Link to="/">Главная</Link>
          </Menu.Item>
          <Menu.Item key="2" icon={<ShopOutlined />}>
            <Link to="/products">Товары</Link>
          </Menu.Item>
          <Menu.Item key="3" icon={<ShoppingCartOutlined />}>
            <Link to="/OrderItems">Корзина</Link>
          </Menu.Item>
          <SubMenu key="sub1" icon={<UserOutlined />} title="Профиль">
            <Menu.Item key="4" icon={<LoginOutlined />}>
              <Link to="/login">Войти</Link>
            </Menu.Item>
            <Menu.Item key="5" icon={<UserAddOutlined />}>
              <Link to="/register">Регистрация</Link>
            </Menu.Item>
            <Menu.Item key="6" icon={<LogoutOutlined />}>
              <Link to="/logout">Выход</Link>
            </Menu.Item>
          </SubMenu>
          {user.userRole == 'admin' ? (
            <>
              <Menu.Item key="7" icon={<FileAddOutlined />}>
                <Link to="/addproduct">Добавить продукт</Link>
              </Menu.Item>
              <Menu.Item key="8" icon={<ProfileOutlined />}>
                <Link to="/ordermanagment">Управление заказами</Link>
              </Menu.Item>
            </>
          ) : (<p></p>
          )}
        </Menu>
      </Sider>
      <LayoutAntd className="inner-layout">
        <Header className="header">
          <Button
            style={{
              color: 'rgba(255, 255, 255, 0.65)',
              alignSelf: 'center',
            }}
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
          />
          <div style={{color: 'rgba(255, 255, 255, 0.65)'}}>
            {user.isAuthenticated != false ? (
              <strong>{user.userName}</strong>
            ) : (
              <strong>Гость</strong>
            )}
          </div>
        </Header>
        <Content className="content">
          <Outlet />
        </Content>
        <Footer style={{textAlign: 'center'}}>Internet Shop ©2023</Footer>
      </LayoutAntd>
    </LayoutAntd>
  );
};

export default Layout;
