<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUserMainPage.master" AutoEventWireup="true" CodeFile="AlbumPage.aspx.cs" Inherits="AlbumPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnNewAlbum" runat="server" Text="创建相册" OnClick="btnNewAlbum_Click" /><br />
    <div id="divAddPhoto" runat="server" >
        <asp:Button ID="btnAddPhoto" runat="server" Text="上传照片" OnClick="btnAddPhoto_Click" />
        <asp:Button ID="btnUploading" runat="server" Text="确定上传" OnClick="btnUploading_Click"  Visible="false"/>
        <asp:FileUpload ID="fupAddPhoto" runat="server"  Visible="false"/>
        <asp:DropDownList ID="dplstAlbums" runat="server" Visible="false" ></asp:DropDownList> <%--用来显示上传到哪个相册--%>
    </div>
    
    <div id="divNewAlbum" runat="server" > <%--创建新相册--%>
        <asp:Label ID="lblNewname" runat="server" Text="新相册名称"></asp:Label>
        <asp:TextBox ID="txtNewName" runat="server"  ></asp:TextBox> <%--新相册名称--%> <br />
        <asp:Label ID="lblNewDescript" runat="server" Text="新相册简介"></asp:Label>
        <asp:TextBox ID="txtNewDescipt" runat="server" ></asp:TextBox>  <%--新相册简介--%><br />
        <asp:DropDownList ID="dplstLimit" runat="server" OnSelectedIndexChanged="dplstLimit_SelectedIndexChanged"  ></asp:DropDownList> <%--新相册权限设置--%>
        <asp:CheckBoxList ID="ckbxLimit" runat="server"  Visible="false"></asp:CheckBoxList>
        <asp:Button ID="btnAddNew" runat="server"  Text="立刻创建" OnClick="btnAddNew_Click" />  <%--点击即可创建--%>
    </div>
   
    <div id="divAllAlbums" runat="server" > <%--显示所有相册--%>
        <asp:Repeater ID="rptAllAlbum" runat="server" OnItemDataBound="rptAllAlbum_ItemDataBound" OnItemCommand="rptAllAlbum_ItemCommand">
            <ItemTemplate>
                <%# Eval("AlbumName") %>
                <asp:ImageButton ID="imgBtnAlbum" runat="server" ImageUrl="~/images/default.png" PostBackUrl='<%# "AlbumPage.aspx?albumId="+Eval("id")%>' CommandName="imgBtnAlbum" />
                <asp:Button ID="btnDelAlbum" runat="server" Text="删除相册" CommandName="btnDelAlbum"  CommandArgument='<%# Eval("id") %>'/> <%--删除相册按钮--%>
                <asp:Button ID="btnCommentAlbum" runat="server" Text="评论相册" CommandName="btnCommentAlbum" /> <%--评论相册按钮--%>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div id="divPagesForAll" runat="server">
        <asp:Button ID="btnUp" runat="server" Text="上一页" OnClick="btnUp_Click" />
        <asp:Button ID="btnDown" runat="server" Text="下一页"  OnClick="btnDown_Click"/>
        <asp:Button ID="btnFirst" runat="server" Text="首页"  OnClick="btnFirst_Click"/>
        <asp:Button ID="btnLast" runat="server" Text="尾页"  OnClick="btnLast_Click"/>
        页次<asp:Label ID="lblNow" runat="server" ></asp:Label>
        /<asp:Label ID="lblTotal" runat="server" ></asp:Label>
        转<asp:TextBox ID="txtJump" runat="server" ></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="Go"  OnClick="btnJump_Click"/>
    </div>



    <div id="divShowPhotos" runat="server" visible="false"> <%--显示照片--%>
        <asp:Repeater ID="rptPhotos" runat="server" OnItemCommand="rptPhotos_ItemCommand">
            <ItemTemplate>
<%--                <asp:Image id="img" runat="server"  ImageUrl='<%# Eval("picture") %>' />--%>
                <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("picture") %>' Width="50px" Height="50px" />
                <asp:Button ID="btnDelPhoto" runat="server" Text="删除照片" CommandName="btnDelPhoto" CommandArgument='<%# Eval("id") %>' />
                <asp:Button ID="btnLikePhoto" runat="server" Text="赞" CommandName="btnLikePhoto" CommandArgument='<%# Eval("id") %>' />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div id="divPagesForPhoto" runat="server" visible="false">
        <asp:Button ID="btnUpPhoto" runat="server" Text="上一页"  OnClick="btnUpPhoto_Click" />
        <asp:Button ID="btnDownPhoto" runat="server" Text="下一页"  OnClick="btnDownPhoto_Click"/>
        <asp:Button ID="btnFirstPhoto" runat="server" Text="首页" OnClick="btnFirstPhoto_Click"/>
        <asp:Button ID="btnLastPhoto" runat="server" Text="尾页"  OnClick="btnLastPhoto_Click"/>
        页次<asp:Label ID="lblNowPhoto" runat="server" ></asp:Label>
        /<asp:Label ID="lblTotalPhoto" runat="server" ></asp:Label>
        转<asp:TextBox ID="txtJumpPhoto" runat="server" ></asp:TextBox>
        <asp:Button ID="btnJumpPhoto" runat="server" Text="Go"  OnClick="btnJumpPhoto_Click"/>
    </div>
    <asp:HyperLink ID="hplkBack" runat="server" Text="返回我的个人中心" NavigateUrl="~/PersonalCenterPage.aspx"></asp:HyperLink>
  
</asp:Content>

