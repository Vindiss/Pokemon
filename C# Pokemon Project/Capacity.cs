using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Capacity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public float Power { get; set; }
    public float Accuracy { get; set; }
    public bool Critical { get; set; }

    public Capacity(string name, string description, string type, string category, float power, float accuracy, bool critical)
    {
        Name = name;
        Description = description;
        Type = type;
        Category = category;
        Power = power;
        Accuracy = accuracy;    
        Critical = critical;
    }

    public Capacity() { }

    public Capacity(string[] values, List<string> type_list) 
    {
        string _name = values[1];
        string _des = "";
        int k = 2;
        while (!type_list.Contains(values[k].ToLower()))
        {
            _des += values[k];
            k++;
        }
        string type = values[k];
        string category = values[k + 1];
        string power = values[k + 2];
        if (power == "—")
        {
            power = "0";
        }
        string accuracy = values[k + 3];
        if (accuracy == "—")
        {
            accuracy = "0";
        }
        else
        {
            string new_val = "";
            for (int i = 0; i < values[k + 3].Length; i++)
            {
                if (values[k + 3][i] == '%')
                {
                    break;
                }
                else
                {
                    new_val += values[k + 3][i];
                }
            }
            accuracy = new_val;
        }
        bool critical = false;
        if (values[k + 7] == "1")
        {
            critical = true;
        }

        Name = _name;
        Description = _des;
        Type = type;
        Category = category;
        Power = int.Parse(power);
        Accuracy = int.Parse(accuracy);
        Critical = critical;
    }
}