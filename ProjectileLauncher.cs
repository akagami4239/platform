using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectilePrefab;
    public GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }


    public void FireProjectile()
    {
        if(player.transform.localScale == new Vector3(-1, player.transform.localScale.y, player.transform.localScale.z)) {

            GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            Vector3 origScale = projectile.transform.localScale;

            projectile.transform.localScale = new Vector3(
                origScale.x * transform.localScale.x > 0 ? 1 : -1,
                origScale.y * transform.localScale.y < 0 ? 1: -1,
                origScale.z
                );
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
            Vector3 origScale = projectile.transform.localScale;

            projectile.transform.localScale = new Vector3(
                origScale.x * transform.localScale.x > 0 ? 1 : -1,
                origScale.y * transform.localScale.y,
                origScale.z
                );
        }
       
       
    }
        
    
}
