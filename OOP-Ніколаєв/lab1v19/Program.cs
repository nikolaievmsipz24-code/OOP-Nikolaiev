using System;

namespace lab1v19
{
    // оголошення класу weather згідно з варіантом
    public class Weather
    {
        // приватні поля
        private string city;
        private string date;
        private double temperature;

        // публічні властивості для доступу до полів
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        // властивість temperature за завданням
        public double Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        // конструктор для ініціалізації об’єкта
        public Weather(string city, string date, double temperature)
        {
            this.city = city;
            this.date = date;
            this.temperature = temperature;
        }

        // метод що виконує дію повязану з класом
        public void PrintForecast()
        {
            Console.WriteLine($"Прогноз погоди для міста {city} на {date}: {temperature}°C");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // створення 2-3 обєктів класу weather
            Weather weather1 = new Weather("Рівне", "2026-06-15", 22.5);
            Weather weather2 = new Weather("Київ", "2026-06-15", 25.0);
            Weather weather3 = new Weather("Львів", "2026-06-16", 19.8);

            // виклик їхніх методів та виведення результату в консоль
            weather1.PrintForecast();
            weather2.PrintForecast();
            weather3.PrintForecast();
        }
    }
}