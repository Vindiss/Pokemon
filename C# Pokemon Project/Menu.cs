using System;

public class Menu
{
    bool exit = false;
    Game game = Game.Instance;

    public void Start()
    {
        Event event_choice = new Event();

        Console.WriteLine("Pokemon façon David et Evan\n");
        Console.WriteLine("> Nouvelle partie");
        Console.WriteLine("  Charger partie");
        Console.WriteLine("  Quitter");

        while (!exit) 
        {
            bool choice_event = event_choice.ChoiceEvent(3);
            if (choice_event)
            {
                if (event_choice.action_count == 0)
                {
                    game.Start();
                }
                else if (event_choice.action_count == 1)
                {
                    game.LoadGame();
                    game.GameLoop();
                }
                else if (event_choice.action_count == 2)
                {
                    exit = true;
                }
            }
            Console.Clear();
            Console.WriteLine("Pokemon façon David et Evan\n");
            if (event_choice.action_count == 0)
            {
                Console.WriteLine("> Nouvelle partie");
                Console.WriteLine("  Charger partie");
                Console.WriteLine("  Quitter");
            }
            else if (event_choice.action_count == 1)
            {
                Console.WriteLine("  Nouvelle partie");
                Console.WriteLine("> Charger partie");
                Console.WriteLine("  Quitter");
            }
            else if (event_choice.action_count == 2)
            {
                Console.WriteLine("  Nouvelle partie");
                Console.WriteLine("  Charger partie");
                Console.WriteLine("> Quitter");
            }
        }
    }
}