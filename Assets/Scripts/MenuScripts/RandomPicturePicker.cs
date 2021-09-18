using System;
using UnityEngine;
using UnityEngine.UI;

public class RandomPicturePicker : MonoBehaviour
{
    [Serializable]
    public struct RandomPicture : IComparable<RandomPicture>
    {
        public float Chance;
        public Sprite Picture;

        public int CompareTo(RandomPicture other)
        {
            return Chance < other.Chance ? -1 : 1;
        }
    }

    [SerializeField] private RandomPicture[] pictures;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        SetupArray();
    }

    private void SetupArray()
    {
        Array.Sort(pictures);

        float lastChance = 0f;
        for(int i = 0; i < pictures.Length; i++)
        {
            pictures[i].Chance += lastChance;
            lastChance = pictures[i].Chance;
        }
    }

    private void OnEnable()
    {
        _image.sprite = PickRandomPicture();
    }

    public Sprite PickRandomPicture()
    {
        if(pictures is null || pictures.Length == 0)
        {
            return null;
        }

        float roll = UnityEngine.Random.value;
        foreach(var randomPicture in pictures)
        {
            if(roll <= randomPicture.Chance)
            {
                return randomPicture.Picture;
            }
        }

        return null;
    }
}
