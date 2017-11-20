<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <b>注册：</b>
        <br />
        姓名：
        <asp:TextBox ID="txtName" runat ="server" ></asp:TextBox>
        <br />
        性别：
        <asp:RadioButton ID="btnMan" runat="server" Text="男" TextAlign="Right"  GroupName="sex"/>
        <asp:RadioButton ID="btnWoman" runat="server" Text="女" TextAlign="Right" GroupName="sex" />
        <br />
        邮箱：
        <asp:TextBox ID="txtEmail" runat ="server" ></asp:TextBox>
        <asp:Button ID="btnSendEmail" runat="server" Text="发送验证码" OnClick="btnSendEmail_Click" />
        <br />
        验证码：<asp:TextBox ID="txtCode" runat="server" ></asp:TextBox><br />
        设置密码：
        <asp:TextBox ID="txtPwdSet" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        确认密码：
        <asp:TextBox ID="txtPwdSure" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <asp:Button ID="btnRegister" runat="server" Text="注册" OnClick="btnRegister_Click" />
        <asp:Button ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click"  />
        <br />
    </div>
    </form>
</body>
</html>
