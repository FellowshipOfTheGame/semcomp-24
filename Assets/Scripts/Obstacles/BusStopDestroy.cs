using UnityEngine;

public class BusStopDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject healingArea;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            healingArea.SetActive(false);
        }
    }
}
