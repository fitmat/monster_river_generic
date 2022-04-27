using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class LevelGenerator : MonoBehaviour
    {
        public static LevelGenerator Instance { get; set; }

        public bool SHOW_COLLIDERS = false;

        [SerializeField] private float spawnDistanceUpto = 100f;
        [SerializeField] private int initalSegmentsToSpawn = 10;
        [SerializeField] private int maximumNoOfSegmentsOnScreen = 15;
        private Transform cameraHolder;
        private int amountOfActiveSegments;
        private int continousSegments;
        private int currentSpawnZ;
        private int currentLevel;
        private int y1, y2, y3;

        public List<Pieces> ramps = new List<Pieces>();
        public List<Pieces> longBlocks = new List<Pieces>();
        public List<Pieces> longBlocks2 = new List<Pieces>();
        public List<Pieces> longBlocks3 = new List<Pieces>();
        public List<Pieces> jumps = new List<Pieces>();
        public List<Pieces> slides = new List<Pieces>();

        [HideInInspector] public List<Pieces> pieces = new List<Pieces>();

        public List<Segment> availableSegements = new List<Segment>();
        public List<Segment> availableTransitions = new List<Segment>();

        [HideInInspector] public List<Segment> segments = new List<Segment>();

        private bool isMoving = false;

        private void Awake()
        {
            Instance = this;
            cameraHolder = Camera.main.transform;
            currentSpawnZ = 0;
            currentLevel = 0;
        }

        private void Start()
        {
            for (int i = 0; i < initalSegmentsToSpawn; i++)
            {
                if (i < 2)
                    SpawnTransition();
                else
                    GenerateSegments();
            }
        }

        private void Update()
        {
            if (currentSpawnZ - cameraHolder.position.z < spawnDistanceUpto)
            {
                GenerateSegments();
            }
            if(amountOfActiveSegments >= maximumNoOfSegmentsOnScreen)
            {
                segments[amountOfActiveSegments - 1].Despawn();
                amountOfActiveSegments--;
            }
        }

        private void GenerateSegments()
        {

            SpawnSegments();

            if (Random.Range(0f, 1f) < (continousSegments * 0.25f))
            {
                continousSegments = 0;
                SpawnSegments();
            }
            else
            {
                continousSegments++;
            }
        }

        private void SpawnSegments()
        {
            List<Segment> possibleSegments = availableSegements.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
            int id = Random.Range(0, possibleSegments.Count);
            Segment s = GetSegment(id, false);

            y1 = s.endY1;
            y2 = s.endY2;
            y3 = s.endY3;

            s.transform.SetParent(this.transform);
            s.transform.localPosition = Vector3.forward * currentSpawnZ;

            currentSpawnZ += s.length;
            amountOfActiveSegments++;
            s.Spawn();
        }

        private void SpawnTransition()
        {
            List<Segment> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
            int id = Random.Range(0, possibleTransition.Count);
            Segment s = GetSegment(id, true);

            y1 = s.endY1;
            y2 = s.endY2;
            y3 = s.endY3;

            s.transform.SetParent(this.transform);
            s.transform.localPosition = Vector3.forward * currentSpawnZ;

            currentSpawnZ += s.length;
            amountOfActiveSegments++;
            s.Spawn();
        }

        public Segment GetSegment(int id, bool transition)
        {
            Segment s = null;
            s = segments.Find(x => x.SegmentId == id && x.transition == transition && !x.gameObject.activeSelf);

            if (s == null)
            {
                GameObject gameObject = null;
                gameObject = Instantiate(
                    (transition) ? availableTransitions[id].gameObject : availableSegements[id].gameObject
                    ) as GameObject;

                s = gameObject.GetComponent<Segment>();
                s.SegmentId = id;
                s.transition = transition;
                segments.Insert(0, s);
            }
            else
            {
                segments.Remove(s);
                segments.Insert(0, s);
            }

            return s;
        }

        public Pieces GetPiece(PiecesType piecesType, int visualIndex)
        {
            Pieces pieces = this.pieces.Find(x => x.piecesType == piecesType &&
                x.visualIndex == visualIndex &&
                !x.gameObject.activeSelf
            );

            if (pieces == null)
            {
                GameObject gameObject = null;

                if (piecesType == PiecesType.ramp)
                    gameObject = ramps[visualIndex].gameObject;
                else if (piecesType == PiecesType.longblock)
                    gameObject = longBlocks[visualIndex].gameObject;
                else if (piecesType == PiecesType.longblock2)
                    gameObject = longBlocks2[visualIndex].gameObject;
                else if (piecesType == PiecesType.longblock3)
                    gameObject = longBlocks3[visualIndex].gameObject;
                else if (piecesType == PiecesType.jump)
                    gameObject = jumps[visualIndex].gameObject;
                else if (piecesType == PiecesType.slide)
                    gameObject = slides[visualIndex].gameObject;
                else if (piecesType == PiecesType.none)
                    gameObject = slides[visualIndex].gameObject;

                gameObject = Instantiate(gameObject);
                pieces = gameObject.GetComponent<Pieces>();
                this.pieces.Add(pieces);

            }

            return pieces;
        }

    }
}
