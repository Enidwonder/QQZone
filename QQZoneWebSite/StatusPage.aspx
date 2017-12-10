<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUserMainPage.master" AutoEventWireup="true" CodeFile="StatusPage.aspx.cs" Inherits="StatusPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--写说说--%>
    <div id="divStatusAdd" runat="server" >
        <asp:TextBox ID="txtNewStatus" runat="server" ></asp:TextBox>
        <asp:Button ID="btnAddStatus" runat="server" Text="发布说说" OnClick="btnAddStatus_Click" />
        <asp:DropDownList ID="dplstLimit" runat="server"></asp:DropDownList> <%--权限设置下拉框--%>
    </div>
    <%--显示说说--%>
    <asp:Repeater ID="rptStatusShow" runat="server" OnItemCommand="rptStatusShow_ItemCommand" OnItemDataBound="rptStatusShow_ItemDataBound">
        <ItemTemplate>
            <b>昵称：</b> <%# Eval("nickname") %> <br />
            <b>发布时间:</b> <%# Eval("time") %> <br />
            <b>内容：</b> <%# Eval("statusContent") %> <br />
            <asp:Button ID="btnStatusDel" runat="server" Text="删除" CommandName="btnStatusDel" CommandArgument='<%# Eval("id") %>' />  <%--删除动态按钮--%>
            <asp:Button ID="btnComment" runat="server" Text="评论" CommandName="btnComment"   /> <%-- 评论按钮 --%>
            <asp:Button ID="btnLike" runat="server" Text='<%#"赞"+Eval("likeAmount") %>' CommandName="btnLike"   CommandArgument='<%# Eval("id") %>' /> <%-- 点赞按钮 --%>
            <br />
            <%-- 嵌套，用于评论、回复 --%>    
                <asp:Repeater ID="rptInReply" runat="server" OnItemCommand="rptInReply_ItemCommand" OnItemDataBound="rptInReply_ItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("nickname") %> 评论：<br />
                        <%# Eval("commentContent") %> <br />
                        <asp:Button ID="btnCommentDel" runat="server" Text="删除" CommandName="btnCommentDel" CommandArgument='<%# Eval("id") %>' /> <%--删除评论按钮--%>
                        <asp:Button ID="btnReply" runat="server" Text="回复" CommandName="btnReply"   />
                        <asp:TextBox ID="txtReply" runat="server"  Visible="false"></asp:TextBox>
                        <asp:Button ID="btnAddReply" runat="server"  Text="发表" CommandName="btnAddReply" CommandArgument='<%# Eval("id") %>' Visible="false"  /><br />
                        <asp:Repeater ID="rptInInReply" runat="server"  OnItemCommand="rptInInReply_ItemCommand" OnItemDataBound="rptInInReply_ItemDataBound">
                            <ItemTemplate>
                                <%# Eval("name1") %> 回复：<%# Eval("name2") %><br />
                                回复时间：<%# Eval("Time") %> <br />
                                <%# Eval("commentContent") %> <br />
                                
                                <asp:Button ID="btnReplyDel" runat="server" Text="删除" CommandName="btnReplyDel" CommandArgument='<%# Eval("id") %>' />  <%--删除回复按钮--%>
                                <asp:TextBox ID="txtFatherCommentID" runat="server" Text='<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "id") %>'  Visible="false"></asp:TextBox>
                                <asp:Button ID="btnInReply" runat="server" Text="回复" CommandName="btnInReply" CommandArgument='<%# Eval("id") %>' />
                                <asp:TextBox ID="txtInReply" runat="server" Visible="false"></asp:TextBox>
                                <asp:Button ID="btnInAddReply" runat="server" Text="发表" CommandName="btnInAddReply" CommandArgument='<%# Eval("id") %>' Visible="false" />
                                <br />
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                    </ItemTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
                <%-- 嵌套结束 --%>
                
              <asp:TextBox ID="txtNewComment" runat="server"  Visible ="false"></asp:TextBox>
              <asp:Button ID="btnAddComment" runat="server" Text="发表评论" CommandName="btnAddComment"  Visible="false" CommandArgument='<%# Eval("id") %>'/>
              <asp:Button ID="btnCancelComment" runat="server" Text="取消评论" CommandName="btnCancelComment" Visible="false" />
              <br />
        </ItemTemplate>
    </asp:Repeater>
    <%--返回个人中心--%> 
    <asp:LinkButton ID="lkbtnBack" runat="server" Text="返回个人中心"  PostBackUrl="~/PersonalCenterPage.aspx"></asp:LinkButton>
</asp:Content>

