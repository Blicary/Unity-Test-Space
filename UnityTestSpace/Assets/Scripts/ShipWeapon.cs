using UnityEngine;
using System.Collections;

public class ShipWeapon : MonoBehaviour
{
    public EnergyBullet bullet_prefab;
    public Movement movement;
    public Transform weapon_tip;

    public void Update()
    {
        bool input = Input.GetButtonDown("Fire");

        if (input)
        {
            Fire();
        }


    }

    private void Fire()
    {
        EnergyBullet b = (EnergyBullet)GameObject.Instantiate(bullet_prefab, weapon_tip.position, transform.rotation);

        b.Initialize(transform, movement.DirectionPointing());
    }
	
}
