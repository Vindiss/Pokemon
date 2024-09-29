using System;

public class MenuPause
{
    bool exit = false;
    Game Game = Game.Instance;

    public void Stop()
    {
        Event event_choice = new Event();

        Console.WriteLine("Pause\n");
        Console.WriteLine("> Sauvegarde partie");
        Console.WriteLine("  Charger partie");
        Console.WriteLine("  Quitter");

        while (!exit)
        {
            bool choice_event = event_choice.ChoiceEvent(3);
            if (choice_event)
            {
                if (event_choice.action_count == 0)
                {
                    Game.SaveGame();
                }
                else if (event_choice.action_count == 1)
                {
                    Game.LoadGame();
                    exit = true;
                }
                else if (event_choice.action_count == 2)
                {
                    Game.isRunning = false;
                    exit = true;
                }
            }
            Console.Clear();
            Console.WriteLine("Pause\n");
            if (event_choice.action_count == 0)
            {
                Console.WriteLine("> Sauvegarde partie");
                Console.WriteLine("  Charger partie");
                Console.WriteLine("  Quitter");
            }
            else if (event_choice.action_count == 1)
            {
                Console.WriteLine("  Sauvegarde partie");
                Console.WriteLine("> Charger partie");
                Console.WriteLine("  Quitter");
            }
            else if (event_choice.action_count == 2)
            {
                Console.WriteLine("  Sauvegarde partie");
                Console.WriteLine("  Charger partie");
                Console.WriteLine("> Quitter");
            }
        }
    }
}