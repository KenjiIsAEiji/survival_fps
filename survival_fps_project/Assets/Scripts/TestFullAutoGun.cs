using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFullAutoGun : Gun
{
    public float fireRate = 1f;
    private bool triggerDownFlag = false;

    public override void Shoot(bool trigger)
    {
        if(trigger){
            if(!triggerDownFlag){
                triggerDownFlag = true;
                StartCoroutine(triggerCycle());
            }
        }else{
            triggerDownFlag = false;
        }
    }

    IEnumerator triggerCycle()
    {
        while(true){
            if(!triggerDownFlag) break;
            fireBullet();
            yield return new WaitForSeconds(fireRate);
        }
        yield break;
    }

    private void fireBullet()
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
