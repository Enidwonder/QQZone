<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUserMainPage.master" AutoEventWireup="true" CodeFile="DiaryWritingPage.aspx.cs" Inherits="DiaryWritingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    文章标题：
    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
    <div id="divEditor" runat="server">
       
    <script type="text/javascript" charset="utf-8" src="../../content/utf8/ueditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../content/utf8/ueditor.all.min.js"> </script>
   <script type="text/javascript" charset="utf-8" src="../../content/utf8/lang/zh-cn/zh-cn.js"></script>

         文章内容：
     <textarea id="myEditor11" name="myEditor11" runat="server" onblur="setUeditor()" style="width: 1030px; height: 250px;"></textarea>
            <%-- 上面这个style这里是实例化的时候给实例化的这个容器设置宽和高，不设置的话，或默认为auto可能会造成部分显示的情况--%>

            <script type="text/javascript">
                <%--var editor = new baidu.editor.ui.Editor();--%>
                var ue = UE.getEditor('content', {
                    toolbars: [
                        [
                    //自定义的工具栏，自定义增减 
                            'emotion', //表情
                            'redo', //重做
                            'bold', //加粗
                            'indent', //首行缩进
                            'snapscreen', //截图
                            'italic', //斜体
                            'underline', //下划线
                            'fontfamily', //字体
                            'fontsize', //字号
                            'justifyleft', //居左对齐
                            'justifyright', //居右对齐
                            'justifycenter', //居中对齐
                            'justifyjustify', //两端对齐
                            'forecolor', //字体颜色
                        ]
                    ],
                    
                });
                ue.render("<%=myEditor11.ClientID%>");
            </script>

            <script type="text/javascript">
                
                function setUeditor() {
                    var myEditor11 = document.getElementById("myEditor11");
                    myEditor11.value = ue.getContent();//把得到的值给textarea
                }
            </script>
        
    </div>
    <div id="divShow" runat="server" visible="false">
        <asp:Literal ID="ltrShow" runat="server"></asp:Literal>
    </div>
    <div id="divCreate" runat="server">
        <asp:Button ID="btnAdd" runat="server" Text="保存日志" OnClick="btnAdd_Click" />
        <asp:DropDownList ID="dplstClass" runat="server"  ></asp:DropDownList>
        <%--这里还要加一个下拉框用来选择分类--%>
        <asp:DropDownList ID="dplstLimit" runat="server" ></asp:DropDownList> <%--权限设置下拉框--%>
        
    </div>
    <asp:HyperLink ID="hplkBack" runat="server" Text="返回日志列表" NavigateUrl="~/DiaryPage.aspx"></asp:HyperLink>

    <%--下面是评论区--%>
    <div id="divComments" runat="server" >
        <asp:Button ID="btnComment" runat="server" Text="评论" OnClick="btnComment_Click"  /> <%-- 评论按钮 --%>
            <asp:Button ID="btnLike" runat="server" Text='<%#"赞"+Eval("likeAmount") %>' OnClick="btnLike_Click" /> <%-- 点赞按钮 --%>
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
              <asp:Button ID="btnAddComment" runat="server" Text="发表评论" Visible="false" OnClick="btnAddComment_Click"/>
              <asp:Button ID="btnCancelComment" runat="server" Text="取消评论" OnClick="btnCancelComment_Click" Visible="false" />
              <br />
    </div>

</asp:Content>

