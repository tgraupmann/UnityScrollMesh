using UnityEngine;

namespace ScrollMesh
{
    public class Player : MonoBehaviour
    {
        public float _mMoveForce = 100f;
        public float _mJumpForce = 5f;
        public TerrainData _mTerrainData = null;
        public Rigidbody _mRigidBody = null;
        public GameObject _mGraphics = null;

        private void Start()
        {
            Vector3 pos = _mRigidBody.transform.position;
            pos.y = _mTerrainData.GetHeight(pos.x) + 5;
            _mRigidBody.transform.position = pos;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = transform.position;
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 force = -_mMoveForce * Vector3.right * Time.deltaTime;
                _mRigidBody.AddForce(force, ForceMode.VelocityChange);

                _mGraphics.transform.rotation = Quaternion.Lerp(
                    _mGraphics.transform.rotation,
                    Quaternion.Euler(0, 45, 0),
                    Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                Vector3 force = _mMoveForce * Vector3.right * Time.deltaTime;
                _mRigidBody.AddForce(force, ForceMode.VelocityChange);

                _mGraphics.transform.rotation = Quaternion.Lerp(
                    _mGraphics.transform.rotation,
                    Quaternion.Euler(0, -45, 0),
                    Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                _mRigidBody.AddForce(_mJumpForce * Vector3.up, ForceMode.VelocityChange);
            }

            transform.position = _mRigidBody.transform.position;
        }
    }
}
