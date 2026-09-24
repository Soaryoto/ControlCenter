// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

class WebSocketService {

    connect(onMessage) {
        this.socket = new WebSocket("ws://localhost:8765");
        this.socket.onmessage = (event) => {
            onMessage(event.data);
        };
    }

    disconnect() {
        this.socket?.close();
    }
}

export default new WebSocketService();