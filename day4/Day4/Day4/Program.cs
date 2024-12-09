using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day4;

internal static class Program
{
    public static void Main(string[] args)
    {
        //var input = File.ReadLines("puzzle-part1-test.txt").ToList();
        var inputPart1 = File.ReadLines("puzzle-part1.txt").ToList();
        Part1(inputPart1);
        
        //var inputPart2 = File.ReadLines("puzzle-part2-test.txt").ToList();
        var inputPart2 = File.ReadLines("puzzle-part2.txt").ToList();
        Part2(inputPart2);
    }

    private static void Part1(List<string> input)
    {
        var matrix = input.Select(x => x.ToCharArray()).ToArray();
        
        var sum = Enumerable
            .Range(0, matrix.Length)
            .SelectMany(i => Enumerable.Range(0, matrix[0].Length)
                .Where(j => matrix[i][j] == 'X')
                .SelectMany(j => FindXmas(matrix, i, j))).Count(x => x is "XMAS" or "SAMX");

        Console.WriteLine("Sum: " + sum);
    }
    
    private static IEnumerable<string> FindXmas(char[][] matrix, int i, int j)
    {
        var height = matrix.Length;
        var width = matrix[0].Length;
        var searchWordLength = "XMAS".Length;
        
        if (j + searchWordLength <= width)
            yield return string.Join("", matrix[i].Skip(j).Take(searchWordLength));

        if (j - searchWordLength + 1 >= 0)
            yield return string.Join("", matrix[i].Skip(j - searchWordLength + 1).Take(searchWordLength));
        
        if (i + searchWordLength <= height)
            yield return string.Join("", matrix.Skip(i).Take(searchWordLength).Select(x => x[j]));
        
        if (i - searchWordLength + 1 >= 0)
            yield return string.Join("", matrix.Skip(i - searchWordLength + 1).Take(searchWordLength).Select(x => x[j]));
        
        if (i + searchWordLength <= height && j + searchWordLength <= width)
            yield return string.Join("", Enumerable.Range(0, searchWordLength).Select(x => matrix[i + x][j + x]));
        
        if (i + searchWordLength <= height && j - searchWordLength + 1 >= 0)
            yield return string.Join("", Enumerable.Range(0, searchWordLength).Select(x => matrix[i + x][j - x]));
        
        if (i - searchWordLength + 1 >= 0 && j + searchWordLength <= width)
            yield return string.Join("", Enumerable.Range(0, searchWordLength).Select(x => matrix[i - x][j + x]));
        
        if (i - searchWordLength + 1 >= 0 && j - searchWordLength + 1 >= 0)
            yield return string.Join("", Enumerable.Range(0, searchWordLength).Select(x => matrix[i - x][j - x]));
    }

    private static void Part2(List<string> lines)
    {
        var matrix = lines.Select(x => x.ToCharArray()).ToArray();
        
        var sum = Enumerable.Range(0, matrix.Length)
            .SelectMany(i => Enumerable.Range(0, matrix[0].Length)
                .Where(j => matrix[i][j] == 'A')
                .Select(j => FindXmas2(matrix, i, j)))
            .Sum();

        Console.WriteLine("Sum: " + sum);
    }

    private static int FindXmas2(char[][] matrix, int i, int j)
    {
        var height = matrix.Length;
        var width = matrix[0].Length;
        var searchWordLength = "MAS".Length;
        
        if (i + 1 >= height || i - 1 < 0 || j + 1 >= width || j - 1 < 0)
        {
            return 0;
        }

        var word1 = string.Join("",
            Enumerable.Range(0, searchWordLength).Select(x => matrix[(i - 1) + x][(j - 1) + x]));
        var word2 = string.Join("",
            Enumerable.Range(0, searchWordLength).Select(x => matrix[(i - 1) + x][(j + 1) - x]));
        
        return word1 is "MAS" or "SAM" && word2 is "MAS" or "SAM" ? 1 : 0;
    }
}