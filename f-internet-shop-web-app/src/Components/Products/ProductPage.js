/* eslint-disable max-len */
// ProductPage.js
import React from 'react';
import {Descriptions, Typography} from 'antd';
import {useParams} from 'react-router-dom';
import {useState, useEffect} from 'react';

const {Title} = Typography;

const ProductPage = () => {
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

        const imageResponse = await fetch(`/api/Product/GetImage/${productCode}`, requestOptions);
        const imageBlob = await imageResponse.blob();
        const imageUrl = URL.createObjectURL(imageBlob);

        setProduct({...data, imageUrl});
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getProduct();
  }, [productCode]);

  return (
    product ? (
      <>
        <Title level={3}>{product.nameProduct}</Title>
        <img
          src={product.imageUrl}
          alt={product.nameProduct}
          style={{width: '100%', maxHeight: '300px', objectFit: 'contain'}}
        />
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
      </>
    ) : (
      <div>Loading...</div>
    )
  );
};

export default ProductPage;
