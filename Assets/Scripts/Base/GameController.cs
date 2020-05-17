using UnityEngine;

/*游戏控制器，负责创建和管理其他模块的子控制器*/
public class GameController : Reference
{
    //唯一实例
    private static GameController sInstance = null;
    //公有的访问接口
    public static GameController Instance
    {
        get { return sInstance; }
    }

    //场景模块
    public GameObject SceneCtrlPrefab;
    SceneController SceneControl;

    //UI模块
    public GameObject UICtrlPrefab;
    UIController UIControl;

	//保证游戏控制器的初始化先于其他所有脚本
    void Awake()
    {
        //将此控制器应用单例模式
        if (sInstance == null)
            sInstance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //在整个游戏过程中永久存在
        DontDestroyOnLoad(gameObject);

        //接下来陆续的创建出其他子控制器来管理子模块
        SceneControl = Instantiate(SceneCtrlPrefab).GetComponent<SceneController>();
        UIControl = Instantiate(UICtrlPrefab).GetComponent<UIController>();

        //游戏初始化
        OnGameInit();
    }

    //游戏全局初始化方法，所有全局资源以及参数在此进行初始化，
    //按照自定义方法的调用顺序来保证子控制器的初始化顺序
    void OnGameInit()
    {
        //初始化随机数种子  ？
        Random.InitState(System.Environment.TickCount);
        //初始化Dotween

        //调用其他控制器的OnLoad方法按照顺序初始化其资源和参数,scene先初始化，UI后初始化
        SceneControl.OnLoad();
        UIControl.OnLoad();
    }
}
