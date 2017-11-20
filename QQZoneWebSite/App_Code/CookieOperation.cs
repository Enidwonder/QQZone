using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CookieOperation 的摘要说明
/// </summary>
public class CookieOperation
{
    public CookieOperation()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    //写入
    public void addUser(string number,string pwd)
    {
        HttpCookie newcookie = new HttpCookie("user");
      //  HttpResponse response = new HttpResponse();
        newcookie.Values["userNum"] = number;
        newcookie.Values["password"] = pwd;
        newcookie.Expires = DateTime.Now.AddDays(7); //cookie有效期七天
        HttpContext.Current.Response.Cookies.Add(newcookie);
    }

    //读取
    public string getValue(string subKey,string key)
    {
        if(HttpContext.Current.Response.Cookies[key] != null)
        {
            return HttpContext.Current.Response.Cookies[key][subKey].ToString();
        }
        else
        {
            return null;
        }
    }

    public void change(string key,string subkey,string value)
    {
        if (HttpContext.Current.Response.Cookies[key] != null)
        {
            HttpContext.Current.Response.Cookies[key][subkey] = value;
        }
    }

    public void delete(string key)
    {
        HttpCookie cookie = HttpContext.Current.Response.Cookies[key];
        if (cookie != null)
        {
            cookie.Expires = DateTime.Now.AddDays(-2);
            HttpContext.Current.Response.Cookies.Set(cookie);

        }
    }
    
}

