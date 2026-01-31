using DG.Tweening;
using GDC.Utils;
using System.Collections;
using UnityEngine;

public class FaceController : Singleton<FaceController>
{
    [SerializeField] private Transform dotContainer;
    [SerializeField] private Transform pimpleContainer;
    [SerializeField] private Transform freckleContainer;
    public Transform maskObjectContainer;
    [SerializeField] LayerMask cucumberLayerMask;
    [SerializeField] Animator animator;

    [SerializeField] private Transform eye1;
    [SerializeField] private Transform eye2;
    [SerializeField] private float maxEyeOffset;

    Vector2 eye1OriginLocalPos, eye2OriginLocalPos;

    [SerializeField] private bool isCanSneeze;
    [SerializeField] private float sneezeTimesptamp;
    
    public int numberOfPimple = 2;

    // Start is called before the first frame update

    public bool isPimpleLegit, isFreckleLegit;
    Coroutine sneezeCor;
    [SerializeField] private GameObject stopSneezeVfx;

    public float dotweenYTarget = -10f;

    void Start()
    {
        transform.position = new Vector2(0.94f, dotweenYTarget);
        transform.DOMoveY(0, 1f).SetDelay(1f).OnComplete(() =>
        {
            GamePlayManager.Instance.StartLevel();
            MaskItemConveyorManager.Instance.OnCustomerArrived(numberOfPimple, numberOfPimple * 5);
        });

        eye1OriginLocalPos = eye1.localPosition;
        eye2OriginLocalPos = eye2.localPosition;

        animator.Play("Idle");

        if (isCanSneeze)
        {
            sneezeCor = StartCoroutine(Cor_Sneeze());
        }
    }

    IEnumerator Cor_Sneeze()
    {
        yield return new WaitForSeconds(sneezeTimesptamp);
        animator.Play("Sneeze");
        yield return new WaitForSeconds(2);
        RemoveAllMaskObject();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void PressOnNose()
    {
        stopSneezeVfx.SetActive(true);
        animator.Play("Idle");
        StopCoroutine(sneezeCor);
    }

    public int CalculateResult()
    {
        int overlapPointCount = 0;
        for (int i = 0; i < dotContainer.childCount; i++)
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

            Collider2D[] cols = Physics2D.OverlapCircleAll(pimpleContainer.GetChild(i).position, 0.2f);

            if (cols != null)
            {
                foreach (var col in cols)
                {
                    if (col)
                    {
                        MaskObject maskObject = col.GetComponent<MaskObject>();
                        if (maskObject)
                        {
                            var maskObjectType = maskObject.itemType;
                            if (maskObject.itemType == MaskItemType.AcnePatch && maskObject.GetState() == MaskObject.State.IDLE)
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
        RemoveAllMaskObject();
        //TODO: Call lose instant in system
    }

    public void Happy()
    {
        animator.Play("Happy");
        RemoveAllMaskObject();

        GameUtils.ClearAllChild(pimpleContainer);
    }

    void RemoveAllMaskObject()
    {
        for (int i = 0; i < maskObjectContainer.childCount; i++)
        {
            MaskObject obj = maskObjectContainer.GetChild(i).GetComponent<MaskObject>();
            obj.Disappear();
        }
    }
}
