using UnityEngine;

public class DeactivateIfWebGL : MonoBehaviour
{
#if UNITY_WEBGL
    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
#endif
}
