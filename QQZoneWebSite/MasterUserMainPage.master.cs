using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterUserMainPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["NowUserId"] != null) //if not logged in,you can't do anything
            {
                if (Request.QueryString["id"] == null) //not visiting 
                {
                    /*----------控件可见性--------------*/
                    divAddSearch.Visible = true;
                    /*-----------------取值赋值---------------*/
                    string userId = Session["NowUserId"].ToString();
                    SQLOperation sql = new SQLOperation();
                    DataTable dt = sql.select(" title ", " zoneInfo ", " UserId =" + userId);
                    lblTitle.Text = dt.Rows[0][0].ToString().Trim();
                    dt = sql.select(" headPicture ", " users ", " id= " + userId);
                    string imageUrl = dt.Rows[0][0].ToString();
                    imgHead.Src = imageUrl.Trim(); //notice:when url firstly got, it adds blanks,so you must use Trim（）to delete blanks
                }
                else //visiting
                {
                    /*----------------控件可见性--------------------*/
                    hylkAllFriends.Visible = false;
                    btnAllApplication.Visible = false;
                    divAddSearch.Visible = false;
                    /*---------------取值赋值----------------------*/
                    string visitId = Request.QueryString["id"].ToString();
                    SQLOperation sql = new SQLOperation();
                    DataTable dt = sql.select(" title ", " zoneInfo ", " UserId =" + visitId); //获取被访问者的空间信息
                    lblTitle.Text = dt.Rows[0][0].ToString().Trim();
                    dt = sql.select(" headPicture ", " users ", " id= " + visitId);
                    string imageUrl = dt.Rows[0][0].ToString();
                    /*-------------改变跳转路径--------------------*/
                    hylkDiary.NavigateUrl = "~/DiaryPage.aspx?id=" + visitId;
                    hylkAlbum.NavigateUrl = "~/AlbumPage.aspx?id=" + visitId;
                    hylkMessage.NavigateUrl = "~/MessagePage.aspx?id=" + visitId;
                    hylkInfo.NavigateUrl = "~/PersonalInfoPage.aspx?id=" + visitId;
                    hylkStatus.NavigateUrl = " ~/ StatusPage.aspx?id=" + visitId;
                    //imgHead.ImageUrl = imageUrl.Trim(); //notice:when url firstly got, it adds blanks,so you must use Trim（）to delete blanks
                }
            }
            else
            {
                Response.Write("<script> alert('您尚未登陆！');location='MainPage.aspx'</script> ");
            }
        }
        
    }

   /* protected void btnAddFriend_Click(object sender, EventArgs e)
    {
        divAddSearch.Visible = true;
    }
    */
    protected void btnSearch_Click(object sender, EventArgs e) //查找按钮
    {
        SQLOperation sql = new SQLOperation();
        RepeaterOperate rptHelper = new RepeaterOperate();

        string text = txtSearch.Text;
        if(txtSearch == null)
        {
            txtSearch.Text = "请填写内容！";
        }
        else
        {
            DataTable dt = sql.select(" nickname,loginstatus,id,number ", " users ", " number= '" + text + "' or nickname='" + text + "'"); //nickname or number to find
            if(dt == null)
            {
                txtSearch.Text = "没有相匹配的用户";
            }
            else
            {
                lblNowResult.Text = "1";
                rptHelper.dataBound(ref rptSearchResult, ref dt, 1,ref lblTotalResult, 5); 
            }
        }

        divSearchResult.Visible = true;
        divResearchPages.Visible = true;
        rptSearchResult.Visible = true;
    }

    protected void btnLogout_Click(object sender, EventArgs e) //注销按钮
    {
        SQLOperation sql = new SQLOperation();

        string id = Session["NowUserId"].ToString();
        sql.update(" users ", " loginStatus =N'下线'", " id=" + id);
        Session["NowUserId"] = null;
        Response.Redirect("~/MainPage.aspx");
    }

    protected void hylkAllFriends_Click(object sender, EventArgs e) //所有好友的显示
    {
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();

        if (divAllFriends.Visible)
        {
            hylkAllFriends.Text = "查看好友";
            divAllFriends.Visible = false;
        }
        else
        {
            string id;

            id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friends.friendId,users.nickname,users.loginstatus ", " friends,users ", " friends.userId= " + id + " and friends.friendId = users.id ");
            if(dt.Rows.Count == 0)
            {
                dt = sql.select(" friends.userId,users.nickname,users.loginstatus ", " friends,users ", " friends.friendId= " + id + " and friends.friendId = users.id ");
                dt.Columns["userId"].ColumnName = "friendId";
            }
            rptHelper.dataBound(ref rptFriends, ref dt, 1, ref lblTotal, 5);
            lblNow.Text = "1";
            hylkAllFriends.Text = "收起";

            divAllFriends.Visible = true;
        }

    }

    protected void rptFriends_ItemCommand(object source, RepeaterCommandEventArgs e) //所有好友的结果显示--按钮判断
    {
        SQLOperation sql = new SQLOperation();
        string userID = Session["NowUserId"].ToString();

        if(e.CommandName == "btnDel")
        {
            string friendID = e.CommandArgument.ToString();
            if(sql.delete(" friends "," (userID= "+userID+" and friendID ="+friendID+") or (friendID ="+userID+" and userID =" + friendID+")"))
            {
                Response.Write("<script> alert('删除好友成功！');location='MainPage.aspx'</script> ");
            }
        }
    }

    protected void btnUp_Click(object sender, EventArgs e) //所有好友的结果显示--上一页按钮
    {
        string nowPage = lblNow.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        if (toPage <= Convert.ToInt32(lblTotal.Text))
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friends.friendId,users.nickname,users.loginstatus ", " friends,users ", " friends.userId= " + id + " and friends.friendId = users.id ");

            rptHelper.dataBound(ref rptFriends, ref dt, toPage, ref lblTotal, 5);
                
        }
    }

    protected void btnDown_Click(object sender, EventArgs e) //所有好友的结果显示-- 下一页按钮
    {
        string nowPage = lblNow.Text;
        int toPage = Convert.ToInt32(nowPage) - 1;

        if (toPage >= 1)
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friends.friendId,users.nickname,users.loginstatus ", " friends,users ", " friends.userId= " + id + " and friends.friendId = users.id ");

            rptHelper.dataBound(ref rptFriends, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void btnFirst_Click(object sender, EventArgs e) //所有好友的结果显示--首页按钮
    {
        int toPage = 1;

            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friends.friendId,users.nickname,users.loginstatus ", " friends,users ", " friends.userId= " + id + " and friends.friendId = users.id ");

            rptHelper.dataBound(ref rptFriends, ref dt, toPage, ref lblTotal, 5);

        
    }

    protected void btnLast_Click(object sender, EventArgs e) //所有好友的结果显示 -- 末页按钮
    {
        string nowPage = lblTotal.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friends.friendId,users.nickname,users.loginstatus ", " friends,users ", " friends.userId= " + id + " and friends.friendId = users.id ");

            rptHelper.dataBound(ref rptFriends, ref dt, toPage, ref lblTotal, 5);

    }

    protected void btnJump_Click(object sender, EventArgs e) //所有好友的结果显示--跳转按钮
    {
        
        int toPage = Convert.ToInt32(txtJump.Text);

        if (toPage <= Convert.ToInt32(lblTotal.Text) && toPage >= 1)
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friends.friendId,users.nickname,users.loginstatus ", " friends,users ", " friends.userId= " + id + " and friends.friendId = users.id ");

            rptHelper.dataBound(ref rptFriends, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void rptSearchResult_ItemCommand(object source, RepeaterCommandEventArgs e) //搜索结果的结果显示--按钮判断
    {
        if(e.CommandName== "btnAddFriend") //添加好友
        {
            Button btn = (Button)e.Item.FindControl("btnAddFriend");
            if (e.Item.FindControl("txtApplication").Visible == false)
            {
                e.Item.FindControl("txtApplication").Visible = true;
                e.Item.FindControl("btnSendApplication").Visible = true;
                btn.Text = "取消添加";
            }
            else
            {
                e.Item.FindControl("txtApplication").Visible = false;
                e.Item.FindControl("btnSendApplication").Visible = false;
                btn.Text = "加为好友";
            }
        }
        else if(e.CommandName== "btnSendApplication") //发送请求
        {
            SQLOperation sql = new SQLOperation();

            TextBox textbox = (TextBox)e.Item.FindControl("txtApplication");
            string note;
            if(textbox == null)
            {
                note = null;
            }
            else
            {
                note = "'" + textbox.Text + "'";
            }
            string id = Session["NowUserId"].ToString();
            string friendID = e.CommandArgument.ToString();
            if(sql.add(" friendApplication "," "+id + "," + friendID + ","+note+""))
            {
                e.Item.FindControl("txtApplication").Visible = false;
                e.Item.FindControl("btnSendApplication").Visible = false;
                Response.Write("<script> alert('您的申请已发送！');</script> ");
            }
            else
            {
                Response.Write("<script> alert('申请发送失败！');</script> ");
            }
    }




}

    protected void rptSearchResult_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //判断是否已经是好友，如果已经是好友，则加为好友按钮不可点
        SQLOperation sql = new SQLOperation();
        string userID = Session["NowUserId"].ToString();
        int count = rptSearchResult.Items.Count;
        
            Button lkbtn = (Button)e.Item.FindControl("btnAddFriend");
            string id = lkbtn.CommandArgument.ToString();
            DataTable dt = sql.select(" * ", " friends ", " (friendID=" + id + " and userId=" + userID + ") or (userID =" + id + " and friendID =" + userID + ")");
            if (dt.Rows.Count != 0) lkbtn.Enabled = false;
    } //搜索结果的处理--判断是否已经是好友

    protected void btnUpResult_Click(object sender, EventArgs e) //搜索结果的结果显示上一页按钮
    {
        string nowPage = lblNowResult.Text;
        string text = txtSearch.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        if (toPage <= Convert.ToInt32(lblTotalResult.Text))
        {
            lblNowResult.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" nickname,loginstatus,id,number ", " users ", " number= '" + text + "' or nickname='" + text + "'"); //nickname or number to find

            rptHelper.dataBound(ref rptSearchResult, ref dt, toPage, ref lblTotalResult, 5);

        }
    }

    protected void btnDownResult_Click(object sender, EventArgs e) //搜索结果的结果显示下一页按钮
    {
        string nowPage = lblNowResult.Text;
        string text = txtSearch.Text;
        int toPage = Convert.ToInt32(nowPage) - 1;

        if (toPage >= 0)
        {
            lblNowResult.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" nickname,loginstatus,id,number ", " users ", " number= '" + text + "' or nickname='" + text + "'"); //nickname or number to find

            rptHelper.dataBound(ref rptSearchResult, ref dt, toPage, ref lblTotalResult, 5);

        }
    }

    protected void btnFirstResult_Click(object sender, EventArgs e) //搜索结果的结果显示--首页按钮
    {
        string text = txtSearch.Text;
        lblNowResult.Text = "1";
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" nickname,loginstatus,id,number ", " users ", " number= '" + text + "' or nickname='" + text + "'"); //nickname or number to find

            rptHelper.dataBound(ref rptSearchResult, ref dt, 1, ref lblTotalResult, 5);

        
    }

    protected void btnLastResult_Click(object sender, EventArgs e) //搜索结果的结果显示--末页按钮
    {
        string text = txtSearch.Text;
        lblNowResult.Text = lblTotalResult.Text;
        int toPage = Convert.ToInt32(lblTotalResult.Text);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" nickname,loginstatus,id,number ", " users ", " number= '" + text + "' or nickname='" + text + "'"); //nickname or number to find

        rptHelper.dataBound(ref rptSearchResult, ref dt, toPage, ref lblTotalResult, 5);
    }

    protected void btnJumpResult_Click(object sender, EventArgs e) //搜索结果的结果显示--跳转按钮
    {
        string text = txtSearch.Text;
        int toPage = Convert.ToInt32(txtJumpResult.Text);

        if (toPage <= Convert.ToInt32(lblTotalResult.Text) && toPage >= 1)
        {
            lblNowResult.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" nickname,loginstatus,id,number ", " users ", " number= '" + text + "' or nickname='" + text + "'"); //nickname or number to find

            rptHelper.dataBound(ref rptSearchResult, ref dt, toPage, ref lblTotalResult, 5);

        }
    }

    protected void btnAllApplication_Click(object sender, EventArgs e) //所有加好友申请显示按钮
    {
        if (divApplications.Visible)
        {
            btnAllApplication.Text = "新朋友";
            divApplications.Visible = false;
            
        }
        else
        {
            divApplications.Visible = true;
            btnAllApplication.Text = "收起";

            RepeaterOperate rptHelper = new RepeaterOperate();
            DataTable dt = new DataTable();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            dt = sql.select(" friendApplication.id,users.nickname,users.number,friendApplication.note ", " friendApplication,users ", " friendApplication.toId = " + id + " and users.id = friendApplication.fromId");
            rptHelper.dataBound(ref rptApplications, ref dt, 1, ref lblNowApp, 5);
            lblNowApp.Text = "1";
        }
        
    }

    protected void rptApplications_ItemCommand(object source, RepeaterCommandEventArgs e) //所有加好友申请显示--按钮判断
    {
        SQLOperation sql = new SQLOperation();
        string userID = Session["NowUserId"].ToString();
        if(e.CommandName == "btnPass")
        {
            string AppID = e.CommandArgument.ToString();
            DataTable dt = sql.select(" fromID ", " friendApplication ", " id=" + AppID);
            string friendID = dt.Rows[0][0].ToString();
            if (sql.add(" friends ", " " + userID + "," + friendID)) 
            {
                sql.delete(" FriendApplication ", " id="+AppID); //好友申请通过后删除申请
                Response.Write("<script> alert('添加好友成功！');</script> ");
            }
        }
        else if (e.CommandName == "btnIgnore")
        {
            string AppID = e.CommandArgument.ToString();
            if (sql.delete(" FriendApplication ", " id=" + AppID))
            {
                Response.Write("<script> alert('您已删除该申请！');</script> ");
            }
        }
    }

    protected void btnUpApp_Click(object sender, EventArgs e)
    {
        string nowPage = lblNowApp.Text;
        //string text = txtSearch.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        if (toPage <= Convert.ToInt32(lblTotalResult.Text))
        {
            lblNowApp.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friendApplication.id,users.nickname,users.number,friendApplication.note ", " friendApplication,users ", " friendApplication.toId = " + id + " and users.id = friendApplication.fromId");

            rptHelper.dataBound(ref rptApplications, ref dt, toPage, ref lblTotalApp, 5);

        }
    } //收到的全部好友申请 --上一页按钮

    protected void btnDownApp_Click(object sender, EventArgs e) //收到的全部好友申请 --下一页按钮
    {
        string nowPage = lblNowApp.Text;
        //string text = txtSearch.Text;
        int toPage = Convert.ToInt32(nowPage) - 1;

        if (toPage >= 0)
        {
            lblNowApp.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friendApplication.id,users.nickname,users.number,friendApplication.note ", " friendApplication,users ", " friendApplication.toId = " + id + " and users.id = friendApplication.fromId");

            rptHelper.dataBound(ref rptApplications, ref dt, toPage, ref lblTotalApp, 5);

        }
    }

    protected void btnFirstApp_Click(object sender, EventArgs e)
    {
        string text = txtSearch.Text;
        lblNowApp.Text = "1";
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" friendApplication.id,users.nickname,users.number,friendApplication.note ", " friendApplication,users ", " friendApplication.toId = " + id + " and users.id = friendApplication.fromId");

        rptHelper.dataBound(ref rptApplications, ref dt, 1, ref lblTotalApp, 5);
    }

    protected void btnLastApp_Click(object sender, EventArgs e)
    {
       // string text = txtSearch.Text;
        lblNowApp.Text = lblTotalApp.Text;
        int toPage = Convert.ToInt32(lblTotalApp.Text);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" friendApplication.id,users.nickname,users.number,friendApplication.note ", " friendApplication,users ", " friendApplication.toId = " + id + " and users.id = friendApplication.fromId");

        rptHelper.dataBound(ref rptApplications, ref dt, toPage, ref lblTotalApp, 5);
    }

    protected void btnJumpApp_Click(object sender, EventArgs e)
    {
        int toPage = Convert.ToInt32(txtJumpApp.Text);

        if (toPage <= Convert.ToInt32(lblTotalApp.Text) && toPage >= 1)
        {
            lblNowApp.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" friendApplication.id,users.nickname,users.number,friendApplication.note ", " friendApplication,users ", " friendApplication.toId = " + id + " and users.id = friendApplication.fromId");

            rptHelper.dataBound(ref rptApplications, ref dt, toPage, ref lblTotalApp, 5);

        }
    }

    
}
