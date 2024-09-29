using System.Collections.Generic;

public class Trainer
{
    public string Name { get; set; }
    public int PokeMoney { get; set; }
    public Inventory Inventory { get; set; }
    public List<Pokemon> Team { get; set; }
    public List<Pokemon> BattleTeam { get; set; }
    public List<Pokemon> Pokedex { get; set; }

    public Trainer(string name)
    {
        Name = name;
        Team = new List<Pokemon>();
        Inventory = new Inventory();
        BattleTeam = new List<Pokemon>();
        Pokedex = new List<Pokemon>();
        PokeMoney = 200;
    }


    public void AddPokemon(Pokemon pokemon)
    {
        Team.Add(pokemon);
    }

    public bool TeamIsAlive()
    {
        foreach (var pokemon in Team)
        {
            if (pokemon.IsAlive())
            {
                return true;
            }
        }
        return false;
    }

    public Pokemon FindByName(string pokeName)
    {
        foreach (var item in Team)
        {
            if(item.Name == pokeName)
            {
                return item;
            }
        }
        return null;
    }
}