// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

import avatar_idle from "../assets/avatar/idle.webp";
import avatar_talking from "../assets/avatar/talking.webp";

function Avatar({ isTalking }) {

    const image = isTalking ? avatar_talking : avatar_idle;

    return (
        <div className="avatar-container">
            <img
                src={image}
                alt="Avatar"
                className="avatar"
            />
        </div>
    );
}

export default Avatar;