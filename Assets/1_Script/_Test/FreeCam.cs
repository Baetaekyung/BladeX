using UnityEngine;

namespace Swift_Blade
{
    public class FreeCam : MonoBehaviour
    {
        private float xRotation;
        private float yRotation;
        private void Update()
        {
            const float mouseSensitivitiy = 0.5f;
            xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivitiy;
            yRotation += Input.GetAxis("Mouse X") * mouseSensitivitiy;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float y = Input.GetKey(KeyCode.E) ? 1 :
                Input.GetKey(KeyCode.Q) ? -1 : 0;
            y *= 0.5f;

            Vector3 input = transform.rotation * new Vector3(horizontal, y, vertical);

            float speed = 5 * Time.deltaTime;
            transform.Translate(input * speed, Space.World);
        }
    }
}
