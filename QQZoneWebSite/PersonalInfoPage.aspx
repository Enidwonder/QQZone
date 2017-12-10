<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUserMainPage.master" AutoEventWireup="true" CodeFile="PersonalInfoPage.aspx.cs" Inherits="PersonalInfoPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    QQ空间----个人档
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <asp:Image ID="imgHead" runat="server" />
    
    
    昵称：<asp:TextBox ID="txtNickName" runat="server" ReadOnly="true"></asp:TextBox><br />
    姓名：<asp:TextBox ID="txtName" runat="server" ReadOnly="true"></asp:TextBox><br />
    性别：<asp:TextBox ID="txtSex" runat="server" ReadOnly="true"></asp:TextBox><br />
    年龄：<asp:TextBox ID="txtAge" runat="server" ReadOnly="true"></asp:TextBox><br />
    邮箱：<asp:TextBox ID="txtEmail" runat="server" ReadOnly="true"></asp:TextBox><br />
    生日：<asp:TextBox ID="txtBirthday" runat="server" ReadOnly="true"></asp:TextBox><br />
    <asp:HyperLink ID="hylkBack" runat="server" Text="返回个人中心" NavigateUrl="~/PersonalCenterPage.aspx" ></asp:HyperLink>
    <asp:Calendar ID="calBirthday" runat="server" Visible="false" OnSelectionChanged="calBirthday_SelectionChanged" ></asp:Calendar>
    <div id="divChanges" runat="server" visible="false">
        <asp:Button ID="btnChangeInfo" runat="server" Text="修改个人信息" OnClick="btnChangeInfo_Click" />
        <asp:Button ID="btnChangePhoto" runat="server" Text="修改头像" OnClick="btnChangePhoto_Click" />

    </div>
    <div id="divUpload" runat="server" visible="false">
        <asp:Button ID="btnUpload" runat="server" Text="上传" OnClick="btnUpload_Click" />
        <asp:FileUpload ID="fileUpHead" runat="server" />
    </div>
    <div id="divSaveChange" runat="server" visible="false">
        <asp:Button ID="btnSave" runat="server" Text="保存修改" OnClick="btnSave_Click" />
    </div>
    <div id="divChangeZone" runat="server" >
        <asp:Button ID="btnZoneChange" runat="server" Text="空间设置修改" OnClick="btnZoneChange_Click" /><br />
        空间标题：<asp:TextBox ID="txtZoneTitle" runat="server" Visible="false" ></asp:TextBox><br />
        权限设置：<asp:DropDownList ID="dplstLimit" runat="server" Visible="false" ></asp:DropDownList><br />
        <asp:Button ID="btnZoneSave" runat="server" Text="保存修改"  Visible="false" OnClick="btnZoneSave_Click" />
    </div>
</asp:Content>

