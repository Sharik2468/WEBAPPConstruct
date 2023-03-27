import React, { useState } from 'react'
import ReactDOM from "react-dom/client"
import OrderItem from './Components/OrderItem/OrderItem'
import OrderItemCreate from './Components/OrderItemCreate/OrderItemCreate'

const App = () => {
  const [OrderItems, setOrderItems] = useState([])
  const addOrderItem = (OrderItem) => setOrderItems([...OrderItems, OrderItem])
  const removeOrderItem = (removeId) => setOrderItems(OrderItems.filter(({ order_Item_Code }) => order_Item_Code
!== removeId));

  return (
    <div>
      <OrderItemCreate
        OrderItem={addOrderItem}
      />
      <OrderItem
        OrderItems={OrderItems}
        setOrderItems={setOrderItems}
        removeBlog={removeOrderItem}
      />
    </div>
  )
}

const root = ReactDOM.createRoot(document.getElementById("root"))
root.render(
  // <React.StrictMode>
  <App />
  // </React.StrictMode>
)