/* eslint-disable no-unused-vars */
/* eslint-disable max-len */
import React, {useEffect, useState} from 'react';
import {Card, Col, Row} from 'antd';
import {useNavigate} from 'react-router-dom';

const {Meta} = Card;

const Product = () => {
  const [products, setProducts] = useState([]);

  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  const handleCardClick = (productCode) => {
    navigate(`/products/${productCode}`); // измените history.push на navigate
  };

  useEffect(() => {
    const getProducts = async () => {
      const requestOptions = {
        method: 'GET',
      };

      try {
        const response = await fetch('/api/Product/', requestOptions);
        const data = await response.json();
        console.log('Data:', data);

        const getImagePromises = data.map(async (product) => { // Исправлено здесь
          console.log('ProductCode:', product.ProductCode);
          const imageResponse = await fetch(`/api/Product/GetImage/${product.productCode}`);
          const imageBlob = await imageResponse.blob();
          const imageUrl = URL.createObjectURL(imageBlob);
          return {...product, imageUrl};
        });

        const productsWithImages = await Promise.all(getImagePromises);
        setProducts(productsWithImages);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getProducts();
  }, []);


  const deleteProduct = async (ProductCode) => {
    const requestOptions = {
      method: 'DELETE',
    };

    return await fetch(`/api/Product/${ProductCode}`, requestOptions)
        .then(
            (response) => {
              if (response.ok) {
                setProducts(products.filter((product) => product.ProductCode !== ProductCode));
              }
            },
            (error) => console.log(error),
        );
  };

  return (
    <Row gutter={[16, 16]}>
      {products.map((product) => (
        <Col
          key={product.ProductCode} // Добавьте ключ здесь
          xs={{
            span: 24,
            offset: 0,
          }}
          sm={{
            span: 12,
            offset: 0,
          }}
          md={{
            span: 8,
            offset: 0,
          }}
          lg={{
            span: 6,
            offset: 0,
          }}
        >
          <Card
            hoverable
            style={{width: '100%'}}
            cover={
              <img
                alt={product.NameProduct}
                src={product.imageUrl}
                style={{
                  maxWidth: '100%',
                  maxHeight: '200px',
                  objectFit: 'cover',
                }}
                onClick={() => handleCardClick(product.productCode)} // Добавьте обработчик onClick здесь
              />
            }
          >
            <Meta
              title={product.nameProduct}
              description={
                product.description.length > 20 ?
                  product.description.substring(0, 20) + '...' :
                  product.description
              }
            />
          </Card>
        </Col>
      ))}
    </Row>
  );
};

export default Product;
