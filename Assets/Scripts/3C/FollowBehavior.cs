using UnityEngine;

public class FollowBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform FollowTarget;
    [SerializeField]
    private Vector3 Offset;
    [SerializeField]
    private float FollowSpeed;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, FollowTarget.position + Offset, FollowSpeed * Time.deltaTime);
    }
}
