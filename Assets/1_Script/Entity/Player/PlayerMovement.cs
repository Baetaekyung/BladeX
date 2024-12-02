using UnityEngine;

namespace Swift_Blade
{
    public class PlayerMovement : PlayerComponentBase, IEntityComponentRequireInit
    {
        public Vector3 InputDirection { get; set; }
        private CharacterController controller;
        public void EntityComponentAwake(Entity entity)
        {
            controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }
        private void ApplyMovement()
        {
            float speed = 5 * Time.deltaTime;
            Vector3 result = InputDirection * speed;
            controller.Move(result);
        }
    }
}
