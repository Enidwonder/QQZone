using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MainPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CookieOperation cok = new CookieOperation();
        if(HttpContext.Current.Response.Cookies["user"] != null)
        {

            txtNumber.Text = cok.getValue("userNum", "user");
            txtPwd.Text = cok.getValue("password", "user");
        }
        
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        SpecialOperations operate = new SpecialOperations();
        string number = txtNumber.Text;
        string pwd = operate.MD5String(txtPwd.Text); //md5加密
        //SELECT 列名称 FROM 表名称 WHERE 列 运算符 值
        string getPwd;
        SQLOperation sqlOperate = new SQLOperation();
        DataTable dt = new DataTable();
        dt = sqlOperate.select(" password,id ", " Users ", " number='" + number + "'");
        if (dt.Rows.Count == 0) Response.Write("<script> alert('账号不存在');</script> ");
        else if ((getPwd = dt.Rows[0][0].ToString().Trim()) != pwd)
        {
            Response.Write("<script> alert('密码错误！');</script> ");
            txtPwd.Text = null;
        }
        else if ((getPwd = dt.Rows[0][0].ToString().Trim()) == pwd)
        {
            Session["NowUserId"] = dt.Rows[0][1].ToString().Trim();
            if(btnRememberPwd.Checked == true)
            {
                CookieOperation cok = new CookieOperation();
                cok.addUser(number, pwd);
            }
            Response.Write("<script> alert('登陆成功！');</script> ");
        }


    }
}