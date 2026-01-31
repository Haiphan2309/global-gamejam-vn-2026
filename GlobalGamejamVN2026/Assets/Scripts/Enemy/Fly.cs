using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

public class Fly : MonoBehaviour
{
    [Header("Settings")]
    public float flySpeed = 5f;
    public float eatTime = 3f;

    private bool isEating = false;
    private bool isDragging = false;
    private Rigidbody2D rb;

    private Vector3 lastPosition;
    private Vector3 throwVelocity;
    public MaskObject targetSlice;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        TryToFindTarget();
    }

    void TryToFindTarget()
    {
        if (targetSlice != null) return;

        GameObject container = GameObject.FindGameObjectWithTag("CucumberContainer");
        if (container == null) return;

        MaskObject[] allSlices = container.GetComponentsInChildren<MaskObject>();

        List<MaskObject> availableSlices = allSlices.Where(s => !s.isTargeted && s.itemType == MaskItemType.CucumberSlice).ToList();

        if (availableSlices.Count > 0)
        {
            targetSlice = availableSlices[Random.Range(0, availableSlices.Count)];
            targetSlice.isTargeted = true;
        }
    }

    void Update()
    {
        if (isDragging) return;

        if (targetSlice == null)
        {
            TryToFindTarget();
        }

        if (targetSlice != null && !isEating)
        {
            Vector3 direction = targetSlice.transform.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetSlice.transform.position, flySpeed * Time.deltaTime);

            RotateTowardsTarget(direction);

            if (Vector3.Distance(transform.position, targetSlice.transform.position) < 0.1f)
            {
                StartCoroutine(EatRoutine());
            }
        }
    }

    void RotateTowardsTarget(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    IEnumerator EatRoutine()
    {
        isEating = true;

        yield return new WaitForSeconds(eatTime); // Chờ 3s

        if (targetSlice != null)
        {
            targetSlice.GetEaten();
            targetSlice = null;    
        }

        isEating = false; 
        
        TryToFindTarget();
    }

    void OnMouseDown()
    {
        isDragging = true;
        lastPosition = transform.position;
        StopAllCoroutines(); 
        isEating = false; 
        
        if (targetSlice != null) 
        {
            targetSlice.isTargeted = false; 
            targetSlice = null;
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        throwVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    void OnMouseUp()
    {
        isDragging = false;
        rb.velocity = throwVelocity * 0.5f;
        rb.angularVelocity = 720f;
        Destroy(gameObject, 2f);
    }
}