using UnityEngine;

namespace ScrollMesh
{
    public class Player : MonoBehaviour
    {
        [System.NonSerialized]
        public bool _mCanJump = false;

        public float _mMoveForceAir = 10f;
        public float _mMoveForceGround = 1000f;
        public float _mJumpForce = 5f;
        public float _mTurnSpeed = 10f;
        public TerrainData _mTerrainData = null;
        public Rigidbody _mRigidBody = null;
        public GameObject _mGraphics = null;
        public Camera _mCamera = null;

        private void Start()
        {
            Vector3 pos = _mRigidBody.transform.position;
            pos.x = -MeshManager.WORLD_SIZE + 2;
            pos.y = _mTerrainData.GetHeight(pos.x) + 5;
            _mRigidBody.transform.position = pos;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = transform.position;
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 force;
                if (_mCanJump)
                {
                    force = -_mMoveForceGround * Vector3.right * Time.deltaTime;
                }
                else
                {
                    force = -_mMoveForceAir * Vector3.right * Time.deltaTime;
                }
                _mRigidBody.AddForce(force, ForceMode.VelocityChange);

                _mGraphics.transform.rotation = Quaternion.Lerp(
                    _mGraphics.transform.rotation,
                    Quaternion.Euler(0, 45, 0),
                    _mTurnSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                Vector3 force;
                if (_mCanJump)
                {
                    force = _mMoveForceGround * Vector3.right * Time.deltaTime;
                }
                else
                {
                    force = _mMoveForceAir * Vector3.right * Time.deltaTime;
                }
                _mRigidBody.AddForce(force, ForceMode.VelocityChange);

                _mGraphics.transform.rotation = Quaternion.Lerp(
                    _mGraphics.transform.rotation,
                    Quaternion.Euler(0, -45, 0),
                    _mTurnSpeed * Time.deltaTime);
            }

            if (_mCanJump &&
                Input.GetKey(KeyCode.Space))
            {
                _mRigidBody.AddForce(_mJumpForce * Vector3.up, ForceMode.VelocityChange);
            }

            float height = _mTerrainData.GetHeight(transform.position.x);
            if (_mRigidBody.transform.position.y < 0f)
            {
                Vector3 newPos = _mRigidBody.transform.position;
                newPos.y = height + 10;
                _mRigidBody.transform.position = newPos;
                _mRigidBody.velocity = Vector3.zero;
                return;
            }

            if ((_mRigidBody.transform.position.x + 4) > MeshManager.WORLD_SIZE)
            {
                Vector3 newPos = _mRigidBody.transform.position;
                newPos.x = -MeshManager.WORLD_SIZE + 2;
                newPos.y = _mTerrainData.GetHeight(newPos.x) + 10;
                _mRigidBody.transform.position = newPos;
                _mRigidBody.velocity = Vector3.zero;
                return;
            }

            transform.position = _mRigidBody.transform.position;

            float delta = transform.position.y - height;
            _mCamera.orthographicSize = Mathf.Lerp(_mCamera.orthographicSize, Mathf.Clamp(delta*2, 10, 1000), Time.deltaTime);
        }
    }
}
