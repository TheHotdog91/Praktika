using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите строку:");
        string input = Console.ReadLine();

        Tuple<string, Dictionary<char, int>> result = ProcessString(input);

        if (result != null)
        {
            Console.WriteLine("Обработанная строка: " + result.Item1);
            Console.WriteLine("Информация о повторении символов:");
            foreach (var pair in result.Item2)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value} раз");
            }
        }

        Console.ReadLine(); // Чтобы консольное окно не закрывалось сразу после выполнения программы
    }

    static Tuple<string, Dictionary<char, int>> ProcessString(string input)
    {
        if (IsValidInput(input))
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            if (input.Length % 2 == 0)
            {
                // Если строка имеет чётное количество символов
                int middleIndex = input.Length / 2;
                string firstHalf = input.Substring(0, middleIndex);
                string secondHalf = input.Substring(middleIndex);

                // Переворачиваем обе подстроки и соединяем
                string reversedResult = ReverseString(firstHalf) + ReverseString(secondHalf);

                // Считаем повторения символов
                foreach (char c in reversedResult)
                {
                    if (charCount.ContainsKey(c))
                    {
                        charCount[c]++;
                    }
                    else
                    {
                        charCount.Add(c, 1);
                    }
                }

                return Tuple.Create(reversedResult, charCount);
            }
            else
            {
                // Если строка имеет нечётное количество символов
                string reversedInput = ReverseString(input);

                // Добавляем изначальную строку к перевёрнутой
                string result = reversedInput + input;

                // Считаем повторения символов
                foreach (char c in result)
                {
                    if (charCount.ContainsKey(c))
                    {
                        charCount[c]++;
                    }
                    else
                    {
                        charCount.Add(c, 1);
                    }
                }

                return Tuple.Create(result, charCount);
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
