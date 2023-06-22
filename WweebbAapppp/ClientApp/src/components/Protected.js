import React, { useState, useEffect } from 'react';
import { Navigate } from 'react-router-dom';

const Protected = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false); // Предполагается, что пользователь уже аутентифицирован
    const [isTokenExpired, setIsTokenExpired] = useState(false); // Флаг, указывающий, что срок действия Access Token истек

    useEffect(() => {
        checkAuthentication(); // Проверяем аутентификацию при загрузке компонента
        checkTokenExpiration(); // Проверяем срок действия Access Token при загрузке компонента
    }, []);

    const checkTokenExpiration = () => {
        const tokenExpiration = localStorage.getItem('tokenExpiration'); // Получаем информацию о сроке действия Access Token из localStorage или другого хранилища

        if (tokenExpiration) {
            const expirationTimestamp = parseInt(tokenExpiration);
            const currentTimestamp = Date.now() / 1000; // Получаем текущую метку времени в секундах

            if (expirationTimestamp <= currentTimestamp) {
                setIsTokenExpired(true); // Устанавливаем флаг, что срок действия Access Token истек
            }
        }
    };

    const checkAuthentication = () => {
        // Проверяем, есть ли у пользователя доступный токен или другие данные аутентификации в локальном хранилище (например, localStorage)
        const accessToken = localStorage.getItem('accessToken');

        if (accessToken) {
            setIsAuthenticated(true); // Устанавливаем флаг аутентификации в true, если доступный токен найден
        }
    };

    if (isTokenExpired) {
        // Если срок действия Access Token истек, перенаправляем пользователя на страницу входа
        return <Navigate to="/" />;
    }

    if (!isAuthenticated) {
        // Если пользователь не аутентифицирован, также перенаправляем на страницу входа
        return <Navigate to="/" />;
    }

    // Возвращаем защищенный компонент или контент
    return <div>Защищенный компонент</div>;
};

export default Protected;