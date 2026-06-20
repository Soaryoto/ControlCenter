class MessageDispatcher {

    constructor() {
        this._messageHandlers = {};
    }

    register(module, event, handler) {
        this._messageHandlers[`${module}.${event}`] = handler;
    }

    dispatch(json) {
        const message = JSON.parse(json);

        const key = `${message.Module}.${message.Event}`;
        
        const handler = this._messageHandlers[key];
        
        if (handler) {
            handler(message.Payload);
        }
    }
}

export default new MessageDispatcher();