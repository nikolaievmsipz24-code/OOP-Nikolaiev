using System;

namespace lab2v19
{
    // Клас Weather за моїм Варіантом 19
    class Weather
    {
        // 1. Приватні поля
        private string _city;
        private DateTime _date;
        private double _temperatureCelsius;

        // 2. Публічні властивості з валідацією
        public string City
        {
            get => _city;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва міста не може бути порожньою.");
                _city = value;
            }
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public double TemperatureCelsius
        {
            get => _temperatureCelsius;
            set
            {
                // Валідація: температура не може бути нижчою за абсолютний нуль (-273.15 °C)
                if (value < -273.15)
                    throw new ArgumentOutOfRangeException(nameof(value), "Температура не може бути нижчою за абсолютний нуль.");
                _temperatureCelsius = value;
            }
        }

        // 3. Конструктори з ланцюговим викликом (: this(...))
        // Конструктор за замовчуванням 
        public Weather() : this("Kyiv", DateTime.Now, 10.0)
        {
        }

        // Параметризований конструктор
        public Weather(string city, DateTime date, double temperatureCelsius)
        {
            City = city;
            Date = date;
            TemperatureCelsius = temperatureCelsius;
        }

        // 4. Метод, що виконує дію (виведення прогнозу погоди)
        public void PrintForecast()
        {
            Console.WriteLine($"[Прогноз] Місто: {City} | Дата: {Date.ToShortDateString()} | Температура: {TemperatureCelsius} °C");
        }

        // 5. Деструктор (фіналізатор)
        ~Weather()
        {
            Console.WriteLine($"[Деструктор] Об'єкт погоди для міста {City} знищено з пам'яті.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Створення об'єктів ---");

            // Створення об'єктів різними конструкторами
            Weather weather1 = new Weather(); // Використає конструктор за замовчуванням
            Weather weather2 = new Weather("Рівне", new DateTime(2026, 6, 15), 22.5); // Параметризований
            Weather weather3 = new Weather("Львів", DateTime.Now.AddDays(1), 18.0);   // Параметризований

            
            weather1.PrintForecast();
            weather2.PrintForecast();
            weather3.PrintForecast();

            Console.WriteLine("\n--- Кінець роботи програми, запуск збирача сміття ---");

            
            weather1 = null;
            weather2 = null;
            weather3 = null;

            // Примусовий виклик збирача сміття (для навчальних цілей, щоб побачити роботу деструкторів)
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("--- Програму завершено успішно ---");
        }
    }
}