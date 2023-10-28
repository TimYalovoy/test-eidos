using Following;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Eyes 
{
    internal enum Side
    {
        LEFT = 0, 
        RIGHT = 1
    }

    public class Eye : MonoBehaviour, IFollowable
    {
        private float f_maxHorizontalConstraint = 62f;
        private float f_minHorizontalConstraint = -62f;

        private float f_maxVerticalConstraint = 50f;
        private float f_minVerticalConstraint = -70f;
        
        [SerializeField] private Side side;
        
        private Transform _target;
        private Quaternion _initialRotation;
        private bool _isFollowingForTarget = true;

        public event Action<Transform> BoundsIsReached;

        private void Awake()
        {
            _initialRotation = transform.rotation;
        }

        private void OnEnable()
        {
            
        }

        private void Start()
        {

        }

        public void FixedUpdate()
        {
            if (_isFollowingForTarget) LookAtTarget();
            else LookAtRandomPoint();
        }

        private void LookAtTarget()
        {
            var relativePos = transform.position - _target.position;

            var rotation = Quaternion.LookRotation(relativePos);

            // Vertical (rotation around transform.up (Y) vector)
            var verticallAngle = GetSignedAngle(relativePos.normalized, transform.up, transform.parent.up);
            if (verticallAngle > f_maxVerticalConstraint)
            {
                BoundsIsReached.Invoke(_target);
            }
            if (verticallAngle < f_minVerticalConstraint)
            {
                BoundsIsReached.Invoke(_target);
            }

            // Horizontal (rotation around transform.right (X) vector)
            var horizontalAngle = GetSignedAngle(relativePos.normalized, transform.right, transform.parent.right);
            if (horizontalAngle > f_maxHorizontalConstraint)
            {
                BoundsIsReached.Invoke(_target);
            }
            if (horizontalAngle < f_minHorizontalConstraint)
            {
                BoundsIsReached.Invoke(_target);
            }

            transform.rotation = rotation;
        }

        private float GetSignedAngle(Vector3 normal, Vector3 selfAxis, Vector3 parentAxis)
        {
            var angle = Vector3.Angle(selfAxis, parentAxis);
            var cross = Vector3.Cross(selfAxis, parentAxis);
            var dot = Vector3.Dot(normal, cross);
            var sign = Mathf.Sign(dot);
            var signedAngle = angle * sign;

            return signedAngle;
        }

        private void LookAtRandomPoint()
        {
            
        }

        public void Initialize()
        {
            if (side == Side.LEFT)
            {

            }
            else
            {

            }
        }

        public void FollowFor(Transform target)
        {
            _target = target;
        }

        public void ToggleFollowingLogic()
        {
            _isFollowingForTarget = !_isFollowingForTarget;
            transform.rotation = _initialRotation;
        }

        private void OnDrawGizmos()
        {
            if (_target == null) return;

            Gizmos.DrawLine(transform.position, _target.position);
        }
    }
}
