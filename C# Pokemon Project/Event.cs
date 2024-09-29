using System;
using System.Collections.Generic;
using System.Linq;

public class Event
{
    public int action_count = 0;
    public bool ChoiceEvent(int nb_choice)
    {
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (action_count - 1 < 0)
                {
                    action_count = nb_choice - 1;
                }
                else
                {
                    action_count--;
                }
                return false;

            case ConsoleKey.DownArrow:
                if (action_count + 1 == nb_choice)
                {
                    action_count = 0;
                }
                else
                {
                    action_count++;
                }
                return false;

            case ConsoleKey.Enter:
                return true;

            default:
                return false;
        }
    }

    public bool QuantityEvent(int pokemoney, int prizeObject)
    {
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
        

        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (prizeObject * action_count+1 <= pokemoney)
                {
                    action_count += 1;
                }
                return false;

            case ConsoleKey.DownArrow:
                if(action_count - 1 < 0)
                {
                    action_count = 0;
                }
                else 
                {
                    action_count -= 1;
                }
                return false;

            case ConsoleKey.Enter:
                return true;

            default:
                return false;
        }
    }

    public bool QuantityEventSale(int quantityObject)
    {
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();


        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (action_count + 1 <= quantityObject)
                {
                    action_count += 1;
                }
                return false;

            case ConsoleKey.DownArrow:
                if (action_count - 1 >= quantityObject)
                {
                    action_count -= 1;
                }
                return false;

            case ConsoleKey.Enter:
                return true;

            default:
                return false;
        }
    }

    public bool ListPokedex(List<Pokemon> pokedex)
    {
        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();


        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (action_count - 1 < 0)
                {
                    action_count = 0;
                }
                else
                {
                    action_count -= 1;
                }
                return false;

            case ConsoleKey.DownArrow:
                if (action_count + 1 > pokedex.Count -1)
                {
                    action_count = pokedex.Count - 1;
                }
                else
                {
                    action_count += 1;
                }
                return false;

            case ConsoleKey.Enter:
                return true;

            default:
                return false;
        }
    }

}