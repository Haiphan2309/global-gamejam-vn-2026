using GDC.Managers;
using System.Collections;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public float windStartTimeStamp;
    public float windingDuration;
    public float windingSpeed;
    public bool isWinding;

    public GameObject windVfx;

    private void Start()
    {
        isWinding = false;
        StartCoroutine(Cor_Wind());
    }

    private void Update()
    {
        for (int i = 0; i < FaceController.Instance.maskObjectContainer.childCount; i++)
        {
            MaskObject obj = FaceController.Instance.maskObjectContainer.GetChild(i).GetComponent<MaskObject>();
            if (isWinding && obj.itemType == MaskItemType.CucumberSlice)
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

        for (int i = 0; i < FaceController.Instance.maskObjectContainer.childCount; i++)
        {
            MaskObject obj = FaceController.Instance.maskObjectContainer.GetChild(i).GetComponent<MaskObject>();
            obj.ReCheck();
        }
    }
}
