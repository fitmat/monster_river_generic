using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RunnerGame
{
    public class Projection : MonoBehaviour
    {
        [SerializeField] float throwSpeed = 5f;
        private void Update()
        {
            transform.Translate(-Vector3.forward * throwSpeed * Time.deltaTime);
            Destroy(this.gameObject, 7f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
                Destroy(this.gameObject);
        }

    }
}
