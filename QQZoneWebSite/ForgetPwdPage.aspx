<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPwdPage.aspx.cs" Inherits="ForgetPwdPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <b>忘记密码：</b><br />
       
        QQ账号：<asp:TextBox ID="txtNumber" runat="server"></asp:TextBox><br />
        验证码：<asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
        <asp:Button ID="btnSendCode" runat="server" Text="发送验证码" OnClick="btnSendCode_Click" /><br />
        设置新密码：<asp:TextBox ID="txtPwdSet" runat="server" ></asp:TextBox><br />
        确认新密码：<asp:TextBox ID="txtPwdSure" runat="server"></asp:TextBox><br />
        <asp:Button ID="btnPwdSet" runat="server" Text="重置密码" OnClick="btnPwdSet_Click"/><br />
        <asp:Button ID="btnBack" runat="server" Text="返回" OnClick="btnBack_Click" />
    </div>
    </form>
</body>
</html>
