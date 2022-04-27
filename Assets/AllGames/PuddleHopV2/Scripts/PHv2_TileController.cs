using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHv2_TileController : MonoBehaviour
{
    private int direction;
    private float speed;

    private float startZ, endZ, currentZ;

    private void Update()
    {
        currentZ = transform.localPosition.z;
        if ((endZ > startZ && currentZ > endZ) || (endZ < startZ && currentZ < endZ))
        {
            gameObject.SetActive(false);
        }
    }


    public void SpawnTile(int Direction, float Speed, float RightZ, float LeftZ)
    {
        speed = Speed;
        direction = Direction;

        if (direction == -1)
        {
            startZ = LeftZ;
            endZ = RightZ;
        }
        else
        {
            startZ = RightZ;
            endZ = LeftZ;
        }

        transform.localPosition = new Vector3(0, 0, startZ);
        StartCoroutine(MoveTile());
    }


    private IEnumerator MoveTile()
    {
        transform.Translate(Vector3.forward * direction * speed);

        yield return new WaitForSeconds(0.02f);

        StartCoroutine(MoveTile());
    }
}
