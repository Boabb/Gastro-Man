using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Weapon weapon;

    void Start()
    {
        UpdateAmmoText();
    }

    void Update()
    { 
        UpdateAmmoText();
    }
    void UpdateAmmoText()
    {
        text.text = $"{weapon.currentTank} / {weapon.maxTank}";
        if(weapon.currentTank > weapon.maxTank)
        {
            text.color = Color.blue;
        }
        else if(weapon.currentTank < 11)
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.white;
        }
    }
}
