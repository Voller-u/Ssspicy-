using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Collectable
{
    public override void Eaten(Player player)
    {
        base.Eaten(player);
        player.RecordPos();
    }
}
