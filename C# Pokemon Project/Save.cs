using System;
using System.Collections.Generic;
using System.IO;

public class SaveData
{
    public string NamePlayer;
    public int PokeMoney;
    public List<Pokemon> Pokemons = new List<Pokemon>();
    public List<Pokemon> Pokedex = new List<Pokemon>(); 
    public List<Object> Inventory = new List<Object>();
    public int[] playerPos = new int[2];

    public SaveData(string nameplayer, int pokemoney, List<Pokemon> pokemons, List<Pokemon> pokedex, List<Object> inventorys, int[] player)
    {
        NamePlayer = nameplayer;
        PokeMoney = pokemoney;
        Pokemons = pokemons;
        Pokedex = pokedex;
        Inventory = inventorys;
        playerPos = player;
    }
}