using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_LeftPlayerController : MonoBehaviour
{
    public static FS_LeftPlayerController instance;

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

    public bool isDartReady;
    private GameObject dartObject;
    public GameObject dartPrefab;
    public int throwForce;

    // Start is called before the first frame update
    void Start()
    {
        isDartReady = false;
        StartCoroutine(SpawnDart());
    }

    private IEnumerator SpawnDart()
    {
        yield return new WaitForSeconds(1f);
        if (!isDartReady)
        {
            isDartReady = true;
            dartObject = Instantiate(dartPrefab, transform);
        }
        StartCoroutine(SpawnDart());
    }

    public void ThrowDart()
    {
        if (isDartReady && FS_GameController.instance.gameState == FS_GameController.GameStates.playing)
        {
            FS_AudioManager.instance.PlayAudio("KnifeThrow");
            dartObject.GetComponent<Rigidbody>().AddForce(dartObject.transform.rotation * Vector3.down * throwForce, ForceMode.Impulse);
            dartObject.GetComponent<FS_DartController>().isThrown = true;
            dartObject.GetComponent<FS_DartController>().dartOwner = 1;
            dartObject.GetComponent<BoxCollider>().isTrigger = false;
            isDartReady = false;
        }
    }
}
