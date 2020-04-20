using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public Vector3 velocity;
    public float speed;
    private Camera mainCamera;
    private GameObject bulletObject;

    public void Initialize(Vector3 v, float speed)
    {
        this.velocity = v;
        this.speed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(velocity == Vector3.zero)
        {
            Destroy(this.gameObject);
            return;
        }
        mainCamera = Camera.main;
        bulletObject = this.transform.Find("Bullet").gameObject;
    }

    private void FixedUpdate()
    {
        this.transform.position = this.transform.position + (velocity * speed * Time.deltaTime);

        Vector3 viewPoint = mainCamera.WorldToViewportPoint(this.transform.position);
        if (Math.Max(viewPoint.x, viewPoint.y) > 1.5f || Math.Min(viewPoint.x, viewPoint.y) < -.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.tag == "Player")
        {
            this.GetComponent<ParticleSystem>().Play();
            if(collision.gameObject.name == "Follower")
            {
                this.GetComponent<AudioSource>().Play();
            }
            Destroy(bulletObject);
        }
    }

}
