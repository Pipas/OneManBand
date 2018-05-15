using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public Transform[] points;
    public EnemyHitbox hitbox;

    private float speed = 1.5f;
    private int currentPoint = 0;
    private bool running;

	// Use this for initialization
	void Start () {
        running = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (hitbox.isPlayerWithinRange())
        {
            if (running)
            {
                GetComponent<Animator>().SetInteger("States", 0);
                running = false;
            }
        }
        else {

            if (!running)
            {
                GetComponent<Animator>().SetInteger("States", 1);
                running = true;
            }

            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                if (currentPoint < points.Length) // this is to check if it's less than the length of the points
                {
                    float dist = Vector3.Distance(transform.position, points[currentPoint].position);

                    if (dist < 0.1f)
                    {
                        int tmpIndex = currentPoint + 1;

                        if (tmpIndex >= points.Length)
                        {
                            tmpIndex = 0;
                        }

                        Vector3 targetDir = points[tmpIndex].position - transform.position;
                        float step = speed * Time.deltaTime;
                        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                        Debug.DrawRay(transform.position, newDir, Color.red);

                        transform.rotation = Quaternion.LookRotation(newDir);

                        Vector3 dist_vec = (points[tmpIndex].position - transform.position).normalized;
                        float dotProd = Vector3.Dot(dist_vec, transform.forward);

                        if (dotProd >= 1.00)
                        {
                            currentPoint++;
                        }
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(this.transform.position, points[currentPoint].position, Time.deltaTime * speed);
                    }
                }
                else
                {
                    currentPoint = 0; // this is to loop it back to zero again
                }
            }
        }
	}


}
