using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hover : MonoBehaviour
{
    [SerializeField, Tooltip("Whether to apply the hover force in the global up direction or local up direction")] private bool applyLocalHoverForce;
    [SerializeField, Tooltip("Whether to apply the hover force in the global up direction or local up direction")] private bool applyGlobalHoverForce;
    [SerializeField, Tooltip("The height above the object below it that this object will try to hover.")] private float targetHeight = 3;
    public float TargetHeight => targetHeight;
    [SerializeField, Tooltip("How much vertical drag will this gameObject have. This helps to prevent excessive bouncy oscillation.")] private float verticalDragModifier = 1;
    private Rigidbody rb;
    
    /// <summary>
    /// The amount of upward force to apply to this gameobject
    /// </summary>
    private Vector3 LocalUpwardThrust
    {
        get
        {
            if(applyLocalHoverForce && rb)
            {
                Vector3 forceDirection = transform.up;
                RaycastHit hit = new RaycastHit();
                // if the gameobject is above a collider, apply a force to hover.
                if(Physics.Raycast(transform.position, -forceDirection, out hit,targetHeight * 2f, ~LayerMask.GetMask("Player")))
                {
                    float height = hit.distance;
                    return (Mathf.Abs(Physics.gravity.y) * targetHeight / height - rb.velocity.y * verticalDragModifier) * forceDirection;
                }
            }
            return Vector3.zero;
        }
    }
    
    private Vector3 GlobalUpwardThrust
    {
        get
        {
            if(applyGlobalHoverForce && rb)
            {
                Vector3 forceDirection = Vector3.up;
                RaycastHit hit = new RaycastHit();
                // if the gameobject is above a collider, apply a force to hover.
                if(Physics.Raycast(transform.position, -forceDirection, out hit,targetHeight * 2f, ~LayerMask.GetMask("Player")))
                {
                    float height = hit.distance;
                    return (Mathf.Abs(Physics.gravity.y) * targetHeight / height - rb.velocity.y * verticalDragModifier) * forceDirection;
                }
            }
            return Vector3.zero;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(LocalUpwardThrust + GlobalUpwardThrust);
    }
}
