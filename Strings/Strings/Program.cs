// See https://aka.ms/new-console-template for more information


using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

public class Program
{
    public static void Main()
    {
        string str1 = "Hello, ";
        string str2 = "World!";

        Console.WriteLine("Concatenating two strings without using built-in methods:");
        string concatenated = concatStrings(str1, str2);
        Console.WriteLine($"{concatenated}\n");

        Console.WriteLine("Reversing a string without using built-in methods:");
        string reversed = reverseString(str2);
        Console.WriteLine($"Original word: {str2}, Reversed: {reversed}\n");

        char targetChar = 'l';
        Console.WriteLine($"Counting occurrences of character '{targetChar}' in string \"{str1}\":");
        int count = letterCount(str1, targetChar);
        Console.WriteLine($"The character '{targetChar}' appears {count} times in \"{str1}\".");
        var toupper = toUpperCase("Hello World");
        Console.WriteLine($"Uppercase conversion without built-in methods: {toupper}\n");


    }

    public static string concatStrings(string str1, string str2)
    {

        string result = "";

        for (int i = 0; i < str1.Length; i++)
        {
            result += str1[i];
        }

        for(int j = 0; j < str2.Length; j++)
        {
            result += str2[j];
        }

        return result;

    }

    public static string reverseString(string str)
    {
        string result = "";

        for(int i = str.Length -1; i >= 0; i--)
        {
            result += str[i];
        }

        return result;
    }

    public static int letterCount(string str, char target)
    {
        int count = 0;
        for(int i = 0; i < str.Length; i++)
        {
            if (str[i] == target)
            {
                count++;
            }
        }
        return count;
    }

    public static bool isPalindrome(string str)
    {
        int left = 0;
        int right = str.Length - 1;

        while (left < right)
        {
            if (str[left] != str[right])
            {
                return false;
            }
            left++;
            right--;
        }
        return true;
    }

    public static string toUpperCase(string str)
    {
        string result = "";
        for(int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (c >= 'a' && c <= 'z')
            {
                c = (char)(c - ('a' - 'A'));
            }
            result += c;
        }
        return result;
    }



}


