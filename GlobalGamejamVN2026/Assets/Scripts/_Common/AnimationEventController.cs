using UnityEngine;
using UnityEngine.Events;

public class AnimationEventController : MonoBehaviour
{
    [SerializeField] private UnityEvent onFunc;

    public void OnFuncEvent() //this was called by animation event
    {
        onFunc?.Invoke();
    }
}
