import {Navigate, Route, useLocation} from 'react-router-dom';
import {useAuthStore} from "../../pages/auth/useAuthStore";

export const PrivateRoute = ({children, requiredRole}: {
    children: JSX.Element;
    requiredRole: string;
}) => {
    let location = useLocation();
    const {isLoggedIn, isLoading, role} = useAuthStore();

    if (isLoading) {
        return <p className="container">Checking auth..</p>;
    }
    const userHasRequiredRole = requiredRole == role;

    if (!isLoggedIn || !userHasRequiredRole) {
        return <Navigate to="/login" state={{from: location}}/>;
    }

    return children;
};