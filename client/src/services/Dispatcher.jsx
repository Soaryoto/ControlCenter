class Dispatcher {

    constructor(callbacks) {
        this.callbacks = callbacks;
    }

    dispatch(data) {
        const message = JSON.parse(data);
        if(message.module === "Avatar") {
            this.callbacks.avatar(message.payload);
        }
    }
}

export default Dispatcher;