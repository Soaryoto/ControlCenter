# StreamingController

Центральный интерфейс для управления различными сервисами и компонентами стриминговой системы.

Проект состоит из серверной части `ControlCenter` и клиентского интерфейса на React.

> [!IMPORTANT]
> **Licensing notice**
>
> StreamingController contains two separately licensed components:
>
> - **Source code** — GNU GPLv3 or later
> - **Avatar and artwork** — separate proprietary terms in
>   [LICENSE-ASSETS](LICENSE-ASSETS)
>
> The source code of StreamingController is licensed under the
> **GNU General Public License, version 3 or any later version
> (GPL-3.0-or-later)**.

## Структура проекта

```text
StreamingController/
│
├── ControlCenter/
│   ├── Infrastructure/
│   │   ├── ApplicationHost
│   │   └── ServiceContainer
│   │
│   ├── Services/
│   │   ├── LoggingService
│   │   ├── MicrophoneListenerService
│   │   ├── ProcessManagerService
│   │   └── WebSocketServerService
│   │
│   ├── Assets/
│   ├── Contracts/
│   ├── Interfaces/
│   ├── Extensions/
│   ├── Views/
│   ├── ViewModels/
│   ├── Models/
│   ├── logs/
│   └── docs/
│
├── client/
│   └── ...
│
└── startControlCenter.bat
```

## Используемые технологии

### ControlCenter

* .NET
* Avalonia 12.0.4
* CommunityToolkit.Mvvm 8.4.1

### Client

* React
* JavaScript

## Как собрать

Перейдите в директорию `ControlCenter` и выполните:

```bash
dotnet run
```

## Как запустить

После сборки исполняемый файл `ControlCenter.exe` находится в соответствующей папке внутри `bin`.

Также для запуска можно использовать:

```text
startControlCenter.bat
```

## Статус проекта

🚧 Проект находится в разработке.

Архитектура и отдельные компоненты проекта могут изменяться по мере развития проекта.

## Сторонние компоненты

Проект использует сторонние библиотеки и технологии, которые распространяются на условиях их собственных лицензий.

В частности:

* Avalonia 12.0.4
* CommunityToolkit.Mvvm 8.4.1
* React и связанные зависимости

Лицензии сторонних компонентов сохраняют силу и не изменяются лицензией этого проекта.

## License

StreamingController uses separate licensing terms for its source code
and its visual assets.

### Source Code

The source code of StreamingController is licensed under the
**GNU General Public License v3.0 or later (GPLv3+)**.

You may use, study, modify, and redistribute the source code under the
terms of the GPLv3.

If you distribute a modified version of the GPL-licensed software, you
must comply with the applicable requirements of the GPLv3, including
the requirements concerning licensing and source code.

See [LICENSE](LICENSE) for the full license text.

### Avatar and Artwork

**The avatar, character design, and visual assets are NOT licensed
under the GPLv3.**

The following types of files are covered by a separate license:

- avatar artwork;
- character design;
- character poses and expressions;
- avatar components;
- graphical assets used to construct the avatar;
- images located in `client/assets/avatar/`.
- application icons depicting the character;

> The artwork license applies regardless of whether an avatar asset is
> stored as a complete image or as an individual graphical component.

These assets are copyright of **Soaryoto** and are distributed under
the terms of [LICENSE-ASSETS](LICENSE-ASSETS).

The GPLv3 license applies to the source code only.

The inclusion of artwork in this repository does not grant permission
to use, modify, redistribute, sell, sublicense, or claim authorship of
the artwork or character design.

### Attribution

When permitted to use the artwork, please attribute it as:

> **Soaryoto — original character and artwork**

Project:

https://github.com/Soaryoto/StreamingController

Author:

https://github.com/Soaryoto