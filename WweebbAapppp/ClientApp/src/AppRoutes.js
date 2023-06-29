import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import Protected from "./components/Protected";
import LoginForm from "./components/Login";
import Logout from "./components/Logout";
import SignUp from "./components/SignUp";
import ProfileForm from "./components/Profile";
import ChatAllUsers from "./components/Chat";

const AppRoutes = [
  {
    path: '/',
    element: <Home />,
    isPrivate: false
  },
  {
    path: '/counter',
    element: <Counter />,
    isPrivate: false
  },
  {
    path: '/fetch-data',
    element: <FetchData />,
    isPrivate: true
  },
  {
    path: '/logout',
    element: <Logout />,
    isPrivate: false
  },
  {
    path: '/login',
    element: <LoginForm />,
    isPrivate: false
  },
  { 
    path: '/signup',
    element: <SignUp />,
    isPrivate: false
  },
  {
      path: '/profile',
      element: <ProfileForm />,
      isPrivate: true
  },
  {
      path: '/chat',
      element: <ChatAllUsers />,
      isPrivate: true
  }
];

export default AppRoutes;
