using Soaryoto.Interfaces;
using NAudio.Wave;

namespace Soaryoto
{
    public class MicrophoneListener : IMicrophoneListener
    {
        private readonly IWebSocketServer _webSocketServer;
        private WaveInEvent? _waveIn;
        private int _deviceIndex = 0; // Индекс устройства для записи (0 - по умолчанию)
        private int _minAmplitudeForTalking = 1000;
        private int lastLoudTime = 0;
        private int _silenceThreshold = 300; // Время в миллисекундах, после которого считается, что пользователь перестал говорить
        private bool _isTalking = false;
        public MicrophoneListener(IWebSocketServer webSocketServer)
        {
            _webSocketServer = webSocketServer;
        }
        private void StartListening()
        {
            ConsoleWriter.WriteLine("[MIC] Начало прослушивания микрофона...", ConsoleColor.Green);

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
                ConsoleWriter.WriteLine("[MIC] Устройство 'HECATE G5BT' не найдено. Используется устройство по умолчанию.", ConsoleColor.Red);
            }
            else
            {
                ConsoleWriter.WriteLine($"[MIC] Используется устройство: {WaveInEvent.GetCapabilities(_deviceIndex).ProductName}", ConsoleColor.Green);
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
                ConsoleWriter.WriteLine($"[MIC] Прослушивание микрофона остановлено", ConsoleColor.Red);
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
                    ConsoleWriter.WriteLine("[MIC] Громкий звук обнаружен!", ConsoleColor.White);
                    NextStateValue();
                }
            }
            else
            {
                if (Environment.TickCount - lastLoudTime > _silenceThreshold && _isTalking)
                {
                    ConsoleWriter.WriteLine("[MIC] Тишина...", ConsoleColor.Gray);
                    NextStateValue();
                }
            }
        }

        private void NextStateValue()
        {
            _isTalking = !_isTalking;
            _webSocketServer.Broadcast($"{{\"isTalking\": {_isTalking.ToString().ToLower()}}}");
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
}