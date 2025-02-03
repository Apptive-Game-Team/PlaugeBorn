using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * ��ư�� Ŭ�� ��ɿ� ���� �ڵ� �ߺ��� ���̱� ���� Ŭ����.
 * ��ư �̸��� ���εǴ� ����� �̸� �����صξ�, ��ư �̸��� ���� ������ ����� ��.
 * ���� ��ư ������Ʈ �̸��� ��Ȯ�ϰ� �Է��ؾ� ��
 */

public class ButtonClickHandler : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClick(gameObject.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "StartButton":
                SceneController.Instance.LoadScene("Main");
                break;
            case "ExitButton":
                Application.Quit();
                break;
            case "ShopButton":
                // SceneController.Instance.LoadScene("Shop");
                Debug.Log("���� ����");
                break;
            case "TitleButton":
                SceneController.Instance.LoadScene("Title");
                break;
            case "ResumeButton":
                // PanelPanel���� ����
                break;
            default:
                Debug.LogError("Button " + buttonName + " not found.");
                break;
        }
    }
}
