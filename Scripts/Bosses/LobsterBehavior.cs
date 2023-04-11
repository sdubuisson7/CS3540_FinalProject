using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobsterBehavior : BossBehavior
{
    public int maxHealth;

public override void BossStart()
    {
        currentHealth = maxHealth;
    }

    public override void BossUpdate()
    {
        throw new System.NotImplementedException();
    }
}
