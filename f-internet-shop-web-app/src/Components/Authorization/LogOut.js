import React, {useEffect} from 'react';
import {useNavigate} from 'react-router-dom';
import {Button, Result} from 'antd';

const LogOff = ({setUser}) => {
  const navigate = useNavigate();
  useEffect(() => {
    // showModal();
  }, []);
  const logOff = async (event) => {
    event.preventDefault();
    const requestOptions = {
      method: 'POST',
    };
    return await fetch('api/account/logoff', requestOptions).then(
        (response) => {
          response.status === 200 &&
setUser({isAuthenticated: false, userName: '', userID: -1});
response.status === 401 ? navigate('/login') : navigate('/');
        },
    );
  };
  const handleCancel = () => {
    console.log('Clicked cancel button');
    navigate('/');
  };
  return (
    <>
      <Result
        title="Вы действительно хотите выйти?"
        extra={
          <>
            <Button type="primary" key="Exit" onClick={logOff}>
          Выход
            </Button>
            <Button key="Cancel" onClick={handleCancel}>
          Отмена
            </Button>
          </>
        }
      />
    </>
  );
};
export default LogOff;
