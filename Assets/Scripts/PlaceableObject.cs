using UnityEngine;

namespace DefaultNamespace
{
    public class PlaceableObject : MonoBehaviour
    {
        public Transform InstantiationPoint;
        public GameObject ObjectToShoot;
        private bool IsBeingPlaced => _placeableData != null;
        private PlaceableData _placeableData;

        private void Update()
        {
            if (IsBeingPlaced)
            {
                FollowMouseCursor();
                return;
            }

            DoTheLogic();
        }

        private void DoTheLogic()
        {
            
        }

        private void FollowMouseCursor()
        {
            
        }

        public void Place(PlaceableData placeableData)
        {
        }
    }
}