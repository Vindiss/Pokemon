using System;
using System.IO;

public class Map

{
    public char[,] map;
    public char[,] copy_map;
    public int size_x;
    public int size_y;

    public Map()
    {
        string[] lines = File.ReadAllLines("data/carte.txt");
        size_x = lines.Length;
        size_y = lines[0].Length;

        for (int i = 1; i < size_x; i++)
        {
            if (lines[i].Length != size_y)
            {
                throw new ArgumentException("Les lignes du fichier ne sont pas de la m�me longueur.");
            }
        }

        map = new char[size_x, size_y];
        copy_map = new char[size_x, size_y];

        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                map[x, y] = lines[x][y];
                copy_map[x,y] = lines[x][y];
            }
        }
    }

    public void Draw()
    {
        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                if (map[x,y] == '"')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (map[x,y] == '\u256C' /*╬*/)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                }
                else if (map[x,y] == '/' || map[x,y] == '|' || map[x,y] == '_' || map[x, y] == map[12,38])
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (map[x, y] == '\u00A5'/*¥*/)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else if (map[x, y] == '°')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (map[x, y] == '0')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (map[x, y] == 'O')
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                Console.Write(map[x, y]);
                Console.ResetColor();
            }
            if (x == 3 )
            {
                Console.Write("\t\t\t  0 : Player");
            }
            if(x == 6)
            {
                Console.Write("\t\t\t  O : PNJ");
            }
            if (x == 9)
            {
                Console.Write("\t\t\t  \u00A5 : TeamRocket");
            }
            if (x == 12)
            {
                Console.Write("\t\t\t   ° : Objet");
            }
            if (x == 15)
            {
                Console.Write("\t\t\t  \u256C : Centre Pokemon$");
            }
            Console.Write('\n');
        }
    }
}