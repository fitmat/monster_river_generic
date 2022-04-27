using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MR_RaftMovement : MonoBehaviour
{
    public static MR_RaftMovement instance;

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

    public float speed;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartRaftMovement()
    {
        StartCoroutine(MoveDownRiver());
    }

    private IEnumerator MoveDownRiver()
    {
        if (MR_GameController.instance.isGameRunning)
        {
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * speed);
            yield return null;
            StartCoroutine(MoveDownRiver());
        }
    }
}
