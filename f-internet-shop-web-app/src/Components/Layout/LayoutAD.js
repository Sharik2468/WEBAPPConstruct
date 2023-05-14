/* eslint-disable max-len */
import React, {useState, useContext} from 'react';
import {Outlet, Link} from 'react-router-dom';
import {Layout as LayoutAntd, Menu, Button, Row, Col, Image} from 'antd';
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
  EditOutlined,
  DollarOutlined,
  IdcardOutlined,
} from '@ant-design/icons';
import './LayoutStyle.css';
import UserContext from '../Authorization/UserContext';

const {Header, Content, Footer, Sider} = LayoutAntd;
const {SubMenu} = Menu;

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
          {user.userRole == 'admin' ? (
            <SubMenu key="sub2" icon={<EditOutlined />} title="Управление магазином">
              <Menu.Item key="7" icon={<FileAddOutlined />}>
                <Link to="/addproduct">Добавить продукт</Link>
              </Menu.Item>
              <Menu.Item key="8" icon={<ProfileOutlined />}>
                <Link to="/ordermanagment">Управление</Link>
              </Menu.Item>
              <Menu.Item key="10" icon={<DollarOutlined />}>
                <Link to="/adminorderhistory">О продажах</Link>
              </Menu.Item>
              <Menu.Item key="11" icon={<IdcardOutlined />}>
                <Link to="/usermanagment">Пользователи</Link>
              </Menu.Item>
            </SubMenu>
          ) : (<></>
          )}
          {user.userRole == 'user' ? (
            <SubMenu key="sub3" icon={<EditOutlined />} title="Управление заказами">
              <Menu.Item key="3" icon={<ShoppingCartOutlined />}>
                <Link to="/OrderItems">Корзина</Link>
              </Menu.Item>
              <Menu.Item key="8" icon={<FileAddOutlined />}>
                <Link to="/userorderhistory">История покупок</Link>
              </Menu.Item>
              <Menu.Item key="9" icon={<ProfileOutlined />}>
                <Link to="/userordermanagment">Неоплаченные заказы</Link>
              </Menu.Item>
            </SubMenu>
          ) : (<></>
          )}
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
          <Row justify="center" align="middle">
            <Col>
              <Link to="/">
                <Image
                  src={process.env.PUBLIC_URL + '/ElectronixLogoNF.png'}
                  preview={false}
                />
              </Link>
            </Col>
          </Row>
          <p></p>
          <Row justify="center" align="middle">
            <Col span={24}>
              <Outlet />
            </Col>
          </Row>
        </Content>
        <Footer style={{textAlign: 'center'}}>Internet Shop ©2023</Footer>
      </LayoutAntd>
    </LayoutAntd>
  );
};

export default Layout;
