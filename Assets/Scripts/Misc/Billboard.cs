using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private BillboardType billboardType;
    private SpriteRenderer parentRenderer;
    private SpriteRenderer childRenderer;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    public float activationRadius = 80f; // Adjust this radius as needed

    private Vector3 originalRotation;
    /* private float distanceFromPlayer; */

    public enum BillboardType { LookAtCamera, CameraForward };
    
    private void Awake() {
        originalRotation = transform.rotation.eulerAngles;
    }
    private void Start() {
        parentRenderer = transform.GetComponentInChildren<SpriteRenderer>();
    }
    private void LateUpdate() {
        // Calculate the distance between the object and the player
        /* distanceFromPlayer = Vector3.Distance(transform.position, Camera.main.transform.position);  */

        /* manageCollider(); */
        billBoardObject();
        /* SetSortingLayerBasedOnDistance(); */
    }

    /* void manageCollider()
    {
        // Check if the player is within the activation radius
        if (distanceFromPlayer <= activationRadius)
        {
            GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            GetComponent<Collider2D>().enabled = false;
        }
    } */

    void billBoardObject()
    {
        /* if(Vector3.Distance(transform.position, Camera.main.transform.position) > activationRadius) return; */

        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform/* .position, Vector3.up */);
                break;
            case BillboardType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        /* Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; } */
    }
    
    /* void SetSortingLayerBasedOnDistance()
    {
        if (gameObject.tag == "Target"){
        SpriteRenderer[] childRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer childRenderer in childRenderers)
        {
            // Check if the child has a SpriteRenderer component
            if (childRenderer != null)
            {
                // Calculate the distance between the child and the target object
                float distance = Vector3.Distance(childRenderer.transform.position, Camera.main.transform.position);

                // Set the sorting order based on distance (lower distance = higher sorting order)
                int sortingOrder = Mathf.RoundToInt(-distance * 1000); // You can adjust the factor as needed

                // Set the sorting order
                childRenderer.sortingOrder = sortingOrder;
            }
        }
        }
    } */

    public void setSortingLayers(SpriteRenderer _childRenderer)
    {
        childRenderer = _childRenderer;
        // Find the SpriteRenderer components for both the parent and child objects.
        parentRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        // Ensure both SpriteRenderer components exist.
        if (parentRenderer != null && childRenderer != null)
        {
            // Set the child's sorting order to be one greater than the parent's sorting order.
            childRenderer.sortingOrder = parentRenderer.sortingOrder + 1;
        }
    }
}
