/* eslint-disable no-unused-vars */
/* eslint-disable max-len */
import React, {useEffect, useState} from 'react';
import {Card, Col, Row, Carousel, Image} from 'antd';
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

        data.reverse();
        setProducts(data);
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
    <>
      <Row gutter={[8, 48]}>
        <Col>
          <Carousel autoplay>
            <div>
              <h3 style={{
                height: '300px',
                color: '#fff',
                lineHeight: '300px',
                textAlign: 'center',
                background: '#364d79'}}>
                <Image
                  src={process.env.PUBLIC_URL + '/slider1.png'}
                  width={1010}
                  height={300}
                  preview={false}
                />
              </h3>
            </div>

            <div>
              <h3 style={{
                height: '300px',
                color: '#fff',
                lineHeight: '300px',
                textAlign: 'center',
                background: '#364d79'}}>
                <Image
                  src={process.env.PUBLIC_URL + '/slider2.jpg'}
                  width={1010}
                  height={300}
                  preview={false}
                />
              </h3>
            </div>
            <div>
              <h3 style={{
                height: '300px',
                color: '#fff',
                lineHeight: '300px',
                textAlign: 'center',
                background: '#364d79'}}>
                <Image
                  src={process.env.PUBLIC_URL + '/slider3.jpg'}
                  width={1010}
                  height={300}
                  preview={false}
                />
              </h3>
            </div>
            <div>
              <h3 style={{
                height: '300px',
                color: '#fff',
                lineHeight: '300px',
                textAlign: 'center',
                background: '#364d79'}}>
                <Image
                  src={process.env.PUBLIC_URL + '/slider4.png'}
                  width={1010}
                  height={300}
                  preview={false}
                /></h3>
            </div>
          </Carousel>
        </Col>
      </Row>
      <Row style={{display: 'flex', justifyContent: 'center', alignItems: 'center', textAlign: 'center'}}>
        <h1>Новые товары</h1>
      </Row>
      <Row gutter={[16, 16]}>
        {products.slice(0, 4).map((product) => (
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
              onClick={() => handleCardClick(product.productCode)}
              style={{width: '100%'}}
              cover={
                <div
                  style={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    height: '200px',
                    overflow: 'hidden',
                  }}
                >
                  <Image
                    src={`data:image/jpeg;base64,${product.image}`}
                    width={200}
                    height={200}
                    preview={false}
                  />
                </div>
              }
            >
              <Meta
                title={product.nameProduct}
                description={
                product.description.length > 50 ?
                  product.description.substring(0, 50) + '...' :
                  product.description
                }
              />
            </Card>
          </Col>
        ))}
      </Row>
    </>
  );
};

export default Product;
