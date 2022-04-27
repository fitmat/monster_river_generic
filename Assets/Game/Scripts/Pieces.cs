using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public enum PiecesType
    {
        none = -1,
        ramp = 0,
        longblock = 1,
        jump = 2,
        slide = 3,
        longblock2 = 4,
        longblock3 = 5,
        enemyAttacking = 6,
        enemyShield = 7
    }

    public class Pieces : MonoBehaviour
    {
        public PiecesType piecesType;
        public int visualIndex;
    }
}
