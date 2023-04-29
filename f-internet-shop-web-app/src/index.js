/* eslint-disable no-unused-vars */
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
import Product from './Components/Products/Products';
import StartPage from './Components/Layout/StartPage';
import ProductPage from './Components/Products/ProductPage';

const App = () => {
  const [OrderItems, setOrderItems] = useState([]);
  const addOrderItem = (OrderItem) => setOrderItems([...OrderItems, OrderItem]);
  const removeOrderItem = (removeId) =>
    setOrderItems(OrderItems.filter(({order_Item_Code}) =>
      order_Item_Code !== removeId));

  const [user, setUser] = useState({isAuthenticated: false, userName: '', userRole: ''});

  const [Products, setProducts] = useState([]);
  const addProduct = (Product) => setProducts([...Products, Product]);
  const removeProduct = (removeId) =>
    setProducts(Products.filter(({ProductCode}) =>
      ProductCode !== removeId));

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout user={user} />}>
          <Route index element={
            <>
              <StartPage
                user={user}
                Products={Products}
                setProducts={setProducts}
                removeProduct={removeProduct}
              />
            </>
          } />
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
          <Route
            path="/products"
            element={
              <>
                <Product
                  user={user}
                  Products={Products}
                  setProducts={setProducts}
                  removeProduct={removeProduct}
                />
              </>
            }
          />
          <Route path="/products/:productCode" element={<ProductPage />} />
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
