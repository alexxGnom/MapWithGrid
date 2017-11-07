using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnixanTest
{
    public class FieldGridDrawer : MonoBehaviour
    {
        #region Unity properties

        [SerializeField]
        private float lineWidth = 0.01f;

        [SerializeField]
        private Material lineMaterial;

        [SerializeField]
        private Transform container;

        #endregion

        #region Private fields

        private Field _field;

        #endregion

        #region Interface

        public void ShowGridToggle()
        {
            container.gameObject.SetActive(!container.gameObject.activeSelf);
        }

        #endregion

        private void Start()
        {
            if (container == null)
                container = this.transform;

            container.gameObject.SetActive(false);

            _field = FieldController.Instance.GetField();

            CreateLines();
        }

        private void CreateLine(Vector3[] positions, string name = "Line")
        {
            var go = new GameObject(name);

            go.transform.parent = container;

            var lr = go.AddComponent<LineRenderer>();
            lr.SetPositions(positions);

            lr.material = lineMaterial;

            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
        }

        private void CreateLines()
        {
            float lineHeigh = 0.5f;    

            for (var x = 0; x <= _field.Width; x++)
            {
                var positions = new Vector3[] { new Vector3(_field.CellSize * x, lineHeigh, 0), new Vector3(_field.CellSize * x, lineHeigh, _field.CellSize * _field.Heigh) };
                CreateLine(positions, "Line_Vert_" + x); 
            }

            for (var y = 0; y <= _field.Heigh; y++)
            {
                var positions = new Vector3[] { new Vector3(0, lineHeigh, _field.CellSize * y), new Vector3(_field.CellSize * _field.Width, lineHeigh, _field.CellSize * y) };
                CreateLine(positions, "Line_Hor_" + y);
            }
        }
     }
}
