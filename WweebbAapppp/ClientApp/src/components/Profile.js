import React, { useState, useEffect } from 'react';
/*import { connect } from 'react-redux';
import { setAuthenticated } from '../redux/actions';
import { useDispatch, useSelector } from 'react-redux';*/

const ProfileForm = () => {
    const [profileData, setProfileData] = useState({});
    const [editedData, setEditedData] = useState({});
    const [editMode, setEditMode] = useState(false);
    const [isDataChanged, setIsDataChanged] = useState(false);
    const [isPasswordChanged, setIsPasswordChanged] = useState(false);
    const [editedPassword, setEditedPassword] = useState("");
    const [editPassword, setEditPassword] = useState(false);
    const [statEditPassword, setStatEditPassword] = useState(false);


    //const dispatch = useDispatch();

    useEffect(() => {
        fetchProfileData();
        console.log("useEffect", profileData.userName);
    }, []);

    /*useEffect(() => {
        console.log("EditedData2", editedData.userName);
    }, [editedData.userName]);*/


    async function fetchProfileData() {
        try {
            const accessToken = localStorage.getItem('accessToken');
            console.log(accessToken);
            const response = await fetch('https://localhost:7089/api/Users/data-profile', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${accessToken}`
                }
            });

            if (response.ok) {
                const letprofileData = await response.json();

                /*console.log("json():", letprofileData);
                console.log(letprofileData.passwordHash);
                console.log(letprofileData.userName);*/

                setProfileData(letprofileData);
                setEditedData(letprofileData);
                /*setEditedData({
                    ...letprofileData,
                    UserName: letprofileData.userName || "" 
                });*/

            } else {
                console.log('Ошибка');
            }
        } catch (error) {
            console.log('Ошибка при отправке запроса', error);
        }
    };

    const handleInputChange = (fieldName, value) => {
        console.log("символ", value);
        if (profileData[fieldName] === value || value === "") {
            setIsDataChanged(false);
        } else {
            setIsDataChanged(true);
        }

        setEditedData(prevData => ({
            ...prevData,
            [fieldName]: value
        }));

        console.log("EditedData", editedData.userName);
    };

    const handleCancelDate = () => {
        setStatEditPassword(false);
        setEditedData(profileData);
        setIsDataChanged(false);
        setEditMode(false);
    };

    const handleSaveClick = () => {
        console.log("тууут", editedData.userName);

        fetch('https://localhost:7089/api/Users/save-data-profile', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            },
            body: JSON.stringify({
                username: editedData.userName
            })
        }).then(response => {
            if (response.ok) {
                setProfileData(editedData);
                setIsDataChanged(false);
                setEditMode(false);
                setStatEditPassword(false);
            } else {
                console.log('Ошибка сохранения данных');
            }
        })
        .catch(error => {
            console.log('Ошибка при отправке запроса', error);
        });
    };

    const handleEditClick = () => {
        setEditMode(true);
        setStatEditPassword(false);
    };


    const handleEditPassword = () => {
        setEditPassword(true);
        setStatEditPassword(true);
    }

    const handleInputChangePassword = (value) => { /////////////
        setEditedPassword(value);
        if (value === "") {
            setIsPasswordChanged(false);
        } else {
            setIsPasswordChanged(true);
        }
    }

    const handlerCanseSavePassword = () => {
        setEditPassword(false);
        setStatEditPassword(false);
        setEditedPassword("");
        setIsPasswordChanged(false);
    }

    const handlerSavePassword = () => {
        fetch('https://localhost:7089/api/Users/save-data-profile', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
            },
            body: JSON.stringify({
                password: editedPassword
            })
        }).then(response => {
            if (response.ok) {
                setProfileData(editedData);
                setIsDataChanged(false);
                setEditMode(false);
            } else {
                console.log('Ошибка сохранения данных');
            }
        })
        .catch(error => {
            console.log('Ошибка при отправке запроса', error);
        });

        
    };

    return (
          <div>
            <p>Id: {profileData.id}</p>
            <p>UserName: {profileData.userName}</p>
            <p>Email: {profileData.email}</p>
            <p>PasswordHash: {profileData.passwordHash}</p>

            {editMode ? (
                <div>
                    <input
                        type="text"
                        value={editedData.UserName}
                        onChange={e => handleInputChange('userName', e.target.value)}
                    />
                    <button onClick={handleCancelDate}>Отменить</button>
                    {isDataChanged && (
                        <button onClick={handleSaveClick}>Сохранить</button>
                    )}
                </div>
            ) : !statEditPassword && (
                <div>
                        <button onClick={handleEditClick}>Редактировать данные</button>
                        <button onClick={handleEditPassword}>Сменить пароль</button>
                </div>  
            )}

            {editPassword && (
                <div>
                    <input
                        type="password"
                        value={editedPassword}
                        onChange={e => handleInputChangePassword(e.target.value)}
                    />

                    <button onClick={handlerCanseSavePassword}>Отменить</button>
                    {isPasswordChanged && (
                        <button onClick={handlerSavePassword}>Сохранить</button>
                    )}
                </div>
            )}
          </div>
    );
};

export default ProfileForm;