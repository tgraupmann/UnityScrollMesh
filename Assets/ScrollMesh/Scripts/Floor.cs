using UnityEngine;

namespace ScrollMesh
{
    public class Floor : MonoBehaviour
    {
        public float _mHeight = 0f;

        void OnCollisionEnter(Collision collision)
        {
            Vector3 pos = collision.gameObject.transform.position;
            pos.y = _mHeight + 0.5f;
            collision.gameObject.transform.position = pos;
        }
    }
}
