import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';

const Register = ({user, setUser}) => {
  const [errorMessages, setErrorMessages] = useState([]);
  const [registrationSuccess, setRegistrationSuccess] = useState(false);

  const navigate = useNavigate();
  const register = async (event) => {
    event.preventDefault();
    const {email, password, reppassword} = document.forms[0];
    // console.log(email.value, password.value)
    const requestOptions = {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({
        email: email.value,
        password: password.value,
        passwordConfirm: reppassword.value,
      }),
    };
    return await fetch(
        "api/account/register",
        requestOptions,
    )
        .then((response) => {
        // console.log(response.status)

          response.status === 200 &&
          setUser({isAuthenticated: true, userName: email.value});
          return response.json();
        })
        .then(
            (data) => {
              console.log('Data:', data);
              if (
                typeof data !== 'undefined' &&
            typeof data.userName !== 'undefined'
              ) {
                setUser({isAuthenticated: true, userName: data.userName});
                setRegistrationSuccess(true); // <-- добавьте эту строку
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
      {user.isAuthenticated ? (
        <h3>Пользователь {user.userName} уже вошел в систему</h3>
      ) : (
        <>
          <h3>Регистрация</h3>
          <form onSubmit={register}>
            <label>Пользователь </label>
            <input type="text" name="email" placeholder="Логин" />
            <br />
            <label>Пароль </label>
            <input type="text" name="password" placeholder="Пароль" />
            <br />
            <label>Повторите Пароль </label>
            <input type="text" name="reppassword"
              placeholder="Пароль" />
            <br />
            <button type="submit">Зарегистрироваться</button>
          </form>
          {registrationSuccess && (
            // eslint-disable-next-line max-len
            <p>Регистрация прошла успешно. Вы будете перенаправлены на главную страницу.</p>
          )}
          {renderErrorMessage()}
        </>
      )}
    </>
  );
};
export default Register;
