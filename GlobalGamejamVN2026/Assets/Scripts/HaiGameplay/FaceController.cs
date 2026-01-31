using System.Collections;
using UnityEngine;

public class FaceController : Singleton<FaceController>
{
    [SerializeField] private Transform dotContainer;
    [SerializeField] private Transform pimpleContainer;
    [SerializeField] private Transform freckleContainer;
    [SerializeField] LayerMask cucumberLayerMask;
    [SerializeField] Animator animator;

    [SerializeField] private Transform eye1;
    [SerializeField] private Transform eye2;
    [SerializeField] private float maxEyeOffset;

    Vector2 eye1OriginLocalPos, eye2OriginLocalPos;

    // Start is called before the first frame update

    public int testResult;
    public bool isPimpleLegit, isFreckleLegit;
    void Start()
    {
        eye1OriginLocalPos = eye1.localPosition;
        eye2OriginLocalPos = eye2.localPosition;

        animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        testResult = CalculateResult();
        isPimpleLegit = CheckPimpleLegit();
        isFreckleLegit = CheckAllFrecklesLegit();

        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = 10;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector3 dir1 = mouseWorld - eye1.position;
        Vector2 offset1 = Vector2.ClampMagnitude(dir1, maxEyeOffset);
        eye1.localPosition = eye1OriginLocalPos + offset1;

        Vector3 dir2 = mouseWorld - eye2.position;
        Vector2 offset2 = Vector2.ClampMagnitude(dir2, maxEyeOffset);
        eye2.localPosition = eye2OriginLocalPos + offset2;
    }

    public int CalculateResult()
    {
        int overlapPointCount = 0;
        for(int i = 0; i< dotContainer.childCount; i++)
        {
            Collider2D col = Physics2D.OverlapCircle(dotContainer.GetChild(i).position, 0.2f, cucumberLayerMask);

            if (col)
            {
                overlapPointCount++;
            }
        }

        return (int)(overlapPointCount * 100 / (float)dotContainer.childCount);
    }

    public bool CheckPimpleLegit()
    {
        bool isLegit = true;
        for (int i = 0; i < pimpleContainer.childCount; i++)
        {
            if (!pimpleContainer.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }

            Collider2D col = Physics2D.OverlapCircle(pimpleContainer.GetChild(i).position, 0.2f);

            if (col)
            {
                MaskObject maskObject = col.GetComponent<MaskObject>();
                if (maskObject)
                {
                    var maskObjectType = maskObject.itemType;
                    if (maskObject.itemType == MaskItemType.AcnePatch)
                    {
                        pimpleContainer.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        isLegit = false;
                    }
                }
            }
        }

        return isLegit;
    }    

    public bool CheckAllFrecklesLegit()
    {
        for (int i = 0; i < freckleContainer.childCount; i++)
        {
            Collider2D col = Physics2D.OverlapCircle(freckleContainer.GetChild(i).position, 0.2f, cucumberLayerMask);

            if (!col)
            {
                return false;
            }
        }

        return true;
    }

    public void Angry()
    {
        StartCoroutine(Cor_CallLose()); 
    }

    IEnumerator Cor_CallLose()
    {
        yield return new WaitForSeconds(1);
        animator.Play("Angry");
        //TODO: Call lose in system
    }

    public void Happy()
    {
        animator.Play("Angry");
    }
}
