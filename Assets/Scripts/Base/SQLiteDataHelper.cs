using UnityEngine;
using System;
//使用sqlite3.dll第三方库文件
using Mono.Data.Sqlite;

/*SQLite底层功能封装类，对外部提供操作数据库接口*/
public class SQLiteDataHelper 
{
    //数据库名称，为了操作多个数据库
    string dbName;
    public string DBName
    {
        get { return DBName; }
    }
    //单例模式
    static SQLiteDataHelper instance;
    public static SQLiteDataHelper Instance
    {
        get
        {
            if (instance == null)
                instance = new SQLiteDataHelper();
            return instance;
        }
    }
    private SQLiteDataHelper() { }

    //数据库连接定义
    SqliteConnection dbConnect;
    //SQL语句命令
    SqliteCommand dbCmd;
    //查询结果集（数据读取器）
    SqliteDataReader dbReader;

    //打开数据库
    public void OpenDataBase(string _dbName = "gamedb")
    {
        if(dbName != null)
        {
            Debug.Log("数据库" + dbName + "正处于打开状态!");
            return;
        }
        try
        {
            //定义数据库连接字符串
            string connectString = "data source=" + Application.dataPath 
                + "/DataBase/" + _dbName + ".db";
            //构建以及打开数据库连接
            dbConnect = new SqliteConnection(connectString);
            //打开数据库，如果不存在则创建新的
            dbConnect.Open();
            //保存数据库名称
            dbName = _dbName;
            Debug.Log("打开数据库" + dbName + "成功!");
        }
        catch(Exception _ex)
        {
            Debug.Log("连接数据库失败: " + _ex.Message);
            dbName = null;
        }
    }

    //关闭数据库
    public void CloseDataBase()
    {
        if (dbName == null)
            return;
        //销毁指令对象
        if(dbCmd != null)
        {
            dbCmd.Cancel();
            dbCmd = null;
        }
        //销毁结果集对象
        if(dbReader != null)
        {
            dbReader.Close();
            dbReader = null;
        }
        //销毁连接对象
        if(dbConnect != null)
        {
            dbConnect.Close();
            dbConnect = null;
        }
        dbName = null;
        Debug.Log("已成功关闭数据库" + dbName);
    }

    //执行一条SQL命令
    public SqliteDataReader ExecuteQuery(string _queryString)
    {
        //判断数据库状态是否关闭
        if(dbName == null)
        {
            Debug.Log("当前没有打开任何数据库，请先打开数据库!");
            return null;
        }
        //创建一个指令对象
        dbCmd = dbConnect.CreateCommand();
        //指定命令对象要执行的SQL语句
        dbCmd.CommandText = _queryString;
        //执行dbCmd指令并返回查询结果
        dbReader = dbCmd.ExecuteReader();
        return dbReader;
    }

    /*
    CREATE TABLE IF NOT EXISTS Player (
                                        ID       INTEGER,
                                        NicName  TEXT,
                                        Age      INTEGER,
                                        Descript TEXT
                                  );
     */
    //创建数据表
    //参数：表名、所有字段名字、所有字段类型
    public SqliteDataReader CreateTable(string _tName, string[] _colNames, string[] _colTypes)
    {
        //判空
        if(_tName == null || _colNames == null || _colTypes == null)
        {
            Debug.Log("参数一律不能为空，创建失败!");
            return null;
        }
         //字段长度必须一致
        if(_colNames.Length != _colTypes.Length)
        {
            Debug.Log("字段长度不一致，创建失败!");
            return null;
        }
        //拼接创建表SQL语句字符串
        string sqlString = "CREATE TABLE IF NOT EXISTS "
            + _tName + "(" + _colNames[0] + " " + _colTypes[0];
        //拼接后面的字段
        for(int i = 1; i < _colNames.Length; i++)
        {
            sqlString += ", " + _colNames[i] + " " + _colTypes[i];
        }
        sqlString += ")";
        return ExecuteQuery(sqlString);
    }

    /*
     * SELECT * FROM Person;
     */
    //读取整张数据表
    //参数：表名
    public SqliteDataReader ReadFullTable(string _tName)
    {
        string sqlString = "SELECT * FROM " + _tName;
        return ExecuteQuery(sqlString);
    }

    /*
     * INSERT INTO Person VALUES (5, '詹姆斯', 24, '拉斯维加斯', 10000.00);
     */
     //向数据表中插入记录
     //参数：表名、所有字段的值
    public SqliteDataReader InsertValues(string _tName, string[] _colValues)
    {
        //判空
        if (_tName == null || _colValues == null)
        {
            Debug.Log("参数一律不能为空，创建失败!");
            return null;
        }
        //首先获取表中字段的个数
        int fieldCount = ReadFullTable(_tName).FieldCount;
        //字段长度必须一致
        if(_colValues.Length != fieldCount)
        {
            Debug.Log("插入数据与字段个数不一致!");
            return null;
        }
        //拼接插入SQL字符串
        string sqlString = "INSERT INTO " + _tName + " VALUES(" + _colValues[0];
        for(int i = 1; i <_colValues.Length; i++)
        {
            sqlString += ", " + _colValues[i];
        }
        sqlString += ")";
        return ExecuteQuery(sqlString);
    }

    /*
     * UPDATE Person SET ADDRESS = '纽约', SALARY = 30000.00 WHERE NAME = '詹姆斯';
     */
     //更新数据表中指定的记录
     //参数：表名、待修改字段名、待修改字段值、条件子句左操作数、运算符、右操作数
     public SqliteDataReader UpdateValues(string _tName, string[] _colNames, 
                        string[] _colValues, string _left, string _oper, string _right)
    {
        //判空
        if (_tName == null || _colNames == null || _colValues == null ||
            _left == null || _oper == null || _right == null)
        {
            Debug.Log("参数一律不能为空，创建失败!");
            return null;
        }
        //字段长度必须一致
        if(_colNames.Length != _colValues.Length)
        {
            Debug.Log("字段长度不一致，更新失败!");
            return null;
        }
        //拼接UPDATE语句
        string sqlString = "UPDATE " + _tName + " SET " + _colNames[0] + "=" + _colValues[0];
        for (int i = 1; i < _colValues.Length; i++)
        {
            sqlString += ", " + _colNames[i] + "=" + _colValues[i];
        }
        sqlString += " WHERE " + _left + _oper + _right;
        return ExecuteQuery(sqlString);
    }

    /*
     * DELETE FROM Person WHERE ID = 5;
     */
    //删除数据表中指定的记录
    //参数：表名、条件子句左操作数、运算符、右操作数
    public SqliteDataReader DeleteValues(string _tName, string _left, string _oper, string _right)
    {
        //判空
        if (_tName == null || _left == null || _oper == null || _right == null)
        {
            Debug.Log("参数一律不能为空，创建失败!");
            return null;
        }
        //拼接DELETE语句字符串
        string sqlString = "DELETE FROM " + _tName + " WHERE " + _left + _oper + _right;
        return ExecuteQuery(sqlString);
    }

    /*
     * SELECT * FROM Person WHERE AGE >= 25 AND SALARY >= 65000.00;
     */
     //查询检索数据表中指定的记录
     //参数：表名、查询字段名列表、条件左值列表、运算符列表、条件右值列表、逻辑运算符
     public SqliteDataReader ReadTableData(string _tName, string[] fieldNames, 
         string[] _colNames, string[] _opers, string[] _colValues, string _mode)
    {
        //判空
        if (_tName == null || fieldNames == null || _colNames == null || _opers == null ||
            _colValues == null || _mode == null)
        {
            Debug.Log("参数一律不能为空，创建失败!");
            return null;
        }
        //字段长度必须一致
        if (_colNames.Length != _opers.Length || _opers.Length != _colValues.Length)
        {
            Debug.Log("字段长度不一致，更新失败!");
            return null;
        }
        //拼接SELECT查询语句
        string sqlString = "SELECT " + fieldNames[0];
        for(int i = 1; i < fieldNames.Length; i++)
        {
            sqlString += ", " + fieldNames[i];
        }
        sqlString += " FROM " + _tName + " WHERE " + 
            _colNames[0] + _opers[0] + _colValues[0];
        for(int i = 1; i < _colNames.Length; i++)
        {
            sqlString += " " + _mode + " " + _colNames[i] + _opers[i] + _colValues[i];
        }
        return ExecuteQuery(sqlString);
    }

    /*
     * DELETE FROM Person;
     */
     //清空表中所有记录
     //参数：表名
     public SqliteDataReader ClearTable(string _tName)
    {
        string sqlString = "DELETE FROM " + _tName;
        return ExecuteQuery(sqlString);
    }
}
