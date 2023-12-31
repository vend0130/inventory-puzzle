﻿using Code.Data;
using Code.Data.Items;
using UnityEngine;

namespace Code.Game.Item.Items
{
    public class ItemMagazine : BaseItem
    {
        [field: SerializeField] public MagazineData Data { get; private set; }

        public override void OpenInfo() =>
            ItemInfo.Open(Data);
    }
}