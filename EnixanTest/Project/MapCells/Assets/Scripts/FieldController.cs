using System.Collections;
using UnityEngine;

namespace EnixanTest
{ 
    [System.Serializable]
    public struct FieldInfo
    {
        public int width;
        public int heigh;

        public int cellSize;
    }

    public class FieldController : MonoSingleton<FieldController>
    {
        #region Unity properties

        [SerializeField]
        private FieldInfo fieldInfo;

        [SerializeField]
        private Transform unitContainer;

        [SerializeField]
        private int randomPropsEdgeSize = 10;

        #endregion

        #region Private fields

        private Field _field;

        private int _width;
        private int _heigh;
        private int _cellSize;

        #endregion

        #region Interface

        public bool CanSetUnitToCell(BaseUnit unit, Cell cell)
        {
            if (cell.IsBusy()) return false;
            for (var i = cell.x; i < cell.x + unit.Width; i++)
            {
                for (var j = cell.y; j < cell.y + unit.Heigh; j++)
                {
                    if (!_field.IsCorrectCellAdr(i, j) || _field.GetCell(i, j).IsBusy()) return false;
                }
            }
            return true;
        }

        public void SetUnitOnField(BaseUnit unit, Cell cell)
        {
            for (var i = cell.x; i < cell.x + unit.Width; i++)
            {
                for (var j = cell.y; j < cell.y + unit.Heigh; j++)
                {
                    _field.GetCell(i, j).unit = unit;
                }
            }

            unit.Spawn(unitContainer, cell.position);
        }

        public Field GetField()
        {
            return _field;
        }

        #endregion

        protected override void Init()
        {
            base.Init();

            _field = new Field(fieldInfo.width, fieldInfo.heigh, fieldInfo.cellSize);

            _heigh = _field.Heigh;
            _width = _field.Width;
            _cellSize = _field.CellSize;
        }

        private void Start()
        {
            StartCoroutine(CreateRandomProps());
        }

        private IEnumerator CreateRandomProps()
        {
            var poolParams = new[]
            {
                Constants.UNITS_POOL.Tree1,
                Constants.UNITS_POOL.Tree2,
                Constants.UNITS_POOL.Tree3,
                Constants.UNITS_POOL.Bush1,
                Constants.UNITS_POOL.Bush2,
                Constants.UNITS_POOL.Bush3,
            };

            var unit = GetRandomUnitFromPool(poolParams);
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _heigh; j++)
                {
                    if (    i > randomPropsEdgeSize && i < _field.Width - randomPropsEdgeSize
                        &&  j > randomPropsEdgeSize && j < _field.Heigh - randomPropsEdgeSize   )
                    {
                        continue;
                    }

                    var cell = _field.GetCell(i, j);
                    if (CanSetUnitToCell(unit, cell))
                    {
                        SetUnitOnField(unit, cell);
                        yield return new WaitForEndOfFrame();
                        unit = GetRandomUnitFromPool(poolParams);
                    }
                }
            }

            unit.Despawn();
        }

        private BaseUnit GetRandomUnitFromPool(params Constants.UNITS_POOL[] poolTypes)
        {
            var poolNum = poolTypes[Random.Range(0, poolTypes.Length)];
            return PoolManager.Instance.Units_Pools[(int)poolNum].GetObject();
        }

    }
}
