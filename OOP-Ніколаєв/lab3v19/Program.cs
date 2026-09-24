using System;

namespace lab3v19
{
    public class EventLogger : IDisposable
    {
        private bool _disposed = false;
        private string _logName;
        private bool _isLogging;

        public EventLogger(string logName)
        {
            _logName = logName;
            _isLogging = true;
            Console.WriteLine($"[Конструктор] Логер '{_logName}' створено. Поточний стан: логування активне.");
        }
        public void LogEvent(string eventName)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(EventLogger), "Неможливо писати логи: логер вже знищено.");
            }

            if (_isLogging)
            {
                Console.WriteLine($"[Лог - {_logName}] Подія: {eventName} (Час: {DateTime.Now:HH:mm:ss})");
            }
            else
            {
                Console.WriteLine($"[Лог - {_logName}] Логування зупинено. Подію '{eventName}' проігноровано.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                    Console.WriteLine($"-> [Dispose(true)] Звільнення керованих ресурсів для логера '{_logName}'.");
                }


                if (_isLogging)
                {
                    _isLogging = false;
                    Console.WriteLine($"-> [Dispose] Логування для '{_logName}' зупинено, ресурси звільнено.");
                }

                _disposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        ~EventLogger()
        {
            Console.WriteLine($"~ [Фіналізатор] Викликано для незвільненого логера '{_logName}'.");
            Dispose(false);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 1. Демонстрація оператора using ===");
            {
                using (var logger = new EventLogger("SystemLog"))
                {
                    logger.LogEvent("Систему запущено");
                    logger.LogEvent("Користувач увійшов у систему");
                }
            }
            Console.WriteLine("Блок using завершено.\n");


            Console.WriteLine("=== 2. Демонстрація ручного виклику Dispose() ===");
            var manualLogger = new EventLogger("SecurityLog");
            manualLogger.LogEvent("Спроба авторизації");
            manualLogger.Dispose(); // Явне звільнення ресурсів
            try
            {
                manualLogger.LogEvent("Успішний вхід");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Перехоплено виняток: {ex.Message}\n");
            }


            Console.WriteLine("=== 3. Демонстрація роботи деструктора через Garbage Collector ===");
            CreateLoggerWithoutDispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nРоботу програми завершено. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }


        static void CreateLoggerWithoutDispose()
        {
            var tempLogger = new EventLogger("TempLog");
            tempLogger.LogEvent("Тимчасова подія");

        }
    }
}