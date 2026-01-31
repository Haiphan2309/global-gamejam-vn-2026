using GDC.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public float windStartTimeStamp;
    public float windingDuration;
    public float windingSpeed;
    public bool isWinding;

    public GameObject windVfx;

    private GameObject maskItemContainer;

    private void Start()
    {
        isWinding = false;
        maskItemContainer = GameObject.FindGameObjectWithTag("CucumberContainer");
        StartCoroutine(Cor_Wind());
    }

    private void Update()
    {
        if (!isWinding || maskItemContainer == null)
        {
            return;
        }

        MaskObject[] allSlices = GetAllMaskItemInFace(maskItemContainer);

        foreach (var obj in allSlices)
        {
            if (obj.itemType == MaskItemType.CucumberSlice)
            {
                obj.transform.Translate(Vector3.left * windingSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator Cor_Wind()
    {
        yield return new WaitForSeconds(windStartTimeStamp);
        isWinding = true;
        SoundManager.Instance.PlaySound(AudioPlayer.SoundID.SFX_WIND);
        windVfx.SetActive(true);
        yield return new WaitForSeconds(windingDuration);
        isWinding = false;
        windVfx.SetActive(false);

        MaskObject[] allSlices = GetAllMaskItemInFace(maskItemContainer);

        foreach (var obj in allSlices)
        {
            obj.ReCheck();
        }
    }

    private MaskObject[] GetAllMaskItemInFace(GameObject container)
    {
        return container.GetComponentsInChildren<MaskObject>();
    }
}
