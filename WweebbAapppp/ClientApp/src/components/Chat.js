import React, { useState, useEffect, useRef } from 'react';
import jwt from 'jsonwebtoken';
import * as signalR from '@microsoft/signalr';
import '../styles/style.css';
import chatSound from './chat-sound.mp3';

const ChatAllUsers = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');
    const accessToken = localStorage.getItem('accessToken');

    const [selectedMessage, setSelectedMessage] = useState(null);
    const [isMessageSelected, setIsMessageSelected] = useState(false);

    const decodedToken = jwt.decode(accessToken);
    const nameIdentifier = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    const messageContainerRef = useRef(null);

    useEffect(() => {
        fetchChatData();
        console.log("useEffect1");
    }, []);

    useEffect(() => {
        scrollToBottom();
        console.log("useEffect2");
    }, [messages]);

    useEffect(() => {
        console.log("useEffect3");
       const connection = new signalR.HubConnectionBuilder()
            .withUrl("/message", {
                accessTokenFactory: () => accessToken, // Передаем функцию для получения access токена
            })
            .withAutomaticReconnect()
            .build();

        connection
            .start()
            .then(() => {
                console.log("SignalR connection started");
            })
            .catch(error => console.log("Error starting SignalR connection:", error));

        connection.on("sendToReact", data => {
            console.log("sendToReact");
            console.log(data.messages);
            setMessages(data.messages);
            
            if (data.userId !== nameIdentifier) {
                const alertSound = new Audio(chatSound);
                console.log("const alertSound = new Audio(chatSound);");
                alertSound.play();
            }

            if (data.userId === nameIdentifier && messageContainerRef.current) {
                /*const container = messageContainerRef.current;
                container.scrollTop = container.scrollHeight - container.clientHeight;

                setTimeout(() => {
                    container.scrollTo(0, container.scrollHeight);
                }, 50);*/

                const container = messageContainerRef.current;
                const distanceToScroll = container.scrollHeight - container.scrollTop;
                const numSteps = 10;
                const step = distanceToScroll / numSteps;
                let currentStep = 0;

                const scrollInterval = setInterval(() => {
                    if (currentStep >= numSteps) {
                        clearInterval(scrollInterval);
                    } else {
                        container.scrollTop += step;
                        currentStep++;
                    }
                }, 50);
            }
        });

        return () => {
            connection.stop();
        };
    }, []);

    const scrollToBottom = () => {
        // Используйте свойство 'current' у рефа для доступа к DOM-элементу
        if (messageContainerRef.current) {
            messageContainerRef.current.scrollTop = messageContainerRef.current.scrollHeight;
        }
    };

    

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
                        //fetchChatData();
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
    //////////////////////////////////////////
    const handlePress = (message) => {
        if (selectedMessage === message) {
            setSelectedMessage(null); // Если сообщение уже выбрано, сбрасываем его выбор
        } else {
            setSelectedMessage((prevSelectedMessage) =>
                prevSelectedMessage === message ? null : message);
        }

        setIsMessageSelected(!isMessageSelected);
    };

    const handleDelete = () => {
        // Здесь вы можете добавить код для удаления сообщения.
        // Например, вызвать функцию API для удаления сообщения по его идентификатору (message.id).
        setSelectedMessage(null);
    };

    const handleEdit = () => {
        // Здесь вы можете добавить код для редактирования сообщения.
        // Например, открыть модальное окно с формой для редактирования текста сообщения.
        setSelectedMessage(null);
    };

    return (
        <div className="mycontainer">
            <div className="myheader">💬Чат всех пользователей</div>
            {/*<h2 style={{ color: 'purple' }}>Вы являетесь: {userName}</h2>*/}
            <div className="message-section" ref={messageContainerRef}>
                {/*<div className="msg-box received">
                    hi
                </div>

                <div className="msg-box send">
                    hello
                </div>

                <div className="msg-box received">
                    asdfasdfdfffffffffffffffffsdf dsfsd dsfsdfsd dsfsdf
                </div>

                <div className="msg-box send">
                    sadfsad dsfsdfsd
                </div>

                <div className="msg-box received">
                    asdfвыфавыфафыва
                </div>

                <div className="msg-box send">
                    sфывпфвыпвыфпкуцецуке
                </div>

                <div className="msg-box received">
                    фвыакуцекуцгрнмсчи  
                </div>

                <div className="msg-box send">
                    выфаимчсргнекгнек
                </div>*/}


                {/*{messages.slice(-15).map((message) => (
                    <div key={message.id}
                        className={userName === message.userId ? 'msg-box send' : 'msg-box received'}
                        onClick={() => handlePress(message)}
                        onTouchStart={(e) => {
                            const touchDuration = 500; // Время, после которого считается долгое нажатие (в миллисекундах)
                            const timer = setTimeout(() => handlePress(message), touchDuration);
                            // Обработчик события `onTouchEnd` для отмены обработки короткого нажатия
                            e.target.addEventListener('touchend', () => clearTimeout(timer), { once: true });
                        }}>
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
                {selectedMessage && (
                    <div>
                        */}{/* Отображение контекстного меню для удаления и редактирования */}{/*
                        <button onClick={handleDelete}>Удалить</button>
                        <button onClick={handleEdit}>Редактировать</button>
                    </div>
                )}*/}

                {/*{messages.slice(-15).map((message) => (
                    <div
                        key={message.id}
                        className={userName === message.userId ? 'msg-box send' : 'msg-box received'}
                        onClick={() => handlePress(message)}
                        onTouchStart={(e) => {
                            const touchDuration = 500; // Время, после которого считается долгое нажатие (в миллисекундах)
                            const timer = setTimeout(() => handlePress(message), touchDuration);
                            // Обработчик события `onTouchEnd` для отмены обработки короткого нажатия
                            e.target.addEventListener('touchend', () => clearTimeout(timer), { once: true });
                        }}
                    >
                        <span style={{ color: 'red' }}>[{message.sentAt}]</span>
                        <span
                            style={{
                                backgroundColor: userName === message.userId ? 'purple' : 'blue',
                                color: 'white'
                            }}
                        >
                            {message.userId}:
                        </span>
                        <span style={{ color: 'green' }}> {message.text}</span>

                        {userName === message.userId && selectedMessage?.id === message.id && (
                            <div>
                                */}{/* Отображение контекстного меню для удаления и редактирования */}{/*
                                <button onClick={handleDelete}>Удалить</button>
                                <button onClick={handleEdit}>Редактировать</button>
                            </div>
                        )}
                    </div>
                ))}*/}

                {messages.slice(-15).map((message) => (
                    <div
                        key={message.id}
                        className={
                            userName === message.userId
                                ? 'msg-box send' + (selectedMessage === message ? ' selected' : '')
                                : 'msg-box received'
                        }
                        onClick={() => handlePress(message)}
                        onTouchStart={(e) => {
                            const touchDuration = 500; // Время, после которого считается долгое нажатие (в миллисекундах)
                            const timer = setTimeout(() => handlePress(message), touchDuration);
                            // Обработчик события `onTouchEnd` для отмены обработки короткого нажатия
                            e.target.addEventListener('touchend', () => clearTimeout(timer), { once: true });
                        }}
                    >
                        <span style={{ color: 'red' }}>[{message.sentAt}]</span>
                        <span
                            style={{
                                backgroundColor: userName === message.userId ? 'purple' : 'blue',
                                color: 'white'
                            }}
                        >
                            {message.userId}:
                        </span>
                        <span style={{ color: 'green' }}> {message.text}</span>

                        {userName === message.userId && selectedMessage === message && (
                            <div>
                                {/* Отображение контекстного меню для удаления и редактирования */}
                                <button onClick={handleDelete}>Удалить</button>
                                <button onClick={handleEdit}>Редактировать</button>
                            </div>
                        )}
                    </div>
                ))}

            </div>
            <div className="input-section">
                <input className="input"
                    type="text"
                    value={newMessage}
                    onChange={handleMessageChange}
                    placeholder="Введите сообщение..."
                />
                <button className="btn" onClick={handleSendMessage}>Отправить</button>
            </div>
        </div>
    );
};

export default ChatAllUsers;