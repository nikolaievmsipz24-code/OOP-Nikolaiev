using System;

namespace lab6v19
{
    // Базовий клас Account
    public class Account
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Конструктор базового класу
        public Account(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public virtual void Login()
        {
            Console.WriteLine($"[Account] Користувач '{Username}' виконує стандартний вхід до системи.");
        }

        public string GetAccountType()
        {
            return "Базовий акаунт (Account)";
        }
    }

    // Похідний клас UserAccount
    public class UserAccount : Account
    {
        public string Email { get; set; }

        // Конструктор, що викликає base(...)
        public UserAccount(string username, string passwordHash, string email)
            : base(username, passwordHash)
        {
            Email = email;
        }
        // Перевизначення віртуального методу (override)
        public override void Login()
        {
            Console.WriteLine($"[UserAccount] Користувач '{Username}' (Email: {Email}) увійшов через систему користувачів.");
        }

        public void ChangePassword(string newPassword)
        {
            PasswordHash = newPassword;
            Console.WriteLine($"[UserAccount] Пароль для користувача '{Username}' успішно змінено.");
        }

        // Демонстрація new: приховування методу базового класу (не віртуального)
        public new string GetAccountType()
        {
            return "Акаунт звичайного користувача (UserAccount)";
        }
    }

    public class AdminAccount : Account
    {
        public int AccessLevel { get; set; }


        public AdminAccount(string username, string passwordHash, int accessLevel)
            : base(username, passwordHash)
        {
            AccessLevel = accessLevel;
        }


        public override void Login()
        {
            Console.WriteLine($"[AdminAccount] Адміністратор '{Username}' увійшов з рівнем доступу {AccessLevel}. Усі права розширено.");
        }


        public void GrantPermissions()
        {
            Console.WriteLine($"[AdminAccount] Адміністратор '{Username}' надав нові привілеї системі.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. СТВОРЕННЯ ОБ'ЄКТІВ ТА ПОЛІМОРФІЗМ (virtual / override) ===");


            Account acc1 = new Account("guest_user", "hash000");
            Account acc2 = new UserAccount("maks_nik", "hash123", "maks@gmail.com");
            Account acc3 = new AdminAccount("admin_root", "hash999", 5);
            acc1.Login();
            acc2.Login();
            acc3.Login();

            Console.WriteLine("\n=== 2. ВИКЛИК УНІКАЛЬНИХ МЕТОДІВ ЧЕРЕЗ ПРИВЕДЕННЯ ТИПІВ (downcasting) ===");

            ((UserAccount)acc2).ChangePassword("new_hash_456");
            ((AdminAccount)acc3).GrantPermissions();

            Console.WriteLine("\n=== 3. ДЕМОНСТРАЦІЯ РІЗНИЦІ МІЖ override ТА new ===");

            UserAccount concreteUser = new UserAccount("ivan_u", "hash789", "ivan@gmail.com");

            Account refUserAsAccount = concreteUser;

            Console.WriteLine("Виклик GetAccountType() через посилання типу UserAccount:");
            Console.WriteLine($"-> {concreteUser.GetAccountType()} (Викликається метод з new)");

            Console.WriteLine("\nВиклик GetAccountType() через посилання типу Account (базового):");
            Console.WriteLine($"-> {refUserAsAccount.GetAccountType()} (Викликається базовий метод через те, що new не створює поліморфного зв'язку)");

            Console.WriteLine("\nРоботу програми завершено.");
        }
    }
}
