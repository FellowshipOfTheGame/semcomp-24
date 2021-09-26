
using UnityEngine;
using UnityEngine.UI;

namespace SubiNoOnibus.UI
{
    public class InstructionsMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private InstructionsCard instructions;
        [SerializeField] private TMPro.TextMeshProUGUI textMesh;
        [SerializeField] private TMPro.TextMeshProUGUI titleTextMesh;
        [SerializeField] private Image image;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button backButton;

        private int _index = 0;

        private void Start()
        {
            nextButton.onClick.AddListener(MoveNext);
            backButton.onClick.AddListener(MoveBack);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            
            if(instructions.TryGetAt(_index, out InstructionCard? card))
            {
                SetInstruction(card.Value);
            }
            UpdateInteractables();
        }

        public void MoveNext()
        {
            if (instructions.TryGetAt(_index + 1, out InstructionCard? card))
            {
                SetInstruction(card.Value);
                _index++;
            }
            UpdateInteractables();
        }

        public void MoveBack()
        {
            if (instructions.TryGetAt(_index - 1, out InstructionCard? card))
            {
                SetInstruction(card.Value);
                _index--;
            }
            UpdateInteractables();
        }

        private void UpdateInteractables()
        {
            if (_index <= 0)
            {
                backButton.interactable = false;
                nextButton.interactable = true;
            }
            else if(_index >= instructions.instructions.Length - 1)
            {
                backButton.interactable = true;
                nextButton.interactable = false;
            }
            else
            {
                backButton.interactable = true;
                nextButton.interactable = true;
            }
        }

        private void SetInstruction(in InstructionCard card)
        {
            titleTextMesh.SetText(card.title);
            textMesh.SetText(card.text);
            image.sprite = card.sprite;
        }
    }
}