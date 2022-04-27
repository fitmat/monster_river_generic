using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TR_RaftController : MonoBehaviour
{
    public static TR_RaftController instance;

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

    public float speed, moveTime;

    private int hitCounter;

    [SerializeField] private List<GameObject> inactiveFlames, activeFlames, highFlames;
    private GameObject flames;
    private GameObject[] flamesChildren;
    private int flamesChildrenCount;

    public int raftHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TerrainEnd")
        {
            MR_TerrainGenerator.instance.SpawnNewTerrain();
            MR_GameController.instance.SpeedUpGame();
        }
        else if (other.gameObject.tag == "GameEndPoint")
        {
            MR_GameController.instance.isGameWon = true;
            TR_UIController.instance.GameOver();
        }
    }

    public void StartRaftState()
    {
        raftHealth = 100;
        hitCounter = 0;

        foreach (GameObject flame in inactiveFlames)
        {
            flame.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void MoveRaft()
    {
        StartCoroutine(MoveDownRiver());
    }

    private IEnumerator MoveDownRiver()
    {
        float timeStep, currentTime, newSpeed;
        timeStep = 0.02f;
        currentTime = 0f;

        if (MR_GameController.instance.isGameRunning)
        {
            while (currentTime < moveTime)
            {
                gameObject.transform.parent.Translate(Vector3.right * Time.deltaTime * speed);
                yield return new WaitForSeconds(timeStep);
                currentTime += timeStep;
            }
            currentTime = 0f;
            newSpeed = speed;
            while (currentTime < moveTime * 2)
            {
                gameObject.transform.parent.Translate(Vector3.right * Time.deltaTime * newSpeed);
                yield return new WaitForSeconds(timeStep);
                currentTime += timeStep;
                newSpeed /= 5;
            }
        }
    }

    public void DamageRaft(GameObject target)
    {
        target.GetComponentInChildren<ParticleSystem>().Play();
        int randomInt = Random.Range(1, 4);
        MR_AudioManager.instance.PlayAudio("RaftDamage" + randomInt.ToString());
        raftHealth -= 5;
        hitCounter++;

        if (raftHealth == 0)
        {
            MR_GameController.instance.isGameWon = false;
            TR_UIController.instance.GameOver();
        }

        if (hitCounter > 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (inactiveFlames.Count != 0)
                {
                    ActivateFlame();
                }
                else if (activeFlames.Count != 0)
                {
                    IncreaseFlame();
                }
            }
            else
            {
                if (activeFlames.Count != 0)
                {
                    IncreaseFlame();
                }
                else if (inactiveFlames.Count != 0)
                {
                    ActivateFlame();
                }
            }
        }

        //MR_UIController.instance.UpdateHealth();
    }


    private void ActivateFlame()
    {
        flames = inactiveFlames[Random.Range(0, inactiveFlames.Count)];
        flames.GetComponent<ParticleSystem>().Play();
        inactiveFlames.Remove(flames);
        activeFlames.Add(flames);
    }

    private void IncreaseFlame()
    {
        flames = activeFlames[Random.Range(0, activeFlames.Count)];
        flamesChildrenCount = flames.transform.childCount;
        for (int i = 0; i < flamesChildrenCount; i++)
        {
            flames.transform.GetChild(i).localScale = new Vector3(2, 2, 2);
        }
        activeFlames.Remove(flames);
        highFlames.Add(flames);
    }
}
