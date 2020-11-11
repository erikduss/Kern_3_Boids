using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    public float speed = 0.001f;
    private float rotationSpeed = 4.0f;
    private Vector3 averageHeading;
    private Vector3 averagePosition;
    private float neighbourDistance = 2.0f;

    private float minSpeed = 0.3f;
    private float maxSpeed = 1.5f;

    private bool turning = false;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        speed = Random.Range(minSpeed, maxSpeed);
        anim.SetFloat("AnimSpeed", speed);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,Vector3.zero) >= GlobalFlock.flyingSpaceSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

            speed = Random.Range(minSpeed, maxSpeed);
            anim.SetFloat("AnimSpeed", speed);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    private void ApplyRules()
    {
        GameObject[] otherBirds;
        otherBirds = GlobalFlock.allBirds;

        Vector3 vcenter = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float groupSpeed = 0.1f;
        
        Vector3 goalPos = GlobalFlock.goalPos;

        float dist;

        int groupSize = 0;

        foreach(GameObject bird in otherBirds)
        {
            if(bird != this.gameObject)
            {
                dist = Vector3.Distance(bird.transform.position, this.transform.position);
                if(dist <= neighbourDistance)
                {
                    vcenter += bird.transform.position;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - bird.transform.position);
                    }

                    FlockAgent anotherFlock = bird.GetComponent<FlockAgent>();
                    groupSpeed = groupSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vcenter = vcenter / groupSize + (goalPos - this.transform.position);
            speed = (groupSpeed / groupSize);
            if (speed > maxSpeed) speed = maxSpeed;
            anim.SetFloat("AnimSpeed", speed);

            Vector3 direction = (vcenter + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
