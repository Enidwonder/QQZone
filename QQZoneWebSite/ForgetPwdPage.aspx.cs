using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgetPwdPage : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnSendCode_Click(object sender, EventArgs e)
    {
        string number = txtNumber.Text;
        string getEmail;
        SpecialOperations operate = new SpecialOperations();
        SQLOperation sqlOperate = new SQLOperation();
        DataTable dt = new DataTable();
        if (number == null || number == "")
        {
            Response.Write("<script> alert('请输入账号！');</script> ");
            txtPwdSet.Text = null;
            txtPwdSure.Text = null;
        }
        else
        {
            dt = sqlOperate.select(" email ", " users ", " number='" + number + "'");
            if (dt.Rows.Count == 0) { Response.Write("<script> alert('账号不存在');</script> "); txtNumber.Text = null; }
            else
            {
                getEmail = dt.Rows[0][0].ToString();

                Session["checkCode"] = operate.generateRandomNum(6);
                string body = "你的验证码是" + Session["checkCode"].ToString(); //生成六位验证码发送至邮箱

                if (operate.EmailSend(body, getEmail))
                    Response.Write("<script> alert('验证码已发送至邮箱，请及时查收！');</script> ");
                else
                    Response.Write("<script> alert('验证码发送失败！');</script> ");
            }
        }
    }

    protected void btnPwdSet_Click(object sender, EventArgs e)
    {
        string number = txtNumber.Text;
        if (number == null || number == "")
        {
            Response.Write("<script> alert('请输入账号！');</script> ");
            txtPwdSet.Text = null;
            txtPwdSure.Text = null;
        }
        else
        {
            SQLOperation sqlOperate = new SQLOperation();
            if(Session["checkCode"] == null)
            {
                Response.Write("<script> alert('请先获得验证码！');</script> ");
            }
            else if (txtCode.Text != Session["checkCode"].ToString())
            {
                if (txtCode.Text == null || txtCode.Text == "") Response.Write("<script> alert('请输入六位验证码！');</script> ");
                else
                {
                    Response.Write("<script> alert('验证码错误！');</script> ");
                    txtNumber = null;
                    txtPwdSet = null;
                    txtPwdSure = null;
                }

            }
            else
            {
                if (txtPwdSet.Text != txtPwdSure.Text)
                {
                    Response.Write("<script> alert('新密码不一致！');</script> ");
                    txtPwdSet.Text = null;
                    txtPwdSure.Text = null;
                }
                else if(txtPwdSet.Text == "" || txtPwdSet.Text == null)
                {
                    Response.Write("<script> alert('密码不能为空！');</script> ");
                }
                else
                {
                    SpecialOperations op = new SpecialOperations();

                    string pwd = op.MD5String(txtPwdSet.Text); 
                    sqlOperate.update(" users ", " password = '" + pwd + "'", " number='" + number + "'");
                    Response.Write("<script> alert('密码已修改，请重新登录！');location='MainPage.aspx'</script> ");
                    Session["checkCode"] = null;
                }
            }

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PersonalCenterPage.aspx");
    }
}