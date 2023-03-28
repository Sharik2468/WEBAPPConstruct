import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
const LogOut = ({user, setUser}) => {
  const [errorMessages, setErrorMessages] = useState([]);
  const navigate = useNavigate();
  const logOut = async (event) => {
    event.preventDefault();
    const {email, password} = document.forms[0];
    // console.log(email.value, password.value)
    const requestOptions = {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': '*/*',
      },
      body: JSON.stringify({}),
    };


    return await fetch(
        'https://localhost:7194/api/account/logoff',
        requestOptions,
    )
        .then((response) => {
        // console.log(response.status)

          response.status === 200 &&
          setUser({isAuthenticated: true, userName: ''});
          return response.json();
        })
        .then(
            (data) => {
              console.log('Data:', data);
              if (
                typeof data !== 'undefined' &&
            typeof data.userName !== 'undefined'
              ) {
                setUser({isAuthenticated: false, userName: data.userName});
                navigate('/');
              }
              typeof data !== 'undefined' &&
            typeof data.error !== 'undefined' &&
            setErrorMessages(data.error);
            },
            (error) => {
              console.log(error);
            },
        );
  };
  const renderErrorMessage = () =>
    errorMessages.map((error, index) => <div key={index}>{error}</div>);
  return (
    <>
      <>
        <h3>Выход</h3>
        <form onSubmit={logOut}>
          <button type="submit">Выйти</button>
        </form>
        {renderErrorMessage()}
      </>
    </>
  );
};
export default LogOut;
