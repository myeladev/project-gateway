using UnityEngine;

namespace ProjectGateway
{
    public class GrabController : MonoBehaviour
    {
        public Camera cam;
        public float spring = 800f;
        public float damper = 60f;
        public float maxDistance = 0.05f;

        Rigidbody anchorRb; // kinematic proxy that follows the mouse
        SpringJoint joint;
        float grabDistance;

        void Awake()
        {
            var go = new GameObject("GrabAnchor");
            anchorRb = go.AddComponent<Rigidbody>();
            anchorRb.isKinematic = true;
            anchorRb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        void Update()
        {
            // Place the anchor at the mouse ray distance (or a plane if you prefer)
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 target = ray.origin + ray.direction * (grabDistance > 0 ? grabDistance : 5f);
            anchorRb.MovePosition(target);

            if (Input.GetMouseButtonDown(0) && !OptionsUI.Instance.IsViewingOptions)
            {
                if (Physics.Raycast(ray, out var hit, 1000f) && hit.rigidbody)
                {
                    grabDistance = hit.distance;

                    joint = hit.rigidbody.gameObject.AddComponent<SpringJoint>();
                    joint.autoConfigureConnectedAnchor = false;
                    joint.connectedBody = anchorRb;

                    // anchor on the object at the hit point (in object local space)
                    joint.anchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                    joint.connectedAnchor = Vector3.zero;

                    joint.spring = spring;
                    joint.damper = damper;
                    joint.maxDistance = maxDistance;

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
