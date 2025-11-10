using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemSlot
    {
        public Image icon;
        public TMP_Text countText;
        [HideInInspector] public int lastCount;   // track previous value
    }

    [Header("Inventory Slots")]
    public ItemSlot woodSlot;
    public ItemSlot fishSlot;
    public ItemSlot iceSlot;

    public void UpdateWood(int amount) => UpdateSlot(woodSlot, amount);
    public void UpdateFish(int amount) => UpdateSlot(fishSlot, amount);
    public void UpdateIce(int amount) => UpdateSlot(iceSlot, amount);

    private void UpdateSlot(ItemSlot slot, int amount)
    {
        if (slot.countText != null)
            slot.countText.text = amount.ToString();

        if (slot.icon != null)
            slot.icon.color = amount > 0 ? Color.white : new Color(1, 1, 1, 0.4f);

        // Trigger pop if value increased
        if (amount > slot.lastCount)
        {
            var pop = slot.icon.GetComponent<UIItemPop>();
            if (pop != null)
                pop.PlayPop();
        }

        slot.lastCount = amount;
    }
}
