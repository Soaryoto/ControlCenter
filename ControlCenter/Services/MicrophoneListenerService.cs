using System;
using NAudio.Wave;
using Soaryoto.Contracts.Messages;
using Soaryoto.Interfaces.Services;
using Soaryoto.Models;

namespace Soaryoto;
public sealed class MicrophoneListener : IMicrophoneListener
{
    private readonly ILoggingService _loggingService;
    private readonly IWebSocketServerService _webSocketServerService;
    private WaveInEvent? _waveIn;
    private int _deviceIndex = 0; // Индекс устройства для записи (0 - по умолчанию)
    private int _minAmplitudeForTalking = 700;
    private int lastLoudTime = 0;
    private int _silenceThreshold = 300; // Время в миллисекундах, после которого считается, что пользователь перестал говорить
    private bool _isTalking = false;
    public MicrophoneListener(ILoggingService loggingService, IWebSocketServerService webSocketServerService)
    {
        _loggingService = loggingService;
        _webSocketServerService = webSocketServerService;
    }
    private void StartListening()
    {
        _loggingService.WriteLine("[MIC] Начало прослушивания микрофона...", LogLevel.Info);

        _waveIn = new WaveInEvent();

        for (int i = 0; i < WaveInEvent.DeviceCount; i++)
        {
            var caps = WaveInEvent.GetCapabilities(i);
            if (caps.ProductName.Contains("HECATE G5BT")) // Ищем устройство с именем "HECATE G5BT"
            {
                _deviceIndex = i;
                break;
            }
        }

        if (_deviceIndex == 0)
        {
            _loggingService.WriteLine("[MIC] Устройство 'HECATE G5BT' не найдено. Используется устройство по умолчанию.", LogLevel.Warning);
        }
        else
        {
            _loggingService.WriteLine($"[MIC] Используется устройство: {WaveInEvent.GetCapabilities(_deviceIndex).ProductName}", LogLevel.Info);
        }
        _waveIn.DeviceNumber = _deviceIndex;        // Используем найденное устройство
        _waveIn.WaveFormat = new WaveFormat(44100, 1);  // 44100 Hz, моно

        _waveIn.DataAvailable += WaveIn_DataAvailable;
        _waveIn.StartRecording();
    }

    private void StopListening()
    {
        if (_waveIn != null)
        {
            _waveIn.StopRecording();
            _waveIn.Dispose();
            _waveIn = null;
            _loggingService.WriteLine($"[MIC] Прослушивание микрофона остановлено", LogLevel.Info);
        }
    }

    private void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
    {
        short maxAmplitude = 0;
        for (int i = 0; i < e.BytesRecorded; i += 2)
        {
            short sample = Math.Abs(BitConverter.ToInt16(e.Buffer, i));
            if (sample > maxAmplitude) maxAmplitude = sample;
        }

        if (maxAmplitude >= _minAmplitudeForTalking)
        {
            lastLoudTime = Environment.TickCount;
            if(!_isTalking) {
                _loggingService.WriteLine("[MIC] Громкий звук обнаружен!", LogLevel.Info);
                NextStateValue();
            }
        }
        else
        {
            if (Environment.TickCount - lastLoudTime > _silenceThreshold && _isTalking)
            {
                _loggingService.WriteLine("[MIC] Тишина...", LogLevel.Info);
                NextStateValue();
            }
        }
    }

    private void NextStateValue()
    {
        _isTalking = !_isTalking;
        WebSocketMessage message = new WebSocketMessage(
            "Avatar",
            "isTalking",
            new { isTalking = _isTalking }
        );
        _webSocketServerService.Broadcast(message);
    }

    public void StartService()
    {
        StartListening();
    }
    public void StopService()
    {
        StopListening();
    }
}