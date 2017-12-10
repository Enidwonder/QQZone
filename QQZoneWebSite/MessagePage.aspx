<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUserMainPage.master" AutoEventWireup="true" CodeFile="MessagePage.aspx.cs" Inherits="MessagePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%--添加留言--%>
    <div id="divAddMessage" runat="server" >
        <asp:TextBox ID="txtMessageContent" runat="server" Text="请在这里留言" ></asp:TextBox>
        <asp:Button ID="btnMessageAdd" runat="server" Text="添加留言" OnClick="btnMessageAdd_Click" />
       权限设置：<asp:DropDownList ID="dplstLimit" runat="server" ></asp:DropDownList> 
    </div>

    <%--显示留言--%>
    <div id="divMessageShow" runat="server">
        <asp:Repeater ID="rptMessageShow" runat="server" OnItemCommand="rptMessageShow_ItemCommand" OnItemDataBound="rptMessageShow_ItemDataBound" >
            <ItemTemplate>
                <b>昵称：</b> <%# Eval("nickname") %> <br />
               <b>留言时间:</b> <%# Eval("time") %> <br />
               <b>内容：</b> <%# Eval("messageContent") %> <br />
                <asp:Button ID="btnMessageDel" runat="server" Text="删除" CommandName="btnMessageDel" CommandArgument='<%# Eval("id") %>' />  <%--删除动态按钮--%>
                <asp:Button ID="btnComment" runat="server" Text="回复" CommandName="btnComment"   /> <%-- 用户回复留言按钮 --%>

                <%-- 嵌套，用于评论、回复 --%>
              
                <asp:Repeater ID="rptReply" runat="server" OnItemCommand="rptReply_ItemCommand" OnItemDataBound="rptReply_ItemDataBound">
                    <ItemTemplate>
                        <%# Eval("nickname") %> 评论：<br />
                        <%# Eval("commentContent") %> <br />
                        <asp:Button ID="btnCommentDel" runat="server" Text="删除" CommandName="btnCommentDel" CommandArgument='<%# Eval("id") %>' /> <%--删除评论按钮--%>
                        
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
                <%-- 嵌套结束 --%>
                
                    <asp:TextBox ID="txtNewComment" runat="server"  Visible ="false"></asp:TextBox>
                    <asp:Button ID="btnAddComment" runat="server" Text="发表评论" CommandName="btnAddComment"  Visible="false" CommandArgument='<%# Eval("id") %>'/>
                    <asp:Button ID="btnCancelComment" runat="server" Text="取消评论" CommandName="btnCancelComment" Visible="false" />
               <br />
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <%--返回个人中心--%>
    <asp:LinkButton ID="lkbtnBack" runat="server" Text="返回个人中心" PostBackUrl="~/PersonalCenterPage.aspx"></asp:LinkButton>
</asp:Content>

