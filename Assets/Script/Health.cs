using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected int maxHitPoints = 100;
    protected int currentHits = 0;
    protected bool isAlive = false;

    public virtual void TakeDamage()
    {


    }
}
