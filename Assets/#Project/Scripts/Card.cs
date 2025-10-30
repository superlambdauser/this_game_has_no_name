using UnityEngine;

public abstract class Card
{
    public string Name { get; private set; }
    public int Range { get; private set; }

    public abstract void Play();

    // Method to check if in range 
}
