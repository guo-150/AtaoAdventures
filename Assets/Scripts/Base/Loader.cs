using UnityEngine;

/*游戏启动类，游戏加载器，一切都从此开始*/
public class Loader : MonoBehaviour 
{
    //游戏控制器预制体
    public GameController GameCtrlPrefab;

	void Awake () 
    {
        //游戏一启动就立刻生成游戏控制器对象
        Instantiate(GameCtrlPrefab);
	}
}
