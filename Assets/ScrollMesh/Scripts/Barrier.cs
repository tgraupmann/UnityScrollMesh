using UnityEngine;

namespace ScrollMesh
{
    public class Barrier : MonoBehaviour
    {
        public Transform _mSpawnPoint = null;

        void OnCollisionEnter(Collision collision)
        {
            collision.gameObject.transform.position = _mSpawnPoint.transform.position;
        }
    }
}
