using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class Segment : MonoBehaviour
    {
        public int SegmentId { get; set; }
        public bool transition;

        public int length;
        public int beginY1, beginY2, beginY3;
        public int endY1, endY2, endY3;

        private PiecesSpawner[] pieces;

        private void Awake()
        {
            pieces = this.gameObject.GetComponentsInChildren<PiecesSpawner>();

            if (LevelGenerator.Instance.SHOW_COLLIDERS)
            {
                for (int i = 0; i < pieces.Length; i++)
                {
                    foreach (MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
                    {
                        mr.enabled = LevelGenerator.Instance.SHOW_COLLIDERS;
                    }
                }
            }
        }

        public void Spawn()
        {
            this.gameObject.SetActive(true);
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].Spawn();
            }
        }

        public void Despawn()
        {
            this.gameObject.SetActive(false);
            for (int i = 0; i < pieces.Length; i++)
            {
                pieces[i].Despawn();
            }
        }
    }
}
