using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScrollMesh
{
    public class PlayerSphere : MonoBehaviour
    {
        public Player _mPlayer = null;

        void OnCollisionEnter(Collision collision)
        {
            _mPlayer._mMoveForce = 100f;
        }

        void OnCollisionExit(Collision collision)
        {
            _mPlayer._mMoveForce = 1f;
        }
    }
}
