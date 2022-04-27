using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class MovingObstacle : MonoBehaviour
    {
        [SerializeField] private float[] speedRanges;
        private float speed;
        public Vector3 position;
        [HideInInspector] public bool move = false;

        private void Start()
        {
            speed = speedRanges[Random.Range(0, speedRanges.Length)];
        }

        private void OnEnable()
        {
            if (position == null || position == Vector3.zero)
                position = transform.position;
            this.gameObject.transform.position = position;
            move = false;
        }

        private void Update()
        {
            if (!move)
                return;

            if(this.gameObject.activeSelf)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

            StartCoroutine(disableObject());
        }

        private IEnumerator disableObject()
        {
            yield return new WaitForSecondsRealtime(10f);
            this.gameObject.SetActive(false);
        }

    }
}
