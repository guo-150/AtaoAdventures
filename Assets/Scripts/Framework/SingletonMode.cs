using UnityEngine;

/*单例模板类，想实现单例模式均直接继承此类就可以了*/
public class SingletonMode<T> : MonoBehaviour where T: MonoBehaviour
{
    //唯一的实例要能被子类访问
    protected static T instance;
	//公有的外部访问接口
    public static T Instance
    {
        get
        {
            //不要进行空判断，不要new，直接返回实例
            return instance;
        }
    }

    //重写Awake方法，在内部通过判断确保实例唯一仅有一个
    public virtual void Awake() 
    {
        //如果实例为空，说明是第1个实例
        if(instance == null)
        {
            //保存this为当前唯一实例
            instance = this as T;
        }
        //如果不为空，说明是第2次的实例
        else
        {
            //说明this是多出来的实例（不是第1个实例），应该立刻销毁掉
            Destroy(gameObject);
        }
        //重载场景或切换场景时不销毁实例
        DontDestroyOnLoad(gameObject);
    } 
}
