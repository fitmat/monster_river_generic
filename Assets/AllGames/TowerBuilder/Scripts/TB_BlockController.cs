using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TB_BlockController : MonoBehaviour
{
    [SerializeField] private Mesh[] apartmentTypes;
    [SerializeField] private Material[] apartmentTints;

    [SerializeField] private Material burnDown;

    private GameObject objectList;
    private int numberOfObjects;

    private void Awake()
    {
        objectList = transform.GetChild(2).gameObject;
        numberOfObjects = objectList.transform.childCount;
    }


    private void OnEnable()
    {
        // Randomly select new block type
        objectList.transform.GetChild(Random.Range(0, numberOfObjects)).gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        for(int i=0;i< numberOfObjects; i++)
        {
            objectList.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        // Add additional external force to falling block
        if (GetComponent<Rigidbody>().useGravity == true)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.down * 10f,ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play VFX and SFX of block falling
        try
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
            transform.GetChild(0).GetChild(2).gameObject.GetComponent<ParticleSystem>().Play();
        }
        catch
        {

        }
        TB_AudioManager.instance.PlayAudio("BlockFall");
        //if (gameObject.CompareTag(collision.gameObject.tag))
        //{
            StartCoroutine(StabalizeBlock(collision.gameObject));
        //}
    }

    // Function to stabalize block after falling
    private IEnumerator StabalizeBlock(GameObject lowerBlock)
    {
        //StartCoroutine(TB_GameController.instance.CameraShake());
        yield return new WaitForSeconds(5f);
        try
        {
            // Set block as kinematic and fix rotation to zero
            if (!gameObject.GetComponent<Rigidbody>().isKinematic)
            {
                gameObject.transform.localRotation = Quaternion.identity;
                //gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, lowerBlock.transform.localPosition.y + 3, gameObject.transform.localPosition.z);
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        catch
        {

        }
    }

    public IEnumerator BurnDown()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);

    }

}
