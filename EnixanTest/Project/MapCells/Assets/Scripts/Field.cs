using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnixanTest
{
    public class Field
    {
        #region Private fields

        private int _cellSize;

        private Cell[,] _data;

        #endregion

        #region Public properties

        public int CellSize
        { get { return _cellSize; } }

        public int Width
        { get { return _data.GetLength(0); } }

        public int Heigh
        { get { return _data.GetLength(1); } }

        #endregion

        #region Interface

        public Cell GetCell(int x, int y)
        {
            if (!IsCorrectCellAdr(x, y)) return null;

            return _data[x, y];
        }

        public Cell GetCellFromPoint(Vector3 point)
        {
            return GetCell((int)(point.x / _cellSize), (int)(point.y / _cellSize));
        }

        public bool IsCorrectCellAdr(int x, int y)
        {
            if ((x < 0 || x >= Width)
            || (y < 0 || y >= Heigh))
            {
                return false;
            }
            return true;
        }

        #endregion

        public Field(int width, int heigh, int cellSize)
        {
            _data = new Cell[width, heigh];

            _cellSize = cellSize;

            CreateField();
        }

        private void CreateField()
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Heigh; j++)
                {
                    var pos = new Vector3(i * _cellSize + _cellSize / 2f, j * _cellSize + _cellSize / 2f, 0f);

                    _data[i, j] = new Cell { x = i, y = j, position = pos, unit = null };
                }
            }
        }
    }

     public class Cell
    {
        public int x;
        public int y;

        public Vector2 position;

        public BaseUnit unit;

        public bool IsBusy()
        {
            return unit != null;
        }
    }
}
