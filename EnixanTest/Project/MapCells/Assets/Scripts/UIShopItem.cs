using UnityEngine;
using UnityEngine.UI;

namespace EnixanTest
{
    public class UIShopItem : MonoBehaviour
    {
        #region Unity properties

        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private Text itemDescription;

        [SerializeField]
        private Button setButton;

        #endregion

        #region Private fields

        private Constants.UNITS_POOL _poolType;

        private UIWindowBase _parentWindow;

        #endregion

        #region Interface

        public void Init(string description, Sprite ico, Constants.UNITS_POOL poolType)
        {
            itemDescription.text = description;
            itemImage.sprite = ico;

            _poolType = poolType;
        }

        #endregion

        private void Start()
        {
            setButton.onClick.AddListener(Set);

            _parentWindow = UIMainController.GetWindowById(Constants.UI_SHOP_WINDOW);
        }

        private void Set()
        {
            var unit = PoolManager.Instance.Units_Pools[(int)_poolType].GetObject();
            _parentWindow.CloseWindow();
            BuildManager.Instance.StartBuildUnit(unit);
        }
    }
}
