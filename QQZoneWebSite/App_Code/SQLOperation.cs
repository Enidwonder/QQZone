using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// SQLOperation 的摘要说明
/// </summary>
public class SQLOperation
{
    private string SQLstr;
    private SqlConnection sqlconnect;
    public SQLOperation()
    {
        SQLstr = @"server=DESKTOP-CQU2A6I;Integrated Security=SSPI;database=QQZone;";
    }

    public void SQLconnect() //连接数据库
    {
        sqlconnect = new SqlConnection(SQLstr);
    }

    public bool add(string into, string values) //增
    {
        SQLconnect();
        //if(sqlconnect != null)
        // {
        sqlconnect.Open();
        // }

        // insert into into values()
        string sqlCommandStr = "insert into " + into + " values(" + values + ")";
        SqlCommand sqlcmd = new SqlCommand(sqlCommandStr, sqlconnect);
        try
        {
            sqlcmd.ExecuteNonQuery();
            sqlconnect.Close();
            return true;
        }
        catch (Exception e)
        {
            sqlconnect.Close();
            return false;
        }

        
    }

    public bool update(string TableName, string set, string where)//改
    {
        SQLconnect();
        sqlconnect.Open();
        // UPDATE Person SET FirstName = 'Fred' WHERE LastName = 'Wilson' 
        string sqlCommandStr = "update " + TableName + "set " + set + "where" + where;
        SqlCommand sqlcmd = new SqlCommand(sqlCommandStr, sqlconnect);
        try
        {
            sqlcmd.ExecuteNonQuery();
            sqlconnect.Close();
            return true;
        }
        catch (Exception e)
        {
            sqlconnect.Close();
            return false;
        }
    }

    public bool delete(string TableName, string where) //删
    {
        SQLconnect();
        sqlconnect.Open();
        // DELETE FROM Person WHERE LastName = 'Wilson' 
        string sqlCommandStr = "delete from" + TableName + "where" + where;
        SqlCommand sqlcmd = new SqlCommand(sqlCommandStr, sqlconnect);
        try
        {
            sqlcmd.ExecuteNonQuery();
            sqlconnect.Close();
            return true;
        }
        catch (Exception e)
        {
            sqlconnect.Close();
            return false;
        }
        sqlconnect.Close();
    }

    public DataTable select(string select, string TableName, string where) //查
    {
        SQLconnect();
        //SELECT LastName, FirstName FROM Persons
        DataTable dt = new DataTable();
        sqlconnect.Open();
        string sqlCommandStr = "select" + select + "from" + TableName;
        if (where != null && where != "")
            sqlCommandStr = sqlCommandStr + "where" + where;
        // SqlCommand sqlcmd = new SqlCommand(sqlCommandStr, sqlconnect);
        SqlDataAdapter da = new SqlDataAdapter(sqlCommandStr, sqlconnect);
        da.Fill(dt);
        /* try
         {

         }
         catch (Exception e) { result = null; }*/
        sqlconnect.Close();
        return dt;
    }
}