using UnityEngine;

namespace SubiNoOnibus.UI
{
    public class CreditsMenu : MonoBehaviour, IMenu
    {
        public void Close()
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void Open()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}

