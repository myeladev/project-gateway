using System;
using ProjectGateway.UI;
using UnityEngine;

namespace ProjectGateway.Logic
{
    public class GrabController : MonoBehaviour
    {
        public Camera cam;
        public float spring = 800f;
        public float damper = 60f;
        public float maxDistance = 0.05f;

        Rigidbody anchorRb; // kinematic proxy that follows the mouse
        ConfigurableJoint joint;
        float grabDistance;

        void Awake()
        {
            var go = new GameObject("GrabAnchor");
            anchorRb = go.AddComponent<Rigidbody>();
            anchorRb.isKinematic = true;
            anchorRb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        private void FixedUpdate()
        {
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            var target = ray.origin + ray.direction * grabDistance;

            // Smooth towards target to avoid huge teleports
            var maxSpeed = 12f; // m/s
            var desired = target - anchorRb.position;
            var step = Vector3.ClampMagnitude(desired, maxSpeed * Time.fixedDeltaTime);
            anchorRb.MovePosition(anchorRb.position + step);
        }

        void Update()
        {
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Input.GetMouseButtonDown(0) && !OptionsUI.Instance.IsViewingOptions)
            {
                if (Physics.Raycast(ray, out var hit, 1000f) && hit.rigidbody)
                {
                    grabDistance = hit.distance;
                    anchorRb.position = hit.point;

                    joint = hit.rigidbody.gameObject.AddComponent<ConfigurableJoint>();
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedBody = anchorRb;

                    // anchor on the object at the hit point (in object local space)
                    joint.anchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                    joint.connectedAnchor = Vector3.zero;

                    var jointDrive = new JointDrive() { positionSpring = spring, positionDamper = damper, maximumForce = (float)6e4 };
                    joint.xDrive = jointDrive;
                    joint.yDrive = jointDrive;
                    joint.zDrive = jointDrive;
                    joint.angularXDrive = jointDrive;
                    joint.angularYZDrive = jointDrive;
                    joint.xMotion = ConfigurableJointMotion.Limited;
                    joint.yMotion = ConfigurableJointMotion.Limited;
                    joint.zMotion = ConfigurableJointMotion.Limited;
                    joint.linearLimit = new SoftJointLimit() { limit = 0.03f };
                    joint.projectionMode = JointProjectionMode.PositionAndRotation;
                    
                    joint.breakForce = 50000;

                    // extra stability
                    //hit.rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    //hit.rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (joint)
                {
                    Destroy(joint);
                    joint = null;
                }
                grabDistance = 0;
            }
        }
    }
}
