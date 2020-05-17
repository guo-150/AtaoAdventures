using UnityEngine;
using UnityEngine.UI;

/*UI模块控制器，负责场景切换时的UI的显示、隐藏、创建、销毁等通用逻辑*/
public class UIController : Reference 
{
    //唯一实例
    static UIController sInstance = null;
    public static UIController Instance
    {
        get { return sInstance; }
    }

    //读条界面预制体
    public GameObject loadingObjectPrefab;
    //读条UI对象
    GameObject loadingObject;
    //所有读条时可能随机到的背景图列表
    public Sprite[] loadingBKs;
    //读条背景Image组件
    Image bkImg;
    //读条进度条
    Slider pgrSlider;
    //百分比文本
    Text pgrValue;

    //自定义初始化方法
    public void OnLoad()
    {
        sInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    //显示、关闭读条界面方法
    public void ShowLoadingCanvas(bool _Show)
    {
        //如果显示，则说明要切换场景
        if(_Show)
        {
            //生成进度条界面UI，保存相关的UI组件
            loadingObject = Instantiate(loadingObjectPrefab);
            bkImg = loadingObject.GetComponentInChildren<Image>();
            pgrSlider = loadingObject.GetComponentInChildren<Slider>();
            pgrValue = loadingObject.GetComponentInChildren<Text>();
            //随机一个背景图片
            bkImg.sprite = loadingBKs[Random.Range(0, loadingBKs.Length)];
        }
        //如果隐藏，则说明已切换到新的场景
        else
        {
            //如果正在读条
            if(loadingObject != null)
            {
                //销毁进度条UI
                Destroy(loadingObject);
                //进度条引用置空
                loadingObject = null;
            }
        }
    }

    //同步场景切换进度条数据
    public void SetPgrValue(int _Value)
    {
        //更新进度条
        pgrSlider.value = (float)_Value / 100;
        //更新文本
        pgrValue.text = _Value + "%";
    }
}
