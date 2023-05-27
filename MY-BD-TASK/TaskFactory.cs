using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY_BD_TASK
{
    public static class MyTaskFactory
    {
        const string connectionString = "Server=127.0.0.1;Port=5432;Database=BDTask;Username=postgres;Password=******;";

        public static async void StartExecuteTasks()
        {
            int num = 0;
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            while (num != 999)
            {
                num = InputOutput();

                switch (num)
                {
                    case 1:
                        Task1(connection);
                        break;
                    case 2:
                        Task2(connection);
                        break;
                    case 3:
                        Task3(connection);
                        break;
                    case 4:
                        Console.WriteLine("Тут ничего нет)");
                        break;
                    case 999:
                        break;
                    default:
                        Console.WriteLine("Неверно, повторите ввод");
                        break;
                }
            }
            connection.Close();
        }

        public static int InputOutput()
        {
            int num = 0;

            Console.WriteLine("Добро пожаловать в программу управления таблицами для Сбербанка. Введите номер задачи:");
            Console.WriteLine("Задача 1: Создать таблицы");
            Console.WriteLine("Задача 2: Заполнить таблицы");
            Console.WriteLine("Задача 3: Вывести содержимое таблиц");
            Console.WriteLine("Задачу 4 не сделаль :(");
            Console.WriteLine("999 - выйти\n");

            Int32.TryParse(Console.ReadLine(), out num);
            
            return num;

        }

        public static void Task1(NpgsqlConnection connection)
        {
            Console.WriteLine("Задача 1: добавить таблицы. Здесь будет три таблицы: Клиент, Счет, Операции");
            var script1 = "CREATE TABLE Client\r\n" +
                "(\r\n" +
                "   \"Id\" serial NOT NULL,\r\n " +
                "   \"Surname\" \"text\" NOT NULL,\r\n" +
                "   \"Name\" \"text\" NOT NULL,\r\n" +
                "   \"Patronymic\" \"text\",\r\n" +
                "   \"Doc_Series\" \"text\" NOT NULL,\r\n" +
                "   \"Doc_Number\" \"text\" NOT NULL,\r\n" +
                "   PRIMARY KEY (\"Id\")\r\n);";
            var script2 = "CREATE TABLE Bank_Account\r\n" +
                "(\r\n    " +
                "    \"Id\" serial NOT NULL,\r\n" +
                "    \"Accout_Number\" \"text\" NOT NULL,\r\n" +
                "    \"ClientId\" INTEGER REFERENCES Client (\"Id\"),\r\n" +
                "    \"IsBlocked\" boolean NOT NULL,\r\n" +
                "    \"Amount\" BIGINT NOT NULL,\r\n" +
                "    PRIMARY KEY (\"Id\")\r\n);";
            var script3 = "CREATE TABLE Operations\r\n" +
                "(\r\n" +
                "    \"Id\" serial NOT NULL,\r\n" +
                "    \"Id-Sender\" INTEGER REFERENCES Bank_Account (\"Id\"),\r\n" +
                "    \"Id-Receiver\" INTEGER REFERENCES Bank_Account (\"Id\"),\r\n" +
                "    \"Summ\" BIGINT NOT NULL,\r\n" +
                "    PRIMARY KEY (\"Id\")\r\n);";
            
            var cmd1 = new NpgsqlCommand(script1, connection);
            cmd1.ExecuteNonQuery();
            var cmd2 = new NpgsqlCommand(script2, connection);
            cmd2.ExecuteNonQuery();
            var cmd3 = new NpgsqlCommand(script3, connection);
            cmd3.ExecuteNonQuery();
            
            Console.WriteLine("Задача 1: выполнение окончено\n");
        }

        public static void Task2(NpgsqlConnection connection)
        {
            Console.WriteLine("Задача 2: заполнить таблицы");
            var script1 = "INSERT INTO client(\r\n" +
                "\t\"Surname\", \"Name\", \"Patronymic\", \"Doc_Series\", \"Doc_Number\")\r\n" +
                "\tVALUES ('Белов', 'Никита', 'Павлович', '8437', '123456'), " +
                "('Максимов', 'Максим', 'Максимович', '1256', '351345'), " +
                "('Иванов', 'Иван', 'Иванович', '7843', '082145'), " +
                "('Павлов', 'Павел', 'Павлович', '4134', '613123'), " +
                "('Елисеев', 'Елисей', 'Елисеевич', '1212', '121212');";
            var script2 = "INSERT INTO bank_account" +
                "(\r\n" +
                "\t\"Accout_Number\", \"ClientId\", \"IsBlocked\", \"Amount\")\r\n" +
                "\tVALUES ('123456', 1, false, 1337)," +
                "('431356', 2, false, 14121)," +
                "('789024', 3, false, 111111)," +
                "('535353', 4, false, 131237)," +
                "('627465', 5, false, 223443);";
            var script3 = "INSERT INTO operations" +
                "(\r\n" +
                "\t\"Id-Sender\"," +
                " \"Id-Receiver\"," +
                " \"Summ\")\r\n" +
                "\tVALUES (1, 2, 100)," +
                "(1, 2, 1090)," +
                "(1, 3, 1060)," +
                "(1, 3, 1040)," +
                "(3, 5, 1010);";

            var cmd1 = new NpgsqlCommand(script1, connection);
            cmd1.ExecuteNonQuery();
            var cmd2 = new NpgsqlCommand(script2, connection);
            cmd2.ExecuteNonQuery();
            var cmd3 = new NpgsqlCommand(script3, connection);
            cmd3.ExecuteNonQuery();

            Console.WriteLine("Задача 2: выполнение окончено\n");
        }

        public static void Task3(NpgsqlConnection connection)
        {
            Console.WriteLine("Задача 3: Вывести содержимое таблиц \n");
            Console.WriteLine("Содержимое таблицы клиенты:");
            Console.WriteLine(" Id | Surname | Name | Patronymic | Doc_Series | Doc_Number |");

            var script1 = "SELECT \"Id\", \"Surname\", \"Name\", \"Patronymic\", \"Doc_Series\", \"Doc_Number\"\r\n" +
                "\tFROM client;";
            var cmd1 = new NpgsqlCommand(script1, connection);
            var reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                Console.WriteLine($" {reader1.GetInt32(0)} | {reader1.GetString(1)}" +
                    $" | {reader1.GetString(2)} | {reader1.GetString(3)} | {reader1.GetString(4)} | {reader1.GetString(5)} |");
            }
            reader1.Close();

            Console.WriteLine("\nСодержимое таблицы банковские счета:");
            Console.WriteLine("Id | Accout_Number | ClientId | IsBlocked | Amount |");

            var script2 = "SELECT \"Id\", \"Accout_Number\", \"ClientId\", \"IsBlocked\", \"Amount\"\r\n" +
                "\tFROM bank_account;";
            var cmd2 = new NpgsqlCommand(script2, connection);
            var reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                Console.WriteLine($" {reader2.GetInt32(0)} | {reader2.GetString(1)}" +
                    $" | {reader2.GetInt32(2)} | {reader2.GetBoolean(3)} | {reader2.GetInt32(4)} |");
            }
            reader2.Close();

            Console.WriteLine("\nСодержимое таблицы операции:");
            Console.WriteLine("Id | Id-Sender | Id-Receiver | Summ |");

            var script3 = "SELECT \"Id\", \"Id-Sender\", \"Id-Receiver\", \"Summ\"\r\n" +
                "\tFROM operations;";
            var cmd3 = new NpgsqlCommand(script3, connection);
            var reader3 = cmd3.ExecuteReader();
            while (reader3.Read())
            {
                Console.WriteLine($" {reader3.GetInt32(0)} | {reader3.GetInt32(1)}" +
                    $" | {reader3.GetInt32(2)} | {reader3.GetInt32(3)} |");
            }
            reader3.Close();

            Console.WriteLine("\nЗадача 3: выполнение окончено\n");
        }
    }
}
