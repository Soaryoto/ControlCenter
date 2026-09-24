ControlCenter

Описание:
Проект центрального интерфейса для контроля различными сервисами

Как собрать:
dotnet run

Как запустить:
В папке bin найти корневую папку и запустить ControlCenter.exe

Структура проекта:
ControlCenter/
│
├── Infrastructure/
│   ├── ApplicationHost
│   └── ServiceContainer
│
├── Services/
│   ├── LoggingService
│   ├── MicrophoneListenerService
│   ├── ProcessManagerService
│   └── WebSocketServerService
│
├── Assets/
├── Contracts/
├── Interfaces/
├── Extensions/
├── Views/
├── ViewModels/
├── Models/
├── logs/
└── docs/

Используемые технологии:
Avalonia - 12.0.4
CommunityToolkit.Mvvm - 8.4.1

## License

GNU GPLv3.
See [LICENSE](LICENSE).