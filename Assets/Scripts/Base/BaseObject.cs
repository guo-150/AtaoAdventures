using UnityEngine;

/*基础游戏对象类，是游戏中所有游戏对象的父类，包含了一些基础的公有字段*/
public abstract class BaseObject : Reference 
{
    //游戏对象的ID
    public int id;
    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    //游戏对象的名称
    public string name1;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    //游戏对象的描述信息
    public string descript;
    public string Descript
    {
        get { return descript; }
        set { descript = value; }
    }
}
