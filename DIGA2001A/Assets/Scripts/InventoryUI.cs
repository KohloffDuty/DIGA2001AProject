using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemSlot
    {
        public TMP_Text countText;
        public Image icon;
    }

    public ItemSlot[] slots; // assign these in Inspector

    public void UpdateCount(int index, int value)
    {
        if (index < 0 || index >= slots.Length) return;

        slots[index].countText.text = value.ToString();
        slots[index].icon.color = value > 0 ? Color.white : new Color(1, 1, 1, 0.3f);
    }
}
