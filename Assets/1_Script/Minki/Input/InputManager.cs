using UnityEngine;

namespace Swift_Blade.Inputs
{
    public class InputManager : MonoSingleton<InputManager>
    {
        [SerializeField] private CustomInputSO _input;

        private Plane _plane;
        
        public Vector2 InputDirection => _input.Movement;
        public Vector2 MousePos => _input.MousePosition;
        public Vector2 MousePosWorld {
            get {
                _plane.SetNormalAndPosition(Vector3.up, transform.position);

                Ray ray = Camera.main.ScreenPointToRay(MousePos);
                if (_plane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    return hitPoint;
                }
                
                return Vector2.zero;
            }
        }

        protected override void Awake() {
            base.Awake();

            _plane = new Plane();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _input.ResetInputs();
        }

        private void HandleRoll() {
            if(PopupManager.Instance.IsRemainPopup)
                return;
        }
    }
}
