/* eslint-disable no-unused-vars */
/* eslint-disable max-len */
import React, {useEffect, useState} from 'react';
import {Card, Col, Row, Input, Dropdown, Image, Menu, Button} from 'antd';
import {useNavigate} from 'react-router-dom';
import {DownOutlined} from '@ant-design/icons';

const {Search} = Input;
const {Meta} = Card;

const items = [
  {
    key: '1',
    label: 'Бытовая техника',
    children: [
      {
        key: '1-1',
        label: 'Техника для кухни',
      },
      {
        key: '1-2',
        label: 'Техника для дома',
      },
      {
        key: '1-3',
        label: 'Красота и здоровье',
      },
    ],
  },
  {
    key: '2',
    label: 'Смартфоны',
    children: [
      {
        key: '2-1',
        label: 'Смартфоны',
      },
      {
        key: '2-2',
        label: 'Планшеты',
      },
      {
        key: '2-3',
        label: 'Фототехника',
      },
    ],
  },
  {
    key: '3',
    label: 'Телевизоры',
    children: [
      {
        key: '3-1',
        label: 'Телевизоры и аксессуары',
      },
      {
        key: '3-2',
        label: 'Консоли',
      },
    ],
  },
  {
    key: '4',
    label: 'Компьютеры',
    children: [
      {
        key: '4-1',
        label: 'Компьютеры',
      },
      {
        key: '4-2',
        label: 'Ноутбуки',
      },
      {
        key: '4-3',
        label: 'Комплектующие для ПК',
      },
    ],
  },
  {
    key: '5',
    label: 'Без категории',
  },
];

const Product = () => {
  const [products, setProducts] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);

  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  const handleCardClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  const handleCategoryClick = (event) => {
    setSelectedCategory(event.item.props.label);
    console.log('Выбранная категория:', event.item.props.label);
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

        setProducts(data);
      } catch (error) {
        console.log('Error:', error);
      }
    };

    getProducts();
  }, []);

  const onSearch = async (value) => {
    const requestOptions = {
      method: 'GET',
    };

    try {
      console.log('Request:', `/api/Product/Search/${selectedCategory}.${value}`);
      const response = await fetch(`/api/Product/Search/${selectedCategory}.${value}`, requestOptions);
      const data = await response.json();
      console.log('Data:', data);
      setProducts(data);
    } catch (error) {
      console.log('Error:', error);
    }
  };

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

  // Создайте меню с обработчиком onClick
  const renderMenuItems = (items) => {
    return items.map((item) => {
      if (item.children) {
        return (
          <Menu.SubMenu key={item.key} title={item.label}>
            {renderMenuItems(item.children)}
          </Menu.SubMenu>
        );
      }
      return (
        <Menu.Item key={item.key} label={item.label}>
          {item.label}
        </Menu.Item>
      );
    });
  };

  const menu = (
    <Menu onClick={handleCategoryClick}>
      {renderMenuItems(items)}
    </Menu>
  );

  return (
    <>
      <Row gutter={[16, 48]}>
        <Search
          placeholder="Введите поисковый запрос"
          allowClear
          enterButton="Search"
          size="large"
          onSearch={onSearch}
        />
      </Row>
      <Row gutter={[16, 48]}>
        <Dropdown overlay={menu}>
          <Button>
            Категории <DownOutlined />
          </Button>
        </Dropdown>
      </Row>
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
                  />
                </div>
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
    </>
  );
};

export default Product;
