using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireForce = 20f;
    [SerializeField] public int currentTank = 100;
    public int maxTank { get; set; } = 100;

    // Start is called before the first frame update
    public void Fire()
    {
        if (currentTank > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
            currentTank -= 5;
        }
    }

    // Update is called once per frame
    public void Reload()
    {
        int reloadAmount = maxTank - currentTank;
        currentTank += reloadAmount;
    }

    public void AddAmmo(int increment)
    {
        currentTank += increment;
    }
}