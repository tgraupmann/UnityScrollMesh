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
        public GameObject _mSkis = null;

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

            // move graphics to rigidbody position
            transform.position = _mRigidBody.transform.position;

            // adjust skis
            float x1 = pos.x - MeshManager.WIDTH * 0.5f;
            float x2 = pos.x + MeshManager.WIDTH * 0.5f;
            float height1 = _mTerrainData.GetHeight(x1);
            float height2 = _mTerrainData.GetHeight(x2);
            Vector2 p1 = new Vector2(x1, height1);
            Vector2 p2 = new Vector2(x2, height2);
            Vector2 v = p1 - p2;
            float angle = Vector2.Angle(Vector2.right, v);
            if (_mGraphics.transform.localRotation.eulerAngles.y < 180f)
            {
                //invert angle
                angle = -angle;
            }
            Quaternion newRot;
            if (height1 < height2)
            {
                newRot = Quaternion.Euler(180 - angle, 0, 0);
            }
            else
            {
                newRot = Quaternion.Euler(180 + angle, 0, 0);
            }

            _mSkis.transform.localRotation = Quaternion.Lerp(_mSkis.transform.localRotation,
                    newRot, 3 * Time.deltaTime);

            // adjust camera
            float height = _mTerrainData.GetHeight(transform.position.x);
            float delta = transform.position.y - height;
            _mCamera.orthographicSize = Mathf.Lerp(_mCamera.orthographicSize, Mathf.Clamp(delta*2, 10, 1000), Time.deltaTime);
        }
    }
}
