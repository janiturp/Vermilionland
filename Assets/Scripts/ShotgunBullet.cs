using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    /*
     * Current system is not the greatest, but it works.
     * Later can fix the solution to use the GetDamage()-function.
     */

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 4f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("ShotgunBullet"))
        {
            Destroy(gameObject);
        }
    }

    public float GetDamage()
    {
        return damage;
    }
}
