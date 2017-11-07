
using UnityEngine;
using UnityEngine.UI;

namespace EnixanTest
{
    public class UICancelBuildWindow : UIWindowBase
    {
        #region Unity properties

        [SerializeField]
        private Button cancelBuildButton;

        #endregion

        protected override void Start()
        {
            base.Start();
            cancelBuildButton.onClick.AddListener(BuildManager.Instance.StopBuildUnit);
        }
    }
}
