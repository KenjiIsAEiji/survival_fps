using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    // 発射時の初速
    public float bulletSpeed = 100f;
    
    // 銃における弾丸の発射位置
    public Transform muzzle;
    
    // 弾丸プレファブ
    public GameObject bulletPrefab;

    public virtual void Shoot()
    {
        Debug.Log("GunShoot");
    }
}
