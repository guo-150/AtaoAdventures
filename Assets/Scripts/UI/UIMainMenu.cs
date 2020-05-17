using UnityEngine;
using UnityEngine.UI;

/*主菜单UI脚本，负责处理主菜单场景的UI逻辑***** 大型游戏用，登录按键与数据认证通用，该脚本失效不i用*/
public class UIMainMenu : MonoBehaviour 
{
	void Start () 
    {
        Button loginBtn = transform.Find("LoginBtn").GetComponent<Button>();
        loginBtn.onClick.AddListener(OnLoginBtnClick);
	}

    //登录按钮回调函数
    void OnLoginBtnClick()
    {
        SceneController.Instance.LoadScene(SceneType.Select);
    }
}
