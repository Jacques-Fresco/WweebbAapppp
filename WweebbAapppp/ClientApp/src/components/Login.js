import React, { useState } from 'react';
import { connect } from 'react-redux';
import { setAuthenticated } from '../redux/actions';
import { useDispatch, useSelector } from 'react-redux';

const LoginForm = () => {
    const [userName, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const dispatch = useDispatch();

    console.log(window.location.href);

    //const value = useSelector(state => state.auth.isAuthenticated);

    const handleSubmit = async (e) => {
        e.preventDefault();

        console.log(userName);
        console.log(password);

        try {
            //const response = await fetch('https://192.168.0.102:44486/api/Users/login', {
            //const response = await fetch('https://localhost:44486/api/Users/login', {
            const response = await fetch('/api/Users/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ userName, password })
            });

            if (response.ok) {
                // Обработка успешного ответа от сервера
                console.log('Успешная авторизация');

                const data = await response.json();

                localStorage.setItem("accessToken", data.accessToken);
                localStorage.setItem("expirationAccessToken", data.expirationAccessToken);
                localStorage.setItem("refreshToken", data.refreshToken);
                localStorage.setItem("expirationRefreshToken", data.expirationRefreshToken);

                //console.log(value);

                dispatch(setAuthenticated());

                //console.log(value);
            } else {
                // Обработка ошибки от сервера
                console.log('Ошибка авторизации');
            }
        } catch (error) {
            // Обработка ошибок связанных с отправкой запроса
            console.log('Ошибка при отправке запроса', error);
        }
    }

    

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="text"
                placeholder="Логин"
                value={userName}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button type="submit">Войти</button>
        </form>
    );
};

export default LoginForm;