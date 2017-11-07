using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnixanTest
{
    public class BuildManager : MonoSingleton<BuildManager>
    {
        #region Unity properties

        [SerializeField]
        private InputController inputController;

        [SerializeField]
        private Transform indicatorsContainer;

        [SerializeField]
        private Transform unitContainer;

        #endregion

        #region Private fields

        private List<List<Indicator>> _indicators = new List<List<Indicator>>();

        private BaseUnit _currentUnit;

        private Field _field;

        private bool _isStarted = false;

        private UIWindowBase _cancelWindow;

        #endregion

        #region Interface

        public void StartBuildUnit(BaseUnit unit)
        {
            if (unit == null) return;

            _currentUnit = unit;

            for (var i = 0; i < unit.Width; i++)
            {
                _indicators.Add(new List<Indicator>());
                for (var j = 0; j < unit.Heigh; j++)
                {
                    _indicators[i].Add(PoolManager.Instance.Indicator_Pool.GetObject().Spawn(indicatorsContainer, Vector3.zero));
                }
            }

            if (_cancelWindow != null)
                _cancelWindow.OpenWindow();

            _isStarted = true;
        }

        public void StopBuildUnit()
        {
            _isStarted = false;

            if (_currentUnit != null)
                _currentUnit.Despawn();

            foreach (var lineInd in _indicators)
            {
                foreach (var ind in lineInd)
                {
                    ind.Despawn();
                }
            }
            _indicators = new List<List<Indicator>>();

            if (_cancelWindow != null)
                _cancelWindow.CloseWindow();
        }

        #endregion

        protected override void Init()
        {
            base.Init();
            _cancelWindow = UIMainController.GetWindowById(Constants.UI_BUILD_CANCEL_WINDOW);
        }

        private void Start()
        {
            _field = FieldController.Instance.GetField();

            //var car = PoolManager.Instance.Tree_Pools[0].GetObject();

            //StartBuildUnit(car);
        }

        private void Update()
        {
            if (!_isStarted) return;

            var target = inputController.GetMousePosOnTerrain();
            var c = _field.GetCellFromPoint(target);

            if (c != null)
            {
                for (var i = 0; i < _currentUnit.Width; i++)
                {
                    for (var j = 0; j < _currentUnit.Heigh; j++)
                    {
                        var ind = _indicators[i][j];

                        ind.transform.localPosition = new Vector3(c.position.x + _field.CellSize * i, c.position.y + _field.CellSize * j, -0.5f);
                        if (!_field.IsCorrectCellAdr(i + c.x, j + c.y) || _field.GetCell(i + c.x, j + c.y).IsBusy())
                            ind.ShowBusy();
                        else
                            ind.ShowEmpty();
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (inputController.CheckUIRaycast()) return;    

                    if (FieldController.Instance.CanSetUnitToCell(_currentUnit, c))
                    {

                        FieldController.Instance.SetUnitOnField(_currentUnit, c);
                        _currentUnit = null;
                        StopBuildUnit();

                    }
                }
            }



        }

    }
}
