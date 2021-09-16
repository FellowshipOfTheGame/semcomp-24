using UnityEngine;

namespace SubiNoOnibus.UI
{
    public class PlayerRankUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI nameTextMesh;
        [SerializeField] private TMPro.TextMeshProUGUI scoreTextMesh;
        [SerializeField] private TMPro.TextMeshProUGUI positionTextMesh;
        
        [SerializeField] private GameObject personalMarker;

        public void SetStatus(string name, int score, int position)
        {
            nameTextMesh.SetText(name);
            scoreTextMesh.SetText(score.ToString());
            positionTextMesh.SetText(position.ToString());
        }

        public void IsPersonal(bool value)
        {
            personalMarker.SetActive(value);
        }

        public void IsDisplayed(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}