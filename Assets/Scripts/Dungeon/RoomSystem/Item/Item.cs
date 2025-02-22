using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [SerializeField]
    private GameObject dropItem;
    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private BoxCollider2D itemCollider;

    [SerializeField]
    int health = 3;

    [SerializeField]
    bool nonDestructible;

    [SerializeField]
    private GameObject hitFeedback, destroyFeedback;

    public UnityEvent OnGetHit
    {
        get => throw new System.NotImplementedException();
        set => throw new System.NotImplementedException();
    }

    public void Init(ItemData itemData)
    {
        dropItem = itemData.DropItem;
        spriteRenderer.sprite = itemData.Sprite;

        // ��������Ʈ�� ���� ũ�� ��������
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // ��������Ʈ�� ������ ������ ũ�⿡ ���߱� ���� ������ ����
        spriteRenderer.transform.localScale = new Vector2(itemData.Size.x / spriteSize.x, itemData.Size.y / spriteSize.y);
        // sprite�� ��ġ ����
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.Size.x, 0.5f * itemData.Size.y);

        itemCollider.size = itemData.Size;
        itemCollider.offset = new Vector2(spriteRenderer.transform.localPosition.x, itemData.Size.y);

        if (itemData.NonDestructible) nonDestructible = true;

        this.health = itemData.Health;
    }

    public void GetHit()
    {
        if (nonDestructible) return;

        // Ÿ�� ����Ʈ�ε�, ������ �����ϱ�
        //if (health > 1)
        //    Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        //else
        //    Instantiate(destroyFeedback, spriteRenderer.transform.position, Quaternion.identity);

        // ȸ�� ���� ����Ʈ
        spriteRenderer.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 20), 50, 90, true).OnComplete(ReduceHealth);
    }

    private void ReduceHealth()
    {
        health--;
        
        if (health <= 0)
        {
            spriteRenderer.transform.DOComplete();
            DropItem();
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        if (dropItem == null) return;

       float riseHeight = 0.5f;    // �������� ����
       float riseDuration = 0.5f; // �������� �ð�
       float dropDuration = 0.5f; // �������� �ð�

        // ���߿� ��� �����ۺ� Ȯ�� ���ؼ� �ؾ� ��
        if ((int)Random.Range(0, 6) >= 0)
        {
            GameObject dropped = Instantiate(dropItem, spriteRenderer.transform.position, Quaternion.identity);
            Sequence dropSequence = DOTween.Sequence();

            // ��� �ִϸ��̼� ����
            dropSequence
                .Append(dropped.transform.DOMoveY((spriteRenderer.transform.position.y + spriteRenderer.size.y * 0.5f) + riseHeight, riseDuration)
                    .SetEase(Ease.OutSine)) // ���ڿ��� ���� �ε巴�� ������
                .Append(dropped.transform.DOMoveY(spriteRenderer.transform.position.y, dropDuration)
                    .SetEase(Ease.OutQuad)); // �ٽ� ���� ��ġ�� ������
        }
    }
}
