import React, { useState } from 'react';

const LoginForm = () => {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    const handlerSignUp = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/api/Users/signup', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ username, password, email, confirmPassword })
            });

            if (response.ok) {
                // Обработка успешного ответа от сервера
                console.log('Успешная Регестрация');

                const data = await response.json();

                localStorage.setItem("accessToken", data.accessToken);
                localStorage.setItem("expirationAccessToken", data.expirationAccessToken);
                localStorage.setItem("refreshToken", data.refreshToken);
                localStorage.setItem("expirationRefreshToken", data.expirationRefreshToken);
            } else {
                // Обработка ошибки от сервера
                console.log('Ошибка регестрации');
            }
        } catch (error) {
            // Обработка ошибок связанных с отправкой запроса
            console.log('Ошибка при отправке запроса', error);
        }
    }



    return (
        <form onSubmit={handlerSignUp}>
            <input
                type="text"
                placeholder="Почта"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="text"
                placeholder="Логин"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <input
                type="password"
                placeholder="Repeat пароль"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
            />
            <button type="submit">Регестрация</button>
        </form>
    );
};

export default LoginForm;