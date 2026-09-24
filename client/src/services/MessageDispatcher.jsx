// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

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