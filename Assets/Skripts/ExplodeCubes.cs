using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject RestartButton;
    private bool _collisionSet;
    private void OnCollisionEnter(Collision collision) // ���� ���-�� �������������� � ground ��
    {
        if (collision.gameObject.tag == "Cube" && !_collisionSet) // ���� ��� = ��� �����
        {
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i); // ����������� ����� ������ � �������� i
                child.gameObject.AddComponent<Rigidbody>();// ��������� rugidbody
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up,5f);// ������ ���� ����������
                child.SetParent(null);  // ��������� ����� ����� ������(������� ��� �� AllCubes)
            }
            Destroy(collision.gameObject);
            _collisionSet = true;

            RestartButton.SetActive(true);
            
        }
    }
}
