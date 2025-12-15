using UnityEngine;

public class PlayCardGA : GameAction
{
    CardView Card { get; set; }
    public PlayCardGA(CardView card)
    {
        Card = card;
    }
}
