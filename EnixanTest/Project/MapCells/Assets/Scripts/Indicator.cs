using System;
using UnityEngine;

namespace EnixanTest
{
    public class Indicator : MonoBehaviour, IUnityPoolable
    {
        #region Private fields

        private MeshRenderer _meshRenderer;

        #endregion

        #region Interface

        public void ShowBusy()
        {
            _meshRenderer.material.color = Color.red;
        }

        public void ShowEmpty()
        {
            _meshRenderer.material.color = Color.gray;
        }

        #endregion

        #region IUnityPoolable

        public Action DespawnAction { get; set; }

        public Indicator Spawn(Transform parent, Vector3 pos)
        {
            transform.SetParent(parent, false);
            transform.position = pos;

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

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}
