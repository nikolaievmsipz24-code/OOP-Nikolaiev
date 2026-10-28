using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab8v19
{

    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public LogEntry(string message)
        {
            Timestamp = DateTime.Now;
            Message = message;
        }


        public virtual void Display()
        {
            Console.WriteLine($"[{Timestamp:HH:mm:ss}] [LOG]: {Message}");
        }
    }


    public class InfoLog : LogEntry
    {
        public string Source { get; set; }

        public InfoLog(string message, string source) : base(message)
        {
            Source = source;
        }

        public override void Display()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[{Timestamp:HH:mm:ss}] [INFO] ({Source}): {Message}");
            Console.ResetColor();
        }
    }


    public class WarningLog : LogEntry
    {
        public int Severity { get; set; }

        public WarningLog(string message, int severity) : base(message)
        {
            Severity = severity;
        }

        public override void Display()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{Timestamp:HH:mm:ss}] [WARNING] [Severity: {Severity}]: {Message}");
            Console.ResetColor();
        }
    }


    public class ErrorLog : LogEntry
    {
        public string ExceptionType { get; set; }

        public ErrorLog(string message, string exceptionType) : base(message)
        {
            ExceptionType = exceptionType;
        }

        public override void Display()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{Timestamp:HH:mm:ss}] [ERROR] [{ExceptionType}]: {Message}");
            Console.ResetColor();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;


            List<LogEntry> logs = new List<LogEntry>
            {
                new InfoLog("Користувач успішно авторизувався в системі.", "AuthService"),
                new WarningLog("Використання пам'яті перевищило 80%.", 3),
                new ErrorLog("Не вдалося підключитися до бази даних.", "SqlException"),
                new InfoLog("Завантаження модулю конфігурації завершено.", "ConfigLoader"),
                new WarningLog("З'єднання з інтернетом нестабільне.", 2),
                new ErrorLog("NullReferenceException у фоновому сервісі.", "NullReferenceException")
            };

            Console.WriteLine("=== ПОЛІМОРФНИЙ ВИВІД ЖУРНАЛІВ (ЛОГІВ) ===");

            foreach (var log in logs)
            {
                log.Display();
            }

            Console.WriteLine("\n=== АГРЕГАЦІЯ РЕЗУЛЬТАТІВ ТА СТАТИСТИКА ===");


            int totalLogs = logs.Count;


            int infoCount = logs.OfType<InfoLog>().Count();
            int warningCount = logs.OfType<WarningLog>().Count();
            int errorCount = logs.OfType<ErrorLog>().Count();


            int maxSeverity = logs.OfType<WarningLog>().Any()
                ? logs.OfType<WarningLog>().Max(w => w.Severity)
                : 0;


            Console.WriteLine($"Всього записів у лозі: {totalLogs}");
            Console.WriteLine($"  - Інформаційних (InfoLog): {infoCount}");
            Console.WriteLine($"  - Попереджень (WarningLog): {warningCount}");
            Console.WriteLine($"  - Помилок (ErrorLog): {errorCount}");
            Console.WriteLine($"Максимальний рівень важливості попереджень (Max Severity): {maxSeverity}");

            Console.WriteLine("\nНатисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
