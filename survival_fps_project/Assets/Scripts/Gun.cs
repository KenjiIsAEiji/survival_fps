using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    /// <summary>
    /// 発射時の初速
    /// </summary>
    public float bulletSpeed = 100f;
    
    /// <summary>
    /// 銃における弾丸の発射位置
    /// </summary>
    public Transform muzzle;
    
    /// <summary>
    /// 弾丸プレファブ
    /// </summary>
    public GameObject bulletPrefab;

    public virtual void Shoot(bool trigger)
    {
        Debug.Log("GunShoot");
    }
}
