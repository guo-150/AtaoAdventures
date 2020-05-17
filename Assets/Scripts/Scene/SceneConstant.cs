/*场景枚举类型，标识每一个场景*/
public enum SceneType
{
    Enter,//登录
    Select,//选择界面
    Demo1
}

/*场景通用工具参数类，与所有场景相关的静态函数以及常量字段定义在此*/
public class SceneConstant 
{
    //根据场景的类型获得场景名称
    public static string GetNameWithType(SceneType _SceneType)
    {
        string tmpName = "";
        switch(_SceneType)
        {
            case SceneType.Enter:
                tmpName = "Enter";
                break;
            case SceneType.Select:
                tmpName = "Select";
                break;
            case SceneType.Demo1:
                tmpName = "Demo1";
                break;
        }
        return tmpName;
    }
}
