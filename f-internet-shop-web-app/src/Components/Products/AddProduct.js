/* eslint-disable require-jsdoc */
/* eslint-disable max-len */
import React, {useState} from 'react';
import {
  Button,
  Cascader,
  DatePicker,
  Form,
  Input,
  InputNumber,
  notification,
  Upload,
} from 'antd';
import {PlusOutlined} from '@ant-design/icons';

const {TextArea} = Input;


const items = [
  {
    id: 1,
    key: '1',
    label: 'Бытовая техника',
    children: [
      {
        id: 5,
        key: '1-1',
        label: 'Техника для кухни',
      },
      {
        id: 6,
        key: '1-2',
        label: 'Техника для дома',
      },
      {
        id: 7,
        key: '1-3',
        label: 'Красота и здоровье',
      },
    ],
  },
  {
    id: 2,
    key: '2',
    label: 'Смартфоны',
    children: [
      {
        id: 8,
        key: '2-1',
        label: 'Смартфоны',
      },
      {
        id: 9,
        key: '2-2',
        label: 'Планшеты',
      },
      {
        id: 10,
        key: '2-3',
        label: 'Фототехника',
      },
    ],
  },
  {
    id: 3,
    key: '3',
    label: 'Телевизоры',
    children: [
      {
        id: 11,
        key: '3-1',
        label: 'Телевизоры и аксессуары',
      },
      {
        id: 12,
        key: '3-2',
        label: 'Консоли',
      },
    ],
  },
  {
    id: 4,
    key: '4',
    label: 'Компьютеры',
    children: [
      {
        id: 13,
        key: '4-1',
        label: 'Компьютеры',
      },
      {
        id: 14,
        key: '4-2',
        label: 'Ноутбуки',
      },
      {
        id: 15,
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

const AddProductForm = () => {
  const [form] = Form.useForm();
  const [imageData, setImageData] = useState(null);

  // Функция для поиска категории по ключу
  function findCategoryById(items, key) {
    for (const item of items) {
      if (item.key === key) {
        return item;
      }

      if (item.children) {
        const foundItem = findCategoryById(item.children, key);
        if (foundItem) {
          return foundItem;
        }
      }
    }

    return null;
  }

  const onFinish = async (values) => {
    try {
      const category = findCategoryById(items, values.category[0]);
      const categoryId = category ? category.id : null;

      const subcategory = values.category.length === 2 ? findCategoryById(items, values.category[1]) : null;
      const parentId = subcategory ? subcategory.id : null;

      const base64Data = imageData.split(',')[1];

      const payload = {
        // productCode: 0,
        nameProduct: values.nameProduct,
        marketPriceProduct: values.marketPriceProduct,
        purchasePriceProduct: values.purchasePriceProduct,
        dateOfManufactureProduct: values.dateOfManufactureProduct.format('YYYY-MM-DDTHH:mm:ss'),
        bestBeforeDateProduct: parseInt(values.bestBeforeDateProduct),
        categoryId: parentId || categoryId,
        description: values.description,
        image: base64Data,
        numberInStock: values.numberInStock,
      };

      const requestOptions = {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(payload),
      };


      console.log('Отправляемые данные:', payload);
      const response = await fetch('/api/Product', requestOptions);

      if (response.ok) {
        notification.success({
          message: 'Товар успешно добавлен',
          description: `${values.nameProduct} был успешно добавлен в базу данных.`,
        });
        form.resetFields();
      } else {
        const errorText = await response.text();
        console.error('Ошибка сервера:', errorText);
        throw new Error('Ошибка при отправке данных на сервер');
      }
    } catch (error) {
      notification.error({
        message: 'Ошибка',
        description: `Не удалось добавить товар в базу данных: ${error.message}`,
      });
    }
  };

  const uploadProps = {
    beforeUpload: (file) => {
      const isImage = file.type.startsWith('image/');
      if (!isImage) {
        notification.error({
          message: 'Ошибка загрузки файла',
          description: 'Вы можете загрузить только изображения.',
        });
      } else {
        readImageFile(file);
      }
      return false; // Prevent immediate upload
    },
    customRequest: () => {}, // Add customRequest to avoid warnings
  };

  const readImageFile = (file) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      setImageData(reader.result);
    };
    reader.onerror = (error) => {
      notification.error({
        message: 'Ошибка чтения файла',
        description: `Не удалось прочитать файл: ${error.message}`,
      });
    };
  };

  return (
    <>
      <Form
        form={form}
        labelCol={{
          span: 4,
        }}
        wrapperCol={{
          span: 14,
        }}
        layout="horizontal"
        style={{
          maxWidth: 600,
        }}
        onFinish={onFinish}
      >
        {/* <Form.Item label="Product Code" name="productCode" rules={[{required: true, message: 'Введите код продукта'}]}>
          <InputNumber min={1} />
        </Form.Item> */}
        <Form.Item label="Название продукта" name="nameProduct" labelCol={{style: {paddingRight: '10px'}}} rules={[{required: true, message: 'Введите название продукта'}]}>
          <Input />
        </Form.Item>
        <Form.Item label="Описание" name="description">
          <TextArea rows={4} />
        </Form.Item>
        <Form.Item label="Изображене" name="image">
          <Upload {...uploadProps}>
            <Button icon={<PlusOutlined />}>Загрузить изображение</Button>
          </Upload>
        </Form.Item>
        <Form.Item label="Рыночная цена" name="marketPriceProduct" labelCol={{style: {paddingRight: '10px'}}} rules={[{required: true, message: 'Введите рыночную цену продукта'}]}>
          <InputNumber min={0} />
        </Form.Item>
        <Form.Item label="Закупочная стоимость" name="purchasePriceProduct" labelCol={{style: {paddingRight: '10px'}}} rules={[{required: true, message: 'Введите закупочную цену продукта'}]}>
          <InputNumber min={0} />
        </Form.Item>
        <Form.Item label="В наличии" name="numberInStock" labelCol={{style: {paddingRight: '10px'}}} rules={[{required: true, message: 'Введите количество продукта на складе'}]}>
          <InputNumber min={0} />
        </Form.Item>
        <Form.Item label="Дата производства" name="dateOfManufactureProduct" labelCol={{style: {paddingRight: '10px'}}}
          rules={[{required: true, message: 'Выберите дату изготовления продукта'}]}
        >
          <DatePicker />
        </Form.Item>
        <Form.Item label="Гарантия" name="bestBeforeDateProduct" labelCol={{style: {paddingRight: '10px'}}} rules={[{required: true, message: 'Введите срок годности продукта (в днях)'}]}>
          <InputNumber min={1} />
        </Form.Item>
        <Form.Item
          label="Категория"
          name="category"
          labelCol={{style: {paddingRight: '10px'}}}
          rules={[{required: true, message: 'Выберите категорию'}]}
        >
          <Cascader
            options={items.map((item) => ({
              value: item.key,
              label: item.label,
              children: item.children?.map((child) => ({
                value: child.key,
                label: child.label,
              })),
            }))}
            placeholder="Выберите категорию"
          />
        </Form.Item>
        <Form.Item wrapperCol={{offset: 4}}>
          <Button type="primary" htmlType="submit">
Добавить товар
          </Button>
        </Form.Item>
      </Form>
    </>
  );
};

export default AddProductForm;
