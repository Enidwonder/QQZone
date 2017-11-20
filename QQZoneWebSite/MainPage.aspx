<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainPage.aspx.cs" Inherits="MainPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <b>欢迎来到QQ空间！请登录...</b><br />
       
        QQ号:
        <asp:TextBox  ID ="txtNumber" runat ="server" ></asp:TextBox>
        <br />
        密码：
        <asp:TextBox ID = "txtPwd" runat = "server"  TextMode="Password"></asp:TextBox>
        <br />
        <asp:RadioButton ID="btnRememberPwd" runat="server" TextAlign="Right" Text="记住密码" />
        <br />
        <asp:Button ID="btnLogin" runat="server" Text="登陆" OnClick="btnLogin_Click" /> <br />
        <asp:HyperLink ID="forgetPwdLINK" runat="server" Text="忘记密码？" NavigateUrl="~/ForgetPwd.aspx" ></asp:HyperLink> <br />
        <asp:Button ID="btnRegister" runat="server" Text="注册" OnClick="btnRegister_Click" />
    </div>
    </form>
</body>
</html>
