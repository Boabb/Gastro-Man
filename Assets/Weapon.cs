using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireForce = 20f;
    [SerializeField] public int currentTank = 100;
    public int maxTank { get; set; } = 100;

    public void Update()
    {
        
    }
    // Start is called before the first frame update
    public void Fire()
    {
        if (currentTank > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

    public void AddAmmo(int increment)
    {
        currentTank += increment;
        if (currentTank > 100)
        {
            StartCoroutine(DecreaseOverfill());
        }
    }
}