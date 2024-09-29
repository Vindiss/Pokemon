using System;
using System.Collections.Generic;
using System.Security.AccessControl;

public class Inventory 
{
    private List<Object> _inventory = new List<Object>();

    public Inventory() { }

    public List<Object> GetInventory()
    { 
        return _inventory;
    }

    public Object GetObjectByName(string objName)
    {
        foreach (Object obj in _inventory)
        {
            if (obj.Name == objName)
            {
                return obj;
            }
        }
        return null;
    }

    public bool IsInInventory(string objName)
    {
        foreach (Object obj in _inventory)
        {
            if(obj.Name == objName && obj.Quantity > 0)
            {
                return true;
            }
        }
        return false;
    }
    public void AddObject(Object obj, int quantity) 
    { 
        Object find = null;
        foreach (var item in _inventory)
        {
            if (item.Name == obj.Name)
            {
                find = item;
                break;
            }
        }
        if(find == null)
        {
            _inventory.Add(obj);
            obj.Quantity += quantity;
        }
        else
        {
            find.Quantity += quantity;
        }
    }

    public void OpenInventory()
    {
        Console.Clear();
        Event choice_event = new Event();
        bool choice = false;
        int nb_choice = 1;
        List<Object> prov_inventory = new List<Object>();
        foreach (var obj in _inventory)
        {
            if(obj.Quantity > 0)
            {
                nb_choice++;
                prov_inventory.Add(obj);
            }
        }
        Console.Clear();
        while (!choice)
        {
            Console.WriteLine("Inventaire:\t\t Entrez pour utiliser");
            foreach (var obj in prov_inventory)
            {
                if(choice_event.action_count == prov_inventory.IndexOf(obj))
                {
                    Console.WriteLine($"> {obj.Name}  x{obj.Quantity}");
                }
                else
                {
                    Console.WriteLine($"  {obj.Name}  x{obj.Quantity}");
                }
            }
            if(choice_event.action_count == prov_inventory.Count)
            {
                Console.WriteLine("> Quitter l'inventaire");
            }
            else
            {
                Console.WriteLine("  Quitter l'inventaire");
            }
          
            choice = choice_event.ChoiceEvent(nb_choice);
            Console.Clear();
            if (choice)
            {
                if (choice_event.action_count != prov_inventory.Count)
                {
                    if (prov_inventory[choice_event.action_count].Name == "PokeBall" || prov_inventory[choice_event.action_count].Name == "SuperBall" || prov_inventory[choice_event.action_count].Name == "HyperBall")
                    {
                        Console.WriteLine("Vous ne pouvez pas utiliser cette objet !\n");
                        choice = false;
                    }
                    else
                    {
                        bool action_validate = prov_inventory[choice_event.action_count].UseObject();
                        if(!action_validate)
                        {
                            Console.WriteLine("Ce Pokemon a deja ses PV au max !");
                        }
                        choice = false;
                    }
                }
            }
        }
    }

    public Object OpenInventoryDuringBattle()
    {
        Console.Clear();
        Event choice_event = new Event();
        bool choice = false;
        int nb_choice = 1;
        List<Object> prov_inventory = new List<Object>();
        foreach (var obj in _inventory)
        {
            if (obj.Quantity > 0)
            {
                nb_choice++;
                prov_inventory.Add(obj);
            }
        }
        Console.Clear();
        while (!choice)
        {
            Console.WriteLine("Inventaire:\t\t Entrez pour utiliser");
            foreach (var obj in prov_inventory)
            {
                if (choice_event.action_count == prov_inventory.IndexOf(obj))
                {
                    Console.WriteLine($"> {obj.Name}  x{obj.Quantity}");
                }
                else
                {
                    Console.WriteLine($"  {obj.Name}  x{obj.Quantity}");
                }
            }
            if (choice_event.action_count == prov_inventory.Count)
            {
                Console.WriteLine("> Quitter l'inventaire");
            }
            else
            {
                Console.WriteLine("  Quitter l'inventaire");
            }

            choice = choice_event.ChoiceEvent(nb_choice);
            Console.Clear();
            if (choice)
            {
                if (choice_event.action_count != prov_inventory.Count)
                {
                    return prov_inventory[choice_event.action_count];
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }
  
      public void RemoveObject(Object obj, int quantity)
    {
        obj.Quantity -= quantity;
        if (obj.Quantity == 0)
        {
            _inventory.Remove(obj);
        }
    }
  
    public List<Object> SaveInventory()
    {
        return _inventory;
    }
}