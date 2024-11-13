#if UNITY_EDITOR
using System.Linq;
using DataStruct;
using DataStruct.ScriptableObjects;
using UnityEditor;


namespace Editor
{
    [CustomEditor(typeof(RewardItemSO))]
    public class RewardItemSOEditor : UnityEditor.Editor
    {
        private static readonly string[] itemNames = GetItemNames();

        private static string[] GetItemNames()
        {
            return typeof(ItemName)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(field => field.IsLiteral && !field.IsInitOnly) // Filters for constants
                .Select(field => (string)field.GetRawConstantValue())
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            var rewardItemSO = (RewardItemSO)target;

            // Create dropdown for item names
            int selectedIndex = System.Array.IndexOf(itemNames, rewardItemSO.itemName);
            selectedIndex = EditorGUILayout.Popup("Item Name", selectedIndex, itemNames);

            // Assign the selected item name
            if (selectedIndex >= 0 && selectedIndex < itemNames.Length)
            {
                rewardItemSO.itemName = itemNames[selectedIndex];
            }

            DrawDefaultInspector();
        }
    }
}
#endif