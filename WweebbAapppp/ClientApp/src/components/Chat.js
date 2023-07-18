﻿import React, { useState, useEffect } from 'react';
import jwt from 'jsonwebtoken';
import * as signalR from '@microsoft/signalr';

const ChatAllUsers = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');
    const accessToken = localStorage.getItem('accessToken');

    const decodedToken = jwt.decode(accessToken);
    const nameIdentifier = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    const [hubConnection, setHubConnection] = useState(null);

    useEffect(() => {
        fetchChatData();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/message")
            .withAutomaticReconnect()
            .build();

        connection
            .start()
            .then(() => {
                console.log("SignalR connection started");
            })
            .catch(error => console.log("Error starting SignalR connection:", error));

        connection.on("sendToReact", messages => {
            console.log("sendToReact");
            console.log(messages);
            setMessages(messages);
        });

        setHubConnection(connection);

        return () => {
            connection.stop();
        };
    }, []);

    

    async function fetchChatData() {
        fetch('/api/message/fetch-messages/1', {
            headers: {
                'Authorization': `Bearer ${accessToken}`
            }
        })
            .then(async response => {
                if (response.ok) {
                    const fetchData = await response.json();
                    console.log(fetchData);
                    console.log("успешное получение данных:", fetchData.messages);
                    setMessages(fetchData.messages || []);
                } else {
                    console.log('Ошибка получения данных');
                }
            })
            .catch(error => {
                console.log('Ошибка при отправке запроса на получение данных', error);
            });
    }

    const handleMessageChange = (e) => {
        setNewMessage(e.target.value);
    };

    const handleSendMessage = () => {
        if (newMessage.trim() !== '') {
            const message = {
                message: newMessage,
                senderid: userName,
                chatid: 1,
            };


            /*hubConnection.invoke("Send", message)
                .catch(function (err) {
                    console.error(err.toString());
                });*/

            fetch('/api/message/сreate-message', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(message)
            })
                .then(response => {
                    if (response.ok) {
                        fetchChatData();
                        setNewMessage('');
                    } else {
                        console.log('Ошибка сохранения данных');
                    }
                })
                .catch(error => {
                    console.log('Ошибка при отправке запроса', error);
                });
        }
    };

    return (
        <div>
            <h2>Чат всех пользователей</h2>
            <h2 style={{ color: 'purple' }}>Вы являетесь: {userName}</h2>
            <div className="messages">
                {messages.slice(-15).map((message) => (
                    <div key={message.id}>
                        <span style={{ color: 'red' }}>[{message.sentAt}]</span>
                        <span style={{
                            backgroundColor: userName === message.userId ? 'purple' : 'blue',
                            color: 'white'
                        }}>
                            {message.userId}:
                        </span>
                        <span style={{ color: 'green' }}>  {message.text}</span>
                    </div>
                ))}
            </div>
            <div className="input-area">
                <input
                    type="text"
                    value={newMessage}
                    onChange={handleMessageChange}
                    placeholder="Введите сообщение..."
                />
                <button onClick={handleSendMessage}>Отправить</button>
            </div>
        </div>
    );
};

export default ChatAllUsers;