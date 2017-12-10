using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        SpecialOperations operate = new SpecialOperations();
        string number = operate.generateRandomNum(10);

        string sex;
        if (btnMan.Checked) sex = btnMan.Text;
        else if (btnWoman.Checked) sex = btnWoman.Text;
        else sex = null;
        
        string checkCode = Session["checkCode"].ToString();

        bool emptyJudge = operate.nullString(txtName.Text) || operate.nullString(sex) || operate.nullString(txtEmail.Text) || operate.nullString(txtCode.Text) || operate.nullString(txtPwdSet.Text) || operate.nullString(txtPwdSure.Text);
        bool overLengthJudge = operate.overLength(txtName.Text, 20) || operate.overLength(txtEmail.Text, 20) || operate.overLength(txtCode.Text, 6) || operate.overLength(txtPwdSet.Text, 10);
        if (emptyJudge) Response.Write("<script> alert('有内容尚未完成！');</script> ");
        else if (overLengthJudge)
        {
            Response.Write("<script> alert('有内容超出限定长度！');</script> ");

        }
        else if (operate.not_equal(txtPwdSet.Text, txtPwdSure.Text)) Response.Write("<script> alert('两次输入的密码不一致');</script> ");
        else if (operate.not_equal(checkCode, txtCode.Text)) Response.Write("<script> alert('验证码错误');</script> ");
        else
        {
            SQLOperation sql = new SQLOperation();
            //    sql = new SQLOperation();
            string nickname = txtName.Text;
            string password = operate.MD5String(txtPwdSet.Text);
            string loginstatus = "下线";
            string email = txtEmail.Text;
            //' nickname  ','number','password','loginstatus','name','sex','age','headpicture','email','birthday'
            string values = "N'" + nickname + "', " + "N'" + number + "','" + password + "',N'" + loginstatus + "','" + null + "',N'" + sex + "','" + null + "',' ~\\images\\default.png ','" + email + "','" + null + "'";
            
            if(sql.add(" users ", values))
            {
                DataTable dt = sql.select(" id ", " users ", " number = '" + number + "'");
                string id = dt.Rows[0][0].ToString();
                string zoneDefultValue = id + ",N'" + nickname + "的空间'"; //give a default name for zone
                //userid,name,classkind,amount
                sql.add(" classes ", " " + id + " ,'所有日志','日志',0,'F'");//注册就得到一个存放所有日志的默认分类
                sql.add(" a_album ", " " + id + ",'所有照片',0,null,null,'F'");//注册的到一个默认相册
                if(sql.add(" zoneInfo ", zoneDefultValue))
                {
                    Response.Write("<script> alert('注册成功你的账号是" + number + "');location=  'MainPage.aspx'</script> ");
                }
                
            }
            else
            {
                Response.Write("<script> alert('注册失败');</script>");
            }

            
            /*全部清空*/
            txtEmail.Text = null;
            txtCode.Text = null;
          
            txtPwdSet.Text = null;
            txtPwdSure.Text = null;
            txtName.Text = null;
        
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {

    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        SpecialOperations operate = new SpecialOperations();
        string checkCode = operate.generateRandomNum(6);
        Session["checkCode"] = checkCode;
        string body = "你的验证码是" + checkCode; //生成六位验证码发送至邮箱
        string toEmail;
        if (txtEmail.Text != null && txtEmail.Text != "" && !operate.non_string_existed(txtEmail.Text, "@") && !operate.non_string_existed(txtEmail.Text, ".com"))
        {
            toEmail = txtEmail.Text;
            if (operate.EmailSend(body, toEmail))
                Response.Write("<script> alert('验证码已发送至邮箱，请及时查收！');</script> ");
            else
                Response.Write("<script> alert('验证码发送失败！');</script> ");
        }
        else
        {
            Response.Write("<script> alert('邮箱格式不正确');</script> ");
            txtEmail.Text = null;
        }
    }
}