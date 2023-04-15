/* eslint-disable camelcase */
/* eslint-disable react/jsx-no-undef */
/* eslint-disable max-len */
import React, {useState} from 'react';
import ReactDOM from 'react-dom/client';
import {BrowserRouter, Route, Routes} from 'react-router-dom';

import OrderItem from './Components/OrderItem/OrderItem';
import OrderItemCreate from './Components/OrderItemCreate/OrderItemCreate';
import Layout from './Components/Layout/LayoutAD';
import LogIn from './Components/Authorization/LogIn';
import Register from './Components/Authorization/Register';
import LogOut from './Components/Authorization/LogOut';

const App = () => {
  const [OrderItems, setOrderItems] = useState([]);
  const addOrderItem = (OrderItem) => setOrderItems([...OrderItems, OrderItem]);
  const removeOrderItem = (removeId) =>
    setOrderItems(OrderItems.filter(({order_Item_Code}) =>
      order_Item_Code !== removeId));
  const [user, setUser] = useState({isAuthenticated: false, userName: '', userRole: ''});

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout user={user} />}>
          <Route index element={<h3>Главная страница</h3>} />
          <Route
            path="/OrderItems"
            element={
              <>
                <OrderItemCreate user={user} addOrderItem={addOrderItem} />
                <OrderItem
                  user={user}
                  OrderItems={OrderItems}
                  setOrderItems={setOrderItems}
                  removeOrderItem={removeOrderItem}
                />
              </>
            }
          />
          <Route
            path="/login"
            element={<LogIn user={user} setUser={setUser} />}
          />
          <Route
            path="/register"
            element={<Register user={user} setUser={setUser} />}
          />
          <Route
            path="/logout"
            element={<LogOut user={user} setUser={setUser} />}
          />
          <Route path="*" element={<h3>404</h3>} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
      <App />,
    </React.StrictMode>,
);
