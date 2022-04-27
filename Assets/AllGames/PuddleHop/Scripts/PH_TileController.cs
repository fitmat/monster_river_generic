using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script handles spawning and movement of tiles

public class PH_TileController : MonoBehaviour
{
    private PH_PlayerController currentPlayer;

    public bool isTileOccupied;
    private int currentPlayerNumber;

    public bool isCrackedTile;

    private int direction;
    private float speed;

    private float startX, endX, currentX;

    private void Update()
    {

        // Check current position of tile and despawn if out of bounds
        currentX = transform.localPosition.x;
        if ((endX > startX && currentX > endX) || (endX < startX && currentX < endX))
        {
            gameObject.SetActive(false);
        }
    }

    // Function Spawn tile with values from parent lane
    public void SpawnTile(int Direction, float Speed, float RightX, float LeftX)
    {
        speed = Speed;
        direction = Direction;

        // Mark start and endpoints of tile
        if (direction == 1)
        {
            startX = LeftX;
            endX = RightX;
        }
        else
        {
            startX = RightX;
            endX = LeftX;
        }

        transform.localPosition = new Vector3(startX, transform.localPosition.y, transform.localPosition.z);
        StartCoroutine(MoveTile());
    }


    private IEnumerator MoveTile()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        yield return null;

        StartCoroutine(MoveTile());
    }

    // Function to handle player landing on tile
    public void LandOnTile(PH_PlayerController newPlayer, int newPlayerNumber)
    {
        Debug.Log("Land On Tile");
        // Check if tile is occupied
        if (currentPlayer != null)
        {
            // Check if new player is on tile
            if (isTileOccupied && currentPlayerNumber != newPlayerNumber)
            {
                // Make first player jump off
                StartCoroutine(currentPlayer.GetPushed());
            }
        }
        // Set new player as current player on tile
        currentPlayer = newPlayer;
        currentPlayerNumber = newPlayerNumber;

        // Set tile as not occupied
        isTileOccupied = true;

        if (isCrackedTile)
        {
            StartCoroutine(DropTile());
        }
    }

    // Function to handle player leaving tile
    public void LeaveTile()
    {
        // Set tile as not occupied
        currentPlayer = null;
        currentPlayerNumber = 0;
        isTileOccupied = false;
    }
    
    public IEnumerator DropTile()
    {
        // Play tile wobbling animation
        gameObject.GetComponentInChildren<Animator>().SetTrigger("Wobble");
        yield return new WaitForSeconds(1.5f);
        try
        {
            // If player is still on tile make player fall
            if (isTileOccupied)
            {
                StartCoroutine(currentPlayer.OnCrackedTile());
            }
        }
        catch
        {

        }
        yield return new WaitForSeconds(0.2f);
        // Cause tile to fall via gravity
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.SetActive(false);
    }
}

