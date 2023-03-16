using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    public GameObject RestartButton;
    private bool _collisionSet;
    private void OnCollisionEnter(Collision collision) // если что-то соприкоснулось с ground то
    {
        if (collision.gameObject.tag == "Cube" && !_collisionSet) // если тег = куб тогда
        {
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = collision.transform.GetChild(i); // присваеваем чаилд кубику с индексом i
                child.gameObject.AddComponent<Rigidbody>();// добавляем rugidbody
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up,5f);// задаем силу разрушения
                child.SetParent(null);  // разрываем связь между кубами(удаляем его из AllCubes)
            }
            Destroy(collision.gameObject);
            _collisionSet = true;

            RestartButton.SetActive(true);
            
        }
    }
}
