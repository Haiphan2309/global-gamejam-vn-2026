using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField] private Transform dotContainer;
    [SerializeField] private Transform pimpleContainer;
    [SerializeField] private Transform freckleContainer;
    [SerializeField] LayerMask cucumberLayerMask;
    // Start is called before the first frame update

    public int testResult;
    public bool isPimpleLegit, isFreckleLegit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        testResult = CalculateResult();
        isPimpleLegit = CheckPimpleLegit();
        isFreckleLegit = CheckAllFrecklesLegit();
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
        for (int i = 0; i < pimpleContainer.childCount; i++)
        {
            if (!pimpleContainer.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }

            Collider2D col = Physics2D.OverlapCircle(pimpleContainer.GetChild(i).position, 0.2f, cucumberLayerMask);

            if (col)
            {
                return false;
            }
        }

        return true;
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
}
