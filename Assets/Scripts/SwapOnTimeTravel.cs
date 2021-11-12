using SubiNoOnibus.Generation;
using UnityEngine;

namespace SubiNoOnibus
{
    public class SwapOnTimeTravel : MonoBehaviour
    {
        [SerializeField] private TimeTravelObjectsSet objectsSet;

        public GameObject PresentObject => objectsSet.PresentObject;
        public GameObject PastObject => objectsSet.PastObject;

        private Transform _transform;

        private void Start()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            TimeTravel.OnTimeTravel += Swap;
        }

        private void OnDisable()
        {
            TimeTravel.OnTimeTravel -= Swap;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Swap(object sender, OnTimeTravelEventArgs e)
        {

            GameObject newGameObject = e.Period switch
            {
                TimeTravel.Period.Present => PresentObject,
                TimeTravel.Period.Past => PastObject,
                _ => throw new System.ArgumentOutOfRangeException(nameof(e.Period), e.Period, null)
            };

            if (newGameObject != null)
            {
                GameObject obj = Instantiate(newGameObject, _transform.position, _transform.rotation, transform.parent);
                obj.transform.localScale = _transform.localScale;
                Generator.LoadedObjects.Add(obj);
            }

            Destroy(gameObject);
        }
    }
}