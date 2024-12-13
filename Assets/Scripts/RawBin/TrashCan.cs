using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public Transform itemIndex;


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
                Transform itemToRemove = player.cariedItems[index];

                sequence.Append(
                itemToRemove.DOJump(itemIndex.position, 2f, 1, 0.5f).OnComplete(() =>
                {
                    Destroy(itemToRemove.gameObject); 
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
