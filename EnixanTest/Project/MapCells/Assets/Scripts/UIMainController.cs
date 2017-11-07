using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EnixanTest
{
    public class UIMainController : MonoSingleton<UIMainController>
    {
        #region Unity properties

        [SerializeField]
        private UIWindowBase[] _windows;

        #endregion

        #region Interface

        public static UIWindowBase GetWindowById(string id)
        {
            foreach (var w in Instance._windows)
            {
                if (w.Id == id)
                    return w;
            }
            return null;
        }

        #endregion
    }
}
