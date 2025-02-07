using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * PanelBase�� ��ӹ޾Ƽ� Panel�� show, hide ���� ���ϸ��̼��̳� ���� Ŀ������ �� �ֵ��� ��.
 */

public abstract class PanelBase : MonoBehaviour
{
    // �ܺο��� ȣ��� �� �ֵ���
    public void TogglePanel()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            OnShow();
        }
        else
        {
            gameObject.SetActive(false);
            OnHide();
        }
    }

    protected abstract void OnShow();
    protected abstract void OnHide();
}
