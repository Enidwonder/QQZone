using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// SpecialOperations 的摘要说明
/// </summary>
public class SpecialOperations : System.Web.UI.Page
{
   // System.Web.UI.Page page;
    public SpecialOperations()
    {
        
    }
    public bool onlyFriendsJudge(string visitorID,string visitedID) //仅好友访问的权限判断
    {
        SQLOperation sql = new SQLOperation();
        DataTable dt = sql.select(" * ", " friends ", " (userID= " + visitorID + " and friendID=" + visitedID + ") or (userID=" + visitedID + " and friendID=" + visitorID + ")");
        if(dt.Rows.Count == 0) //没有好友关系，不允许访问
        {
            return false;
        }
        else //存在好友关系，可以访问
        {
            return true;
        }
    }

    public bool someFriendsCantSee(string visitorID,string visitedID,string combinedID,string kind) //部分好友不可见的权限判断
    {
        bool isFriend = onlyFriendsJudge(visitorID, visitedID);
        if (!isFriend)
        {
            return false;
        }
        else
        {
            SQLOperation sql = new SQLOperation();
            DataTable dt = sql.select(" * ", " whocantsee ", " visitorID= " + visitorID + " and visitedID=" + visitedID + " and combinedID=" + combinedID + " and kind='" + kind + "'");
            if(dt.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void txtJudge(TextBox txt, System.Web.UI.Page page)
    {
        if(txt == null)
        {
            alertHelper(page,"有为空内容，操作失败！");
        }
    }

    public void txtJudge(TextBox txt,int lengthLimit, System.Web.UI.Page page)
    {
        txtJudge(txt,page);
        if(txt.Text.Length > lengthLimit)
        {
            alertHelper(page,"有超出规定长度，操作失败！");
        }
    }

    public void alertHelper(System.Web.UI.Page page,string content)
    {
        page.Response.Write("<script> alert('" + content + "');</script> ");
    }

    public void alertHelper(System.Web.UI.Page page,string content,string toLocation)
    {
        page.Response.Write("<script> alert('"+content+"');location=  '"+toLocation+"'</script> ");

    }

    public string MD5String(string text1) //MD5对字符串进行加密
    {
        //MD5 md5 = new MD5CryptoServiceProvider();
        //byte[] text2 = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(text1));
        string result = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(text1, "MD5");
        return result;
    }

    public bool EmailSend(string body, string toEmail)
    {
        MailMessage msg = new MailMessage();

        msg.To.Add(toEmail);//收件人地址 
                            // msg.CC.Add("enid1999@163.com");//抄送人地址 

        msg.From = new MailAddress("enid1999@163.com", "Library");//发件人邮箱，名称 

        msg.Subject = "Library验证码";//邮件标题 
        msg.SubjectEncoding = Encoding.UTF8;//标题格式为UTF8 

        msg.Body = body;//邮件内容 
        msg.BodyEncoding = Encoding.UTF8;//内容格式为UTF8 

        SmtpClient client = new SmtpClient();

        client.Host = "smtp.163.com";//SMTP服务器地址 
        client.Port = 25;//SMTP端口，QQ邮箱填写587 

        client.EnableSsl = true;//启用SSL加密 
                                //发件人邮箱账号，授权码(注意此处，是授权码你需要到qq邮箱里点设置开启Smtp服务，然后会提示你第三方登录时密码处填写授权码)
        client.Credentials = new System.Net.NetworkCredential("enid1999@163.com", "Ekpm5T");

        try
        {
            client.Send(msg);//发送邮件
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public bool not_equal(string text1, string text2) //检验两个字符串是否相同
    {
        if (text1 != text2) return true;
        else return false;
    }

    public bool overLength(string text, int limit) //检验是否超长
    {
        if (text.Length > limit) return true;
        else return false;
    }

    public bool non_string_existed(string text, string judge) //检验是否不存在一个特殊的判断字符
    {
        int j = text.IndexOf(judge);
        if (j == -1) return true;
        else return false;
    }

    public bool nullString(string text) //判断一个字符串是否为空
    {
        if (text != "" && text != null) return false;
        else return true;
    }

    public string generateRandomNum(int length)
    {
        char[] constant =
        {
        '0','1','2','3','4','5','6','7','8','9'
        //'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
      //  'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
         };

        System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
        Random rd = new Random();
        for (int i = 0; i < length; i++)
        {
            newRandom.Append(constant[rd.Next(10)]);
        }
        return newRandom.ToString();
    }
}