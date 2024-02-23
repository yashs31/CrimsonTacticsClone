using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform weaponTurret;
    [SerializeField] float attackRadius;
    [SerializeField] GameObject bullets;
    [SerializeField] LayerMask enemy;
    Transform target;
    Collider[] colliersInsideAttackRad;
    void Start()
    {
        if(FindObjectOfType<EnemyAI>() != null)
        {
            target = FindObjectOfType<EnemyAI>().transform;
        }
        bullets.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SearchForTarget();
    }

    private void SearchForTarget()
    {
        colliersInsideAttackRad = Physics.OverlapSphere(transform.position, attackRadius, enemy);
        for(int i=0;i<colliersInsideAttackRad.Length;i++)
        {
            if (colliersInsideAttackRad[i].TryGetComponent<EnemyAI>(out EnemyAI enemy))
            {
                bullets.SetActive(true);
                Aim();
            }
            else
            {
                bullets.SetActive(false);
            }
        }

        if(colliersInsideAttackRad.Length<=0 && bullets.activeInHierarchy)
        {
            bullets.SetActive(false);
        }
    }

    private void Aim()
    {
        if(target==null)
        {
            target = FindObjectOfType<EnemyAI>().transform;
        }
        weaponTurret.LookAt(target);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
