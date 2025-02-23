using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PausePanel : PanelBase
{
    [SerializeField] private Button resumeButton;

    // Awake���� �̺�Ʈ�� ������ְ� Start���� ������Ʈ ��Ȱ��ȭ ��Ű��
    private void Awake()
    {
        EventManager.OnPausePressed += TogglePanel;
    }
    private void Start()
    {
        // resumeButton Ŭ�� �� ���� �簳
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        gameObject.SetActive(false);
    }

    private void OnResumeButtonClick()
    {
        // �г� �ݱ� �� ���� �簳
        TogglePanel();
        Time.timeScale = 1f; 
    }

    protected override void OnShow()
    {
        Time.timeScale = 0f;
    }

    protected override void OnHide()
    {
        Time.timeScale = 1f;
    }
}
