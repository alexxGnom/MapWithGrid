using System;
using UnityEngine;
using UnityEngine.UI;

namespace EnixanTest
{
    public class BaseUnit : MonoBehaviour, IUnityPoolable
    {
        #region Unity properties

        [SerializeField]
        private string description;

        [SerializeField]
        private Sprite unitIcon;

        [SerializeField]
        private Constants.UNITS_POOL poolType;

        [SerializeField]
        private int width = 1;

        [SerializeField]
        private int heigh = 1;

        #endregion

        #region Public properties

        public string Description { get { return description; } }

        public int Id { get; private set;}

        public Sprite Ico { get { return unitIcon; } }

        public int Width { get { return width; } }
        public int Heigh { get { return heigh; } }

        #endregion

        #region Interface

        public override string ToString()
        {
            return string.Format("This is {0} with ID {1}. Width = {2}, Heigh = {3}", Description, Id, Width, Heigh);
        }

        public Constants.UNITS_POOL GetPoolType()
        {
            return poolType;
        }

        #endregion

        #region IUnityPoolable

        public Action DespawnAction { get; set; }

        public BaseUnit Spawn(Transform parent, Vector3 pos)
        {
            transform.SetParent(parent, false);
            transform.localPosition = pos;

            return this;
        }

        public void Despawn()
        {
            if (DespawnAction != null)
                DespawnAction();
            else
                Destroy(gameObject);
        }

        public void OnPoolEnter() { }

        public void OnPoolExit() { }

        #endregion

        private void Start()
        {
            Id = gameObject.GetHashCode();
        }

    }
}
