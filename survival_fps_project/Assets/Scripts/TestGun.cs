using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGun : Gun
{
    private bool semiAutoTrigger = false;

    public override void Shoot(bool trigger)
    {
        if(trigger){
            if(!semiAutoTrigger) {
                fireBullet();
                Debug.Log("Test Gun fire!");
                semiAutoTrigger = true;
            }
        }else{
            semiAutoTrigger = false;
        }
    }

    void fireBullet()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            muzzle.position,
            muzzle.rotation
        );

        bullet.GetComponent<Rigidbody>().AddForce(muzzle.forward * bulletSpeed,ForceMode.Impulse);
        Destroy(bullet,10f);
    }
}
