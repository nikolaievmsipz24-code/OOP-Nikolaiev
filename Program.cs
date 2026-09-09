using System;

namespace OOP_Lab2
{
        public class Course
    {
        // 1. Приватні поля
        private string _title;
        private string _teacherName;
        private int _credits;

       
        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) ? "Untitled Course" : value;
        }

        public string TeacherName
        {
            get => _teacherName;
            set => _teacherName = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
        }

        public int Credits
        {
            get => _credits;
            set
            {
                // Валідація: кредити мають бути більші за 0
                if (value <= 0)
                {
                    Console.WriteLine("⚠️ Помилка: кількість кредитів має бути більше 0! Встановлено значення за замовчуванням (1).");
                    _credits = 1;
                }
                else
                {
                    _credits = value;
                }
            }
        }

        

        
        public Course(string title, string teacherName, int credits)
        {
            Title = title;
            TeacherName = teacherName;
            Credits = credits; // спрацьовує сеттер з валідацією
            Console.WriteLine($"[Конструктор] Створено курс: \"{Title}\" ({Credits} кр.), викладач: {TeacherName}");
        }

        
        public Course() : this("New Course", "N/A", 3)
        {
            
        }

        
        public void EnrollStudent(string studentName)
        {
            Console.WriteLine($"🎓 Студента {studentName} успішно зараховано на курс \"{Title}\" ({Credits} кредитів).");
        }

        
        ~Course()
        {
            Console.WriteLine($"❌ [Деструктор] Об'єкт курсу \"{_title}\" знищено з пам'яті.");
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Creating objects ===");

            
            Course course1 = new Course();

            
            Course course2 = new Course("Об'єктно-орієнтоване програмування", "Іванов І. І.", 5);

            
            Course course3 = new Course("Бази даних", "Петров П. П.", -2);

            Console.WriteLine("\n=== Demonstration of methods ===");
            course1.EnrollStudent("Олексій");
            course2.EnrollStudent("Марія");
            course3.EnrollStudent("Дмитро");

            Console.WriteLine("\n=== End of Main, preparing for GC ===");

            
            course1 = null;
            course2 = null;
            course3 = null;

            
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nЗавершення роботи програми.");
        }
    }
}
