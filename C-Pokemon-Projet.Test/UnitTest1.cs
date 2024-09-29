namespace C_Pokemon_Projet.Test;

public class Tests
{
    private Battle _battle;
    private Pokemon _pokemon;
    private Inventory _inventory;

    [SetUp]
    public void Setup()
    {
        _battle = new Battle();
        _pokemon = new Pokemon();
        _inventory = new Inventory();
    }

    [TestCase("PokeTest1", "Fire", "Water", 200, 20, 10, 10, 20, 20, 5)]
    public void AddPokemon(string name, string type1, string type2, int total, int health, int attack, int defence, int attackspecial, int defencespecial, int speed)
    {
        Pokemon pokemon = new Pokemon(name, type1, type2, total, health, attack, defence, attackspecial, defencespecial, speed);
        Assert.IsNotNull(pokemon);
    }

    [TestCase("test","test" , 10)]
    public void AddObjectInInventory(string name, string description, int quantity) 
    {
        Object obj = new Object(name, description);
        _inventory.AddObject(obj, quantity);
        Assert.IsNotNull(_inventory);
    }

    [TestCase("test", "test", 10)]
    public void RemoveObjectInInventory(string name, string description, int quantity)
    {
        Object obj = new Object(name, description);
        _inventory.RemoveObject(obj, quantity);
        Assert.IsEmpty(_inventory.GetInventory());
    }

    [TestCase(10)]
    public void PokemonTakeDamage(int damage) 
    {
        Pokemon pokemon = new Pokemon("test");
        pokemon.TakeDamage(damage); 
        Assert.That(pokemon.Pv, Is.EqualTo(10));
    }

    [Test]
    public void BattleGetPower()
    {
        Pokemon pokemon1 = new Pokemon("PokeTest1", "Fire", "Water", 200, 20, 10, 10, 20, 20, 5);
        Pokemon pokemon2 = new Pokemon("PokeTest2", "Fire", "Water", 200, 20, 10, 10, 20, 20, 5);
        Capacity capacity = new Capacity("Pound","The target is physically pounded with a long tail, a foreleg, or the like.","Normal","Physical",40,100,false);
        int power = _battle.GetPower(capacity);
        Assert.That(power, Is.EqualTo(40));
    }
}