using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_HoleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FT_Fish"))
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            FT_AudioManager.instance.PlayAudio("FishIn");
        }
    }



    private IEnumerator SpawnFish()
    {
        transform.Rotate(0f, Random.Range(1, 13) * 30, 0f);
        yield return new WaitForSeconds(Random.Range(1f,2f));
        FT_ObjectPooler.instance.SpawnFromPool("Fish" + Random.Range(1, 7), transform.position, transform, Quaternion.identity);
        FT_AudioManager.instance.PlayAudio("FishOut");
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1f);
        //transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        //yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        StartCoroutine(SpawnFish());
    }
}
