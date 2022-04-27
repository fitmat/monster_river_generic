using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_ObjectOscillator : MonoBehaviour
{
    public float rightLimit, leftLimit, speed, direction;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(leftLimit, rightLimit), transform.position.y, 0);
        speed = Random.Range(2f, 5f);

        direction = Random.Range(-1f, 1f);
        if (direction >= 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 1)
        {
            transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
            if (transform.position.x > rightLimit)
            {
                direction = -1;
                speed = Random.Range(2f, 5f);
            }
        }
        else if (direction == -1)
        {
            transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
            if (transform.position.x < leftLimit)
            {
                direction = 1;
                speed = Random.Range(2f, 5f);
            }
        }
    }
}
