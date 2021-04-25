using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    [SerializeField]
    private Vector3 Direction;

    private void Update()
    {
        Vector3 position = transform.position;
        position += Direction * Time.deltaTime;
        transform.position = position;
    }
}
