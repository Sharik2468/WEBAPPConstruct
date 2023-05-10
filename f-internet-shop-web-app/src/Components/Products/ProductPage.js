/* eslint-disable max-len */
// ProductPage.js
import React from 'react';
import {Descriptions, Typography, Image, Button, notification, Slider, InputNumber, Row, Col} from 'antd';
import {useParams, useNavigate} from 'react-router-dom';
import {useState, useEffect, useContext} from 'react';
import UserContext from '../Authorization/UserContext';

const {Title} = Typography;

const IntegerStep = ({onChange}) => {
  const [inputValue, setInputValue] = useState(1);

  const handleOnChange = (newValue) => {
    setInputValue(newValue);
    onChange(newValue);
  };

  return (
    <Row>
      <Col span={12}>
        <Slider
          min={1}
          max={20}
          onChange={handleOnChange}
          value={typeof inputValue === 'number' ? inputValue : 0}
        />
      </Col>
      <Col span={4}>
        <InputNumber
          min={1}
          max={20}
          style={{margin: '0 16px'}}
          value={inputValue}
          onChange={handleOnChange}
        />
      </Col>
    </Row>
  );
};


const ProductPage = () => {
  const {user} = useContext(UserContext);
  const {productCode} = useParams();
  const [product, setProduct] = useState(null);
  const [orderItems, setOrderItems] = useState([]);
  const [quantity, setQuantity] = useState(1);
  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  const addOrderItem = (newOrderItem) => {
    setOrderItems([...orderItems, newOrderItem]);
  };

  useEffect(() => {
    const getProduct = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Product/${productCode}`, requestOptions);
        const data = await response.json();
        console.log('Data:', data);

        setProduct(data);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getProduct();
  }, [productCode]);

  const addToCart = async () => {
    try {
      const requestOptions = {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
      };
      const response = await fetch(
          `/api/OrderItem/${user.userID}.${product.productCode}`,
          requestOptions,
      );

      console.log(response);

      if (response.ok) {
        const data = await response.json();
        console.log(data);
        addOrderItem(data);

        // Откройте оповещение
        notification.success({
          message: 'Товар добавлен в корзину',
          description: `${product.nameProduct} был успешно добавлен в вашу корзину.`,
        });
      } else {
        throw new Error(`Ошибка: ${response.statusText}`);
      }
    } catch (error) {
      console.log(error);
      notification.error({
        message: 'Ошибка',
        description: `Не удалось добавить товар в корзину: ${error.message}`,
      });
    }
  };

  // Функция для обновления количества товара
  const updateQuantity = async (newQuantity) => {
    console.log(`/api/Product/${product.productCode}/${newQuantity}`);
    try {
      const requestOptions = {
        method: 'PUT',
      };
      const response = await fetch(
          `/api/Product/${product.productCode}/${newQuantity}`,
          requestOptions,
      );

      if (response.ok) {
        const updatedProduct = {...product, numberInStock: newQuantity};
        setProduct(updatedProduct);

        notification.success({
          message: 'Количество товара обновлено',
          description: `Количество товара ${product.nameProduct} успешно обновлено.`,
        });
      } else {
        throw new Error(`Ошибка: ${response.statusText}`);
      }
    } catch (error) {
      console.log(error);
      notification.error({
        message: 'Ошибка',
        description: `Не удалось обновить количество товара: ${error.message}`,
      });
    }
  };

  const deleteProduct = async () => {
    try {
      const requestOptions = {
        method: 'DELETE',
      };
      const response = await fetch(`/api/Product/${productCode}`, requestOptions);

      if (response.ok) {
        notification.success({
          message: 'Товар удален',
          description: `Товар ${product.nameProduct} успешно удален.`,
        });
        // Направьте пользователя обратно на страницу со списком товаров, после успешного удаления
        navigate(`/products/`);
      } else {
        throw new Error(`Ошибка: ${response.statusText}`);
      }
    } catch (error) {
      console.log(error);
      notification.error({
        message: 'Ошибка',
        description: `Не удалось удалить товар: ${error.message}`,
      });
    }
  };


  return (
    product ? (
      <>
        <Title level={3}>{product.nameProduct}</Title>
        {/* <img
          src={product.imageUrl}
          alt={product.nameProduct}
          style={{width: '100%', maxHeight: '300px', objectFit: 'contain'}}
        /> */}
        <Image src={`data:image/jpeg;base64,${product.image}`} width={200} />
        <Descriptions layout="vertical" column={1}>
          <Descriptions.Item label="Описание">
            {product.description}
          </Descriptions.Item>
          <Descriptions.Item label="Цена">
            {product.marketPriceProduct} ₽
          </Descriptions.Item>
          <Descriptions.Item label="Количество">
            {product.numberInStock}
          </Descriptions.Item>
        </Descriptions>

        {user.userRole == 'user' ? (
            <Button type="primary" onClick={addToCart}>Добавить в корзину</Button>
          ) : (<></>
          )}

        {user.userID!=-1 && user.userRole=='admin' ? (
          <>
            <IntegerStep onChange={setQuantity} /> {/* Добавьте компонент IntegerStep */}
            <Button type="primary" onClick={() => updateQuantity(quantity)}>Обновить количество</Button>
            <Button type="danger" onClick={deleteProduct} style={{marginLeft: '16px'}}>
      Удалить товар
            </Button>
          </>
        ) : (
          <p></p>
        )}
      </>
    ) : (
      <div>Loading...</div>
    )
  );
};

export default ProductPage;
