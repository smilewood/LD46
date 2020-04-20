using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColisionController : MonoBehaviour
{
    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.tag == "Player" && (collision.gameObject.name == "Player" || collision.gameObject.name == "LazorHitbox") )
        {
            Instantiate(Resources.Load("EnemyDeathEffect"), this.transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
    }
}
