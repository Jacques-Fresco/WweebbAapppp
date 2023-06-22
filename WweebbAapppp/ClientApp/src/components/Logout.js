import React, { useState } from 'react';
import { connect } from 'react-redux';
import { removeAuthenticated } from '../redux/actions';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';

const Logout = (props) => {

    const dispatch = useDispatch();
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('expirationAccessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('exprirationRefreshToken');

        dispatch(removeAuthenticated());

        navigate('/', { replace: true });
    }

    return (
        <button onClick={handleLogout}>Выход</button>
    );
};

/*const mapDispatchToProps = {
    setAuthenticated,
};*/

//export default connect(null, mapDispatchToProps)(Logout);
export default Logout;