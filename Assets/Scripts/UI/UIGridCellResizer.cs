using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [ExecuteInEditMode]
    public class UIGridCellResizer : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private RectTransform _containerRect;
        [SerializeField] private Vector2Int _cellsCount = new Vector2Int(4, 2);

        [ContextMenu("Resize")]
        private void Resize()
        {
            var cellSize = GetMinCellDimension();
            _grid.cellSize = new Vector2(cellSize, cellSize);
        }

        private float GetMinCellDimension()
        {
            var rect = _containerRect.rect;

            var maxCellWidth = rect.width / _cellsCount.x - _grid.spacing.x;
            var maxCellHeight = rect.height / _cellsCount.y - _grid.spacing.y;

            return Mathf.Min(maxCellHeight, maxCellWidth);
        }

        private void Start()
        {
            Resize();
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _grid = GetComponent<GridLayoutGroup>();
            _containerRect = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (Application.isPlaying) return;
            Resize();
        }
#endif
    }
}
