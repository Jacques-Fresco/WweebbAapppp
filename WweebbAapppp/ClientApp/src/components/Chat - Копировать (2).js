import React, { useState, useEffect } from 'react';
import jwt from 'jsonwebtoken';
import * as signalR from '@microsoft/signalr';

const ChatAllUsers = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState('');
    //const [webSocket, setWebSocket] = useState(null);
    const accessToken = localStorage.getItem('accessToken');
    //const [pendingMessage, setPendingMessage] = useState('');
    const [webSocket, setWebSocket] = useState(null);

    //const webSocket = new WebSocket('/ws');

/*    const protocolPrefix = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
    let { host } = window.location;*/

/*    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/chatHub', {
            accessTokenFactory: () => accessToken,
        })
        .build();*/

    const decodedToken = jwt.decode(accessToken);
    const nameIdentifier = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    const userName = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    /*const handleNewMessage = (message) => {
        setMessages((prevMessages) => [...prevMessages, message]);
    };*/

    //const socket = new WebSocket('wss://192.168.0.102:44486/chatmessages');
    //const newWebSocket = new WebSocket('wss://192.168.0.102:44486/chat-messages');
    //const newWebSocket = new WebSocket('/ws');




/*
    useEffect(() => {
        *//*const socket = new WebSocket('wss://192.168.0.102:44486/chatmessages');


        console.log("useEffect");*//*


        fetchChatData();

        const hubConnection = new signalR.HubConnectionBuilder()
            //.withUrl("https://192.168.0.102:44486/message")
            .withUrl("/message")
            .withAutomaticReconnect()
            .build();

        hubConnection.start();

        hubConnection.on("sendToReact", messages => {
            console.log(messages);
            setMessages(messages);
        });


        *//*socket.onopen = () => {
            console.log('WebSocket connection opened');
        };

        socket.onmessage = (event) => {
            setMessages(JSON.parse(event.data));
            console.log('Received message:', messages[messages.length - 1]);
        };

        socket.onclose = () => {
            console.log('WebSocket connection closed');
        };

        socket.onerror = (error) => {
            console.error('WebSocket error:', error);
        };

        setWebSocket(socket);

        return () => {
            socket.close();
        };*//*
    }, []);*/

    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/message") //, { accessTokenFactory: () => accessToken })
        .withAutomaticReconnect()
        .build();

    hubConnection.start()
        .then(() => {
            console.log("SignalR connection started");
        })
        .catch(error => console.log("Error starting SignalR connection:", error));

    var list = [];

    const Messages = (messageProps) => {
        const [date, setDate] = useState();

        useEffect(() => {
            messageProps.hubConnection.on("sendToReact", messages => {
                console.log("sendToReact", messages);
                //setMessages(messages);
                messageProps.list = messages;
                setDate(new Date());
            });

            messageProps.fetchChatData();

            return () => {
                messageProps.hubConnection.stop().then(() => {
                    console.log("SignalR connection closed");
                });
            };
        }, [])

        return (
            <div>
                <h2>Чат всех пользователей</h2>
                <h2 style={{ color: 'purple' }}>Вы являетесь: {userName}</h2>
                <div className="messages">
                    {messageProps.list.slice(-15).map((message) => (
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
    }


    useEffect(() => {
        fetchChatData();
    }, []);


/*    const SendMessage = () => {
        const [message, setMassage] = useState("");
    }*/


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
                    console.log("успешное получение данных");
                    console.log(fetchData.messages);
                    //setMessages(fetchData.messages || []);
                    list = fetchData.message;
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

    const handleSendMessage = () => {
        if (newMessage.trim() !== '') {
            const message = {
                message: newMessage,
                senderid: userName,
                chatid: 1,
            };

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

/*    function sendMessage() {
        console.log(webSocket);

        if (newMessage.trim() !== '') {
            const message = {
                //token: accessToken,
                message: newMessage,
                senderid: nameIdentifier,
                chatid: 1,
            };

            webSocket.send(JSON.stringify(message));
            setNewMessage('');
        }
    }*/

    return <Messages hubConnection={hubConnection} fetchChatData={fetchChatData} list={list}></Messages>

    /*return (
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
    );*/
};

export default ChatAllUsers;