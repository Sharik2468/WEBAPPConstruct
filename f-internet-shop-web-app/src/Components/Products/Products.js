/* eslint-disable no-unused-vars */
/* eslint-disable max-len */
import React, {useEffect, useState, useContext} from 'react';
import {Card, Col, Row, Input, Dropdown, Image, Menu, Button, Pagination} from 'antd';
import {useNavigate} from 'react-router-dom';
import {DownOutlined, LoadingOutlined} from '@ant-design/icons';
import UserContext from '../Authorization/UserContext';

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
  const [currentPage, setCurrentPage] = useState(1);
  const [loading, setLoading] = useState(false);

  const {user} = useContext(UserContext);
  const navigate = useNavigate(); // используйте useNavigate вместо useHistory

  const itemsPerPage = 8; // Количество товаров на странице
  const isUserAdmin = user.userRole === 'admin';

  // Фильтруйте продукты, если пользователь не админ и numberInStock больше 0
  const filteredProducts = isUserAdmin ?
    products :
    products.filter((product) => product.numberInStock > 0);

  const handleCardClick = (productCode) => {
    navigate(`/products/${productCode}`);
  };

  const handleCategoryClick = (event) => {
    setSelectedCategory(event.item.props.label);
    console.log('Выбранная категория:', event.item.props.label);
  };

  const onChange = (page) => {
    console.log(page);
    setCurrentPage(page);
  };

  useEffect(() => {
    const getProducts = async () => {
      setLoading(true);
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
      } finally {
        setLoading(false); // Сбрасываем состояние загрузки после завершения загрузки
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

  // Функция для выбора товаров, которые нужно отобразить на текущей странице
  const getProductsToDisplay = (allProducts) => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return allProducts.slice(startIndex, endIndex);
  };

  // Выбираем товары для отображения на текущей странице
  const productsToDisplay = getProductsToDisplay(filteredProducts);

  return (
    <>
      <Row gutter={[16, 48]}>
        <Col>
          <Search
            placeholder="Введите поисковый запрос"
            allowClear
            enterButton="Search"
            size="large"
            onSearch={onSearch}
          />
          <p></p>
        </Col>
      </Row>
      <Row gutter={[16, 48]}>
        <Col>
          <Dropdown overlay={menu}>
            <Button>
            Категории <DownOutlined />
            </Button>
          </Dropdown>
          <p></p>
        </Col>
      </Row>
      <Row gutter={[16, 16]}>
        {loading ? (
    <div style={{display: 'flex', justifyContent: 'center', width: '100%'}}>
      <LoadingOutlined style={{fontSize: 24}} />
    </div>
  ) : (
    productsToDisplay.map((product) => (
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
          style={{width: '200px', height: '300px'}} // Задайте размеры здесь
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
        product.description.length > 20 ?
          product.description.substring(0, 20) + '...' :
          product.description
            }
          />
        </Card>

      </Col>
    ))
        )}
      </Row>
      <p></p>
      <Row gutter={[32, 16]} justify="center">
        <Pagination
          current={currentPage}
          onChange={onChange}
          pageSize={itemsPerPage}
          total={filteredProducts.length}
        />
      </Row>
    </>
  );
};

export default Product;
