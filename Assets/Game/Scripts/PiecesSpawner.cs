using RunnerGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesSpawner : MonoBehaviour
{
    [SerializeField] private PiecesType type;
    private Pieces currentPieces;

    int lastIndex;
    int amountOfObj = 0;

    public void Spawn()
    {

        
        switch (type)
        {
            case PiecesType.jump:
                amountOfObj = LevelGenerator.Instance.jumps.Count;
                break;
            case PiecesType.ramp:
                amountOfObj = LevelGenerator.Instance.ramps.Count;
                break;
            case PiecesType.slide:
                amountOfObj = LevelGenerator.Instance.slides.Count;
                break;
            case PiecesType.longblock:
                amountOfObj = LevelGenerator.Instance.longBlocks.Count;
                break;
            case PiecesType.longblock2:
                amountOfObj = LevelGenerator.Instance.longBlocks2.Count;
                break;
            case PiecesType.longblock3:
                amountOfObj = LevelGenerator.Instance.longBlocks3.Count;
                break;
        }

        currentPieces = LevelGenerator.Instance.GetPiece(type, Random.Range(0, amountOfObj));
        currentPieces.gameObject.SetActive(true);
        currentPieces.transform.SetParent(this.transform, false);
    }

    public void Despawn() 
    {
        currentPieces.gameObject.SetActive(false);
    }

}
