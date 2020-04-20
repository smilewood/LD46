using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour
{

    public LineRenderer MainBeam;
    public LineRenderer SideBeam1;
    public LineRenderer SideBeam2;

    public float MinChargeToFire;
    public AudioSource NoFireSound;
    public AudioSource FireSound;

    private GameObject beams;

    private float length = 20f;
    private int divisions = 200;
    private float firingTime;
    private float MaxTime;

    // Use this for initialization
    void Start()
    {
        //MainBeam.SetPosition(0, this.transform.position);
        //MainBeam.SetPosition(1, this.transform.position + (this.transform.forward * length));
        
        SideBeam1.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0.2f), new Keyframe(1, 0.2f) });
        SideBeam2.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0.2f), new Keyframe(1, 0.2f) });

        beams = this.transform.Find("Beams").gameObject;
        beams.SetActive(false);
    }

    public bool StartFiring(float maxTime )
    {
        if(maxTime < MinChargeToFire)
        {
            NoFireSound.Play();
            return false;
        }

        firingTime = 0f;
        MaxTime = maxTime;

        beams.SetActive(true);
        FireSound.Play();

        return true;
    }

    public void EndFiring()
    {
        beams.SetActive(false);
        FireSound.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(firingTime < MaxTime)
        {
            RenderLaser();
            firingTime += Time.deltaTime;
            if(firingTime >= MaxTime)
            {
                EndFiring();
            }
        }
        
    }

    void RenderLaser()
    {
        MainBeam.positionCount = divisions;
        for (int i = 0; i < divisions; ++i)
        {
            MainBeam.SetPosition(i, Vector3.Lerp(this.transform.position, this.transform.position + (this.transform.forward * length), (float)i / (float)divisions));
        }

        Keyframe[] mainBeamFrames = new Keyframe[divisions];

        for(int i = 0; i < divisions; ++i)
        {
            float val = (divisions - (float)i) + Time.time*(firingTime+10);// *(float)i;
            mainBeamFrames[i].value = Mathf.Min(0.15f, ((Mathf.Sin(val)+1f)/6f)) + 0.3f;
            mainBeamFrames[i].time = (float)i / (float)length;
        }


        AnimationCurve MainBeamWidth = new AnimationCurve(mainBeamFrames);
        
        MainBeam.widthCurve = MainBeamWidth;



        OffsetSideBeams(ref SideBeam1, this.transform.position + (this.transform.right / 2f), this.transform.forward, length, 1f);
        OffsetSideBeams(ref SideBeam2, this.transform.position - (this.transform.right / 2f), this.transform.forward, length, -1f);

    }

    private void OffsetSideBeams(ref LineRenderer beam, Vector3 origion, Vector3 direction, float length, float mult)
    {
        beam.positionCount = divisions;
        for (int i = 0; i < divisions; ++i)
        {
            Vector3 pos = Vector3.Lerp(origion, origion + (direction * length), (float)i / (float)divisions);
            Vector3 sideways = Quaternion.Euler(0, 90, 0) * direction;

            float val = ((divisions - (float)i)/2f) + Time.time * (firingTime+10)/2f;

            pos += sideways * (Mathf.Cos(val)/6) * mult;

            beam.SetPosition(i, pos);
        }
    }
    
}