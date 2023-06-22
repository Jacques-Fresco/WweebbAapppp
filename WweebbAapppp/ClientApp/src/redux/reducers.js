import { SET_AUTHENTICATED, REMOVE_AUTHENTICATED } from "./actions";

const initialState = {
    isAuthenticated: false
};

const authReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_AUTHENTICATED:
            return {
                ...state,
                isAuthenticated: true
            };
        case REMOVE_AUTHENTICATED:
            return {
                ...state,
                isAuthenticated: false
            };
        default:
            return state;
    }
};

export default authReducer;