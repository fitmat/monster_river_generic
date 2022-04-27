using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform lookAt;
        [SerializeField] private float smoothAmount = 0.125f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);

        private void Start()
        {
            Vector3 desiredPostion = lookAt.position + new Vector3(0f, 2.5f, -2.5f);
            transform.position = Vector3.Lerp(transform.position, desiredPostion, Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.isGameStarted)
            {
                Vector3 desiredPostion = lookAt.position + offset;
                transform.position = Vector3.Lerp(transform.position, desiredPostion, smoothAmount);
            }
        }

    }
}
