using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class Object
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Effect { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }

    public Object(string name, string description) 
    { 
        Name = name;
        Description = description;
    }
    public Object() { }

    virtual public bool UseObject() { return true; }
    virtual public bool UseObjectDuringBattle(Pokemon pokemon) {  return true; }

    public List<Object> GetListObjects() 
    {
        var list = new List<Object>();
        PokeBall pokeBall = new PokeBall();
        SuperBall superBall = new SuperBall();
        HyperBall hyperBall = new HyperBall();
        Potion potion = new Potion();
        SuperPotion superPotion = new SuperPotion();
        HyperPotion hyperPotion = new HyperPotion();
        list.Add(pokeBall);
        list.Add(potion);
        list.Add(superBall);
        list.Add(superPotion);
        list.Add(hyperBall);
        list.Add(hyperPotion);

        return list;
    }

    public Pokemon ChoosePokemon()
    {
        bool first = false;
        Trainer trainer = Game.Instance.player;
        int nb_event = 0;
        Console.Clear();
        Console.WriteLine("Quelle Pokemon voullez-vous soignez ?");
        Console.WriteLine("  Nom | Niveau | Type1 | Type2 | Pv/PvMax | Attack | Defense | AttackSpecial | DefenseSpecial");
        for (int i = 0; i < trainer.Team.Count; i++)
        {
            if (!first)
            {
                Console.WriteLine($"> {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv}/{trainer.Team[i].PvMax} PV | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                first = true;
                nb_event++;
            }
            else
            {
                Console.WriteLine($"  {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv}/{trainer.Team[i].PvMax} PV | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                nb_event++;
            }
        }

        bool choice = false;
        Event event_choice = new Event();
        while (!choice)
        {
            choice = event_choice.ChoiceEvent(nb_event);

            Console.Clear();
            Console.WriteLine("Quelle Pokemon voullez-vous soignez ?");
            Console.WriteLine("  Nom | Niveau | Type1 | Type2 | Pv/PvMax | Attack | Defense | AttackSpecial | DefenseSpecial");
            for (int i = 0; i < trainer.Team.Count; i++)
            {
                if (event_choice.action_count == i )
                {
                    Console.WriteLine($"> {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv}/{trainer.Team[i].PvMax} PV | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                }
                else
                {
                    Console.WriteLine($"  {trainer.Team[i].Name} | {trainer.Team[i].Level} | {trainer.Team[i].TypeOne} | {trainer.Team[i].TypeTwo} | {trainer.Team[i].Pv}/{trainer.Team[i].PvMax} PV | {trainer.Team[i].Attack} | {trainer.Team[i].Defense} | {trainer.Team[i].AttackSpecial} | {trainer.Team[i].DefenseSpecial}");
                }
            }
        }
        Console.Clear();
        return trainer.Team[event_choice.action_count];
    }

}

public class PokeBall : Object
{
    public PokeBall() 
    {
        Name = "PokeBall";
        Description = "Il permet de capturer le Pokémon sauvage en face de vous. Il possède un Bonus Ball de x1.";
        Effect = 1;
        Price = 300;
    }

    override public bool UseObjectDuringBattle(Pokemon pokemon)
    {
        return true;
    }
}

public class SuperBall : Object
{
    public SuperBall() 
    {
        Name = "SuperBall";
        Description = "La Super Ball est plus performante que la Poké Ball. Cette dernière a un Bonus Ball de x1.5.";
        Effect = 1.5;
        Price = 700;
    }

    override public bool UseObjectDuringBattle(Pokemon pokemon)
    {
        return true;
    }
}

public class HyperBall : Object
{
    public HyperBall()
    {
        Name = "HyperBall";
        Description = "L'Hyper Ball est la plus performante des Balls standards. Cette dernière a un Bonus Ball de x2.";
        Effect = 2;
        Price = 1300;
    }

    override public bool UseObjectDuringBattle(Pokemon pokemon)
    {
        return true;
    }
}

public class Potion : Object
{
    public Potion()
    {
        Name = "Potion"; 
        Description = "Restore 20 HP.";
        Effect = 20;
        Price = 200;
    }

    override public bool UseObject()
    {
        Pokemon pokemon = ChoosePokemon();
        if(pokemon.Pv < pokemon.PvMax)
        {
            int prev_pv = pokemon.Pv;
            pokemon.Heal(Effect);
            Quantity -= 1;
            Console.WriteLine($"Les pv de {pokemon.Name} sont passe de {prev_pv} a {pokemon.Pv} !\n");
            return true;
        }
        else
        {
            return false;
        }
    }

    override public bool UseObjectDuringBattle(Pokemon pokemon)
    {
        if (pokemon.Pv < pokemon.PvMax)
        {
            int prev_pv = pokemon.Pv;
            pokemon.Heal(Effect);
            Quantity -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class SuperPotion : Object
{
    public SuperPotion()
    {
        Name = "SuperPotion";
        Description = "Restore 60 HP.";
        Effect = 60;
        Price = 700;
    }

    override public bool UseObject()
    {
        Pokemon pokemon = ChoosePokemon();
        if (pokemon.Pv < pokemon.PvMax)
        {
            int prev_pv = pokemon.Pv;
            pokemon.Heal(Effect);
            Quantity -= 1;
            Console.WriteLine($"Les pv de {pokemon.Name} sont passe de {prev_pv} a {pokemon.Pv} !\n");
            return true;
        }
        else
        {
            return false;
        }
    }

    override public bool UseObjectDuringBattle(Pokemon pokemon)
    {
        if (pokemon.Pv < pokemon.PvMax)
        {
            int prev_pv = pokemon.Pv;
            pokemon.Heal(Effect);
            Quantity -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class HyperPotion : Object
{
    public HyperPotion()
    {
        Name = "HyperPotion";
        Description = "Restore 120 HP.";
        Effect = 120;
        Price = 1500;
    }

    override public bool UseObject()
    {
        Pokemon pokemon = ChoosePokemon();
        if (pokemon.Pv < pokemon.PvMax)
        {
            int prev_pv = pokemon.Pv;
            pokemon.Heal(Effect);
            Quantity -= 1;
            Console.WriteLine($"Les pv de {pokemon.Name} sont passe de {prev_pv} a {pokemon.Pv} !\n");
            return true;
        }
        else
        {
            return false;
        }
    }

    override public bool UseObjectDuringBattle(Pokemon pokemon)
    {
        if (pokemon.Pv < pokemon.PvMax)
        {
            int prev_pv = pokemon.Pv;
            pokemon.Heal(Effect);
            Quantity -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }
}