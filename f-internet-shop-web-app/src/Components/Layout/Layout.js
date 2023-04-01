import React from 'react';
import {Outlet, Link} from 'react-router-dom';
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
