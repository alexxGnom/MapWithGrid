using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnixanTest
{
    public class UIWindowBase : MonoBehaviour
    {
        public string Id;

        protected virtual void Start()
        {
            CloseWindow();
        }

        #region Interface

        public virtual void OpenWindow() 
        {
            gameObject.SetActive(true);
        }

        public virtual void CloseWindow() 
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}
