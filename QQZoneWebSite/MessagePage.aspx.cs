using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MessagePage : System.Web.UI.Page
{
    SpecialOperations helper = new SpecialOperations();
    protected void Page_Load(object sender, EventArgs e)
    {
        SQLOperation sql = new SQLOperation();
        if(Session["NowUserId"] != null)
        {
            string id;
            /*权限设置的选项初始化*/
            dplstLimit.Items.Add("所有人可见");
            dplstLimit.Items.Add("仅双方可见");
            if (Request.QueryString["id"] != null) //正在被访问
            {
                id = Request.QueryString["id"].ToString();
            }
            else
            {
                divAddMessage.Visible = false;//自己不可以给自己留言
                id = Session["NowUserId"].ToString();
            }
            DataTable dt = sql.select(" a_message.*,users.nickname ", " a_message,users ", " a_message.userid= "+id+" and users.id=a_message.messagerId");
            /*然后是repeater的数据绑定*/ 
        }
        else
        {
            helper.alertHelper(this, "请先登录", "MainPage.aspx");
        }
    }

    protected void btnMessageAdd_Click(object sender, EventArgs e)
    {
        if(txtMessageContent == null)
        {
            helper.alertHelper(this, "留言不可以为空！");
        }
        else
        {
            string message = txtMessageContent.Text;
            SQLOperation sql = new SQLOperation();
            string messagerID = Session["NowUserId"].ToString();
            string userId = Request.QueryString["id"].ToString();
            string time = DateTime.Now.ToString();
            try
            {
                //userid,messagerid,messagecontent,time,whocansee
                sql.add(" a_message ", " " + userId + "," + messagerID + ",'" + txtMessageContent.Text + "','" + time + "','"+dplstLimit.SelectedValue.ToString()+"'");
                helper.alertHelper(this, "留言成功！", "MessagePage.aspx?id=" + userId);
            }
            catch(Exception exception)
            {
                helper.alertHelper(this, "留言失败！");
            }
        }
    }

    protected void rptMessageShow_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "btnMessageDel")
        {
            SQLOperation sql = new SQLOperation();
            string messageID = e.CommandArgument.ToString();
            try
            {
                sql.delete(" a_message ", " id=" + messageID); //删除该条动态
                DataTable dtComments = sql.select(" id ", " a_comment ", " commentedID=" + messageID + " and kind='message'");
                for (int i = 0; i < dtComments.Rows.Count; i++)
                {
                    string commentID = dtComments.Rows[i][0].ToString(); //删除该动态下面的评论
                    sql.delete(" a_comment ", " id=" + commentID);
                }
                helper.alertHelper(this, "删除回复成功！", "MessagePage.aspx");
            }
            catch (Exception exception)
            {
                helper.alertHelper(this, "删除失败！");
            }
        }
        if (e.CommandName == "btnComment")
        {
            e.Item.FindControl("txtNewComment").Visible = true;
            e.Item.FindControl("btnAddComment").Visible = true;
            e.Item.FindControl("btnCancelComment").Visible = true;

        }
        if (e.CommandName == "btnCancelComment")
        {
            e.Item.FindControl("txtNewComment").Visible = false;
            e.Item.FindControl("btnAddComment").Visible = false;
            e.Item.FindControl("btnCancelComment").Visible = false;
        }
        if (e.CommandName == "btnAddComment")
        {
            TextBox txtNewCom = (TextBox)e.Item.FindControl("txtNewComment");
            string comText = txtNewCom.Text; //获得新评论的值
            if (comText == null)
            {
                txtNewCom.Text = "评论不能为空！";
                txtNewCom.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                string id;
                if (Request.QueryString["id"] != null)
                {
                    id = Request.QueryString["id"].ToString();
                }
                else
                {
                    id = Session["NowUserId"].ToString();

                }

                string commentedId = e.CommandArgument.ToString();
                string time = DateTime.Now.ToString();
                string kind = "message";

                SQLOperation sql = new SQLOperation();
                if (sql.add(" a_comment ", commentedId + "," + id + ",'" + comText + "','" + time + "','" + kind + "',null"))
                {
                    e.Item.FindControl("txtNewComment").Visible = false;
                    e.Item.FindControl("btnAddComment").Visible = false;
                    e.Item.FindControl("btnCancelComment").Visible = false;
                    Response.Write("<script> alert('添加评论成功！');location=  'MessagePage.aspx'</script> ");
                }
                else
                {
                    txtNewCom.Text = "评论添加失败";
                }
            }
        }
    } //所有留言的显示--按钮判断


    protected void rptMessageShow_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        string visitorID = Session["NowUserId"].ToString(); //访问者的ID
        if (Request.QueryString["id"] != null) //当前页面正在被访问
        {
            string ownerID = Request.QueryString["id"].ToString();//被访问者ID 
            e.Item.FindControl("rptReply").Visible = false;//用户的留言板回复不可以给别人看也不可以被评论！！！留言的双方可以回复
            e.Item.FindControl("btnMessageDel").Visible = false;//全都不可以删除
        }
        SQLOperation sql = new SQLOperation();
        /*权限判断*/
        string visitedID = Request.QueryString["id"].ToString();
        Button btn = (Button)e.Item.FindControl("btnDelAlbum");
        string combinedID = btn.CommandArgument.ToString();
        DataTable dt = sql.select(" whocansee ", " a_message ", " id=" + combinedID);
        string limit = dt.Rows[0][0].ToString();
        if (limit == "仅双方可见")
        {
            dt = sql.select(" userId,messagerId ", " a_message ", " id=" + combinedID);
            if(visitorID == dt.Rows[0][1].ToString() && visitedID == dt.Rows[0][0].ToString())
            {
                e.Item.FindControl("rptReply").Visible = true;
            }
            else
            {
                e.Item.Visible = false;
            }

        }
        else if (limit == "仅自己可见" && Request.QueryString["id"] != null)
        {
            e.Item.Visible = false;
        }

        /*绑定评论的数据*/
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //找到外层Repeater的数据项
            DataRowView rowv = (DataRowView)e.Item.DataItem;
            //提取外层Repeater的数据项的ID
            int ID = Convert.ToInt32(rowv["id"]);
            //找到对应ID下的评论
            //string select = "select * from a_comment where commentedid=" + ID.ToString();
            //找到内嵌Repeater
            Repeater rept = (Repeater)e.Item.FindControl("rptReply");
            //数据绑定
            rept.DataSource = sql.select(" a_comment.id,a_comment.commentContent, users.nickname ", " users,a_comment ", " a_comment.commentedId= " + ID.ToString() + " and  a_comment.kind = 'message' and a_comment.userid = users.id");
            rept.DataBind();

        }
    }

    protected void rptReply_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
 
        if (e.CommandName == "btnCommentDel") //删除评论按钮
        {
            try
            {
                SQLOperation sql = new SQLOperation();
                string commentID = e.CommandArgument.ToString();
                sql.delete(" a_comment ", " id=" + commentID);
                helper.alertHelper(this, "删除评论成功！", "MessagePage.aspx");
            }
            catch (Exception exception)
            {
                helper.alertHelper(this, "删除评论失败！");
            }
        }

    } //评论的按钮判断

protected void rptReply_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }
}