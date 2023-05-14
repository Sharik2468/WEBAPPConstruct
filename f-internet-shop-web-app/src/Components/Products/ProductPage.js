/* eslint-disable max-len */
import React from 'react';
import {Image, Button, notification, Slider, InputNumber, Row, Col} from 'antd';
import {useParams, useNavigate} from 'react-router-dom';
import {useState, useEffect, useContext} from 'react';
import UserContext from '../Authorization/UserContext';
import {ProCard} from '@ant-design/pro-components';

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
  const [Category, setCategory] = useState({});
  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  const addOrderItem = (newOrderItem) => {
    setOrderItems([...orderItems, newOrderItem]);
  };

  useEffect(() => {
    if (user === null) {
      return;
    }

    const getCategory = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch(`/api/Cathegory`, requestOptions);
        const usersData = await response.json();

        // Создаем словарь, который сопоставляет normalCode с пользователем
        const usersDict = {};
        for (const user of usersData) {
          usersDict[user.categoryId] = user;
        }

        console.log('Users:', usersDict);
        setCategory(usersDict);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getCategory();
  }, [setCategory, user]);

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
    <ProCard gutter={[16, 16]} >
      <ProCard colSpan="100%" title={product ? product.nameProduct : 'Loading...'} headerBordered bordered>
        {product ? (
          <>
            <ProCard gutter={[16, 16]}>
              <Image
                src={`data:image/jpeg;base64,${product.image}`}
              />
              <ProCard bordered title="Цена">{product.marketPriceProduct} ₽</ProCard>
            </ProCard>

            <ProCard bordered title="Описание">{product.description}</ProCard>

            <ProCard
              gutter={[{xs: 8, sm: 8, md: 16, lg: 24, xl: 32}, 16]}
              style={{marginBlockStart: 16}}
            >
              <ProCard bordered title="Количество в наличии">{product.numberInStock}</ProCard>
              <ProCard bordered title="Гарантия">{product.bestBeforeDateProduct}</ProCard>
              <ProCard bordered title="Категория">{Category[product.categoryId]?.categoryName || 'Загрузка...'}</ProCard>
            </ProCard>
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
        )}
      </ProCard>
    </ProCard>
  );
};
export default ProductPage;
