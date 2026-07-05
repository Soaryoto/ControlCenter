# WebSocketServerService

## Назначение

Запуск локального WebSocket сервера

---

## Ответственность

Запуск локального WebSocket сервера
Остановка локального WebSocket сервера
Передачу даных на WebSocket сервер

---

## Использует

Получает ILoggingService для логирования

---

## Предоставляет

Реализует интерфейс IWebSocketServerService

---

## Жизненный цикл

Создаётся один раз в ApplicationHost при передачи в DI контейнер ServiceContainer как сервис

Используеться в MicrophoneListener для передачи данных реакции на звук

---

## Диаграмма

ApplicationHost

↓

IWebSocketServerService

↓

WebSocketServerService

↓

MicrophoneListener

---

## Связанные ADR

ADR-0005

---