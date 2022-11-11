using UnityEngine;

namespace ApocalipseZ
{
    public class FirstPersonCamera : MonoBehaviour
    {
        [Header("Transform")]
        public Transform characterBody;
		public Vector2 sensitivity = new Vector2(0.5f, 0.5f);
        public Vector2 smoothing = new Vector2(3, 3);

        private Vector2 clampInDegrees = new Vector2(360, 180);

        [SerializeField]private float rotationX = 0;
        [SerializeField]private float rotationY = 0;
        [SerializeField]private float angleYmin = -90;
        [SerializeField]private float angleYmax = 90;

        bool lockCursor;
       
        [HideInInspector]
        public Vector2 targetDirection;
        Animator animator;
        public LayerMask defaultLayer;

        private void Start ( )
        {
            animator = GetComponent<Animator> ( );
            defaultLayer = GetComponent<Camera> ( ).cullingMask;
        }
        public void ActiveCursor ( bool active )
        {
            lockCursor = active;
            Cursor.visible = active;
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        }
        public void UpdateCamera ( )
        {
            if ( lockCursor )
            {
                return;
            }

            Quaternion targetOrientation = Quaternion.Euler(targetDirection);

            rotationX += InputManager.Instance.GetMouseDelta ( ).x * sensitivity.x;
            rotationY += InputManager.Instance.GetMouseDelta ( ).y * sensitivity.y;

            rotationY = Mathf.Clamp ( rotationY , angleYmin , angleYmax );

            characterBody.localRotation = Quaternion.Euler ( 0 , rotationX , 0 );
            transform.localRotation = Quaternion.Euler ( -rotationY , 0 , 0 );

        }
     
        public void RemoveAudioListener ( )
        {
            Destroy ( GetComponent<AudioListener>());
        }

        public void CameraDeath ( )
        {
            animator.Play ( "CameraDeath" );
        }
        public void CameraAlive ( )
        {
            animator.Play ( "CameraAlive" );
        }

        private void OnDrawGizmos ( )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine (transform.position , transform.position + transform.forward * 1000 );
        }
    }
}