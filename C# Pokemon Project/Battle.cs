using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;

public class Battle
{
    private int Mud_Sport { get; set; }
    private int Water_Sport { get; set; }
    private int Nb_Fuite { get; set; }
    private Trainer PlayerBattle { get; set; }
    private Pokemon PokemonBattle { get; set; }
    private Trainer EnemyBattle { get; set; }
    private Pokemon ActivePokemon1 { get; set; }
    private Pokemon ActivePokemon2 { get; set; }
    private string NextAction1 { get; set; }
    private string NextAction2 { get; set; }
    private Dictionary<string, List<int>> XpAndLvlWin = new Dictionary<string, List<int>>();


    public int GetDamage(Pokemon attack_pokemon, Capacity attack_capacity, Pokemon defense_pokemon)
    {
        int level = attack_pokemon.Level;
        int damage = (level * 2 / 5) + 2;
        int power = GetPower(attack_capacity);
        damage *= power;
        int attack = GetAttack(attack_capacity, attack_pokemon);
        damage *= attack;
        float damage_temp = damage / 50;
        damage = (int)Math.Round(damage_temp);
        damage_temp = damage / GetDefense(attack_capacity, defense_pokemon);
        damage = (int)Math.Round(damage_temp);
        damage += 2;
        damage *= GetCritical(attack_capacity);
        damage *= GetRandom();
        damage_temp = damage / 100;
        damage = (int)Math.Round(damage_temp);
        damage_temp = damage * GetStab(attack_capacity, attack_pokemon);
        damage = (int)Math.Round(damage_temp);
        double damage_temp2 = damage * GetType(attack_capacity, defense_pokemon);
        damage = (int)Math.Round(damage_temp2);
        return Math.Max(1, damage);
    }
    public int GetPower(Capacity capacity_attack)
    {
        float MS = 1;
        if (Mud_Sport != 0 && capacity_attack.Type == "Electric")
        {
            MS = 0.5f;
        }
        float WS = 1;
        if (Water_Sport != 0 && capacity_attack.Type == "Fire")
        {
            MS = 0.5f;
        }
        float power = capacity_attack.Power * MS * WS;
        return (int)Math.Round(power);
    }

    public int GetAttack(Capacity capacity_attack, Pokemon pokemon)
    {
        int attack = pokemon.GetStatByFormule(pokemon.Attack);
        if (capacity_attack.Type == "Special")
        {
            attack = pokemon.GetStatByFormule(pokemon.AttackSpecial);
        }
        return attack;
    }

    public int GetDefense(Capacity capacity_attack, Pokemon pokemon)
    {
        int defense = pokemon.GetStatByFormule(pokemon.Defense);
        if (capacity_attack.Type == "Special")
        {
            defense = pokemon.GetStatByFormule(pokemon.DefenseSpecial);
        }
        return defense;
    }

    public int GetCritical(Capacity capacity_attack)
    {
        int critical = 1;
        if (capacity_attack.Critical)
        {
            Random R = new Random();
            if (R.Next(1, 4) == 1)
            {
                critical = 2;
            }
        }
        else
        {
            Random R = new Random();
            if (R.Next(1, 16) == 1)
            {
                critical = 2;
            }
        }
        return critical;
    }

    public int GetRandom()
    {
        Random R = new Random();
        int random = (R.Next(217, 255) * 100) / 255;
        return random;
    }

    public float GetStab(Capacity capacity_attack, Pokemon pokemon)
    {
        float stab = 1;
        if (capacity_attack.Type.Equals(pokemon.TypeOne) || capacity_attack.Type.Equals(pokemon.TypeTwo))
        {
            stab = 1.5f;
        }
        return stab;
    }

    public double GetType(Capacity capacity_attack, Pokemon pokemon)
    {
        int index_type = Game.Instance.type_list.IndexOf(capacity_attack.Type.ToLower());
        return Game.Instance.type_chart[pokemon.TypeOne.ToLower()][pokemon.TypeTwo.ToLower()][index_type];
    }

    public string UseCapacity(Pokemon pokemon_attack, Capacity capacity_attack, Pokemon pokemon_defense)
    {
        Nb_Fuite = 0;
        if (capacity_attack.Category == "Status")
        {
            return "";
        }
        else
        {
            int damage = GetDamage(pokemon_attack, capacity_attack, pokemon_defense);
            pokemon_defense.TakeDamage(damage);
            return  $"{pokemon_attack.Name} utilise {capacity_attack.Name} et inflige {damage} a {pokemon_defense.Name} !";
        }
    }

    public bool GetCanEscape()
    {
        Nb_Fuite++;
        double enemy_speed = ActivePokemon2.Speed / 4;
        int b = (int)Math.Round(enemy_speed);
        int fuite = (((ActivePokemon1.Speed * 32) / b) + 30) * Nb_Fuite;
        if (b == 0 || fuite > 255)
        {
            return true;
        }
        else
        {
            Random R = new Random();
            if (R.Next(0, 255) <= fuite)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public Capacity GetCapacityRandom()
    {
        
        if (EnemyBattle != null || ActivePokemon2.Level > 25)
        {
            List<Capacity> validCapacity = new List<Capacity>();
            double efficacity1 = 0;
            double efficacity2 = 0;
            double efficacity3 = 0;
            if (ActivePokemon2.Capacity1 != null)
            {
                int index_type1 = Game.Instance.type_list.IndexOf(ActivePokemon2.Capacity1.Type.ToLower());
                efficacity1 = Game.Instance.type_chart[ActivePokemon1.TypeOne.ToLower()][ActivePokemon1.TypeTwo.ToLower()][index_type1];
                validCapacity.Add(ActivePokemon2.Capacity1);
            }
            if (ActivePokemon2.Capacity2 != null)
            {
                int index_type2 = Game.Instance.type_list.IndexOf(ActivePokemon2.Capacity2.Type.ToLower());
                efficacity2 = Game.Instance.type_chart[ActivePokemon1.TypeOne.ToLower()][ActivePokemon1.TypeTwo.ToLower()][index_type2];
                if (efficacity2 > efficacity1)
                {
                    validCapacity[0] = ActivePokemon2.Capacity2;
                }
                else if (efficacity2 == efficacity1)
                {
                    validCapacity.Add(ActivePokemon2.Capacity2);
                }
            }
            if (ActivePokemon2.Capacity3 != null)
            {
                int index_type3 = Game.Instance.type_list.IndexOf(ActivePokemon2.Capacity3.Type.ToLower());
                efficacity3 = Game.Instance.type_chart[ActivePokemon1.TypeOne.ToLower()][ActivePokemon1.TypeTwo.ToLower()][index_type3];
                if (validCapacity[0] == ActivePokemon2.Capacity1)
                {
                    if (efficacity3 > efficacity1)
                    {
                        validCapacity.Clear();
                        validCapacity.Add(ActivePokemon2.Capacity3);
                    }
                    else if (efficacity3 == efficacity1)
                    {
                        if (validCapacity.Count == 2)
                        {
                            validCapacity.Add(ActivePokemon2.Capacity3);
                        }
                        else
                        {
                            validCapacity.Add(ActivePokemon2.Capacity3);
                        }
                    }
                }
                else
                {
                    if (efficacity3 > efficacity2)
                    {
                        validCapacity.Clear();
                        validCapacity.Add(ActivePokemon2.Capacity3);
                    }
                    else if (efficacity3 == efficacity2)
                    {
                        validCapacity.Add(ActivePokemon2.Capacity3);
                    }
                }
            }

            Random random = new Random();
            return validCapacity[random.Next(validCapacity.Count)];
        }
        else
        {
            Random random = new Random();
            List<Capacity> validCapacity = new List<Capacity>();
            if (ActivePokemon2.Capacity1 != null)
            {
                validCapacity.Add(ActivePokemon2.Capacity1);
            }
            else if(ActivePokemon2.Capacity2 != null)
            {
                validCapacity.Add(ActivePokemon2.Capacity2);
            }
            else if( ActivePokemon2.Capacity3 != null)
            {
                validCapacity.Add(ActivePokemon2.Capacity3);
            }
            return validCapacity[random.Next(validCapacity.Count)];
        }
    }

    public Object GetRandomObj(Trainer trainer)
    {
        if(ActivePokemon2.PvMax - ActivePokemon2.Pv >= 120 && EnemyBattle.Inventory.IsInInventory("HyperPotion"))
        {
            return EnemyBattle.Inventory.GetObjectByName("HyperPotion");
        }
        else if(ActivePokemon2.PvMax - ActivePokemon2.Pv >= 60 && EnemyBattle.Inventory.IsInInventory("SuperPotion"))
        {
            return EnemyBattle.Inventory.GetObjectByName("SuperPotion");
        }
        else if(EnemyBattle.Inventory.IsInInventory("Potion"))
        {
            return EnemyBattle.Inventory.GetObjectByName("Potion");
        }
        else if (EnemyBattle.Inventory.IsInInventory("SuperPotion"))
        {
            return EnemyBattle.Inventory.GetObjectByName("SuperPotion");
        }
        else
        {
            return EnemyBattle.Inventory.GetObjectByName("HyperPotion");
        }
    }

    public void SetRandomPokemon(Trainer trainer, int activepoke)
    {
        Pokemon pokemon_adverse = null;
        if (activepoke == 1)
        {
            pokemon_adverse = ActivePokemon2;
        }
        else
        {
            pokemon_adverse = ActivePokemon1;
        }
        int moyenneLvlPokemon = 0;
        foreach(Pokemon poke in trainer.Team)
        {
            moyenneLvlPokemon += poke.Level;
        }
        if (moyenneLvlPokemon >= 25)
        {
            Pokemon most_poke = trainer.BattleTeam[0];
            foreach (Pokemon poke in trainer.BattleTeam)
            {
                if(Game.Instance.type_chart[pokemon_adverse.TypeOne.ToLower()][pokemon_adverse.TypeTwo.ToLower()][Game.Instance.type_list.IndexOf(poke.TypeOne.ToLower())] > Game.Instance.type_chart[pokemon_adverse.TypeOne.ToLower()][pokemon_adverse.TypeTwo.ToLower()][Game.Instance.type_list.IndexOf(most_poke.TypeOne.ToLower())])
                {
                    most_poke = poke;
                }
            }
            if (activepoke == 1)
            {
                ActivePokemon1 = most_poke;
            }
            else
            {
                ActivePokemon2 = most_poke;
            }
        }
        else
        {
            Random rand = new Random();
            if (activepoke == 1)
            {
                ActivePokemon1 = trainer.BattleTeam[rand.Next(trainer.BattleTeam.Count)];
            }
            else
            {
                ActivePokemon2 = trainer.BattleTeam[rand.Next(trainer.BattleTeam.Count)];
            }
        }
    }

    public string GetRandomAction(Trainer trainer, Pokemon pokemon, Pokemon pokemon_adverse)
    {
        Random rand = new Random();
        int action_random = rand.Next(10);
        if(Game.Instance.type_chart[pokemon.TypeOne.ToLower()][pokemon.TypeTwo.ToLower()][Game.Instance.type_list.IndexOf(pokemon_adverse.TypeOne.ToLower())] < 1 && trainer.BattleTeam.Count > 1)
        {
            foreach(var poke in trainer.BattleTeam)
            {
                if(Game.Instance.type_chart[poke.TypeOne.ToLower()][poke.TypeTwo.ToLower()][Game.Instance.type_list.IndexOf(pokemon_adverse.TypeOne.ToLower())] > 1)
                {
                    return "change_pokemon";
                }
            }
            if (trainer.BattleTeam.Count > 1)
            {
                if (trainer.Inventory.GetInventory().Count > 0)
                {
                    if (action_random >= 0 && action_random <= 2)
                    {
                        return "attack";
                    }
                    else if (action_random >= 3 && action_random <= 7)
                    {
                        return "change_pokemon";
                    }
                    else
                    {
                        return "use_object";
                    }
                }
                else
                {
                    if (action_random >= 0 && action_random <= 2)
                    {
                        return "attack";
                    }
                    else
                    {
                        return "change_pokemon";
                    }
                }
            }
            else
            {
                if (trainer.Inventory.GetInventory().Count > 0)
                {
                    if (action_random >= 0 && action_random <= 4)
                    {
                        return "attack";
                    }
                    else
                    {
                        return "use_object";
                    }
                }
                else
                {
                    return "attack";
                }
            }
        }
        else if (pokemon.Pv <= pokemon.PvMax / 4)
        {
            if(trainer.BattleTeam.Count > 1)
            {
                if(trainer.Inventory.GetInventory().Count > 0)
                {
                    if(action_random >= 0 && action_random <= 2)
                    {
                        return "attack";
                    }
                    else if(action_random >= 3 && action_random <= 6)
                    {
                        return "change_pokemon";
                    }
                    else
                    {
                        return "use_object";
                    }
                }
                else
                {
                    if (action_random >= 0 && action_random <= 3)
                    {
                        return "attack";
                    }
                    else
                    {
                        return "change_pokemon";
                    }
                }
            }
            else
            {
                if (trainer.Inventory.GetInventory().Count > 0)
                {
                    if (action_random >= 0 && action_random <= 3)
                    {
                        return "attack";
                    }
                    else
                    {
                        return "use_object";
                    }
                }
                else
                {
                    return "attack";
                }
            }
        }
        else
        {
            if (trainer.BattleTeam.Count > 1)
            {
                if (trainer.Inventory.GetInventory().Count > 0)
                {
                    if (action_random >= 0 && action_random <= 5)
                    {
                        return "attack";
                    }
                    else if (action_random >= 6 && action_random <= 8)
                    {
                        return "change_pokemon";
                    }
                    else
                    {
                        return "use_object";
                    }
                }
                else
                {
                    if (action_random >= 0 && action_random <= 7)
                    {
                        return "attack";
                    }
                    else
                    {
                        return "change_pokemon";
                    }
                }
            }
            else
            {
                if (trainer.Inventory.GetInventory().Count > 0)
                {
                    if (action_random >= 0 && action_random <= 8)
                    {
                        return "attack";
                    }
                    else
                    {
                        return "use_object";
                    }
                }
                else
                {
                    return "attack";
                }
            }
        }
    }

    public bool Capture(double ball, Pokemon pokemonCapture)
    {
        Random rnd = new Random();
        int Formule = (int)((1 - (2 / 3) * (pokemonCapture.Pv / pokemonCapture.Pv)) * rnd.Next(3,256) * ball);
        if (Formule >= 255)
        {
            return  true;
        }
        else { return false; }
    }

    public void GiveXpToUsingPokemons(bool capture)
    {
        foreach(var pokemon in PlayerBattle.Team) 
        {
            if (pokemon.IsUsing)
            {
                pokemon.GiveXp(ActivePokemon2, capture);
            }
        }
    }

    public void GiveXpToUsingPokemonDuringBattleTrainer()
    {
        foreach (var pokemon in PlayerBattle.Team)
        {
            if (pokemon.IsUsing)
            {
                pokemon.GiveXp(ActivePokemon2, false);
                pokemon.IsUsing = false;
            }
        }
        CheckLearnCapacityForAllPokemon();
    }

    public void CheckLearnCapacityForAllPokemon()
    {
        foreach (var pokemon in PlayerBattle.Team)
        {
            pokemon.CanLearnNewCapacity();
        }
    }

    public void AffichageVs()
    {
        string versus = "";
        for(int i = 0; i < 20-PlayerBattle.Name.Length; i++) 
        {
            versus += ' ';
        }
        versus += "***";
        Console.Write(versus + " ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{PlayerBattle.Name}");
        Console.ResetColor();
        Console.Write(" VS ");
        Console.ForegroundColor = ConsoleColor.Red;
        if (PokemonBattle == null)
        {
            Console.Write($"{EnemyBattle.Name}");
        } 
        else
        {
            Console.Write($"{PokemonBattle.Name}");
        }
        Console.ResetColor();
        Console.WriteLine(" ***");
        versus = "   ";
        for (int i = 0; i < 20 - PlayerBattle.Name.Length; i++)
        {
            versus += ' ';
        }
        Console.Write(versus);
        string nb_pokemon = "";
        foreach (var pokemon in PlayerBattle.BattleTeam)
        {
            nb_pokemon += "°";
        }
        Console.Write(nb_pokemon);
        if (PokemonBattle == null)
        {
            string space = "";
            for(int i = 0; i <  PlayerBattle.Name.Length - PlayerBattle.BattleTeam.Count + 4; i++) 
            { 
                space += " ";
            }
            Console.Write(space);
            nb_pokemon = "";
            foreach (var pokemon in EnemyBattle.BattleTeam)
            {
                nb_pokemon += "°";
            }
            Console.Write(nb_pokemon);
        }
        Console.WriteLine();
    }

    public void AffichageHUD()
    {
        string pokemon_name = "     ";
        int size_pokemon_name = ActivePokemon1.Name.Length;
        int size_pokemon_name_space = (15 - ActivePokemon1.Name.Length) /2;
        for (int i = 0; i < size_pokemon_name_space; i++)
        {
            pokemon_name += " ";
            size_pokemon_name++;
        }
        Console.Write(pokemon_name);
        Console.Write(ActivePokemon1.Name);
        pokemon_name = "";
        for (int i = 0; i < 15 - size_pokemon_name; i++)
        {
            pokemon_name += " ";
        }
        Console.Write(pokemon_name);
        pokemon_name = "";
        int size_space = 6 + PlayerBattle.Name.Length;
        if(PokemonBattle == null)
        {
            size_space += EnemyBattle.Name.Length;
        }
        else
        {
            size_space += PokemonBattle.Name.Length;
        }
        for (int i = 0; i < size_space; i++)
        {
            pokemon_name += " ";
        }
        Console.Write(pokemon_name);
        pokemon_name = "";
        size_pokemon_name_space = (15 - ActivePokemon2.Name.Length) / 2;
        for (int i = 0; i < size_pokemon_name_space; i++)
        {
            pokemon_name += " ";
        }
        Console.Write(pokemon_name);
        Console.WriteLine(ActivePokemon2.Name);
        string lifebar = "     ";
        double nb_pv = (ActivePokemon1.Pv * 15 / ActivePokemon1.PvMax);
        for( int i = 0; i < nb_pv; i++ )
        {
            lifebar += "\u2588";
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(lifebar);
        lifebar = "";
        float nb_pv_moins = ((ActivePokemon1.PvMax - ActivePokemon1.Pv) * 15) / ActivePokemon1.PvMax;
        for (int i = 0; i < nb_pv_moins; i++)
        {
            lifebar += "\u2588";
        }
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write(lifebar);
        pokemon_name = "";
        for (int i = 0;i < size_space; i++)
        {
            pokemon_name += " ";
        }
        Console.Write(pokemon_name);
        lifebar = "";
        nb_pv = (ActivePokemon2.Pv * 15 / ActivePokemon2.PvMax) ;
        for (int i = 0; i < nb_pv; i++)
        {
            lifebar += "\u2588";
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(lifebar);
        lifebar = "";
        
        nb_pv_moins = ((ActivePokemon2.PvMax - ActivePokemon2.Pv) * 15 ) / ActivePokemon2.PvMax;
        for (int i = 0; i < nb_pv_moins; i++)
        {
            lifebar += "\u2588";
        }
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(lifebar);
        Console.ResetColor();
        pokemon_name = "     ";
        Console.Write(pokemon_name);
        string space = "";
        int size_pv = (15 - (6 + ActivePokemon1.Pv.ToString().Length + ActivePokemon1.PvMax.ToString().Length)) / 2;
        for (int i = 0; i < size_pv; i++)
        {
            space += " ";
        }
        Console.Write(space);
        space = "";
        Console.Write($"{ActivePokemon1.Pv} / {ActivePokemon1.PvMax} PV");
        size_pv = (15 - (6 + ActivePokemon1.Pv.ToString().Length + ActivePokemon1.PvMax.ToString().Length)) - size_pv + size_space;
        for (int i = 0; i < size_pv; i++)
        {
            space += " ";
        }
        Console.Write(space);
        space = "";
        size_pv = (15 - (6 + ActivePokemon2.Pv.ToString().Length + ActivePokemon2.PvMax.ToString().Length)) / 2;
        for (int i = 0; i < size_pv; i++)
        {
            space += " ";
        }
        Console.Write(space);
        Console.WriteLine($"{ActivePokemon2.Pv} / {ActivePokemon2.PvMax} PV");
        Console.WriteLine();
    }

    public Pokemon ChooseActivePokemon(Trainer trainer)
    {
        bool first = false;
        int nb_event = 0;
        Console.Clear();
        AffichageVs();
        Console.WriteLine("Choisissez votre Pokemon actif :");
        Console.WriteLine("  Nom | Niveau | Type1 | Type2 | Pv/PvMax | Attack | Defense | AttackSpecial | DefenseSpecial");
        for (int i = 0; i < trainer.BattleTeam.Count; i++)
        {
            if (!first && trainer.BattleTeam[i].IsAlive())
            {
                Console.WriteLine($"> {trainer.BattleTeam[i].Name} | {trainer.BattleTeam[i].Level} | {trainer.BattleTeam[i].TypeOne} | {trainer.BattleTeam[i].TypeTwo} | {trainer.BattleTeam[i].Pv}/{trainer.BattleTeam[i].PvMax} PV | {trainer.BattleTeam[i].Attack} | {trainer.BattleTeam[i].Defense} | {trainer.BattleTeam[i].AttackSpecial} | {trainer.BattleTeam[i].DefenseSpecial}");
                first = true;
                nb_event++;
            }
            else if (trainer.BattleTeam[i].IsAlive())
            {
                Console.WriteLine($"  {trainer.BattleTeam[i].Name} | {trainer.BattleTeam[i].Level} | {trainer.BattleTeam[i].TypeOne} | {trainer.BattleTeam[i].TypeTwo} | {trainer.BattleTeam[i].Pv}/{trainer.BattleTeam[i].PvMax} PV | {trainer.BattleTeam[i].Attack} | {trainer.BattleTeam[i].Defense} | {trainer.BattleTeam[i].AttackSpecial} | {trainer.BattleTeam[i].DefenseSpecial}");
                nb_event++;
            }
        }

        bool choice = false;
        Event event_choice = new Event();
        while (!choice)
        {
            choice = event_choice.ChoiceEvent(nb_event);

            Console.Clear();
            AffichageVs();
            Console.WriteLine("Choisissez votre Pokemon actif :");
            Console.WriteLine("  Nom | Niveau | Type1 | Type2 | Pv/PvMax | Attack | Defense | AttackSpecial | DefenseSpecial");
            for (int i = 0; i < trainer.BattleTeam.Count; i++)
            {
                if (event_choice.action_count == i && trainer.BattleTeam[i].IsAlive())
                {
                    Console.WriteLine($"> {trainer.BattleTeam[i].Name} | {trainer.BattleTeam[i].Level} | {trainer.BattleTeam[i].TypeOne} | {trainer.BattleTeam[i].TypeTwo} | {trainer.BattleTeam[i].Pv}/{trainer.BattleTeam[i].PvMax} PV | {trainer.BattleTeam[i].Attack} | {trainer.BattleTeam[i].Defense} | {trainer.BattleTeam[i].AttackSpecial} | {trainer.BattleTeam[i].DefenseSpecial}");
                }
                else if (trainer.BattleTeam[i].IsAlive())
                {
                    Console.WriteLine($"  {trainer.BattleTeam[i].Name} | {trainer.BattleTeam[i].Level} | {trainer.BattleTeam[i].TypeOne} | {trainer.BattleTeam[i].TypeTwo} | {trainer.BattleTeam[i].Pv}/{trainer.BattleTeam[i].PvMax} PV | {trainer.BattleTeam[i].Attack} | {trainer.BattleTeam[i].Defense} | {trainer.BattleTeam[i].AttackSpecial} | {trainer.BattleTeam[i].DefenseSpecial}");
                }
            }
        }
        ActivePokemon1 = trainer.BattleTeam[event_choice.action_count];
        return trainer.BattleTeam[event_choice.action_count];
    }

    public void StartBattleVsPokemon(Trainer player, Pokemon pokemon)
    { 
        Mud_Sport = 0;
        Water_Sport = 0;
        player.BattleTeam.Clear();
        foreach(var pokemon_battle in player.Team)
        {
            if (pokemon_battle.IsAlive())
            {
                player.BattleTeam.Add(pokemon_battle);
            }
            pokemon_battle.IsUsing = false;
        }
        Event event_choice = new Event();
        bool fuite = false;
        bool capture = false;
        PlayerBattle = player;
        PokemonBattle = pokemon;
        ActivePokemon2 = pokemon;
        Pokemon activePokemon1 = ChooseActivePokemon(player);
        event_choice.action_count = 0;
        while (player.BattleTeam.Count > 0 && pokemon.IsAlive() && fuite != true && !capture)
        {
            Console.Clear();
            AffichageVs();
            AffichageHUD();
            if (NextAction1 != null)
            {
                Console.WriteLine($"{NextAction1}\n");
            }
            if (NextAction2 != null)
            {
                Console.WriteLine($"{NextAction2}\n");
            }
            Console.WriteLine("Choisissez votre action :");
            if (event_choice.action_count == 0)
            {
                Console.WriteLine("> Attaquer");
                Console.WriteLine("  Changer de Pokemon");
                Console.WriteLine("  Utiliser un objet du sac");
                Console.WriteLine("  Fuir");
            }
            else if (event_choice.action_count == 1)
            {
                Console.WriteLine("  Attaquer");
                Console.WriteLine("> Changer de Pokemon");
                Console.WriteLine("  Utiliser un objet du sac");
                Console.WriteLine("  Fuir");
            }
            else if (event_choice.action_count == 2)
            {
                Console.WriteLine("  Attaquer");
                Console.WriteLine("  Changer de Pokemon");
                Console.WriteLine("> Utiliser un objet du sac");
                Console.WriteLine("  Fuir");
            }
            else if (event_choice.action_count == 3)
            {
                Console.WriteLine("  Attaquer");
                Console.WriteLine("  Changer de Pokemon");
                Console.WriteLine("  Utiliser un objet du sac");
                Console.WriteLine("> Fuir");
            }
            bool choice_event = event_choice.ChoiceEvent(4);
            if (choice_event)
            {
                if (event_choice.action_count == 0)
                {
                    BattleRound(activePokemon1, pokemon);
                    if(!activePokemon1.IsAlive() && player.BattleTeam.Count > 0)
                    {
                        activePokemon1 = ChooseActivePokemon(player);
                    }
                }
                else if (event_choice.action_count == 1)
                {
                    activePokemon1 = ChooseActivePokemon(player);
                }
                else if (event_choice.action_count == 2)
                {
                    Object using_obj = player.Inventory.OpenInventoryDuringBattle();
                    if(using_obj != null)
                    {
                        if(using_obj.Name == "PokeBall" ||  using_obj.Name == "SuperBall" || using_obj.Name == "HyperBall")
                        {
                            using_obj.Quantity -= 1;
                            capture = Capture(using_obj.Effect, ActivePokemon2);
                            if (!capture)
                            {
                                
                                NextAction1 = $"Le pokemon {ActivePokemon2.Name} s'est echappe de la {using_obj.Name} !";
                            }
                        }
                        else if(using_obj.Name == "Potion" || using_obj.Name == "SuperPotion" || using_obj.Name == "HyperPotion")
                        {
                            int prev_pv = ActivePokemon1.Pv;
                            using_obj.UseObjectDuringBattle(ActivePokemon1);
                            NextAction1 = $"Vous avez utilise une {using_obj.Name} sur {ActivePokemon1.Name}, PV passe de {prev_pv} a {ActivePokemon1.Pv} !";
                        }
                        if (!capture)
                        {
                            Capacity capacity_random = GetCapacityRandom();
                            NextAction2 = UseCapacity(ActivePokemon2, capacity_random, ActivePokemon1);
                            if (!ActivePokemon1.IsAlive())
                            {
                                PlayerBattle.BattleTeam.Remove(ActivePokemon1);
                            }
                        }
                    }
                }
                else if (event_choice.action_count == 3)
                {
                    fuite = GetCanEscape();
                }
                ActivePokemon1.IsUsing = true;
            }
        }
        Console.Clear();
        if (fuite)
        {
            Console.WriteLine("Vous vous etes enfuis !\n");
            Console.Write("Appuyer sur une touche pour passer...");
            Console.ReadKey();
        }
        else if (capture)
        {
            Console.Write($"{ActivePokemon2.Name} a ete capture et a ete ajoute a votre ");
            if(player.Team.Count < 6)
            {
                player.Team.Add(ActivePokemon2);
                Console.WriteLine("equipe !\n");
            }
            else
            {
                player.Pokedex.Add(ActivePokemon2);
                Console.WriteLine("liste de pokemon !\n");
            }
            GiveXpToUsingPokemons(true);
            Console.Write("\nAppuyer sur une touche pour passer...");
            Console.ReadKey();
            Console.Clear();
            CheckLearnCapacityForAllPokemon();
            player.PokeMoney += 100;
        }
        else if(!pokemon.IsAlive())
        {
            Console.WriteLine($"Vous avez vaicu {ActivePokemon2.Name} !\n");
            GiveXpToUsingPokemons(false);
            Console.Write("\nAppuyer sur une touche pour passer...");
            Console.ReadKey();
            Console.Clear();
            CheckLearnCapacityForAllPokemon();
            player.PokeMoney += 75;
        }
        else
        {
            Console.WriteLine($"Vous avez ete vaicu par {ActivePokemon2.Name} !\n");
            Console.Write("\nAppuyer sur une touche pour passer...");
            Console.ReadKey();
        }
        Console.Clear();
    }

    public void BattleRound(Pokemon activePokemon1, Pokemon activePokemon2)
    {
        Event event_choice = new Event();
        int nb_event = 0;
        Console.Clear();
        AffichageVs();
        AffichageHUD();
        Console.WriteLine("Choisissez votre attaque :");
        if (activePokemon1.Capacity1 != null)
        {
            Console.WriteLine($"> {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
            nb_event++;
        }
        if (activePokemon1.Capacity2 != null)
        {
            Console.WriteLine($"  {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
            nb_event++;
        }
        if (activePokemon1.Capacity3 != null)
        {
            Console.WriteLine($"  {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
            nb_event++;
        }
        bool choice_event = false;
        while (!choice_event)
        {
            choice_event = event_choice.ChoiceEvent(nb_event);

            Console.Clear();
            AffichageVs();
            AffichageHUD();
            Console.WriteLine("Choisissez votre attaque :");
            if (event_choice.action_count == 0)
            {
                if (activePokemon1.Capacity1 != null)
                {
                    Console.WriteLine($"> {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
                }
                if (activePokemon1.Capacity2 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity2.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
                }
                if (activePokemon1.Capacity3 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
                }
            }
            else if (event_choice.action_count == 1)
            {
                if (activePokemon1.Capacity1 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
                }
                if (activePokemon1.Capacity2 != null)
                {
                    Console.WriteLine($"> {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity2.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
                }
                if (activePokemon1.Capacity3 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
                }
            }
            else if (event_choice.action_count == 2)
            {
                if (activePokemon1.Capacity1 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
                }
                if (activePokemon1.Capacity2 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity2.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
                }
                if (activePokemon1.Capacity3 != null)
                {
                    Console.WriteLine($"> {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
                }
            }
        }

        Capacity capacity_random = GetCapacityRandom();

        Random random_first = new Random();
        int first = random_first.Next(2);

        if (ActivePokemon1.Speed > ActivePokemon2.Speed || first == 0)
        {
            if (event_choice.action_count == 0)
            {
                NextAction1 = UseCapacity(activePokemon1, activePokemon1.Capacity1, activePokemon2);
            }
            else if (event_choice.action_count == 1)
            {
                NextAction1 = UseCapacity(activePokemon1, activePokemon1.Capacity2, activePokemon2);
            }
            else if (event_choice.action_count == 2)
            {
                NextAction1 = UseCapacity(activePokemon1, activePokemon1.Capacity3, activePokemon2);
            }
            if (activePokemon2.IsAlive())
            {
                NextAction2 = UseCapacity(activePokemon2, capacity_random, activePokemon1);
                if (!activePokemon1.IsAlive())
                {
                    PlayerBattle.BattleTeam.Remove(activePokemon1);
                }
            }
        }
        else if (ActivePokemon2.Speed > ActivePokemon1.Speed || first == 1)
        {
            NextAction1 = UseCapacity(activePokemon2, capacity_random, activePokemon1);
            if (activePokemon1.IsAlive())
            {
                if (event_choice.action_count == 0)
                {
                    NextAction2 = UseCapacity(activePokemon1, activePokemon1.Capacity1, activePokemon2);
                }
                else if (event_choice.action_count == 1)
                {
                    NextAction2 = UseCapacity(activePokemon1, activePokemon1.Capacity2, activePokemon2);
                }
                else if (event_choice.action_count == 2)
                {
                    NextAction2 = UseCapacity(activePokemon1, activePokemon1.Capacity3, activePokemon2);
                }
            }
            else
            {
                PlayerBattle.BattleTeam.Remove(activePokemon1);
            }
        }
    }

    public bool StartBattleVsTrainer(Trainer player, Trainer enemy)
    {
        Mud_Sport = 0;
        Water_Sport = 0;
        player.BattleTeam.Clear();
        foreach (var pokemon_battle in player.Team)
        {
            if (pokemon_battle.IsAlive())
            {
                player.BattleTeam.Add(pokemon_battle);
                XpAndLvlWin.Add(pokemon_battle.Name, new List<int> { pokemon_battle.Xp, pokemon_battle.Level});
            }
            pokemon_battle.IsUsing = false;
        }
        foreach (var pokemon_battle in enemy.Team)
        {
            if (pokemon_battle.IsAlive())
            {
                enemy.BattleTeam.Add(pokemon_battle);
            }
            pokemon_battle.IsUsing = false;
        }
        Event event_choice = new Event();
        PlayerBattle = player;
        EnemyBattle = enemy;
        ActivePokemon2 = enemy.BattleTeam[0];
        ChooseActivePokemon(player);
        event_choice.action_count = 0;
        while (player.BattleTeam.Count > 0 && enemy.BattleTeam.Count > 0)
        {
            Console.Clear();
            AffichageVs();
            AffichageHUD();
            if (NextAction1 != null)
            {
                Console.WriteLine($"{NextAction1}\n");
            }
            if (NextAction2 != null)
            {
                Console.WriteLine($"{NextAction2}\n");
            }
            Console.WriteLine("Choisissez votre action :");
            if (event_choice.action_count == 0)
            {
                Console.WriteLine("> Attaquer");
                Console.WriteLine("  Changer de Pokemon");
                Console.WriteLine("  Utiliser un objet du sac");
            }
            else if (event_choice.action_count == 1)
            {
                Console.WriteLine("  Attaquer");
                Console.WriteLine("> Changer de Pokemon");
                Console.WriteLine("  Utiliser un objet du sac");
            }
            else if (event_choice.action_count == 2)
            {
                Console.WriteLine("  Attaquer");
                Console.WriteLine("  Changer de Pokemon");
                Console.WriteLine("> Utiliser un objet du sac");
            }
            bool choice_event = event_choice.ChoiceEvent(3);
            if (choice_event)
            {
                string enemy_action = GetRandomAction(EnemyBattle, ActivePokemon2, ActivePokemon1);
                if (event_choice.action_count == 0)
                {
                    BattleRoundDuringBattleTrainer("attack", enemy_action);
                }
                else if (event_choice.action_count == 1)
                {
                    BattleRoundDuringBattleTrainer("change_pokemon", enemy_action);
                }
                else if (event_choice.action_count == 2)
                {
                    BattleRoundDuringBattleTrainer("use_object", enemy_action);
                }
                ActivePokemon1.IsUsing = true;
            }
        }
        Console.Clear();
        if (EnemyBattle.BattleTeam.Count == 0)
        {
            Console.WriteLine($"Vous avez vaicu {EnemyBattle.Name} !\n");
            foreach(var items in XpAndLvlWin)
            {
                if (items.Value[1] != player.FindByName(items.Key).Level)
                {
                    int nb_xp_win = (int)Math.Pow((items.Value[1] + 1), 3) - items.Value[0] + player.FindByName(items.Key).Xp;
                    for (int i = items.Value[1]; i < player.FindByName(items.Key).Level; i++)
                    {
                        nb_xp_win += (int)Math.Pow((i + 1), 3);
                    }
                    Console.Write($"{items.Key} a gagne {nb_xp_win} XP et est passer du Level {items.Value[1]} au Level {player.FindByName(items.Key).Level} !");
                }
                else
                {
                    Console.Write($"{items.Key} a gagne {player.FindByName(items.Key).Xp - items.Value[0]} XP !");
                }
            }

            Console.Write("\nAppuyer sur une touche pour passer...");
            Console.ReadKey();
            Console.Clear();
            player.PokeMoney += 500;
            return true;
        }
        else
        {
            Console.WriteLine($"Vous avez ete vaicu par {EnemyBattle.Name} !\n");
            Console.Write("\nAppuyer sur une touche pour passer...");
            Console.ReadKey();
            Console.Clear();
            player.PokeMoney += 150;
            return false;
        }
    }

    public void BattleRoundDuringBattleTrainer(string action_player, string action_enemy)
    {
        if (action_player == "use_object" && action_enemy == "use_object")
        {
            Object using_obj = PlayerBattle.Inventory.OpenInventoryDuringBattle();
            if (using_obj != null && using_obj.Name != "PokeBall" && using_obj.Name != "SuperBall" && using_obj.Name != "HyperBall")
            {
                int prev_pv = ActivePokemon1.Pv;
                using_obj.UseObjectDuringBattle(ActivePokemon1);
                NextAction1 = $"Vous avez utilise une {using_obj.Name} sur {ActivePokemon1.Name}, PV passe de {prev_pv} a {ActivePokemon1.Pv} !";
                prev_pv = ActivePokemon2.Pv;
                Object enemyObj = GetRandomObj(EnemyBattle);
                enemyObj.UseObjectDuringBattle(ActivePokemon2);
                NextAction2 = $"{EnemyBattle.Name} a utilise une {enemyObj.Name} sur {ActivePokemon2.Name}, PV passe de {prev_pv} a {ActivePokemon2.Pv} !";
            }
        }
        else if (action_player == "use_object" && action_enemy == "change_pokemon")
        {
            Object using_obj = PlayerBattle.Inventory.OpenInventoryDuringBattle();
            if (using_obj != null && using_obj.Name != "PokeBall" && using_obj.Name != "SuperBall" && using_obj.Name != "HyperBall")
            {
                int prev_pv = ActivePokemon1.Pv;
                using_obj.UseObjectDuringBattle(ActivePokemon1);
                NextAction1 = $"Vous avez utilise une {using_obj.Name} sur {ActivePokemon1.Name}, PV passe de {prev_pv} a {ActivePokemon1.Pv} !";
                Pokemon prev_poke = ActivePokemon2;
                SetRandomPokemon(EnemyBattle, 2);
                NextAction2 = $"{EnemyBattle.Name} a change de Pokemon : {prev_poke.Name} -> {ActivePokemon2.Name} !";
            }
        }
        else if (action_player == "change_pokemon" && action_enemy == "use_object")
        {
            int prev_pv = ActivePokemon2.Pv;
            Object enemyObj = GetRandomObj(EnemyBattle);
            enemyObj.UseObjectDuringBattle(ActivePokemon2);
            NextAction1 = $"{EnemyBattle.Name} a utilise une {enemyObj.Name} sur {ActivePokemon2.Name}, PV passe de {prev_pv} a {ActivePokemon2.Pv} !";
            Pokemon prev_poke = ActivePokemon1;  
            ChooseActivePokemon(PlayerBattle);
            NextAction2 = $"Vous avez change de Pokemon : {prev_poke.Name} -> {ActivePokemon1.Name} !";
        }
        else if (action_player == "use_object" && action_enemy == "attack")
        {
            Object using_obj = PlayerBattle.Inventory.OpenInventoryDuringBattle();
            if (using_obj != null && using_obj.Name != "PokeBall" && using_obj.Name != "SuperBall" && using_obj.Name != "HyperBall")
            {
                int prev_pv = ActivePokemon1.Pv;
                using_obj.UseObjectDuringBattle(ActivePokemon1);
                NextAction1 = $"Vous avez utilise une {using_obj.Name} sur {ActivePokemon1.Name}, PV passe de {prev_pv} a {ActivePokemon1.Pv} !";
                Capacity capacity_random = GetCapacityRandom();
                NextAction2 = UseCapacity(ActivePokemon2, capacity_random, ActivePokemon1);
            }
        }
        else if (action_player == "attack" && action_enemy == "use_object")
        {
            int prev_pv = ActivePokemon2.Pv;
            Object enemyObj = GetRandomObj(EnemyBattle);
            enemyObj.UseObjectDuringBattle(ActivePokemon2);
            NextAction1 = $"{EnemyBattle.Name} a utilise une {enemyObj.Name} sur {ActivePokemon2.Name}, PV passe de {prev_pv} a {ActivePokemon2.Pv} !";
            PlayerAttack(ActivePokemon1, ActivePokemon2, action_enemy);
        }
        else if (action_player == "change_pokemon" && action_enemy == "attack")
        {
            Random random_first = new Random();
            int first = random_first.Next(2);

            if (ActivePokemon1.Speed > ActivePokemon2.Speed || first == 0)
            {
                Pokemon prev_poke = ActivePokemon1;
                ChooseActivePokemon(PlayerBattle);
                NextAction1 = $"Vous avez change de Pokemon : {prev_poke.Name} -> {ActivePokemon1.Name} !";
                Capacity capacity_random = GetCapacityRandom();
                NextAction2 = UseCapacity(ActivePokemon2, capacity_random, ActivePokemon1);
            }
            else if (ActivePokemon2.Speed > ActivePokemon1.Speed || first == 1)
            {
                Capacity capacity_random = GetCapacityRandom();
                NextAction1 = UseCapacity(ActivePokemon2, capacity_random, ActivePokemon1);
                Pokemon prev_poke = ActivePokemon1;
                ChooseActivePokemon(PlayerBattle);
                NextAction2 = $"Vous avez change de Pokemon : {prev_poke.Name} -> {ActivePokemon1.Name} !";
            }
        }
        else if (action_player == "attack" && action_enemy == "change_pokemon")
        {
            PlayerAttack(ActivePokemon1, ActivePokemon2, action_enemy);
        }
        else if (action_player == "attack" && action_enemy == "attack")
        {
            PlayerAttack(ActivePokemon1, ActivePokemon2, action_enemy);
        }
        else if (action_player == "change_pokemon" && action_enemy == "change_pokemon")
        {
            Pokemon prev_poke = ActivePokemon1;
            ChooseActivePokemon(PlayerBattle);
            NextAction1 = $"Vous avez change de Pokemon : {prev_poke.Name} -> {ActivePokemon1.Name} !";
            prev_poke = ActivePokemon2;
            SetRandomPokemon(EnemyBattle, 2);
            NextAction2 = $"{EnemyBattle.Name} a change de Pokemon : {prev_poke.Name} -> {ActivePokemon2.Name} !";
        }
    }

    public void PlayerAttack(Pokemon activePokemon1, Pokemon activePokemon2, string enemy_choice)
    {
        Event event_choice = new Event();
        int nb_event = 0;
        Console.Clear();
        AffichageVs();
        AffichageHUD();
        Console.WriteLine("Choisissez votre attaque :");
        if (activePokemon1.Capacity1 != null)
        {
            Console.WriteLine($"> {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
            nb_event++;
        }
        if (activePokemon1.Capacity2 != null)
        {
            Console.WriteLine($"  {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
            nb_event++;
        }
        if (activePokemon1.Capacity3 != null)
        {
            Console.WriteLine($"  {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
            nb_event++;
        }
        bool choice_event = false;
        while (!choice_event)
        {
            choice_event = event_choice.ChoiceEvent(nb_event);

            Console.Clear();
            AffichageVs();
            AffichageHUD();
            Console.WriteLine("Choisissez votre attaque :");
            if (event_choice.action_count == 0)
            {
                if (activePokemon1.Capacity1 != null)
                {
                    Console.WriteLine($"> {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
                }
                if (activePokemon1.Capacity2 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity2.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
                }
                if (activePokemon1.Capacity3 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
                }
            }
            else if (event_choice.action_count == 1)
            {
                if (activePokemon1.Capacity1 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
                }
                if (activePokemon1.Capacity2 != null)
                {
                    Console.WriteLine($"> {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity2.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
                }
                if (activePokemon1.Capacity3 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
                }
            }
            else if (event_choice.action_count == 2)
            {
                if (activePokemon1.Capacity1 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity1.Name} | {activePokemon1.Capacity1.Type} | {activePokemon1.Capacity1.Category} | {activePokemon1.Capacity1.Power} | {activePokemon1.Capacity1.Accuracy}");
                }
                if (activePokemon1.Capacity2 != null)
                {
                    Console.WriteLine($"  {activePokemon1.Capacity2.Name} | {activePokemon1.Capacity2.Type} | {activePokemon1.Capacity2.Category} | {activePokemon1.Capacity2.Power} | {activePokemon1.Capacity2.Accuracy}");
                }
                if (activePokemon1.Capacity3 != null)
                {
                    Console.WriteLine($"> {activePokemon1.Capacity3.Name} | {activePokemon1.Capacity3.Type} | {activePokemon1.Capacity3.Category} | {activePokemon1.Capacity3.Power} | {activePokemon1.Capacity3.Accuracy}");
                }
            }
        }

        Random random_first = new Random();
        int first = random_first.Next(2);

        if (ActivePokemon1.Speed > ActivePokemon2.Speed || first == 0)
        {
            if (event_choice.action_count == 0)
            {
                NextAction1 = UseCapacity(activePokemon1, activePokemon1.Capacity1, activePokemon2);
            }
            else if (event_choice.action_count == 1)
            {
                NextAction1 = UseCapacity(activePokemon1, activePokemon1.Capacity2, activePokemon2);
            }
            else if (event_choice.action_count == 2)
            {
                NextAction1 = UseCapacity(activePokemon1, activePokemon1.Capacity3, activePokemon2);
            }
            if (activePokemon2.IsAlive())
            {
                if (enemy_choice == "attack")
                {
                    Capacity capacity_random = GetCapacityRandom();
                    NextAction2 = UseCapacity(activePokemon2, capacity_random, activePokemon1);
                    if (!activePokemon1.IsAlive())
                    {
                        PlayerBattle.BattleTeam.Remove(activePokemon1);
                        if (PlayerBattle.BattleTeam.Count > 0)
                        {
                            ChooseActivePokemon(PlayerBattle);
                        }
                    }
                }
                else if(enemy_choice == "change_pokemon")
                {
                    Pokemon prev_poke = ActivePokemon2;
                    SetRandomPokemon(EnemyBattle, 2);
                    NextAction2 = $"{EnemyBattle.Name} a change de Pokemon : {prev_poke.Name} -> {ActivePokemon2.Name} !";
                }
            }
            else
            {
                GiveXpToUsingPokemonDuringBattleTrainer();
                EnemyBattle.BattleTeam.Remove(ActivePokemon2);
                if(EnemyBattle.BattleTeam.Count > 0)
                {
                    Pokemon prev_poke = ActivePokemon2;
                    SetRandomPokemon(EnemyBattle, 2);
                    NextAction2 = $"{EnemyBattle.Name} a change de Pokemon : {prev_poke.Name} -> {ActivePokemon2.Name} !";
                }
            }
        }
        else if (ActivePokemon2.Speed > ActivePokemon1.Speed || first == 1)
        {
            if (enemy_choice == "attack")
            {
                Capacity capacity_random = GetCapacityRandom();
                NextAction1 = UseCapacity(activePokemon2, capacity_random, activePokemon1);
            }
            else if (enemy_choice == "change_pokemon")
            {
                Pokemon prev_poke = ActivePokemon2;
                SetRandomPokemon(EnemyBattle, 2);
                NextAction1 = $"{EnemyBattle.Name} a change de Pokemon : {prev_poke.Name} -> {ActivePokemon2.Name} !";
            }
            if (activePokemon1.IsAlive())
            {
                if (event_choice.action_count == 0)
                {
                    NextAction2 = UseCapacity(activePokemon1, activePokemon1.Capacity1, activePokemon2);
                }
                else if (event_choice.action_count == 1)
                {
                    NextAction2 = UseCapacity(activePokemon1, activePokemon1.Capacity2, activePokemon2);
                }
                else if (event_choice.action_count == 2)
                {
                    NextAction2 = UseCapacity(activePokemon1, activePokemon1.Capacity3, activePokemon2);
                }
                if (!activePokemon2.IsAlive())
                {
                    GiveXpToUsingPokemonDuringBattleTrainer();
                    EnemyBattle.BattleTeam.Remove(ActivePokemon2);
                    if(EnemyBattle.BattleTeam.Count > 0) 
                    {
                        SetRandomPokemon(EnemyBattle, 2);
                    }
                }
            }
            else
            {
                PlayerBattle.BattleTeam.Remove(activePokemon1);
                if (PlayerBattle.BattleTeam.Count > 0)
                {
                    Pokemon prev_poke = ActivePokemon1;
                    ChooseActivePokemon(PlayerBattle);
                    NextAction2 = $"Vous avez change de Pokemon : {prev_poke.Name} -> {ActivePokemon1.Name} !";
                }
            }
        }
    }
}
