using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Введите строку:");
        string input = Console.ReadLine();

        if (IsValidInput(input))
        {
            Console.WriteLine("Выберите алгоритм сортировки:");
            Console.WriteLine("1. Быстрая сортировка (Quicksort)");
            Console.WriteLine("2. Сортировка деревом (Treesort)");

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
            {
                Console.WriteLine("Введите 1 или 2.");
            }

            Tuple<string, Dictionary<char, int>, string, string> result = await ProcessStringAsync(input, choice);

            if (result != null)
            {
                Console.WriteLine("Обработанная строка: " + result.Item1);
                Console.WriteLine("Информация о повторении символов:");
                foreach (var pair in result.Item2)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value} раз");
                }
                Console.WriteLine("Самая длинная подстрока, начинающаяся и заканчивающаяся на гласную: " + result.Item3);
                Console.WriteLine("Отсортированная обработанная строка: " + result.Item1);
                Console.WriteLine("Урезанная обработанная строка: " + result.Item4);
            }
        }
        else
        {
            Console.WriteLine("Ошибка: Введены недопустимые символы. Необходимо вводить только буквы английского алфавита в нижнем регистре.");
        }

        Console.ReadLine(); // Чтобы консольное окно не закрывалось сразу после выполнения программы
    }

    static async Task<Tuple<string, Dictionary<char, int>, string, string>> ProcessStringAsync(string input, int sortChoice)
    {
        Dictionary<char, int> charCount = new Dictionary<char, int>();
        string longestVowelSubstring = FindLongestVowelSubstring(input);

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

            string sortedResult = SortString(reversedResult, sortChoice);
            string truncatedResult = await GetTruncatedStringAsync(sortedResult);

            return Tuple.Create(sortedResult, charCount, longestVowelSubstring, truncatedResult);
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

            string sortedResult = SortString(result, sortChoice);
            string truncatedResult = await GetTruncatedStringAsync(sortedResult);

            return Tuple.Create(sortedResult, charCount, longestVowelSubstring, truncatedResult);
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

    static string FindLongestVowelSubstring(string input)
    {
        string vowels = "aeiouy";
        string longestSubstring = "";
        string currentSubstring = "";

        foreach (char c in input)
        {
            if (vowels.Contains(c))
            {
                currentSubstring += c;
            }
            else
            {
                if (currentSubstring.Length > longestSubstring.Length)
                {
                    longestSubstring = currentSubstring;
                }
                currentSubstring = "";
            }
        }

        // Проверяем подстроку, заканчивающую строку
        if (currentSubstring.Length > longestSubstring.Length)
        {
            longestSubstring = currentSubstring;
        }

        return longestSubstring;
    }

    static string SortString(string input, int sortChoice)
    {
        if (sortChoice == 1)
        {
            // Быстрая сортировка (Quicksort)
            char[] charArray = input.ToCharArray();
            Quicksort(charArray, 0, charArray.Length - 1);
            return new string(charArray);
        }
        else
        {
            // Сортировка деревом (Tree sort)
            List<char> charList = input.ToList();
            charList = Treesort(charList);
            return new string(charList.ToArray());
        }
    }

    static void Quicksort(char[] arr, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high);

            Quicksort(arr, low, pi - 1);
            Quicksort(arr, pi + 1, high);
        }
    }

    static int Partition(char[] arr, int low, int high)
    {
        char pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;

                char temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }

        char temp1 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp1;

        return i + 1;
    }

    static List<char> Treesort(List<char> arr)
    {
        SortedSet<char> treeSet = new SortedSet<char>(arr);
        return new List<char>(treeSet);
    }

    static async Task<string> GetTruncatedStringAsync(string input)
    {
        try
        {
            // Получаем случайное число из удаленного API
            int randomIndex = await GetRandomNumber();

            // Удаляем символ в указанной позиции
            if (randomIndex >= 0 && randomIndex < input.Length)
            {
                input = input.Remove(randomIndex, 1);
            }

            return input;
        }
        catch (Exception)
        {
            // Если возникает ошибка при запросе к удаленному API,
            // то генерируем случайное число средствами .NET
            Random random = new Random();
            int randomIndex = random.Next(0, input.Length);

            // Удаляем символ в случайной позиции
            if (randomIndex >= 0 && randomIndex < input.Length)
            {
                input = input.Remove(randomIndex, 1);
            }

            return input;
        }
    }

    static async Task<int> GetRandomNumber()
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "http://www.randomnumberapi.com/api/v1.0/random";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return int.Parse(result);
            }
            else
            {
                throw new Exception("Error fetching random number from API");
            }
        }
    }
}


