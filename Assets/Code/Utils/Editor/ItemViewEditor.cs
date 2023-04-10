using System.Linq;
using Code.Game.Item;
using Code.Game.Item.Items;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Utils.Editor
{
    [CustomEditor(typeof(BaseItem), true)]
    public class ItemViewEditor : UnityEditor.Editor
    {
        private BaseItem _item;
        private CellInItemData[,] _grid;
        private int _popupIndex = 0;

        private void OnEnable()
        {
            _item = (BaseItem)serializedObject.targetObject;

            UpdateArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var additionalsUpdate = UpdateAdditionals();
            UpdateArray();
            DrawGrid();

            if (IsRefresh() || additionalsUpdate)
            {
                Refresh();
            }
        }

        private bool UpdateAdditionals()
        {
            if (!IsAdditional())
                return false;

            bool isUpdated = false;

            foreach (AdditionalData additional in _item.AdditionalDatas)
            {
                if (additional.Activate != additional.Image.enabled)
                {
                    additional.Image.enabled = additional.Activate;
                    isUpdated = true;
                }
            }

            return isUpdated;
        }

        private bool IsRefresh()
        {
            if (_grid.GetLength(0) != _item.Grid.Length)
                return true;

            for (int y = 0; y < _grid.GetLength(0); y++)
            {
                if (_grid.GetLength(1) != _item.Grid[y].Width.Length)
                    return true;

                for (int x = 0; x < _grid.GetLength(1); x++)
                {
                    if (_grid[y, x].Value != _item.Grid[y].Width[x].Value)
                        return true;
                }
            }

            return false;
        }

        private void Refresh()
        {
            _item.Grid = new WidthData[_grid.GetLength(0)];

            int counter = 0;

            for (int y = 0; y < _item.Grid.Length; y++)
            {
                _item.Grid[y] = new WidthData();

                _item.Grid[y].Width = CreateData(_grid.GetLength(1));
                for (int x = 0; x < _item.Grid[y].Width.Length; x++)
                {
                    _item.Grid[y].Width[x] = _grid[y, x];

                    if (_grid[y, x].Value)
                        counter++;
                }
            }

            _item.CellsCountForItem = counter;

            PrefabUtility.RecordPrefabInstancePropertyModifications(_item);
            Canvas.ForceUpdateCanvases();
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }

        private void UpdateArray()
        {
            _grid = CreateData(_item.Height, _item.Width);

            if (_item.Grid == null)
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
                    _grid[y, x].Value = false;
            }
        }

        private void CopyArray()
        {
            int yLenght = _item.Grid.Length;

            for (int y = 0; y < _item.Height; y++)
            {
                for (int x = 0; x < _item.Width; x++)
                {
                    if (y < yLenght && x < _item.Grid[y].Width.Length)
                        _grid[y, x] = new CellInItemData(_item.Grid[y].Width[x].Value, _item.Grid[y].Width[x].Type);
                    else
                        _grid[y, x] = new CellInItemData(false, ItemType.None);
                }
            }
        }

        private void DrawGrid()
        {
            ItemType typeDraw = GetTypeDraw();

            GUIStyle style = new GUIStyle();
            style.fixedWidth = 1;

            for (int y = 0; y < _grid.GetLength(0); y++)
            {
                EditorGUILayout.BeginHorizontal(style);

                for (int x = 0; x < _grid.GetLength(1); x++)
                    DrawToggle(typeDraw, _grid[y, x]);

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawToggle(ItemType typeDraw, CellInItemData data)
        {
            bool disable = data.Type != ItemType.None && data.Type != typeDraw;

            if (disable)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Toggle(data.Value);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                data.Value = EditorGUILayout.Toggle(data.Value);

                data.Type = !data.Value
                    ? ItemType.None
                    : typeDraw == ItemType.None
                        ? _item.ItemType
                        : typeDraw;
            }
        }

        private ItemType GetTypeDraw()
        {
            ItemType type = _item.ItemType;

            if (IsAdditional())
            {
                ItemType[] names = new ItemType[_item.AdditionalDatas.Length + 1];
                names[0] = type;

                for (int i = 1; i < names.Length; i++)
                {
                    names[i] = _item.AdditionalDatas[i - 1].Type;
                }

                string[] stringNames = names.Select(x => x.ToString()).ToArray();
                _popupIndex = EditorGUILayout.Popup(_popupIndex, stringNames);
                type = names[_popupIndex];
            }

            return type;
        }

        private CellInItemData[] CreateData(int lenght)
        {
            CellInItemData[] datas = new CellInItemData[lenght];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = new CellInItemData(false, ItemType.None);
            }

            return datas;
        }

        private CellInItemData[,] CreateData(int lenght1, int lenght2)
        {
            CellInItemData[,] datas = new CellInItemData[lenght1, lenght2];

            for (int i = 0; i < lenght1; i++)
            {
                for (int j = 0; j < lenght2; j++)
                {
                    datas[i, j] = new CellInItemData(false, ItemType.None);
                }
            }

            return datas;
        }

        private bool IsAdditional() =>
            _item.AdditionalDatas != null && _item.AdditionalDatas.Length > 0;
    }
}