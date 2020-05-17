using System.Collections;
using UnityEngine;

/*场景模块控制器，负责管理所有场景的生命周期逻辑以及切换，加载，卸载等各类操作*/
public class SceneController : Reference 
{
    //唯一实例
    static SceneController sInstance = null;
    public static SceneController Instance
    {
        get { return sInstance; }
    }

    //当前场景
    public SceneType curScene;
    //加载的进度百分比
    int loadPrecent;
    //显示的进度百分比
    int displayPrecent;

	//自定义初始化方法
    public void OnLoad()
    {
        sInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    //加载新的场景（切换场景操作）
    public void LoadScene(SceneType _SceneType)
    {
        //获取场景名称
        string tmpName = SceneConstant.GetNameWithType(_SceneType);
        //如果目标场景存在
        if(!string.IsNullOrEmpty(tmpName))
        {
            //启动异步加载场景协同函数（特殊的函数）
            StartCoroutine(FuncLoadScene(tmpName, _SceneType));
        }
    }

    //异步分帧加载场景协同函数
    IEnumerator FuncLoadScene(string _SceneName, SceneType _SceneType)
    {
        loadPrecent = 0;
        displayPrecent = 0;
        //切换场景首先显示读条UI
        UIController.Instance.ShowLoadingCanvas(true);
        //暂停/休眠1s
        yield return new WaitForSeconds(1f);

        //同步切换场景，不能同时做其他事情，必须等待此操作执行完
        //UnityEngine.SceneManagement.SceneManager.LoadScene(_SceneName);
        //创建一个异步操作用来执行场景异步切换功能 Async->异步，Sync->同步
        AsyncOperation tmpAsync =
                                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_SceneName);
        //此时新场景还未加载，不可以激活（读条时不能响应用户的输入交互）
        tmpAsync.allowSceneActivation = false;

        //做一个循环，不断的分帧加载场景中的资源，更新进度条
        while(tmpAsync.progress < 0.9f)
        {
            //进度换算成百分比数值
            loadPrecent = (int)(tmpAsync.progress * 100);
            //瞬间上升到实际进度->瞬时
            //displayPrecent = loadPrecent;
            //让显示的进度不断的上升到加载实际进度->持续
            while (displayPrecent < loadPrecent)
            {
                //每次循环进度上升1%
                ++displayPrecent;
                //同步显示进度到进度条UI以及百分比文本组件
                UIController.Instance.SetPgrValue(displayPrecent);
                //暂停/休眠0.1s
                //yield return new WaitForSeconds(0.1f);
                //暂停/休眠1帧
                yield return new WaitForEndOfFrame();
            }
        }

        //进度已加载至100%
        loadPrecent = 100;
        while (displayPrecent < loadPrecent)
        {
            //每次循环进度上升1%
            ++displayPrecent;
            //同步显示进度到进度条UI以及百分比文本组件
            UIController.Instance.SetPgrValue(displayPrecent);
            //暂停/休眠0.1s
            //yield return new WaitForSeconds(0.1f);
            //暂停/休眠1帧
            yield return new WaitForEndOfFrame();
        }

        //此时新场景已加载完毕，激活之
        tmpAsync.allowSceneActivation = true;
        //将当前场景切换为新的场景
        curScene = _SceneType;
        //隐藏读条界面
        UIController.Instance.ShowLoadingCanvas(false);
    }
}
