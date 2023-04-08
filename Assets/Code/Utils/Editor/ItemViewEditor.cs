using Code.Game.Item;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Utils.Editor
{
    [CustomEditor(typeof(ItemView))]
    public class ItemViewEditor : UnityEditor.Editor
    {
        private ItemView _itemView;
        private bool[,] _grid;

        private void OnEnable()
        {
            _itemView = (ItemView)serializedObject.targetObject;

            UpdateArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UpdateArray();
            DrawGrid();

            if (IsRefresh())
            {
                Refresh();
            }
        }

        private bool IsRefresh()
        {
            if (_grid.GetLength(0) != _itemView.Grid.Length)
                return true;

            for (int y = 0; y < _grid.GetLength(0); y++)
            {
                if (_grid.GetLength(1) != _itemView.Grid[y].Width.Length)
                    return true;

                for (int x = 0; x < _grid.GetLength(1); x++)
                {
                    if (_grid[y, x] != _itemView.Grid[y].Width[x])
                        return true;
                }
            }

            return false;
        }

        private void Refresh()
        {
            _itemView.Grid = new WidthData[_grid.GetLength(0)];

            int counter = 0;

            for (int y = 0; y < _itemView.Grid.Length; y++)
            {
                _itemView.Grid[y] = new WidthData();

                _itemView.Grid[y].Width = new bool[_grid.GetLength(1)];
                for (int x = 0; x < _itemView.Grid[y].Width.Length; x++)
                {
                    _itemView.Grid[y].Width[x] = _grid[y, x];

                    if (_grid[y, x])
                        counter++;
                }
            }

            _itemView.CellsCountForItem = counter;

            PrefabUtility.RecordPrefabInstancePropertyModifications(_itemView);
            Canvas.ForceUpdateCanvases();
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private void UpdateArray()
        {
            _grid = new bool[_itemView.Height, _itemView.Width];

            if (_itemView.Grid == null)
            {
                CreateArray();
                return;
            }

            CopyArray();
        }

        private void CreateArray()
        {
            for (int y = 0; y < _grid.GetLength(0); y++)
            {
                for (int x = 0; x < _grid.GetLength(1); x++)
                    _grid[y, x] = false;
            }
        }

        private void CopyArray()
        {
            int yLenght = _itemView.Grid.Length;

            for (int y = 0; y < _itemView.Height; y++)
            {
                for (int x = 0; x < _itemView.Width; x++)
                {
                    if (y < yLenght && x < _itemView.Grid[y].Width.Length)
                        _grid[y, x] =
                            _itemView.Grid[y].Width[x];
                    else
                        _grid[y, x] = false;
                }
            }
        }

        private void DrawGrid()
        {
            GUIStyle style = new GUIStyle();
            style.fixedWidth = 1;

            for (int y = 0; y < _grid.GetLength(0); y++)
            {
                EditorGUILayout.BeginHorizontal(style);

                for (int x = 0; x < _grid.GetLength(1); x++)
                    _grid[y, x] = EditorGUILayout.Toggle(_grid[y, x]);

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}