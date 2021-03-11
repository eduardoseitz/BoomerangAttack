using UnityEngine;

public class BillboardBehaviour : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
