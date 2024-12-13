using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tray : MonoBehaviour
{
    public ItemId itemId = ItemId.None;
    public List<Transform> sausageIndex = new List<Transform>();
    public List<Transform> breadIndex = new List<Transform>();

    public int maxItems = 4;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.transform.root.GetComponent<Player>();

        if (player != null && player.cariedItems.Count > 0)
        {
            //Destroy(player.cariedItems[i].gameObject);
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < player.cariedItems.Count; i++)
            {   
                int index = i;
                Transform itemToIndex = player.cariedItems[index];

                sequence.Append(
                itemToIndex.DOJump(sausageIndex[i].position, 2f, 1, 0.5f).OnComplete(() =>
                {
                    itemToIndex.SetParent(sausageIndex[index]);
                    itemToIndex.localPosition = Vector3.zero;
                    itemToIndex.localRotation = Quaternion.identity;
                    itemToIndex.localScale = Vector3.one;
                })
                );

                sequence.OnComplete(() =>
                {
                    player.cariedItems.Clear();
                    player.isHolding = false; // Đặt lại isHolding sau khi tất cả đã xóa
                });
            }
        }
    }
}
