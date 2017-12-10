<%@ Page Title="" Language="C#" MasterPageFile="~/MasterUserMainPage.master" AutoEventWireup="true" CodeFile="DiaryPage.aspx.cs" Inherits="DiaryPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnBackMe" runat="server" Text="返回我的个人中心" OnClick="btnBackMe_Click" />
    <div id="divUsers" runat="server">
        <asp:Button ID="btnNewDiary" runat="server" Text="写日志" OnClick="btnNewDiary_Click" /> 
        <asp:Button ID="btnNewClass" runat="server" Text="添加分类" OnClick="btnNewClass_Click" />
    </div>
    <div id="divNewClass" runat="server" visible="false" >
        新的分类名称：
        <asp:TextBox ID="txtAddClass" runat="server" ></asp:TextBox>
        <asp:Button ID="btnAddClass" runat="server" Text="添加" OnClick="btnAddClass_Click" />
    </div>


    <div id="divAllDiaries" runat="server" >
        <asp:Repeater ID="rptAllDiaries" runat="server" OnItemCommand="rptAllDiaries_ItemCommand"  OnItemDataBound="rptAllDiaries_ItemDataBound"  >
            <HeaderTemplate>
                <table>
                    <tr>
                        <th>日志标题</th>
                        <th>发表时间</th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="lkbtnDiary" runat="server" PostBackUrl='<%#"DiaryWritingPage.aspx?articleId="+Eval("Id")+"&op=visit" %>' Text='<%# Eval("title") %>'  CommandName="lkbtnDiary"></asp:LinkButton>
                <%# Eval("time") %>
                <asp:LinkButton ID="lkbtnEdit"  Text="编辑" runat="server" CommandName="lkbtnEdit" PostBackUrl='<%#"DiaryWritingPage.aspx?articleId="+Eval("id")+"&op=edit"%>' />
                <asp:Button ID="btnDel" runat="server" Text="删除" CommandName="btnDel" CommandArgument='<%# Eval("id") %>' />
                
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div id="divPagesForAll" runat="server">
        <asp:Button ID="btnUp" runat="server" Text="上一页"  OnClick="btnUp_Click"/>
        <asp:Button ID="btnDown" runat="server" Text="下一页"  OnClick="btnDown_Click"/>
        <asp:Button ID="btnFirst" runat="server" Text="首页" OnClick="btnFirst_Click"/>
        <asp:Button ID="btnLast" runat="server" Text="尾页"  OnClick="btnLast_Click"/>
        页次<asp:Label ID="lblNow" runat="server" ></asp:Label>
        /<asp:Label ID="lblTotal" runat="server" ></asp:Label>
        转<asp:TextBox ID="txtJump" runat="server" ></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="Go"  OnClick="btnJump_Click"/>
    </div>
    <div id="divAllClasses" runat="server" >
        <asp:Repeater ID="rptClasses" runat="server" OnItemCommand="rptClasses_ItemCommand">
            <HeaderTemplate>
                <table>
                    <tr>
                        <th> 分类名称 </th>
                        
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="hyptClass" runat="server" Text='<%# Eval("Name") %>' CommandName="hyptClass" CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
                ( <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("amount") %>'></asp:Label> )
                <asp:Button ID="btnDelClass" runat="server" Text="删除分类" CommandName="btnDelClass" CommandArgument='<%# Eval("id") %>' />
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        
    </div>
   
    
</asp:Content>

