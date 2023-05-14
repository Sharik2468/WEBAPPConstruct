/* eslint-disable no-unused-vars */
/* eslint-disable camelcase */
/* eslint-disable react/jsx-no-undef */
/* eslint-disable max-len */
import React, {useState, useEffect} from 'react';
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
import AddProductForm from './Components/Products/AddProduct';
import UserContext from './Components/Authorization/UserContext';
import ManagmentOrder from './Components/Admin/AdminOrderManagment';
import UserManagmentOrder from './Components/User/GetAllPreparedOrderUser';
import UserHistoryOrder from './Components/User/UserHistory';
import AdminHistoryOrder from './Components/Admin/AdminOrderHistory';
import ManagmentUser from './Components/Admin/AdminUserManagment';

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

  const initializeUser = async () => {
    const requestOptions = {
      method: 'GET',
    };

    let data;

    try {
      const response = await fetch(`/api/account/isauthenticated`, requestOptions);
      data = await response.json();
      console.log('Data:', data);
    } catch (error) {
      console.log('Error:', error);
    }
    setUser(data);
  };

  useEffect(() => {
    initializeUser();
  }, []);

  return (
    <BrowserRouter>
      <UserContext.Provider value={{user, setUser}}>
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
                  {/* <OrderItemCreate user={user} addOrderItem={addOrderItem} /> */}
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
              path="/addproduct"
              element={<AddProductForm />}
            />
            <Route
              path="/ordermanagment"
              element={<ManagmentOrder />}
            />
            <Route
              path="/userordermanagment"
              element={<UserManagmentOrder />}
            />
            <Route
              path="/userorderhistory"
              element={<UserHistoryOrder />}
            />
            <Route
              path="/adminorderhistory"
              element={<AdminHistoryOrder />}
            />
            <Route
              path="/usermanagment"
              element={<ManagmentUser />}
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
            <Route path="/products/:productCode" element={<ProductPage user={user} />} />
            <Route path="*" element={<h3>404</h3>} />
          </Route>
        </Routes>
      </UserContext.Provider>
    </BrowserRouter>
  );
};

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
      <App />,
    </React.StrictMode>,
);
