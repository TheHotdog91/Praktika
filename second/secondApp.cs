using System;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите строку:");
        string input = Console.ReadLine();

        string result = ProcessString(input);

        if (result != null)
        {
            Console.WriteLine("Обработанная строка: " + result);
        }

        Console.ReadLine(); // Чтобы консольное окно не закрывалось сразу после выполнения программы
    }

    static string ProcessString(string input)
    {
        if (IsValidInput(input))
        {
            if (input.Length % 2 == 0)
            {
                // Если строка имеет чётное количество символов
                int middleIndex = input.Length / 2;
                string firstHalf = input.Substring(0, middleIndex);
                string secondHalf = input.Substring(middleIndex);

                // Переворачиваем обе подстроки и соединяем
                string reversedResult = ReverseString(firstHalf) + ReverseString(secondHalf);

                return reversedResult;
            }
            else
            {
                // Если строка имеет нечётное количество символов
                string reversedInput = ReverseString(input);

                // Добавляем изначальную строку к перевёрнутой
                string result = reversedInput + input;

                return result;
            }
        }
        else
        {
            Console.WriteLine("Ошибка: Введены недопустимые символы. Необходимо вводить только буквы английского алфавита в нижнем регистре.");
            return null;
        }
    }

    static bool IsValidInput(string input)
    {
        // Проверяем, что в строке есть только буквы английского алфавита в нижнем регистре
        return input.All(char.IsLetter) && input.All(char.IsLower);
    }

    static string ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
