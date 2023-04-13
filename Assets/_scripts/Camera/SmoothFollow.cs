using UnityEngine;

namespace CameraUtils
{
    public class SmoothFollow : MonoBehaviour
    {
        public bool ZoomEnabled = true;
        // The target we are following
        public Transform target;
        // The distance in the x-z plane to the target
        [SerializeField]
        private float distance = 10.0f;
        //public bool IsKinematic = false;

        public float ZoomSensitivity = 0.25f;
        public float scrollSpeed = 1.0f;

        public Transform ControlledObject;

        //public LayerMask ContactMask;
        //[SerializeField] private float ObtaclesBumpingValue = 1f;

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        [SerializeField] private float AndroidDistance = 0.75f;

        // the height we want the camera to be above the target
        [SerializeField]
        private float height = 5.0f;
        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        [SerializeField]
        private float rotationDamping;
        [SerializeField]
        private float heightDamping;
        [SerializeField]
        private float distanceDamping;

        [SerializeField]
        private float MaxDistance = 30f;
        [SerializeField]
        private float MaxHeight = 30f;

        Vector2 f0start;
        Vector2 f1start;
        public float distanceCurrent = 0.0f;

        public float MinDistance = 0f;
        public float MinHeight = 0f;

        private float delatDistance = 0f;

        public void SetRotationDamp(float val)
        {
            rotationDamping = val;
        }

        public void SetTarget(Transform trf)
        {
            target = trf;
        }

        public void AdditionalDeltaDistance(float deltaDist)
        {
            delatDistance = deltaDist;
        }

        void Update()
        {
            if (!ZoomEnabled)
            {
                return;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount < 2)
            {
            f0start = Vector2.zero;
            f1start = Vector2.zero;
            }
        if (Input.touchCount == 2) 
        {
            //Zoom();
        }
            
        return;
#endif
#if  UNITY_STANDALONE || UNITY_EDITOR
            var d_dist = delatDistance * Time.smoothDeltaTime * scrollSpeed;
            var d_input = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

            distanceCurrent -= d_input + d_dist;
            float dH = d_input + d_dist;

            float k = (MaxHeight - MinHeight) / (MaxDistance - MinDistance);
            if (k > 1f)
            {
                Height -= dH;
                Distance -= dH * (1f / k);
                Height = Mathf.Clamp(Height, MinHeight, MaxHeight);
                Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
            }
            else
            {
                Height -= dH * k;
                Distance -= dH;
                Height = Mathf.Clamp(Height, MinHeight, MaxHeight);
                Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
            }
#endif
        }


        void Zoom()
        {
            //Debug.Log("SMouseOrbit Zoom enter - OK");
            if (f0start == Vector2.zero && f1start == Vector2.zero)
            {
                f0start = Input.GetTouch(0).position;
                f1start = Input.GetTouch(1).position;
            }

            Vector2 f0position = Input.GetTouch(0).position;
            Vector2 f1position = Input.GetTouch(1).position;

            //Debug.Log("INPUT EVENT: Zoom on : touch 1 " + f0position.ToString() + "touch 2 " + f0position.ToString());

            float dir = Mathf.Sign(Vector2.Distance(f1start, f0start) - Vector2.Distance(f0position, f1position));
            distanceCurrent += dir * ZoomSensitivity * Time.smoothDeltaTime * Vector3.Distance(f0position, f1position);
            float k = (MaxHeight - MinHeight) / (MaxDistance - MinDistance);
            if (k > 1f)
            {
                Height += dir * ZoomSensitivity * Time.smoothDeltaTime * Vector3.Distance(f0position, f1position);
                Distance += dir * ZoomSensitivity * Time.smoothDeltaTime * Vector3.Distance(f0position, f1position) * (1f / k);
            }
            else
            {
                Height += dir * ZoomSensitivity * Time.smoothDeltaTime * Vector3.Distance(f0position, f1position) * k;
                Distance += dir * ZoomSensitivity * Time.smoothDeltaTime * Vector3.Distance(f0position, f1position);
            }

            Height = Mathf.Clamp(Height, MinHeight, MaxHeight);
            Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            // Early out if we don't have a target
            if (!target)
                return;

            // Calculate the current rotation angles
            var wantedRotationAngle = target.eulerAngles.y;
            var wantedHeight = target.position.y + Height;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.smoothDeltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.smoothDeltaTime);

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = target.position;


#if UNITY_ANDROID && !UNITY_EDITOR
            transform.position -= currentRotation * Vector3.forward * Distance * AndroidDistance;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
            transform.position -= currentRotation * Vector3.forward * Distance;
#endif
            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Always look at the target
            transform.LookAt(target);

            if (ControlledObject)
            {
                ControlledObject.forward = transform.forward;
            }
        }
    }
}