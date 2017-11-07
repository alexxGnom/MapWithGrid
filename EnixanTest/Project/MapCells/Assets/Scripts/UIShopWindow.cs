using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EnixanTest
{
    public class UIShopWindow : UIWindowBase
    {
        #region Unity properties

        [SerializeField]
        private UIShopItem itemPrototype;

        [SerializeField]
        private Transform itemContainer;

        [SerializeField]
        private BaseUnit[] shopUnits;

        #endregion

        protected override void Start()
        {
            base.Start();
            GenerateItems();
        }

        private void GenerateItems()
        {
            foreach (var unit in shopUnits)
            {
                var item = Instantiate(itemPrototype, itemContainer);
                item.Init(unit.Description, unit.Ico, unit.GetPoolType());
            }
        }
    }
}
