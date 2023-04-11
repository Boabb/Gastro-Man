using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Projectile[] prefabs;
    public Projectile ammoType;
    Transform firePoint;

    [Header("Weapon Stats")]
    public float fireForce = 20f;
    public int currentTank = 0;
    public int maxTank { get; set; } = 100;

    void Start()
    {
        firePoint = GetComponentInChildren<Transform>();
    }
    public void Fire()
    {
        if (currentTank > 0)
        {
            Projectile bullet = Instantiate(ammoType, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            GameManager.PlaySound("playershoot");
            currentTank --;
        }
    }

    public void ChangeAmmoType(string ammoPickup)
    {
        if (ammoPickup == "Gastro Liquid")
        {
            ammoType = prefabs[0];
        }
        else if (ammoPickup == "Antibiotic")
        {
            ammoType = prefabs[1];
        }
        else if (ammoPickup == "Antacid")
        {
            ammoType = prefabs[2];
        }
        else if(ammoPickup == "Anasthetic")
        {
            ammoType = prefabs[3];
        }
    }

    public void AddAmmo(int increment)
    {
        currentTank += increment;
        if (currentTank > 100)
        {
            currentTank = 100;
        }
    }
}