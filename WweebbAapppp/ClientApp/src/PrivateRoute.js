import React from 'react';
import { Route, Navigate } from 'react-router-dom';

const ProtectedRoute = ({ isAuthenticated, element: Component }) => {
    if (!isAuthenticated) {
        return <Navigate to="/" replace />;
    }

    return Component;
};

export default ProtectedRoute;