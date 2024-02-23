using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] int maxHits = 20;
    void Start()
    {
        currentHits = maxHits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
        currentHits -= 1;

        if(currentHits<=0)
        {
            currentHits = maxHits;
            this.gameObject.SetActive(false);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        TakeDamage();
    }
}
