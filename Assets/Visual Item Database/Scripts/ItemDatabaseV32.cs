using UnityEngine;

namespace ItemSystem.Database
{
    public class ItemDatabaseV32 : ScriptableObject
    {
        VIDItemListsV32 autoVidLists;
        public static readonly string dbName = "ItemDatabaseV32";

        void OnEnable()
        {
            autoVidLists = Resources.Load<VIDItemListsV32>(VIDItemListsV32.itemListsName);
        }

        /// <summary>
        /// Checks whether an item exists based on id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemExists(ItemBase item)
        {
            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
                if (autoVidLists.usedIDs[i] == item.itemID)
                    return true;

            return false;
        }

        /// <summary>
        /// Checks whether an item exists based on id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ItemExists(int id)
        {
            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
                if (autoVidLists.usedIDs[i] == id)
                    return true;

            return false;
        }

        /// <summary>
        /// Gets a new item ID and adds it to list of used ID's
        /// </summary>
        /// <param name="IDType"></param>
        /// <returns></returns>
        public int GetNewID(ItemType IDType)
        {
            int newID = 0;

            //Loop until you find a new id
            while (true)
            {
                newID = Random.Range(int.MinValue, int.MaxValue);

                //If this ID hasn't been used before and is NOT reserved then add and return it
                if (newID != 0 && newID != -1 && !ItemExists(newID))
                {
                    AddID(newID, IDType);
                    return newID;
                }
            }
        }

        /// <summary>
        /// Adds ID of to used id list
        /// </summary>
        /// <param name="item"></param>
        void AddID(int id, ItemType type)
        {
            autoVidLists.usedIDs.Add(id);
            autoVidLists.typesOfUsedIDs.Add(type);
        }

        /// <summary>
        /// Removes ID from id list
        /// </summary>
        /// <param name="item"></param>
        public void DeleteID(ItemBase item)
        {
            if (!ItemExists(item.itemID))
                return;

            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
            {
                if (autoVidLists.usedIDs[i] == item.itemID)
                {
                    autoVidLists.usedIDs.RemoveAt(i);
                    autoVidLists.typesOfUsedIDs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes ID from id list
        /// </summary>
        /// <param name="id"></param>
        public void DeleteID(int id)
        {
            if (!ItemExists(id))
                return;

            for (int i = 0; i < autoVidLists.usedIDs.Count; i++)
            {
                if (autoVidLists.usedIDs[i] == id)
                {
                    autoVidLists.usedIDs.RemoveAt(i);
                    autoVidLists.typesOfUsedIDs.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Returns item index in respective item list if item exists otherwise returns -1
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetItemIndex(ItemBase item)
        {
            if (!ItemExists(item))
                return -1;

            //Check which type and search repective list
            switch (item.itemType)
            {//#VID-GIIB

				case ItemType.Item:
					for (int i = 0; i < autoVidLists.autoItem.Count; i++)
					{
						if (autoVidLists.autoItem[i].itemID == item.itemID)
							return i;
					}
					break;

				case ItemType.Consumable:
					for (int i = 0; i < autoVidLists.autoConsumable.Count; i++)
					{
						if (autoVidLists.autoConsumable[i].itemID == item.itemID)
							return i;
					}
					break;
            }//#VID-GIIE

            return -1;
        }

        /// <summary>
        /// Returns item index in respective item list if item exists otherwise returns -1
        /// </summary>
        /// <param name="id">ID of item to get</param>
        /// <param name="type">Type of item to get</param>
        /// <returns></returns>
        public int GetItemIndex(int id, ItemType type)
        {
            if (!ItemExists(id))
                return -1;

            //Check which type and search repective list
            switch (type)
            {//#VID-2GIIB

				case ItemType.Item:
					for (int i = 0; i < autoVidLists.autoItem.Count; i++)
					{
						if (autoVidLists.autoItem[i].itemID == id)
							return i;
					}
					break;

				case ItemType.Consumable:
					for (int i = 0; i < autoVidLists.autoConsumable.Count; i++)
					{
						if (autoVidLists.autoConsumable[i].itemID == id)
							return i;
					}
					break;
            }//#VID-2GIIE

            return -1;
        }

        public ItemBase GetItem(int id, ItemType type)
        {
            if (!ItemExists(id))
            {
                Debug.LogError(string.Format("Item of Type '{0}' and of ID '{1}' Does NOT exist", type.ToString(), id));
                return null;
            }

            switch (type)
            {//#VID-GIB

				case ItemType.Item:
					for (int i = 0; i < autoVidLists.autoItem.Count; i++)
					{
						if (autoVidLists.autoItem[i].itemID == id)
							return autoVidLists.autoItem[i];
					}
					break;

				case ItemType.Consumable:
					for (int i = 0; i < autoVidLists.autoConsumable.Count; i++)
					{
						if (autoVidLists.autoConsumable[i].itemID == id)
							return autoVidLists.autoConsumable[i];
					}
					break;
            }//#VID-GIE

            Debug.LogError(string.Format("Item of Type '{0}' and of ID '{1}' Does NOT exist", type.ToString(), id));
            return null;
        }

        public ItemBase GetItem(string itemName, ItemType type)
        {
            switch (type)
            {//#VID-2GIB

				case ItemType.Item:
					for (int i = 0; i < autoVidLists.autoItem.Count; i++)
					{
						if (autoVidLists.autoItem[i].itemName == itemName)
							return autoVidLists.autoItem[i];
					}
					break;

				case ItemType.Consumable:
					for (int i = 0; i < autoVidLists.autoConsumable.Count; i++)
					{
						if (autoVidLists.autoConsumable[i].itemName == itemName)
							return autoVidLists.autoConsumable[i];
					}
					break;
            }//#VID-2GIE

            Debug.LogError(string.Format("Item of Type '{0}' and of Name '{1}' Does NOT exist", type.ToString(), itemName));
            return null;
        }

        /// <summary>
        /// Returns a random item of the passed type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ItemBase GetRandomItem(ItemType type)
        {
            switch (type)
            {//#VID-GRIB

				case ItemType.Item:
					if (autoVidLists.autoItem.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoItem[Random.Range(0, autoVidLists.autoItem.Count)];

				case ItemType.Consumable:
					if (autoVidLists.autoConsumable.Count == 0)
					{
						Debug.LogError(type.ToString() + " has no items in it");
						return null;
					}
					return autoVidLists.autoConsumable[Random.Range(0, autoVidLists.autoConsumable.Count)];
            }//#VID-GRIE

            Debug.LogError(type.ToString() + " type was not found, did you forget to add a check for it?");
            return null;
        }

        /// <summary>
        /// Makes sure no extra/missing IDs exist by checking the item lists against the ID list
        /// </summary>
        /// <param name="listType"></param>
        public void ValidateDatabase()
        {
            //Those id problems only occur because of undo/redo, like when something is deleted and an undo is done, the ID won't be in the database anymore,
            //so we need to readd, similary if an ID is not used at all we remove it
            AddMissingIDs();
            //RemoveExtraIDs();
        }

        void AddMissingIDs()
        {//#VID-AMIDB

			//Item items
			for (int i = 0; i < autoVidLists.autoItem.Count; i++)
				if (!ItemExists(autoVidLists.autoItem[i].itemID))
					AddID(autoVidLists.autoItem[i].itemID, ItemType.Item);

			//Consumable items
			for (int i = 0; i < autoVidLists.autoConsumable.Count; i++)
				if (!ItemExists(autoVidLists.autoConsumable[i].itemID))
					AddID(autoVidLists.autoConsumable[i].itemID, ItemType.Consumable);
        }//#VID-AMIDE

        void RemoveExtraIDs()
        {
            bool removeKey = false;

            //Loop through all keys, find if they are used or not. If a key isn't used then remove it from used keys
            //The loop is reversed since we are iterating and removing at the same time
            for (int i = autoVidLists.usedIDs.Count - 1; i >= 0; i--)
            {
                removeKey = true;

                switch (autoVidLists.typesOfUsedIDs[i])
                {//#VID-REIDB

					//Item
					case ItemType.Item:
						for (int j = 0; j < autoVidLists.autoItem.Count; j++)
						{
							if (autoVidLists.autoItem[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;

					//Consumable
					case ItemType.Consumable:
						for (int j = 0; j < autoVidLists.autoConsumable.Count; j++)
						{
							if (autoVidLists.autoConsumable[j].itemID == autoVidLists.usedIDs[i])
							{
								removeKey = false;
								break;
							}
						}
						break;
                }//#VID-REIDE

                //If the key isn't used in its respective list then remove it from our list of keys
                if (removeKey)
                {
                    autoVidLists.usedIDs.RemoveAt(i);
                    autoVidLists.typesOfUsedIDs.RemoveAt(i);
                }
            }
        }
    }
}
