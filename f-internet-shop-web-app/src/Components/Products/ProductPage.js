/* eslint-disable max-len */
// ProductPage.js
import React from 'react';
import {Descriptions, Typography, Image, Button, notification} from 'antd';
import {useParams} from 'react-router-dom';
import {useState, useEffect, useContext} from 'react';
import UserContext from '../Authorization/UserContext';

const {Title} = Typography;

const ProductPage = () => {
  const {user} = useContext(UserContext);
  const {productCode} = useParams();
  const [product, setProduct] = useState(null);

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
    const requestOptions = {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
    };
    const response = await fetch(
        `/api/OrderItem/${user.userID}.${product.productCode}`,
        requestOptions,
    );

    console.log(response);
    return await response
        .json()
        .then(
            (data) => {
              console.log(data);
              if (response.ok) {
                addOrderItem(data);
                // Откройте оповещение
                notification.success({
                  message: 'Товар добавлен в корзину',
                  description: `${product.nameProduct} был успешно добавлен в вашу корзину.`,
                });
              }
            },
            (error) => console.log(error),
        );
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
        <Button type="primary" onClick={addToCart}>Добавить в корзину</Button>
      </>
    ) : (
      <div>Loading...</div>
    )
  );
};

export default ProductPage;
