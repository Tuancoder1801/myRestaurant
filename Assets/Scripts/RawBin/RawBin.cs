using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawBin : MonoBehaviour
{
    public BaseItem baseItem;
    public Transform itemIndex;

    private void CreateItem(out BaseItem item)
    {
        item = Instantiate(baseItem, itemIndex.position, itemIndex.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.transform.root.GetComponent<Player>();

        if (player != null)
        {
            int availablePositions = player.itemPositions.Count;

            // Tạo danh sách các item cần thêm
            List<Transform> itemsToAdd = new List<Transform>();

            for (int i = 0; i < availablePositions; i++)
            {
                // Nếu vị trí đã có item, bỏ qua
                if (player.itemPositions[i].childCount > 0)
                    continue;

                BaseItem newItem;
                CreateItem(out newItem);

                // Thêm item vào danh sách
                itemsToAdd.Add(newItem.transform);
            }

            player.AddNewItem(itemsToAdd);
        }
    }
}
