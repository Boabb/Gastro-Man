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

    private void Start()
    {
        firePoint = GetComponentInChildren<Transform>();
    }
    public void Fire()
    {
        if (currentTank > 0)
        {
            Projectile bullet = Instantiate(ammoType, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            currentTank --;
        }
    }

    public void Reload()
    {
        int reloadAmount = maxTank - currentTank;
        currentTank += reloadAmount;
    }

    IEnumerator DecreaseOverfill()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (currentTank <= 100)
            {
                break;
            }
            currentTank--;
        }
        yield return null;
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
        else if(ammoPickup == "Anasthetic")
        {
            ammoType = prefabs[2];
            Debug.Log("anasthtic set");
        }
    }

    public void AddAmmo(int increment)
    {
        currentTank += increment;
        if (currentTank > 100)
        {
            StartCoroutine(DecreaseOverfill());
        }
    }
}