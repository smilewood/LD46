using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAhead : ShooterComponent
{
    public GameObject ShotPrefab;
    public float ShotDelay;
    public float ShotSpeed;
    public bool playSound;
    private AudioSource shootAudioSource;

    private void OnEnable()
    {
        if(playSound)
            shootAudioSource = this.GetComponent<AudioSource>();

        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(ShotDelay);
            GameObject newShot = Instantiate(ShotPrefab, this.transform.position, this.transform.rotation);
            newShot.GetComponent<BulletControl>().Initialize(this.transform.forward, ShotSpeed);

            shootAudioSource?.Play();
        }
    }
}
