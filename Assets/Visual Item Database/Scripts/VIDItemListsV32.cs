using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem
{//#VID-ISNB
}//#VID-ISNE

namespace ItemSystem
{//#VID-2ISNB
	public enum ConsumableItems
	{
		None = 0,
		Apple = 50413427,
		Meat = -1370315301,
		Riceball = -694024983,
		Soda = 766657084,
		Mushroom = -1152645558,
	}
	public enum ItemItems
	{
		None = 0,
	}
}//#VID-2ISNE

namespace ItemSystem.Database
{
    public class VIDItemListsV32 : ScriptableObject
    {
        /*Do NOT change the formatting of anything between comments starting with '#VID-'*/

        public static readonly string itemListsName = "VIDItemListsV32";

        //#VID-ICB
		public List<ItemProfile> autoItem = new List<ItemProfile>();
		public List<ConsumableProfile> autoConsumable = new List<ConsumableProfile>();
        //#VID-ICE

        /*Those two lists are 'parallel', one shouldn't be changed without the other*/
        /// <summary>Stores taken IDs</summary>
        [HideInInspector]
        public List<int> usedIDs = new List<int>();

        /// <summary>Stores the types of taken IDs</summary>
        [HideInInspector]
        public List<ItemType> typesOfUsedIDs = new List<ItemType>();

        [HideInInspector]
        public List<ItemSubtypeV25> subtypes = new List<ItemSubtypeV25>();
        [HideInInspector]
        public List<ItemTypeGroup> typeGroups = new List<ItemTypeGroup>();
    }
}
