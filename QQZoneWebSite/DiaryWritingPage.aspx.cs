using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DiaryWritingPage : System.Web.UI.Page
{
    SpecialOperations helper = new SpecialOperations();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(Session["NowUserId"] != null)
            {
                string articleId = Request.QueryString["articleId"];
                /*权限设置的选项初始化*/
                dplstLimit.Items.Add("所有人可见");
                dplstLimit.Items.Add("仅好友可见");
                dplstLimit.Items.Add("仅自己可见");
                dplstLimit.Items.Add("部分好友不可见");

                if (Request.QueryString["id"] == null)
                {
                    string op = Request.QueryString["op"];
                    Session["NowOperate"] = op;
                    SQLOperation sql = new SQLOperation();
                        if (articleId == null) //要新建日志
                        {
                            ltrShow.Visible = false; //不是显示
                                                     /*给分类的下拉列表绑定数据*/
                            DataTable dtt = sql.select(" name,id ", " classes ", " userid=" + Session["NowUserId"].ToString());
                            dplstClass.DataTextField = "name";
                        dplstClass.DataValueField = "id";
                            dplstClass.DataSource = dtt;
                            dplstClass.DataBind();

                        }
                        else
                        {
                            if (op == "visit")
                            {
                                btnAdd.Visible = false;
                                divEditor.Visible = false;
                                divShow.Visible = true;

                                DataTable dt = sql.select(" title,DiaryContent ", " a_diary ", " id=" + articleId);
                                DataTable dtt = sql.select(" classes.name ", " classes,a_diary ", " a_diary.id="+articleId+" and a_diary.classid = classes.id");
                                dplstClass.Text = dtt.Rows[0][0].ToString();
                                txtTitle.ReadOnly = true;
                                txtTitle.Text = dt.Rows[0][0].ToString();
                                ltrShow.Text = dt.Rows[0][1].ToString();
                            }
                            else if (op == "edit")
                            {
                                divShow.Visible = false;
                                btnAdd.Visible = true;
                                divEditor.Visible = true;

                                DataTable dt = sql.select(" title,DiaryContent ", " a_diary ", " id=" + articleId);
                                txtTitle.Text = dt.Rows[0][0].ToString();
                                myEditor11.Value = dt.Rows[0][1].ToString();
                                

                                /*给分类的下拉列表绑定数据*/
                                DataTable dtt = sql.select(" name,id ", " classes ", " userid=" + Session["NowUserId"].ToString());
                                dplstClass.DataTextField = "name";
                            dplstClass.DataValueField = "id";
                                dplstClass.DataSource = dtt;
                                dplstClass.DataBind();
                            }
                        }
                }
                else
                {
                    btnAdd.Visible = false;
                    divEditor.Visible = false;
                    divShow.Visible = true;

                    SQLOperation sql = new SQLOperation();
                    DataTable dt = sql.select(" title,DiaryContent ", " a_diary ", " id=" + articleId);
                    DataTable dtt = sql.select(" classes.name ", " classes,a_diary ", " a_diary.id=" + articleId + " and a_diary.classid = classes.id");
                    dplstClass.Text = dtt.Rows[0][0].ToString();
                    txtTitle.ReadOnly = true;
                    txtTitle.Text = dt.Rows[0][0].ToString();
                    ltrShow.Text = dt.Rows[0][1].ToString();

                    hplkBack.NavigateUrl = "~/DiaryPage.aspx?id=" + Request.QueryString["id"].ToString(); //改变返回的跳转链接值
                }
            }
            else
            {
                Response.Write("<script> alert('请先登录！');location=  'MainPage.aspx'</script> ");
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string content = Server.HtmlDecode(myEditor11.InnerHtml);
        string op = null;

        
        string id = Session["NowUserId"].ToString();
        string title = txtTitle.Text.ToString();
        string time = DateTime.Now.ToString();
        SQLOperation sql = new SQLOperation();
        DataTable dtt = sql.select(" id,amount ", " classes ", " userid = " + id + " and name = '所有日志'");
        string allID = dtt.Rows[0][0].ToString();
        //title,content,0,0,time,classid,null
        string name = dplstClass.SelectedValue.ToString();
        if (name == null) { name = allID; }; //默认所有日志
        if (content == null)
        {

        }
        else if(Request.QueryString["op"] == null)
        {
            
            DataTable dt = sql.select(" amount ", " classes ", " id="+name);
            string classId = name; //获得选中的类别ID
            
            string allAmount = (int.Parse(dtt.Rows[0][1].ToString()) + 1).ToString(); //所有日志类的内存数量要增加1

            string amount = (int.Parse(dt.Rows[0][0].ToString()) + 1).ToString(); //这个类的内存数量要增加1
            string limit = dplstLimit.SelectedValue.ToString();
            if(sql.add(" a_diary ", " '" + title + "','" + content + "',0,0,'" + time + "'," + allID + ",'"+limit+"',null") && sql.update(" classes ", " amount = " + allAmount, " id=" + allID))
            {
                DataTable dtForAll = sql.select(" id ", " a_diary ", " time='" + time+"'");
                string idForAll = dtForAll.Rows[0][0].ToString();
                sql.add(" a_diary ", " '" + title + "','" + content + "',0,0,'" + time + "'," + classId + ",'"+limit+"',"+idForAll);
                sql.update(" classes ", " amount = " + amount, " id=" + classId);
                Response.Write("<script> alert('添加日志成功！');location=  'DiaryPage.aspx'</script> ");
            }
        }
        else if(Request.QueryString["op"].ToString()== "edit")
        {
            

            DataTable dt = sql.select(" idInAll ", " a_diary ", " id = " + Request.QueryString["articleId"].ToString());
            string inAllId = dt.Rows[0][0].ToString(); //获得该文章在所有日志类别下的id
            string classId = dplstClass.SelectedValue.ToString();
           
            if (sql.update(" a_diary ", " title ='" + title + "',Diarycontent='" + content + "',time='" + time + "',classId=" + classId+",whocansee='"+dplstLimit.SelectedValue.ToString()+"'", " id=" + Request.QueryString["articleId"].ToString()))
            {
                sql.update(" a_diary ", " title ='" + title + "',Diarycontent='" + content + "',time='" + time + "',classId=" + allID+",whocansee='"+dplstLimit.SelectedValue.ToString()+"'", " id=" + inAllId);
                Response.Write("<script> alert('修改日志成功！');location=  'DiaryPage.aspx'</script> ");
            }
        }
    } //保存日志按钮，包含新建和编辑的判断

    protected void btnComment_Click(object sender, EventArgs e)
    {
        txtNewComment.Visible = true;
        btnAddComment.Visible = true;
        btnCancelComment.Visible = true;
    }

    protected void btnLike_Click(object sender, EventArgs e)
    {
        SQLOperation sql = new SQLOperation();
        string diaryId = Request.QueryString["articleId"].ToString();
        string id = Session["NowUserId"].ToString();

        if (btnLike.Text.StartsWith("取消赞"))
        {
            DataTable dt = sql.select(" likeamount ", " a_diary ", " id=" + diaryId);
            int midAmount = int.Parse(dt.Rows[0][0].ToString()) - 1;
            string amount = midAmount.ToString();
            if (sql.delete(" a_like ", " likedId=" + diaryId + " and userId=" + id + " and likekind='日志'") && sql.update(" a_diary ", " likeamount =" + amount + " ", " id=" + diaryId))
            {
                btnLike.Text = "赞（" + amount + ")";
            }
        }
        else
        {
            string time = DateTime.Now.ToString();
            DataTable dt = sql.select(" likeamount ", " a_diary ", " id=" + diaryId);
            int midAmount = int.Parse(dt.Rows[0][0].ToString()) + 1;
            string amount = midAmount.ToString();
            //userid,likedid,likekind,time
            if (sql.add(" a_like ", " " + id + "," + diaryId + ",'日志','" + time + "'") && sql.update(" a_diary ", " likeamount =" + amount + " ", " id=" + diaryId))
            {
                btnLike.Text = "取消赞(" + amount + ")";
            }
        }
    }

    protected void btnAddComment_Click(object sender, EventArgs e)
    {
        string comText = txtNewComment.Text; //获得新评论的值
        if (txtNewComment == null)
        {
            txtNewComment.Text = "评论不能为空！";
            txtNewComment.ForeColor = System.Drawing.Color.Red;
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

            string commentedId = Request.QueryString["articleId"].ToString();
            string time = DateTime.Now.ToString();
            string kind = "diary";

            SQLOperation sql = new SQLOperation();
            if (sql.add(" a_comment ", commentedId + "," + id + ",'" + comText + "','" + time + "','" + kind + "',null,null"))
            {
                txtNewComment.Visible = false;
                btnAddComment.Visible = false;
                btnCancelComment.Visible = false;
                Response.Write("<script> alert('添加评论成功！');location=  'DiaryWritingPage.aspx'</script> ");
            }
            else
            {
                txtNewComment.Text = "评论添加失败";
            }
        }
    }

    protected void btnCancelComment_Click(object sender, EventArgs e)
    {
        txtNewComment.Visible = false;
        btnAddComment.Visible = false;
        btnCancelComment.Visible = false;
    }

    protected void rptInReply_ItemCommand(object source, RepeaterCommandEventArgs e)
    {   
        if (e.CommandName == "btnCommentDel") //删除评论按钮
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
                helper.alertHelper(this, "删除评论成功！", "DiaryWritingPage.aspx?op="+Request.QueryString["op"].ToString());
            }
            catch (Exception exception)
            {
                helper.alertHelper(this, "删除评论失败！");
            }
        }
        if (e.CommandName == "btnReply")
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
            if (txt == null)
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

                if (sql.add(" a_comment ", commentedId + "," + id + ",'" + content + "','" + time + "','" + kind + "'," + commentedId))
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

    } //评论的按钮判断


    protected void rptInReply_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        SQLOperation sql = new SQLOperation();

        /*当该页面被访问时，不允许访客删除动态*/
        if (Request.QueryString["id"] != null)
        {
            Button btn = (Button)e.Item.FindControl("btnCommentDel");
            btn.Visible = false;
        }
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


    protected void rptInInReply_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "btnReplyDel") //删除说说按钮
        {
            SQLOperation sql = new SQLOperation();
            string replyID = e.CommandArgument.ToString();
            if (sql.delete(" a_comment ", " id= " + replyID + " or commentedID =" + replyID)) //这条回复本身以及他的回复都应该被删除
            {
                helper.alertHelper(this, "删除回复成功！", "PersonalCenterPage.aspx");
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

                if (sql.add(" a_comment ", commentedId + "," + id + ",'" + content + "','" + time + "','" + kind + "'," + FatherCommentID))
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
    } //回复的按钮判断

    protected void rptInInReply_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        /*当该页面被访问时，不允许访客删除动态*/
        if (Request.QueryString["id"] != null)
        {
            Button btn = (Button)e.Item.FindControl("btnReplyDel");
            btn.Visible = false;
        }
    } //回复的删除按钮控制

}
