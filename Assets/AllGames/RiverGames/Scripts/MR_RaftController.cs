using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MR_RaftController : MonoBehaviour
{
    public static MR_RaftController instance;

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
            MR_UIController.instance.GameOver();
        }
    }

    public void StartRaftMovement()
    {
        StartCoroutine(MoveDownRiver());
        raftHealth = 100;
        hitCounter = 0;

        foreach(GameObject flame in inactiveFlames)
        {
            flame.GetComponent<ParticleSystem>().Stop();
        }
    }

    private IEnumerator MoveDownRiver()
    {
        if (MR_GameController.instance.isGameRunning)
        {
            gameObject.transform.parent.Translate(Vector3.right * Time.deltaTime * speed);
            yield return null;
            StartCoroutine(MoveDownRiver());
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
            MR_UIController.instance.GameOver();
        }

        if (hitCounter > 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (inactiveFlames.Count != 0)
                {
                    ActivateFlame();
                }
                else if(activeFlames.Count != 0)
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
                else if(inactiveFlames.Count != 0)
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
