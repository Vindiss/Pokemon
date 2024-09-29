using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

public class PokeCenter
{
    public bool exit = false;
    public bool exitBoutique = false;
    public bool exitInBoutique = false;
    public bool exitEquipe = false;
    public Trainer Player = new Trainer("");
    public PokeBall pokeBall = new PokeBall();
    public SuperBall superBall = new SuperBall();
    public HyperBall hyperBall = new HyperBall();
    public Potion potion = new Potion();
    public SuperPotion superPotion = new SuperPotion();
    public HyperPotion hyperPotion = new HyperPotion();
    public Object Objectvente = new Object();

    public PokeCenter(Trainer player)
    {
        Player = player;
    }
    

    public void Interface()
    {
        Event event_choice = new Event();

        Console.Clear();
        
        Console.WriteLine("Bienvenue au Centre Pokemon\n");
        Console.WriteLine("> Soigne tes Pokemon");
        Console.WriteLine("  Boutique");
        Console.WriteLine("  Equipe Pokemon");
        Console.WriteLine("  Sortir du Centre");

        while (!exit)
        {
            bool choice_event = event_choice.ChoiceEvent(4);
            if (choice_event)
            {
                if (event_choice.action_count == 0)
                {
                    foreach (var poke in Player.Team)
                    {
                        poke.Pv = poke.PvMax;
                    }
                    Console.WriteLine("Vos Pokemon sont soignée");
                    Console.Write("\nAppuyer pour passer...");
                    Console.ReadKey();
                }
                else if (event_choice.action_count == 1)
                {
                    Boutique();
                }
                else if (event_choice.action_count == 2)
                {
                    if (Player.Pokedex.Count >= 7)
                    {
                        Equipe();
                    }
                    else
                    {
                        Console.WriteLine("Vous ne posséder pas suffissament de pokemon !");
                        Console.Write("\nAppuyer pour passer...");
                        Console.ReadKey();
                    }
                }
                else if (event_choice.action_count == 3)
                {
                    exit = true;
                    Console.WriteLine("Merci de votre visite");
                    Console.Write("\nAppuyer pour passer...");
                    Console.ReadKey();
                }
            }
            Console.Clear();
            Console.WriteLine("Bienvenue au Centre Pokemon\n");
            if (event_choice.action_count == 0)
            {
                Console.WriteLine("> Soigne tes Pokemon");
                Console.WriteLine("  Boutique");
                Console.WriteLine("  Equipe Pokemon");
                Console.WriteLine("  Sortir du Centre"); ;
            }
            else if (event_choice.action_count == 1)
            {
                Console.WriteLine("  Soigne tes Pokemon");
                Console.WriteLine("> Boutique");
                Console.WriteLine("  Equipe Pokemon");
                Console.WriteLine("  Sortir du Centre");
            }
            else if (event_choice.action_count == 2)
            {
                Console.WriteLine("  Soigne tes Pokemon");
                Console.WriteLine("  Boutique");
                Console.WriteLine("> Equipe Pokemon");
                Console.WriteLine("  Sortir du Centre");
            }
            else if (event_choice.action_count == 3)
            {
                Console.WriteLine("  Soigne tes Pokemon");
                Console.WriteLine("  Boutique");
                Console.WriteLine("  Equipe Pokemon");
                Console.WriteLine("> Sortir du Centre");
            }
        }
        exit = false;
    }

    public void Boutique()
    {
        Event event_choice = new Event();
        Event event_boutique = new Event();

        Console.WriteLine("Bienvenue dans la boutique\n");
        Console.WriteLine("> Achete des objects");
        Console.WriteLine("  Vendre des objects");
        Console.WriteLine("  Sortir de la boutique");

        while (!exitBoutique)
        {
            bool choice_event = event_choice.ChoiceEvent(3);
            if (choice_event)
            {
                
                if (event_choice.action_count == 0)
                {
                    bool choice_boutique = false;
                    while (!exitInBoutique)
                    {
                        
                        Console.Clear();
                        if (event_boutique.action_count == 0)
                        {
                            Console.WriteLine($"> {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"  {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"  {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"  {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"  {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"  {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("  Quitter");
                        }
                        else if (event_boutique.action_count == 1)
                        {
                            Console.WriteLine($"  {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"> {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"  {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"  {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"  {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"  {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("  Quitter");
                        }
                        else if (event_boutique.action_count == 2)
                        {
                            Console.WriteLine($"  {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"  {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"> {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"  {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"  {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"  {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("  Quitter");
                        }
                        else if (event_boutique.action_count == 3)
                        {
                            Console.WriteLine($"  {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"  {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"  {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"> {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"  {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"  {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("  Quitter");
                        }
                        else if (event_boutique.action_count == 4)
                        {
                            Console.WriteLine($"  {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"  {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"  {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"  {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"> {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"  {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("  Quitter");
                        }
                        else if (event_boutique.action_count == 5)
                        {
                            Console.WriteLine($"  {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"  {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"  {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"  {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"  {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"> {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("  Quitter");
                        }
                        else if (event_boutique.action_count == 6)
                        {
                            Console.WriteLine($"  {pokeBall.Name} {pokeBall.Price} pokemoney");
                            Console.WriteLine($"  {superBall.Name} {superBall.Price} pokemoney");
                            Console.WriteLine($"  {hyperBall.Name} {hyperBall.Price} pokemoney");
                            Console.WriteLine($"  {potion.Name} {potion.Price} pokemoney");
                            Console.WriteLine($"  {superPotion.Name} {superPotion.Price} pokemoney");
                            Console.WriteLine($"  {hyperPotion.Name} {hyperPotion.Price} pokemoney");
                            Console.WriteLine("> Quitter");
                        }

                        choice_boutique = event_boutique.ChoiceEvent(7);

                        if (choice_boutique)
                        {
                            bool quantity = false;
                            if (event_boutique.action_count == 0)
                            {
                                while (!quantity)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\u02C6 ");
                                    Console.WriteLine($"{event_boutique.action_count}");
                                    Console.WriteLine("v");

                                    quantity = event_boutique.QuantityEvent(Player.PokeMoney, pokeBall.Price);
                                }

                                if (quantity)
                                {
                                    Console.WriteLine($"Tu a achete {event_boutique.action_count}x{pokeBall.Name}");
                                    Player.Inventory.AddObject(pokeBall, event_boutique.action_count);
                                }
                                else
                                {
                                    Console.WriteLine("Tu n'a pas assez d'argents");
                                }
                            }
                            else if (event_boutique.action_count == 1)
                            {
                                while (!quantity)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\u02C6 ");
                                    Console.WriteLine($"{event_boutique.action_count}");
                                    Console.WriteLine("v");

                                    quantity = event_boutique.QuantityEvent(Player.PokeMoney, superBall.Price);
                                }
                                if (quantity)
                                {
                                    Console.WriteLine($"Tu a achete {event_boutique.action_count}x{superBall.Name}");
                                    Player.Inventory.AddObject(superBall, event_boutique.action_count);
                                }
                                else
                                {
                                    Console.WriteLine("Tu n'a pas assez d'argents");
                                }
                            }
                            else if (event_boutique.action_count == 2)
                            {
                                while (!quantity)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\u02C6 ");
                                    Console.WriteLine($"{event_boutique.action_count}");
                                    Console.WriteLine("v");

                                    quantity = event_boutique.QuantityEvent(Player.PokeMoney, hyperBall.Price);
                                }
                                if (quantity)
                                {
                                    Console.WriteLine($"Tu a achete {event_boutique.action_count}x{hyperBall.Name}");
                                    Player.Inventory.AddObject(hyperBall, event_boutique.action_count);
                                }
                                else
                                {
                                    Console.WriteLine("Tu n'a pas assez d'argents");
                                }
                            }
                            else if (event_boutique.action_count == 3)
                            {
                                while (!quantity)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\u02C6 ");
                                    Console.WriteLine($"{event_boutique.action_count}");
                                    Console.WriteLine("v");

                                    quantity = event_boutique.QuantityEvent(Player.PokeMoney, potion.Price);
                                }
                                if (quantity)
                                {
                                    Console.WriteLine($"Tu a achete {event_boutique.action_count}x{potion.Name}");
                                    Player.Inventory.AddObject(potion, event_boutique.action_count);
                                }
                                else
                                {
                                    Console.WriteLine("Tu n'a pas assez d'argents");
                                }
                            }
                            else if (event_boutique.action_count == 4)
                            {
                                while (!quantity)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\u02C6 ");
                                    Console.WriteLine($"{event_boutique.action_count}");
                                    Console.WriteLine("v");

                                    quantity = event_boutique.QuantityEvent(Player.PokeMoney, superPotion.Price);
                                }
                                if (quantity)
                                {
                                    Console.WriteLine($"Tu a achete {event_boutique.action_count}x{superPotion.Name}");
                                    Player.Inventory.AddObject(superPotion, event_boutique.action_count);
                                }
                                else
                                {
                                    Console.WriteLine("Tu n'a pas assez d'argents");
                                }
                            }
                            else if (event_boutique.action_count == 5)
                            {
                                while (!quantity)
                                {
                                    Console.Clear();
                                    Console.WriteLine("\u02C6 ");
                                    Console.WriteLine($"{event_boutique.action_count}");
                                    Console.WriteLine("v");

                                    quantity = event_boutique.QuantityEvent(Player.PokeMoney, hyperPotion.Price);
                                }
                                if (quantity)
                                {
                                    Console.WriteLine($"Tu a achete {event_boutique.action_count}x{hyperPotion.Name}");
                                    Player.Inventory.AddObject(hyperPotion, event_boutique.action_count);
                                }
                                else
                                {
                                    Console.WriteLine("Tu n'a pas assez d'argents");
                                }
                            }
                            else if (event_boutique.action_count == 6)
                            {
                                exitInBoutique = true;
                            }
                        }
                    }
                    exitInBoutique = false;
                } 
                else if (event_choice.action_count == 1)
                {
                    Objectvente = Player.Inventory.OpenInventoryDuringBattle();
                    bool vente = false;
                    event_boutique.action_count = 0;
                    if(Objectvente != null)
                    {
                        while (!vente)
                        {
                            Console.Clear();
                            Console.WriteLine("\u02C6 ");
                            Console.WriteLine($"{event_boutique.action_count}");
                            Console.WriteLine("v");

                            vente = event_boutique.QuantityEventSale(Objectvente.Quantity);
                        }
                        if (vente)
                        {
                            int money = ((Objectvente.Price * event_boutique.action_count) / 2);
                            Player.PokeMoney += money;
                            Player.Inventory.RemoveObject(Objectvente, event_boutique.action_count);
                            Console.WriteLine($"Tu a gagne {money} pokemoney");
                        }
                        else
                        {
                            Console.WriteLine("Tu n'a pas autant d'object de ce type");
                        }
                    }
                }
                else if (event_choice.action_count == 2)
                {
                    exitBoutique = true;
                    Console.WriteLine("Merci de votre visite");
                    Console.Write("\nAppuyer pour passer...");
                    Console.ReadKey();
                }
            }
            Console.Clear();
            Console.WriteLine("Bienvenue dans la boutique\n");
            if (event_choice.action_count == 0)
            {
                Console.WriteLine("> Achete des objects");
                Console.WriteLine("  Vendre des objects");
                Console.WriteLine("  Sortir de la boutique");
            }
            else if (event_choice.action_count == 1)
            {
                Console.WriteLine("  Achete des objects");
                Console.WriteLine("> Vendre des objects");
                Console.WriteLine("  Sortir de la boutique");
            }
            else if (event_choice.action_count == 2)
            {
                Console.WriteLine("  Achete des objects");
                Console.WriteLine("  Vendre des objects");
                Console.WriteLine("> Sortir de la boutique");
            }
        }
        exitBoutique = false;
    }

    public void Equipe()
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

