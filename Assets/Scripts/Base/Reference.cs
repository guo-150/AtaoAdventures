using UnityEngine;

/*脚本引用类，所有Unity脚本的最终父类，表示所有脚本类的基本引用，定义静态常量字段与方法  不实例化，所以abstract*/
public abstract class Reference : MonoBehaviour 
{
    /*********游戏标签*********/
    protected const string TAG_PLAYER = "Player";
    protected const string TAG_ENEMY = "Enemy";
    protected const string TAG_WEAPON = "Weapon";
    protected const string TAG_ITEM = "Item";
    protected const string TAG_NPC = "Npc";
    protected const string TAG_BUILDING = "Building";

    /********游戏默认参数********/
    protected const string Object_Default_Name = "未命名";
    protected const string Object_Default_Descript = "功能未知";

    /********UI面板名称**********/
    protected const string UIPANEL_ACTOR = "UIActorPanel";//人物面板
    protected const string UIPANEL_PACKAGE = "UIPackagePanel";//背包面板
}
