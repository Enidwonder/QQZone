using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PersonalCenterPage : System.Web.UI.Page
{
    SpecialOperations helperSpecial = new SpecialOperations();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(Session["NowUserId"] != null)
            {
                if(Request.QueryString["id"] == null)
                {
                    string id;
                    id = Session["NowUserId"].ToString();
                    
                    SQLOperation sql = new SQLOperation();

                    RepeaterOperate rptOperate = new RepeaterOperate();

                    DataTable dtt = sql.select(" distinct users.NickName,A_Status.* ", " Friends,(users join A_Status on users.id = A_Status.UserId) ", " (users.id = " + id + ") or (Friends.UserId =" + id + " and Users.id = Friends.FriendId)");
                    //rptOperate.dataBound(ref rptAllStatus, ref dtt);
                    rptOperate.dataBound(ref rptAllStatus, ref dtt, 1, ref lblTotal, 5);
                    lblNow.Text = "1";
                }
                else
                {
                    helperSpecial.alertHelper(this, "您无权访问他人的个人中心!");
                }
            }
            else
            {
                Response.Write("<script> alert('请先登录！');location=  'MainPage.aspx'</script> ");
            }
        }
    }

    protected void rptInReply_ItemCommand(object source, RepeaterCommandEventArgs e) //评论区RPT---DONE
    {
        if(e.CommandName == "btnCommentDel") //删除评论按钮
        {
            try
            {
                SQLOperation sql = new SQLOperation();
                string commentID = e.CommandArgument.ToString();
                DataTable dtReply = sql.select(" id ", " a_comment ", " commentedID= " + commentID + " and kind='comment'");
                for (int j = 0; j < dtReply.Rows.Count; j++) //删除评论下面的回复
                {
                    sql.delete(" a_comment ", " id=" + dtReply.Rows[j][0].ToString());
                }
                sql.delete(" a_comment ", " id=" + commentID);
                helperSpecial.alertHelper(this, "删除评论成功！", "PersonalCenterPage.aspx");
            }
            catch(Exception exception)
            {
                helperSpecial.alertHelper(this, "删除评论失败！");
            }
        }
        if(e.CommandName == "btnReply")
        {
            Button btn = (Button)e.Item.FindControl("btnReply");
            if (e.Item.FindControl("btnAddReply").Visible) //正在回复时点击
            {
                btn.Text = "回复";
                e.Item.FindControl("btnAddReply").Visible = false;
                e.Item.FindControl("txtReply").Visible = false;
            }
            else
            {
                e.Item.FindControl("btnAddReply").Visible = true;
                e.Item.FindControl("txtReply").Visible = true;
                btn.Text = "取消回复";
            }
        }
        else if (e.CommandName == "btnAddReply")
        {
            TextBox txt = (TextBox)e.Item.FindControl("txtReply");
            string id;
            string content = txt.Text;
            if(content == null || content == "")
            {
                txt.ForeColor = System.Drawing.Color.Red;
                txt.Text = "回复不可以为空！";
            }
            else
            {
                
                id = Session["NowUserId"].ToString();
                SQLOperation sql = new SQLOperation();
                // if(sql.add(" a_comment ","  "))
                string commentedId = e.CommandArgument.ToString();
                string time = DateTime.Now.ToString();
                string kind = "comment";
                
                if (sql.add(" a_comment ", commentedId + "," + id + ",'" + content + "','" + time + "','" + kind + "',"+commentedId))
                {
                    e.Item.FindControl("txtReply").Visible = false;
                    e.Item.FindControl("btnReply").Visible = false;
                    e.Item.FindControl("btnAddReply").Visible = false;
                    Response.Write("<script> alert('回复成功！');location=  'PersonalCenterPage.aspx'</script> ");
                }
                else
                {
                    txt.Text = "回复失败";
                }
            }
        }
        
    }

    protected void btnAddStatus_Click(object sender, EventArgs e) //添加动态按钮---DONE
    {
        string content = txtNewStatus.Text;
        SQLOperation sqlOp = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        string time = DateTime.Now.ToShortTimeString();
        if(content != null)
        {
            //userid,time,statusContent,picture,likeAmount,SeenAmount,WhoCanSee
            sqlOp.add(" a_status ", id + ",'"+time+ "','" + content + "', null, 0, 0, null");
            Response.Write("<script> alert('添加动态成功！');location=  'PersonalCenterPage.aspx'</script> ");
        }
    }

    protected void rptAllStatus_ItemCommand(object source, RepeaterCommandEventArgs e) //所有动态RPT---DONE
    {

       if(e.CommandName == "btnStatusDel")
        {
            SQLOperation sql = new SQLOperation();
            string statusID = e.CommandArgument.ToString();
            try
            {
                sql.delete(" a_status ", " id=" + statusID); //删除该条动态
                DataTable dtComments = sql.select(" id ", " a_comment ", " commentedID=" + statusID + " and kind='status'");
                for (int i = 0; i < dtComments.Rows.Count; i++)
                {
                    string commentID = dtComments.Rows[i][0].ToString(); //删除该动态下面的评论
                    DataTable dtReply = sql.select(" id ", " a_comment ", " commentedID= " + commentID + " and kind='comment'");
                    for (int j = 0; j < dtReply.Rows.Count; j++) //删除评论下面的回复
                    {
                        sql.delete(" a_comment ", " id=" + dtReply.Rows[j][0].ToString());
                    }
                    sql.delete(" a_comment ", " id=" + commentID);
                }
                helperSpecial.alertHelper(this, "删除回复成功！", "PersonalCenterPage.aspx");
            }
            catch(Exception exception)
            {
                helperSpecial.alertHelper(this, "删除失败！");
            }
        }
        if(e.CommandName == "btnLike")
        {
            int index = e.Item.ItemIndex;

            SQLOperation sql = new SQLOperation();
            string StatusId = e.CommandArgument.ToString();
            string id = Session["NowUserId"].ToString();
            Button btn = (Button)rptAllStatus.Items[index].FindControl("btnLike");

            if (btn.Text.StartsWith("取消赞"))
            {
                DataTable dt = sql.select(" likeamount ", " a_status ", " id=" + StatusId);
                int midAmount = int.Parse(dt.Rows[0][0].ToString()) - 1;
                string amount = midAmount.ToString();
                if (sql.delete(" a_like ", " likedId=" + StatusId + " and userId=" + id + " and likekind='说说'") && sql.update(" a_status ", " likeamount =" + amount + " ", " id=" + StatusId))
                {
                    btn.Text = "赞（" + amount + ")";
                }
            }
            else
            {
                string time = DateTime.Now.ToString();
                DataTable dt = sql.select(" likeamount ", " a_status ", " id=" + StatusId);
                int midAmount = int.Parse(dt.Rows[0][0].ToString()) + 1;
                string amount = midAmount.ToString();
                //userid,likedid,likekind,time
                if (sql.add(" a_like ", " " + id + "," + StatusId + ",'说说','" + time + "'") && sql.update(" a_status ", " likeamount =" + amount + " ", " id=" + StatusId))
                {
                    btn.Text = "取消赞(" + amount + ")";
                }
            }
            
        }
        if(e.CommandName == "btnComment")
        {
            e.Item.FindControl("txtNewComment").Visible = true;
            e.Item.FindControl("btnAddComment").Visible = true;
            e.Item.FindControl("btnCancelComment").Visible = true;
            
        }
        if(e.CommandName == "btnCancelComment")
        {
            e.Item.FindControl("txtNewComment").Visible = false;
            e.Item.FindControl("btnAddComment").Visible = false;
            e.Item.FindControl("btnCancelComment").Visible = false;
        }
        if(e.CommandName == "btnAddComment")
        {
            TextBox txtNewCom = (TextBox)e.Item.FindControl("txtNewComment");
            string comText = txtNewCom.Text; //获得新评论的值
            if(comText == null)
            {
                txtNewCom.Text = "评论不能为空！";
                txtNewCom.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                string id;
                if(Session["NowVisitedId"] != null)
                {
                    id = Session["NowVisitedId"].ToString();
                }
                else
                {
                    id = Session["NowUserId"].ToString();
                    
                }

                string commentedId = e.CommandArgument.ToString();
                string time = DateTime.Now.ToString();
                string kind = "status";

                SQLOperation sql = new SQLOperation();
                if (sql.add(" a_comment ", commentedId + "," + id + ",'" + comText + "','" + time + "','" + kind + "',null"))
                {
                    e.Item.FindControl("txtNewComment").Visible = false;
                    e.Item.FindControl("btnAddComment").Visible = false;
                    e.Item.FindControl("btnCancelComment").Visible = false;
                    Response.Write("<script> alert('添加评论成功！');location=  'PersonalCenterPage.aspx'</script> ");
                }
                else
                {
                    txtNewCom.Text = "评论添加失败";
                }
            }
        }
    }

    protected void rptAllStatus_ItemDataBound(object sender, RepeaterItemEventArgs e)//评论区RPT的数据绑定---DONE
    {
        SQLOperation sql = new SQLOperation();
        /*当该页面被访问时，不允许访客删除动态*/
        if(Request.QueryString["id"] != null)
        {
            int count = rptAllStatus.Items.Count;
            for (int i = 0; i < count; i++)
            {
                Button btn = (Button)rptAllStatus.Items[i].FindControl("btnStatusDel");
                btn.Visible = false;
            }
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
            Repeater rept = (Repeater)e.Item.FindControl("rptInReply");
            //数据绑定
            rept.DataSource = sql.select(" a_comment.id,a_comment.commentContent, users.nickname ", " users,a_comment ", " a_comment.commentedId= " + ID.ToString() + " and  a_comment.kind = 'status' and a_comment.userid = users.id");
            rept.DataBind();

        }
    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        RepeaterOperate rptHelp = new RepeaterOperate();
        
    }

    protected void btnDown_Click(object sender, EventArgs e)
    {

    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {

    }

    protected void btnJump_Click(object sender, EventArgs e)
    {

    }

    protected void rptInInReply_ItemCommand(object source, RepeaterCommandEventArgs e)//---DONE
    {
        
        if(e.CommandName == "btnReplyDel") //删除说说按钮
        {
            SQLOperation sql = new SQLOperation();
            string replyID = e.CommandArgument.ToString();
            if (sql.delete(" a_comment "," id= "+replyID+" or commentedID ="+replyID)) //这条回复本身以及他的回复都应该被删除
            {
                helperSpecial.alertHelper(this, "删除回复成功！", "PersonalCenterPage.aspx");
            }
        }
        if (e.CommandName == "btnInReply")
        {
            Button btn = (Button)e.Item.FindControl("btnInReply");
            if (e.Item.FindControl("btnInAddReply").Visible) //正在回复时点击
            {
                btn.Text = "回复";
                e.Item.FindControl("btnInAddReply").Visible = false;
                e.Item.FindControl("txtInReply").Visible = false;
            }
            else
            {
                e.Item.FindControl("btnInAddReply").Visible = true;
                e.Item.FindControl("txtInReply").Visible = true;
                btn.Text = "取消回复";
            }
        }
        else if (e.CommandName == "btnInAddReply")
        {
            TextBox txt = (TextBox)e.Item.FindControl("txtInReply");
            string id;
            string content = txt.Text;
            if (content == null || content == "")
            {
                txt.ForeColor = System.Drawing.Color.Red;
                txt.Text = "回复不可以为空！";
            }
            else
            {
                id = Session["NowUserId"].ToString();
                SQLOperation sql = new SQLOperation();
                // if(sql.add(" a_comment ","  "))
                string commentedId = e.CommandArgument.ToString();
                string time = DateTime.Now.ToString();
                string kind = "comment";

                /*获得当前所属评论的ID*/
                /* DataRowView rowv = (DataRowView)e.Item.DataItem;
                 string FatherCommentID = rowv["id"].ToString();*/
                TextBox txtFather = (TextBox)e.Item.FindControl("txtFatherCommentID");
                string FatherCommentID = txtFather.Text;

                if (sql.add(" a_comment ", commentedId + "," + id + ",'" + content + "','" + time + "','" + kind + "',"+FatherCommentID))
                {
                    e.Item.FindControl("txtInReply").Visible = false;
                    e.Item.FindControl("btnInReply").Visible = false;
                    e.Item.FindControl("btnInAddReply").Visible = false;
                    Response.Write("<script> alert('回复成功！');location=  'PersonalCenterPage.aspx'</script> ");
                }
                else
                {
                    txt.Text = "回复失败";
                }
            }
    }
}

    protected void rptInReply_ItemDataBound(object sender, RepeaterItemEventArgs e) //回复评论的数据绑定
    {
        SQLOperation sql = new SQLOperation();

        /*绑定评论的数据*/
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //找到外层Repeater的数据项
            DataRowView rowv = (DataRowView)e.Item.DataItem;
            //提取外层Repeater的数据项的ID
            int ID = Convert.ToInt32(rowv["id"]);
            //找到对应ID下的Book
            //string select = "select * from a_comment where commentedid=" + ID.ToString();
            //找到内嵌Repeater
            Repeater rept = (Repeater)e.Item.FindControl("rptInInReply");
            //数据绑定
          
            DataTable dt = new DataTable();
            dt = sql.select(" a1.*,user1.NickName as name1,user2.NickName as name2 ", " (users as user1 join a_comment as a1 on user1.id = a1.userId  ),(users as user2 join A_Comment as a2 on user2.id = a2.userId ) ", " a1.BelongFatherCommentId = " + ID.ToString() + " and user1.id = a1.UserId and a2.id = a1.CommentedId and user2.id = a2.UserId ");
            

            rept.DataSource = dt;
            rept.DataBind();

        }
    }
}