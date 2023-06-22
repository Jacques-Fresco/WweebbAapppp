export const SET_AUTHENTICATED = "SET_AUTHENTICATED";
export const REMOVE_AUTHENTICATED = "REMOVE_AUTHENTICATED";

export const setAuthenticated = () => { 
    return {
        type: SET_AUTHENTICATED
    };
};

export const removeAuthenticated = () => {
    return {
        type: REMOVE_AUTHENTICATED
    };
};