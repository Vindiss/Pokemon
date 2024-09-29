using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

public class Pokemon
{
    public string Name { get; set; }
    public string TypeOne { get; set; }
    public string TypeTwo { get; set; }
    public int Total { get; set; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int AttackSpecial { get; set; }
    public int DefenseSpecial { get; set; }
    public int Speed { get; set; }
    public int Level { get; set; }
    public int PvMax {  get; set; }
    public int Pv { get; set; }
    public int Xp { get; set; }
    public Capacity Capacity1 { get; set; }
    public Capacity Capacity2 { get; set; }
    public Capacity Capacity3 { get; set; }
    public bool IsUsing { get; set; }
    public bool Switch {  get; set; }
    public string[] NextLearnCapacity { get; set; }
    public bool AsLevelUp  = false;


    public Pokemon(string name, string typeOne, string typeTwo, int total ,int health, int attack, int defense, int attackspecial, int defensespecial, int speed)
    {
        Name = name;
        TypeOne = typeOne;
        TypeTwo = typeTwo;
        Total = total;
        Health = health;
        Attack = attack;
        Defense = defense;
        AttackSpecial = attackspecial;
        DefenseSpecial = defensespecial;
        PvMax = GetPvByFormule();
        Pv = GetPvByFormule();
        Speed = speed;
        Level = 1;
        Xp = 0;
    }

    public Pokemon() {
        Level = 1;
        Capacity1 = null; Capacity2 = null; Capacity3 = null;
    }


    public Pokemon(string name)
    {
        Name = name;
        Pv = 20;
        Level = 1;
    }

    public void TakeDamage(int damage)
    {
        Pv -= damage;
        if (Pv < 0)
        {
            Pv = 0;
        }
    }

    public void Heal(double heal)
    {
        if(Pv + heal > PvMax) 
        {
            Pv = PvMax;
        }
        else
        {
            Pv += (int)heal;
        }
    }

    public void GiveXp(Pokemon pokemon, bool capture)
    {
        if (IsAlive())
        {
            if (capture)
            {
                int formuleXp = (int)(((1.5 * 75 * pokemon.Level) / (5 * 1)) * ((2 * pokemon.Level + 10) / Math.Pow((pokemon.Level + Level + 10), 2.5) + 1));
                if (Switch)
                {
                    formuleXp = (int)(formuleXp * 1.5);
                    Xp += formuleXp;
                }
                Xp += formuleXp;
                Console.WriteLine($"{Name} a gagne {formuleXp} XP {CanLevelUp()}");
            }
            else
            {
                int formuleXp = (int)(((1 * 75 * pokemon.Level) / (5 * 1)) * ((2 * pokemon.Level + 10) / Math.Pow((pokemon.Level + Level + 10), 2.5) + 1));
                if (Switch)
                {
                    formuleXp += (int)(formuleXp * 1.5);
                    Xp += formuleXp;
                }
                Xp += formuleXp;
                Console.WriteLine($"{Name} a gagne {formuleXp} XP {CanLevelUp()}");
            }
        }
    }

    public string CanLevelUp()
    {
        if (Xp >= Math.Pow((Level+1),3))
        {
            Xp -= (int)Math.Pow((Level+1),3);
            Level += 1;
            AsLevelUp = true;
            PvMax = GetPvByFormule();
            return $"et passe Level {Level}";
        }
        else
        {
            return "!";
        }
    }

    public void CanLearnNewCapacity()
    {
        foreach (var new_capactiy in NextLearnCapacity)
        {
            string[] parts = new_capactiy.Split('-');
            if (parts[0] == "L" + Level.ToString() + " ")
            {
                foreach (var capacity in Game.Instance.all_capacity)
                {
                    if (parts[1].Contains(capacity.Name))
                    {
                        LearnNewCapacity(capacity);
                        break;
                    }
                }
            }
        }
    }
public void LearnNewCapacity(Capacity capacity)
    {
        Console.Clear();
        if (Capacity1 == null)
        {
            Capacity1 = capacity;
            Console.WriteLine($"{Name} a appris {capacity.Name} !");

        }
        else if (Capacity2 == null)
        {
            Capacity2 = capacity;
            Console.WriteLine($"{Name} a appris {capacity.Name} !");
        }
        else if (Capacity3 == null)
        {
            Capacity3 = capacity;
            Console.WriteLine($"{Name} a appris {capacity.Name} !");
        }
        else
        {
            Event event_choice = new Event();
            bool choice = false;
            while (!choice)
            {
                Console.Clear();
                Console.WriteLine($"{Name} souhaite apprendre {capacity.Name}");
                if (event_choice.action_count == 0)
                {
                    Console.WriteLine("> Oublier une capacite");
                    Console.WriteLine($"  Ne pas apprendre {capacity.Name}");
                }else if (event_choice.action_count == 1)
                {
                    Console.WriteLine("  Oublier une capacite");
                    Console.WriteLine($"> Ne pas apprendre {capacity.Name}");
                }
                choice = event_choice.ChoiceEvent(2);
            }
            if(event_choice.action_count == 0)
            {
                event_choice.action_count = 0;
                choice = false;
                while (!choice)
                {
                    Console.Clear();
                    Console.WriteLine("Quelle capacite souhaitez-vous oublier ?");
                    if (event_choice.action_count == 0)
                    {
                        Console.WriteLine($"> {Capacity1.Name} | {Capacity1.Type} | {Capacity1.Category} | {Capacity1.Power} | {Capacity1.Accuracy}");
                        Console.WriteLine($"  {Capacity2.Name} | {Capacity2.Type} | {Capacity2.Category} | {Capacity2.Power} | {Capacity2.Accuracy}");
                        Console.WriteLine($"  {Capacity3.Name} | {Capacity3.Type} | {Capacity3.Category} | {Capacity3.Power} | {Capacity3.Accuracy}");
                    }
                    else if (event_choice.action_count == 1)
                    {
                        Console.WriteLine($"  {Capacity1.Name} | {Capacity1.Type} | {Capacity1.Category} | {Capacity1.Power} | {Capacity1.Accuracy}");
                        Console.WriteLine($"> {Capacity2.Name} | {Capacity2.Type} | {Capacity2.Category} | {Capacity2.Power} | {Capacity2.Accuracy}");
                        Console.WriteLine($"  {Capacity3.Name} | {Capacity3.Type} | {Capacity3.Category} | {Capacity3.Power} | {Capacity3.Accuracy}");
                    }
                    else if (event_choice.action_count == 2)
                    {
                        Console.WriteLine($"  {Capacity1.Name} | {Capacity1.Type} | {Capacity1.Category} | {Capacity1.Power} | {Capacity1.Accuracy}");
                        Console.WriteLine($"  {Capacity2.Name} | {Capacity2.Type} | {Capacity2.Category} | {Capacity2.Power} | {Capacity2.Accuracy}");
                        Console.WriteLine($"> {Capacity3.Name} | {Capacity3.Type} | {Capacity3.Category} | {Capacity3.Power} | {Capacity3.Accuracy}");
                    }

                    choice = event_choice.ChoiceEvent(3);
                }
                Console.Clear();
                if (event_choice.action_count == 0)
                {
                    Console.WriteLine($"{Name} a oublie {Capacity1.Name} et a appris {capacity.Name} !");
                    Capacity1 = capacity;
                }
                else if (event_choice.action_count == 1)
                {
                    Console.WriteLine($"{Name} a oublie {Capacity2.Name} et a appris {capacity.Name} !");
                    Capacity2 = capacity;
                }
                else if (event_choice.action_count == 2)
                {
                    Console.WriteLine($"{Name} a oublie {Capacity2.Name} et a appris {capacity.Name} !");
                    Capacity3 = capacity;
                }
            }
        }
        Console.Write("\nAppuyer sur une touche pour passer...");
        Console.ReadKey();
    }
    public void CanLearnNewCapacityEnemy()
    {
        foreach (var new_capactiy in NextLearnCapacity)
        {
            string[] parts = new_capactiy.Split('-');
            if (parts[0] == "L" + Level.ToString() + " ")
            {
                foreach (var capacity in Game.Instance.all_capacity)
                {
                    if (parts[1].Contains(capacity.Name))
                    {
                        LearnNewCapacityEnemy(capacity);
                        break;
                    }
                }
            }
        }
    }

    public void LearnNewCapacityEnemy(Capacity capacity)
    {
        if (Capacity1 == null)
        {
            Capacity1 = capacity;
        }
        else if (Capacity2 == null)
        {
            Capacity2 = capacity;
        }
        else if (Capacity3 == null)
        {
            Capacity3 = capacity;
        }
        else
        {
            Random random = new Random();
            int random_choice = random.Next(0, 2);
        
            if (random_choice == 0)
            
                if (random_choice == 0)
                {
                    Capacity1 = capacity;
                }
                else if (random_choice == 1)
                {
                    Capacity2 = capacity;
                }
                else if (random_choice == 2)
                {
                    Capacity3 = capacity;
                }
        }
        

    }

    public bool IsAlive()
    {
        if(Pv > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetPvByFormule()
    {
        int pv = ((2 * Health * Level / 100) + Level + 10);
        return pv;
    }

    public int GetStatByFormule(int statistique)
    {
        int resultat_stat = (((2 * statistique * Level / 100) + 5) * 1);
        return resultat_stat;
    }

    public void AddCapacity(Capacity capacity)
    {
        if (Capacity1 == null)
        {
            Capacity1 = capacity;
        }
        else if (Capacity2 == null)
        {
            Capacity2 = capacity;
        }
        else if (Capacity3 == null)
        {
            Capacity3 = capacity;
        }
    }
}