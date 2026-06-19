class WebSocketService {

    connect(onMessage) {
        this.socket = new WebSocket("ws://localhost:8765");
        this.socket.onmessage = (event) => {
            onMessage(event.data);
        };
    }
    
}

export default new WebSocketService();