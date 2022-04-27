using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EC_PlayerTwoController : MonoBehaviour
{
    public static EC_PlayerTwoController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private float lastX, nextX, currentX;
    public float step;

    // Start is called before the first frame update
    void Start()
    {

    }




    public IEnumerator MoveRight()
    {
        // Calculate start and end positions
        lastX = transform.position.x;
        nextX = lastX + step;
        // Clamp end position
        if (nextX > 25)
        {
            nextX = 25;
        }
        currentX = lastX;

        // Change position in steps
        while (currentX < nextX)
        {
            currentX += step / 20;
            transform.position = new Vector2(currentX, transform.position.y);
            yield return null;
        }
    }
    public IEnumerator MoveLeft()
    {
        lastX = transform.position.x;
        nextX = lastX - step;
        if (nextX < -11)
        {
            nextX = -11;
        }
        currentX = lastX;

        while (currentX > nextX)
        {
            currentX -= step / 20;
            transform.position = new Vector2(currentX, transform.position.y);
            yield return null;
        }
    }

}
