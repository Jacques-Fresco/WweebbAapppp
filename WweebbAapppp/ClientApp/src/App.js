import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import ProtectedRoute from './PrivateRoute';
import { useSelector, connect } from 'react-redux';
import { setAuthenticated, removeAuthenticated } from './redux/actions';

class App extends Component {
    static displayName = App.name;

/*    constructor(props) {
        super(props);

        this.state = {
            isAuthenticated: false
        };
    }

    setAuthenticated = () => {
        this.setState({
            isAuthenticated: true
        });
    }*/



    componentDidMount() {
        this.checkAuthentication();
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevProps.isAuthenticated !== this.props.isAuthenticated) {
            this.checkAuthentication();
        }
    }

    checkAuthentication = async () => {
        console.log("checkAuthentication");

        const accessToken = localStorage.getItem('accessToken');
        const expirationAccessToken = localStorage.getItem('expirationAccessToken');
        const refreshToken = localStorage.getItem('refreshToken');
        const expirationRefreshToken = localStorage.getItem('expirationRefreshToken');
        console.log("expirationAccessToken", expirationAccessToken);
        console.log("expirationRefreshToken", expirationRefreshToken);


        const isAccessTokenExpired =
            !(expirationAccessToken && new Date() < new Date(expirationAccessToken));
        const isRefreshTokenExpired =
            !(expirationRefreshToken && new Date() < new Date(expirationRefreshToken));
        console.log("isAccessTokenExpired", isAccessTokenExpired);
        console.log("isRefreshTokenExpired", isRefreshTokenExpired);

        /*console.log(new Date());
        console.log(new Date(expirationAccessToken));
        console.log(isAccessTokenExpired);
        console.log(isRefreshTokenExpired);*/

        if (accessToken && !isAccessTokenExpired && refreshToken && !isRefreshTokenExpired) {
            console.log("if (accessToken && isAccessTokenExpired");
            // ѕользователь авторизован, устанавливаем isAuthenticated в true
            await this.props.setAuthenticated();
            //console.log("hi");
        } else if (accessToken && isAccessTokenExpired) {
            if (refreshToken && isRefreshTokenExpired) {
                return;
            }

            console.log("if (accessToken && !isAccessTokenExpired");
            const response = await fetch('https://localhost:7089/api/Users/generate-new-token', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ accessToken, refreshToken })
            });

            if (response.ok) {

                const data = await response.json();

                localStorage.setItem('accessToken', data.accessToken);
                localStorage.setItem('expirationAccessToken', data.expirationAccessToken);
                localStorage.setItem('refreshToken', data.refreshToken);
                localStorage.setItem('expirationRefreshToken', data.expirationRefreshToken);

                console.log("this.props.setAuthenticated(); до", this.props.isAuthenticated);
                await this.props.setAuthenticated();
                console.log("this.props.setAuthenticated(); после", this.props.isAuthenticated);

                /*const [, payload] = data.accessToken.split('.');
                const decodedPayload = atob(payload);
                const payloadObj = JSON.parse(decodedPayload);*/
            }
        } else {
            // ѕользователь не авторизован, устанавливаем isAuthenticated в false
            await this.props.removeAuthenticated();
            //console.log("hiyyyyy");
        }

        console.log("method", this.props.isAuthenticated);
    };

    render() {
        // const { isAuthenticated } = this.state;

        const isAuthenticated = this.props.isAuthenticated;
        console.log("render", isAuthenticated);

        return (
            <Layout isAuthenticated={isAuthenticated}>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { path, element: Component, isPrivate } = route;

                        if (isPrivate) {
                            return <Route key={index} path={path} element={
                                <ProtectedRoute isAuthenticated={isAuthenticated} element={Component} />
                            } />
                        }

                        return <Route key={index} path={path} element={Component} />;
                    })}
                </Routes>
            </Layout>
        );
    }
}

const mapStateToProps = state => ({
    isAuthenticated: state.isAuthenticated,
});

const mapDispatchToProps = dispatch => {
    return {
        setAuthenticated: () => dispatch(setAuthenticated()),
        removeAuthenticated: () => dispatch(removeAuthenticated())
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(App);