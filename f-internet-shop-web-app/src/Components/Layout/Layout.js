/* eslint-disable require-jsdoc */
/* eslint-disable max-len */
/* eslint-disable react/jsx-no-undef */
/* eslint-disable react/react-in-jsx-scope */
import React from 'react';
import {Outlet, Link} from 'react-router-dom';
import {Layout as LayoutAntd} from 'antd';

const {Header, Content, Footer} = LayoutAntd;

const items = [
  {
    label: <Link to={'/'}>Главная</Link>,
    key: '1',
  },
  {
    label: <Link to={'/OrderItems'}>Строки заказа</Link>,
    key: '2',
  },
  {
    label: <Link to={'/login'}>Вход</Link>,
    key: '3',
  },
  {
    label: <Link to={'/logout'}>Выход</Link>,
    key: '4',
  },
  {
    label: <Link to={'/register'}>Регистрация</Link>,
    key: '5',
  },
];

const Layout = ({user}) => {
  return (
    <>
      <div>
        {user.isAuthenticated ? (
          <h4>Пользователь: {user.userName}</h4>
        ) : (
          <h4>Пользователь: Гость</h4>
        )}
      </div>
      <nav>
        <Link to="/">Главная</Link> <span> </span>
        <Link to="/OrderItems">Строки заказа</Link> <span> </span>
        <Link to="/login">Вход</Link> <span> </span>
        <Link to="/register">Зарегистрироваться</Link> <span> </span>
        <Link to="/logout">Выход</Link> <span> </span>
      </nav>
      <Outlet />
    </>
  );
};
export default Layout;
