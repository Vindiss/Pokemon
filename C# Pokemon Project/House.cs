using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class House
{
    public bool exitEquipe = false;
    public Trainer Player = new Trainer("");

    public House(Trainer player)
    { 
        Player = player;
    }

    public void Equipe()
    {
        if (Player.Team.Count > 6)
        {
            Event event_equipe = new Event();

            while (!exitEquipe)
            {
                Console.Clear();
                Console.WriteLine("Voici tout vos pokemon :\n");
                if (event_equipe.action_count == 0)
                {
                    Console.WriteLine($"> {Player.Pokedex[event_equipe.action_count].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 1].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 2].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 3].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 4].Name}");
                }
                else if (event_equipe.action_count == 1)
                {
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 1].Name}");
                    Console.WriteLine($"> {Player.Pokedex[event_equipe.action_count].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 1].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 2].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 3].Name}");
                }
                else if (event_equipe.action_count <= Player.Pokedex.Count - 3)
                {
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 2].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 1].Name}");
                    Console.WriteLine($"> {Player.Pokedex[event_equipe.action_count].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 1].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 2].Name}");
                }
                else if (event_equipe.action_count <= Player.Pokedex.Count - 2)
                {
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 3].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 2].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 1].Name}");
                    Console.WriteLine($"> {Player.Pokedex[event_equipe.action_count].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count + 1].Name}");
                }
                else if (event_equipe.action_count >= Player.Pokedex.Count - 1)
                {
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 4].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 3].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 2].Name}");
                    Console.WriteLine($"  {Player.Pokedex[event_equipe.action_count - 1].Name}");
                    Console.WriteLine($"> {Player.Pokedex[event_equipe.action_count].Name}");
                }

                exitEquipe = event_equipe.ListPokedex(Player.Pokedex);
                if (exitEquipe)
                {
                    foreach (var poke in Player.Team)
                    {
                        if (poke == Player.Pokedex[event_equipe.action_count])
                        {
                            exitEquipe = false;
                            Console.Write("Pokemon deja dans l'equipe");
                            Console.Write("\nAppuyer pour passer...");
                            Console.ReadKey();
                            break;
                        }
                    }
                }
            }
            Pokemon newPokenomTeam = Player.Pokedex[event_equipe.action_count];
            exitEquipe = false;
            Pokemon pokemonTeam = ChooseTeamPokemon(Player);
            Player.Team[Player.Team.IndexOf(pokemonTeam, 0)] = newPokenomTeam;
        }
        else
        {
            Console.WriteLine("Vous n'avez pas assez de pokemon pour changer votre équipe");
            Console.Write("\nAppuyer pour passer...");
            Console.ReadKey();
        }
    }


    public Pokemon ChooseTeamPokemon(Trainer trainer)
    {
        bool first = false;
        int nb_event = 0;
        Console.Clear();
        Console.WriteLine("Choisissez votre Pokemon a remplacer :");
        Console.WriteLine("  Nom | Niveau | Type1 | Type2 | Pv/PvMax | Attack | Defense | AttackSpecial | DefenseSpecial");
        for (int i = 0; i < trainer.Team.Count; i++)
        {
            if (!first && trainer.Team[i].IsAlive())
            {
                Console.WriteLine($"> {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv} / {trainer.Team[i].PvMax} PV | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                first = true;
                nb_event++;
            }
            else if (trainer.Team[i].IsAlive())
            {
                Console.WriteLine($"  {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv} / {trainer.Team[i].PvMax} PV | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                nb_event++;
            }
        }

        bool choice = false;
        Event event_choice = new Event();
        while (!choice)
        {
            choice = event_choice.ChoiceEvent(nb_event);

            Console.Clear();
            Console.WriteLine("Choisissez votre Pokemon a remplacer :");
            Console.WriteLine("  Nom | Niveau | Type1 | Type2 | Pv/PvMax | Attack | Defense | AttackSpecial | DefenseSpecial");
            for (int i = 0; i < trainer.Team.Count; i++)
            {
                if (event_choice.action_count == i && trainer.Team[i].IsAlive())
                {
                    Console.WriteLine($"> {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv}/{trainer.Team[i].PvMax} | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                }
                else if (trainer.Team[i].IsAlive())
                {
                    Console.WriteLine($"  {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv}/{trainer.Team[i].PvMax} | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                }
            }
        }
        return trainer.Team[event_choice.action_count];
    }
}
