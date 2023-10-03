using UnityEngine;

namespace ApocalipseZ
{
    public class Sway : MonoBehaviour
    {
        [Tooltip("Sway smooth amount")]
        public float smooth = 2.0f;

        public float AmountX = 0.1f;
        public float AmountY = 0.1f;

        [Tooltip("Max allowed value for sway amount")]
        public float MaxX = 0.35f;
        [Tooltip("Max allowed value for sway amount")]
        public float MaxY = 0.2f;

        public float startX, startY;

        //Local position of an object on start used for further calculations
        private Vector3 localPos;
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if (PInputManager == null)
                {
                    PInputManager = GameController.Instance.InputManager;
                }
                return PInputManager;
            }
        }
        void Start()
        {
            localPos = transform.localPosition;
            startX = AmountX;
            startY = AmountY;
        }

        void Update()
        {

            if (PlayerController.IsInventoryOpen)
            {
                return;
            }
            float fx = new float(), fy = new float();

            //Sway coefficient got from mouse input multyplied on swayAmmount for each axis

            fx = -InputManager.GetMouseDelta().x * AmountX;
            fy = -InputManager.GetMouseDelta().y * AmountY;



            if (fx > MaxX)
                fx = MaxX;

            if (fx < -MaxX)
                fx = -MaxX;

            if (fy > MaxY)
                fy = MaxY;

            if (fy < -MaxY)
                fy = -MaxY;

            //Calculating sway vector 
            Vector3 swayVector = new Vector3(localPos.x + fx, localPos.y + fy, localPos.z);

            //Applying sway Vector to object local position
            transform.localPosition = Vector3.Lerp(transform.localPosition, swayVector, Time.deltaTime * smooth);
        }

    }
}
