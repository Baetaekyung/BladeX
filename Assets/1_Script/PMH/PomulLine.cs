using UnityEngine;

namespace Swift_Blade
{
    public class PomulLine : MonoBehaviour
    {
        [SerializeField] private Vector3 startPos;// = new Vector3(0, 0, 0); // A ����
        [SerializeField] private Vector3 endPos;// = new Vector3(10, 0, 0); // B ����
        float gravity = -9.81f;

        private Vector3 velocity;
        private float time;

        // ���� �Ÿ��� ���� ���� ���
        public void Completexydist()
        {
            float distance = Vector3.Distance(new Vector3(startPos.x, 0, startPos.z), new Vector3(endPos.x, 0, endPos.z));
            float heightDifference = endPos.y - startPos.y;
            float initialVelocity = Mathf.Sqrt((gravity * distance * distance) / (2 * (heightDifference - Mathf.Tan(45f * Mathf.Deg2Rad) * distance)));
            Vector3 velocity = new Vector3((endPos.x - startPos.x) / distance, Mathf.Tan(45f * Mathf.Deg2Rad),
                (endPos.z - startPos.z) / distance) * initialVelocity;
        }

        private void OnEnable()
        {
            startPos = transform.position;
            endPos = Player.Instance.transform.GetChild(0).position;
        }
        void Start()
        { 
            // �ʱ� �ӵ� ����
            velocity = CalculateInitialVelocity(startPos, endPos, gravity);
            time = 0f;
        }

        void Update()
        {
            // �ð� ������Ʈ
            time += Time.deltaTime;

            // ���ο� ��ġ ���
            Vector3 newPos = startPos + velocity * time + 0.5f * new Vector3(0, gravity, 0) * time * time;

            // ��ü ��ġ ������Ʈ
            transform.position = newPos;

            // ��ǥ ������ �����ϸ� ����
            if (Vector3.Distance(transform.position, endPos) < 0.1f)
            {
                Debug.Log("����!");
                enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
            }
        }

        Vector3 CalculateInitialVelocity(Vector3 start, Vector3 end, float gravity)
        {
            float distance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
            float heightDifference = end.y - start.y;
            float initialVelocity = Mathf.Sqrt((gravity * distance * distance) / (2 * (heightDifference - Mathf.Tan(45f * Mathf.Deg2Rad) * distance)));
            return new Vector3((end.x - start.x) / distance, Mathf.Tan(45f * Mathf.Deg2Rad), (end.z - start.z) / distance) * initialVelocity;
        }
    }
}
