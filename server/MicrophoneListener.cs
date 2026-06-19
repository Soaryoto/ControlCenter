using NAudio.Wave;

namespace Soaryoto
{
    public class MicrophoneListener
    {
        private static WaveInEvent? _waveIn;
        private static int _deviceIndex = 0; // Индекс устройства для записи (0 - по умолчанию)
        
        private static int _minAmplitudeForTalking = 1000;
        private static int lastLoudTime = 0;
        private static int _silenceThreshold = 300; // Время в миллисекундах, после которого считается, что пользователь перестал говорить

        public static void StartListening()
        {
            ConsoleWriter.WriteLine("[MIC] Начало прослушивания микрофона...", ConsoleColor.White);

            _waveIn = new WaveInEvent();

            for (int i = 0; i < WaveInEvent.DeviceCount; i++)
            {
                var caps = WaveInEvent.GetCapabilities(i);
                ConsoleWriter.WriteLine($"[{i}] {caps.ProductName}", ConsoleColor.White);
                if (caps.ProductName.Contains("HECATE G5BT")) // Ищем устройство с именем "HECATE G5BT"
                {
                    _deviceIndex = i;
                    break;
                }
            }

            if (_deviceIndex == 0)
            {
                ConsoleWriter.WriteLine("[MIC] Устройство 'HECATE G5BT' не найдено. Используется устройство по умолчанию.", ConsoleColor.White);
            }
            else
            {
                ConsoleWriter.WriteLine($"[MIC] Используется устройство: {WaveInEvent.GetCapabilities(_deviceIndex).ProductName}", ConsoleColor.White);
            }
            _waveIn.DeviceNumber = _deviceIndex;        // Используем найденное устройство
            _waveIn.WaveFormat = new WaveFormat(44100, 1);  // 44100 Hz, моно

            _waveIn.DataAvailable += WaveIn_DataAvailable;
            _waveIn.StartRecording();
        }

        public static void StopListening()
        {
            if (_waveIn != null)
            {
                _waveIn.StopRecording();
                _waveIn.Dispose();
                _waveIn = null;
                ConsoleWriter.WriteLine($"[MIC] Прослушивание микрофона остановлено", ConsoleColor.Red);
            }
        }

        private static void WaveIn_DataAvailable(object? sender, WaveInEventArgs e)
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
                ConsoleWriter.WriteLine("[MIC] Громкий звук обнаружен!", ConsoleColor.White);
            }
            else
            {
                if (Environment.TickCount - lastLoudTime > _silenceThreshold)
                {
                    ConsoleWriter.WriteLine("[MIC] Тишина...", ConsoleColor.Gray);
                }
            }
        }
    }
}