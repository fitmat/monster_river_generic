using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_PlayerTwoController : MonoBehaviour
{
    public static HS_PlayerTwoController instance;

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

    public int snakeLength;

    public float timeStep, moveAmount;
    public int turnAngle;

    [SerializeField] private Transform snakeTransform;
    [SerializeField] private Transform rightSpawn, leftSpawn;

    [SerializeField] private GameObject snakeHead;
    [SerializeField] private GameObject snakePartPrefab;
    [SerializeField] private Material snakeMaterialSolid, snakeMaterialTransparent;

    public bool isTakingDamage, hasMoved, isTurning, isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        isTakingDamage = false;
        hasMoved = false;
        isWaiting = false;
        isTurning = false;
    }


    public IEnumerator TurnRight()
    {
        if (!isTurning)
        {
            isTurning = true;
            snakeTransform.GetChild(0).Rotate(0, turnAngle, 0, Space.Self);
            yield return new WaitForSeconds(0.2f);
            isWaiting = false;
            isTurning = false;
        }
    }

    public IEnumerator TurnLeft()
    {
        if (!isTurning)
        {
            isTurning = true;
            snakeTransform.GetChild(0).Rotate(0, -turnAngle, 0, Space.Self);
            yield return new WaitForSeconds(0.2f);
            isWaiting = false;
            isTurning = false;
        }
    }

    public IEnumerator SpawnAtRight()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            snakeTransform.GetChild(0).position = rightSpawn.position;
            //snakeTransform.GetChild(0).Rotate(0, 180, 0, Space.Self);
            yield return new WaitForSeconds(2f);
            hasMoved = false;
        }
    }

    public IEnumerator SpawnAtLeft()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            snakeTransform.GetChild(0).position = leftSpawn.position;
            //snakeTransform.GetChild(0).Rotate(0, 180, 0, Space.Self); 
            yield return new WaitForSeconds(2f);
            hasMoved = false;
        }
    }

    public IEnumerator MoveAhead()
    {
        yield return new WaitForSeconds(timeStep);
        if (!isTakingDamage)
        {

            Vector3[] _tempPosition = new Vector3[snakeLength];


            for (int i = 0; i < snakeLength; i++)
            {
                try
                {
                    _tempPosition[i] = snakeTransform.GetChild(i).position;
                }
                catch { }
            }
            snakeTransform.GetChild(0).Translate(Vector3.forward * moveAmount);

            for (int i = 1; i < snakeLength; i++)
            {
                snakeTransform.GetChild(i).position = _tempPosition[i - 1];
            }
            if (isWaiting)
            {
                MoveBehind();
            }
        }
        StartCoroutine(MoveAhead());
    }

    private void MoveBehind()
    {
        Vector3[] _tempPosition = new Vector3[snakeLength];


        for (int i = 0; i < snakeLength; i++)
        {
            try
            {
                _tempPosition[i] = snakeTransform.GetChild(i).position;
            }
            catch { }
        }
        snakeTransform.GetChild(0).Translate(Vector3.back * moveAmount * 2);


        for (int i = 0; i < snakeLength - 1; i++)
        {
            snakeTransform.GetChild(i).position = _tempPosition[i + 1];
        }
    }

    public IEnumerator TakeDamage()
    {
        if (!isTakingDamage)
        {
            isWaiting = true;
            isTakingDamage = true;
            MoveBehind();
            HS_AudioManager.instance.PlayAudio("Die");
            for (int j = 0; j < 4; j++)
            {
                if (snakeLength > 1 && j < 2)
                {
                    Destroy(snakeTransform.GetChild(snakeLength - 1).gameObject);
                    snakeLength--;
                }
                else if (snakeLength == 1)
                {
                    HS_GameController.instance.winningPlayer = 1;
                    HS_GameController.instance.GameOver();
                }

                for (int i = 0; i < snakeLength; i++)
                {
                    snakeTransform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = snakeMaterialTransparent;
                }
                yield return new WaitForSecondsRealtime(0.25f);
                for (int i = 0; i < snakeLength; i++)
                {
                    snakeTransform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = snakeMaterialSolid;
                }
                yield return new WaitForSecondsRealtime(0.25f);

            }
            isTakingDamage = false;
        }
    }


    private IEnumerator IncreaseLengthOverTime()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(IncreaseLength(1));
        StartCoroutine(IncreaseLengthOverTime());
    }

    public IEnumerator IncreaseLength(int bonus)
    {
        if (!isTakingDamage)
        {
            for(int i = 1; i < snakeLength; i++)
            {
                snakeTransform.GetChild(i).localScale = new Vector3(1.1f, 1.1f, 1.1f);
                yield return new WaitForSeconds(0.3f);
                snakeTransform.GetChild(i).localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
            HS_AudioManager.instance.PlayAudio("Grow");
            for (int i = 0; i < bonus; i++)
            {
                GameObject _newBodyPart;
                _newBodyPart = Instantiate(snakePartPrefab, new Vector3(snakeTransform.GetChild(snakeLength - 1).localPosition.x, snakeTransform.GetChild(snakeLength - 1).localPosition.y, snakeTransform.GetChild(snakeLength - 1).localPosition.z - (moveAmount / 2)), Quaternion.identity, snakeTransform);
                snakeLength++;
                yield return new WaitForSeconds(0.5f);
                _newBodyPart.GetComponent<MeshRenderer>().material = snakeMaterialSolid;
            }
        }
    }
}
