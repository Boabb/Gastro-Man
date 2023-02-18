using UnityEngine;

public class TankRefill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Weapon weapon = collision.gameObject.GetComponentInChildren<Weapon>();
        if(weapon)
        {
            weapon.AddAmmo(weapon.maxTank);
            Destroy(gameObject);
        }
    }
    
}
