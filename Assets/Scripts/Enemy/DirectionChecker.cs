using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DirectionChecker : MonoBehaviour
{
    enum DefaultFacingSprite
    {
        Left = -1,
        Right = 1
    }

    #region Serialized Fields
    [SerializeField] private ContactFilter2D contactFilter = default;
    [SerializeField] private DefaultFacingSprite defaultFacingSprite = DefaultFacingSprite.Right;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isTouchingWall;
    [SerializeField] private bool isTouchingCeiling;
    [SerializeField] private float checkDistance = 0.01f;

    #endregion

    private float distanceToGround;
    private float distanceToSide;
    private new BoxCollider2D collider;
    private Vector2 center => collider.bounds.center;
    private Vector2 wallCheckDirection => (int)defaultFacingSprite * transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    #region Properties

    public bool IsGrounded { get => isGrounded; }
    public bool IsTouchingWall { get => isTouchingWall; }
    public bool IsTouchingCeiling { get => isTouchingCeiling; }
    public Vector2 WallCheckDirection { get => wallCheckDirection;}
    
    #endregion

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        distanceToGround = collider.bounds.size.y / 2 + checkDistance;
        distanceToSide = collider.bounds.size.x / 2 + checkDistance;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        isGrounded = collider.Cast(-transform.up, contactFilter, new RaycastHit2D[2], checkDistance) > 0;
        isTouchingWall = collider.Raycast(wallCheckDirection, contactFilter, new RaycastHit2D[2], distanceToSide) > 0;
        isTouchingCeiling = collider.Raycast(transform.up, contactFilter, new RaycastHit2D[2], distanceToSide) > 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (collider != null)
        {
            float halfSizeX = collider.bounds.size.x / 2;
            float halfSizeY = collider.bounds.size.y / 2;
        
            Gizmos.color = Color.red;
            if(isTouchingCeiling) Gizmos.color = Color.green;
            Gizmos.DrawLine(center + (Vector2)(transform.up * halfSizeY), center + (Vector2)(transform.up * distanceToGround));
            Gizmos.color = Color.red;
            if(isGrounded) Gizmos.color = Color.green;
            Gizmos.DrawLine(center + (Vector2)(-transform.up * halfSizeY), center + (Vector2)(-transform.up * distanceToGround));
            Gizmos.color = Color.red;
            if(isTouchingWall) Gizmos.color = Color.green;
            Gizmos.DrawLine(center + (Vector2)(transform.right * halfSizeX), center + (Vector2)(transform.right * distanceToSide));
            Gizmos.DrawLine(center + (Vector2)(-transform.right * halfSizeX), center + (Vector2)(-transform.right * distanceToSide));
        }
    }
}