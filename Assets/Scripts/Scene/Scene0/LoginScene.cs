using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;//第三方库文件

/*主登录场景控制器，使用SQLite实现登陆注册功能*/

public class LoginScene : Reference
{
    //错误提示
    public Text ErrorText;
    public InputField UserName;
    public InputField Password;
    public UnityEngine.UI.Toggle Agreement;
    public GameObject panel;

   
    //public int number;

    void Start()
    {
        panel.SetActive(false);
        ErrorText.text = "";
      
        //初始化账号表
        InitAccountTable();
        
    }


    void InitAccountTable()//第一次创建数据库时，创建账号表
    {     // 打开数据库
        SQLiteDataHelper.Instance.OpenDataBase();
        //创建一张名为Account的玩家账号表，字段应包含( ID-integer, NAME-TEXT, PWD-TEXT)
        SQLiteDataHelper.Instance.CreateTable("Account",
            new string[] { "NAME", "PWD" },
            new string[] { "TEXT PRIMARY KEY NOT NULL", "TEXT NOT NULL" });
        //关闭数据库
        SQLiteDataHelper.Instance.CloseDataBase();

    }
    //注册账号按钮回调函数
    public void OnRegisterBtnClick()
    {
        //判断用户名和密码是否为空
        if (UserName.text == "")
        {
            ErrorText.text = "用户名不能为空";
            return;
        }
        if (Password.text == "")
        {
            ErrorText.text = "密码不能为空";
            return;
        }
        //首先打开数据库
        SQLiteDataHelper.Instance.OpenDataBase();
        //查询数据库中有无当前账号
        SqliteDataReader reader =
            SQLiteDataHelper.Instance.ReadTableData("Account", new string[] { "NAME" },
                new string[] { "NAME" }, new string[] { "=" },
                new string[] { "'" + UserName.text + "'" }, "AND");
        //如果数据库中没有指定记录，则插入
        if (!reader.HasRows)
        {
            SQLiteDataHelper.Instance.InsertValues("Account",
                new string[] { "'" + UserName.text + "'", "'" + Password.text + "'" });
            ErrorText.text = "成功注册账号";
        }
        //否则，更新指定用户名的密码
        else
        {
            SQLiteDataHelper.Instance.UpdateValues("Account",
                new string[] { "PWD" }, new string[] { "'" + Password.text + "'" },
                "NAME", "=", "'" + UserName.text + "'");
            ErrorText.text = "账号已存在， 已更新了密码!";
        }
        //关闭数据库
        SQLiteDataHelper.Instance.CloseDataBase();
    }
    public void OnLoginBtnClick()//登录按钮回调函数
    {
        //判断用户名和密码是否为空
        if (UserName.text == "")
        {
            ErrorText.text = "用户名不能为空";
            return;
        }
        if (Password.text == "")
        {
            ErrorText.text = "密码不能为空";
            return;
        }
        if (Agreement.isOn==false)
        {
            ErrorText.text = "请确认用户协议";
            return;
        }
        //首先打开数据库
        SQLiteDataHelper.Instance.OpenDataBase();
        //读表获取指定名称的记录
        SqliteDataReader reader = SQLiteDataHelper.Instance.ReadTableData("Account",
            new string[] { "NAME", "PWD" },
            new string[] { "NAME" }, new string[] { "=" }, new string[] { "'" + UserName.text + "'" }, "AND");
        //如果数据库中没有指定记录，则登录失败
        if (!reader.HasRows)
        {
            ErrorText.text = "用户名不存在";
        }
        //否则，获取相应的密码
        else if (reader.Read())
        {
            string pwd = reader.GetString(reader.GetOrdinal("PWD"));
            //如果密码不匹配
            if (pwd != Password.text)
            {
                ErrorText.text = "密码不正确!";
            }
            //否则用户名以及密码均匹配
            else
            {
                SceneController.Instance.LoadScene(SceneType.Select);
            }
        }
        //关闭这个数据库
        SQLiteDataHelper.Instance.CloseDataBase();
    }
    public void OnAgreeBtnClick()
    {
        panel.SetActive(true);
    }
    public void OnCloseBtnClick()
    {
        panel.SetActive(false);
    }


}
