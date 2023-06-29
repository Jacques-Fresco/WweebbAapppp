import React, { useState, useEffect } from 'react';
import io from 'socket.io-client';
import jwt from 'jsonwebtoken';
//import jwt from 'node-jsonwebtoken';

const ChatAllUsers = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');

    const accessToken = localStorage.getItem('accessToken');

    const socket = io('https://localhost:7089');

    useEffect(() => {
        socket.on('newMessage', handleNewMessage);

        fetchChatData();

        return () => {
            socket.off('newMessage', handleNewMessage);
        };
    }, []);

    async function fetchChatData() {
        fetch('https://localhost:7089/api/chats/fetch-messages/1', { 
            headers: {
                'Authorization': `Bearer ${accessToken}`
            }
        })
        .then(async response => {
            if (response.ok) {
                const fetchData = await response.json();
                console.log(fetchData);
                console.log("успешное получение данных");
                console.log(fetchData.messages);
                setMessages(fetchData.messages || []);
                console.log(fetchData.messages);

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

    const handleNewMessage = (messages) => {
        //setMessages((prevMessages) => [...prevMessages, message]);
        setMessages(messages);
    };

    const decodedToken = jwt.decode(accessToken);
    const nameIdentifier = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    const handleSendMessage = () => {
        if (newMessage.trim() !== '') {
            
            

            const message = {
                //Id: nameIdentifier,
                text: newMessage,
                senderid: userName,
                chatid: 1,
            };

            fetch('https://localhost:7089/api/chats/send-message', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${accessToken}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(message)
            })
            .then(response => {
                if (response.ok) {
                    setMessages((prevMessages) => [...prevMessages, message]);
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
                {messages.map((message) => (
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