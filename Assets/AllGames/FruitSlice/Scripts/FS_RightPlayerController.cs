using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FS_RightPlayerController : MonoBehaviour
{
    public static FS_RightPlayerController instance;

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


    // Spawns a dart, ready to throw
    private IEnumerator SpawnDart()
    {
        yield return new WaitForSeconds(1f);
        // If no dart is currently ready, spawn new dart
        if (!isDartReady)
        {
            isDartReady = true;
            dartObject = Instantiate(dartPrefab, transform);
        }
        StartCoroutine(SpawnDart());
    }

    // Throw dart
    public void ThrowDart()
    {
        if (isDartReady && FS_GameController.instance.gameState == FS_GameController.GameStates.playing)
        {
            FS_AudioManager.instance.PlayAudio("KnifeThrow");
            // Throw dart by appling force
            dartObject.GetComponent<Rigidbody>().AddForce(dartObject.transform.rotation * Vector3.down * throwForce, ForceMode.Impulse);
            // Set state of dart as thrown
            dartObject.GetComponent<FS_DartController>().isThrown = true;
            dartObject.GetComponent<FS_DartController>().dartOwner = 2;
            // Enable collider of dart
            dartObject.GetComponent<BoxCollider>().isTrigger = false;
            // Set state as no current dart is ready
            isDartReady = false;
        }
    }
}
