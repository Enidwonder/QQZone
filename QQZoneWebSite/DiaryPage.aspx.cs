using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DiaryPage : System.Web.UI.Page
{
    static string classID = "0";
    SpecialOperations helper = new SpecialOperations();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id;
            classID = "0";
            SQLOperation sql = new SQLOperation();
            RepeaterOperate rptHelper = new RepeaterOperate();
            DataTable dt = new DataTable();
            divNewClass.Visible = false;
            if(Session["NowUserId"] != null)
            {
                if(Request.QueryString["id"] != null) //正在被他人访问
                {
                    divUsers.Visible = false;
                    /*-------------控件显示控制-------------*/
                    id = Request.QueryString["id"].ToString();
                    dt = sql.select(" a_diary.title,a_diary.id,a_diary.time ", " a_diary,classes ", " a_diary.classId = classes.id and classes.userId = " + id);
                    rptHelper.dataBound(ref rptAllDiaries, ref dt, 1, ref lblTotal, 5);
                    dt = sql.select(" * ", " classes ", " userid =" + id + " and classkind = '日志'");

                    rptHelper.dataBound(ref rptClasses, ref dt, 1, ref lblTotal, 5);
                }
                else
                {
                    id = Session["NowUserId"].ToString();
                    dt = sql.select(" a_diary.title,a_diary.id,a_diary.time  ", " a_diary,classes ", " a_diary.classId = classes.id and classes.userId = " + id);
                    rptHelper.dataBound(ref rptAllDiaries, ref dt, 1, ref lblTotal, 5);
                    dt = sql.select(" * ", " classes ", " userid =" + id + " and classkind = '日志'");
                    rptHelper.dataBound(ref rptClasses, ref dt, 1, ref lblTotal, 5);
                }
            }
            else
            {
                Response.Write("<script> alert('请先登录！');location=  'MainPage.aspx'</script> ");
            }
        }
    }

    protected void btnNewDiary_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/DiaryWritingPage.aspx"); //不带id值传过去说明是新建日志
    }

    protected void rptAllDiaries_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "btnDel") //unfinished,打算在分类跳转那里加一个？传值，通过判断是否传递类的ID判断是在哪里进行删除操作
        {
            SQLOperation sql = new SQLOperation();
            string articleId = e.CommandArgument.ToString(); //日志ID
            DataTable dt = sql.select(" classes.name ", " a_diary,classes ", " a_diary.classid = classes.id ");
            string className = dt.Rows[0][0].ToString();
        }
    }

    protected void rptClasses_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "hyptClass") //查看该分类下的所有日志列表
        {
            classID = e.CommandArgument.ToString();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" a_diary.title,a_diary.id ,a_diary.time ", " a_diary,classes ", " a_diary.classId = classes.id and classes.userId = " + id+" and classes.id ="+classID);
            RepeaterOperate rptHelper = new RepeaterOperate();
            rptHelper.dataBound(ref rptAllDiaries, ref dt, 1, ref lblTotal, 5);

        }
        if(e.CommandName == "btnDelClass") //删除分类
        {
            classID = e.CommandArgument.ToString();
            SQLOperation sql = new SQLOperation();
            
            if(sql.delete(" classes ", " id= " + classID) && sql.delete(" a_diary "," classId= " + classID))
            {
                helper.alertHelper(this, "删除分类成功","DiaryPage.aspx");
            }
        }
    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        string nowPage = lblNow.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        if (toPage <= Convert.ToInt32(lblTotal.Text))
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" * ", " a_diary ", " userId= " + id + " and classid=" + classID);
            rptHelper.dataBound(ref rptAllDiaries, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void btnDown_Click(object sender, EventArgs e)
    {
        string nowPage = lblNow.Text;
        int toPage = Convert.ToInt32(nowPage) - 1;

        if (toPage >= 1)
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" * ", " a_diary ", " userId= " + id + " and classid=" + classID);
            rptHelper.dataBound(ref rptAllDiaries, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        int toPage = 1;

        lblNow.Text = Convert.ToString(toPage);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" * ", " a_diary ", " userId= " + id + " and classid=" + classID);
        rptHelper.dataBound(ref rptAllDiaries, ref dt, toPage, ref lblTotal, 5);
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        string nowPage = lblTotal.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        lblNow.Text = Convert.ToString(toPage);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" * ", " a_diary ", " userId= " + id + " and classid=" + classID);
        rptHelper.dataBound(ref rptAllDiaries, ref dt, toPage, ref lblTotal, 5);
    }

    protected void btnJump_Click(object sender, EventArgs e)
    {
        int toPage = Convert.ToInt32(txtJump.Text);

        if (toPage <= Convert.ToInt32(lblTotal.Text) && toPage >= 1)
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            DataTable dt = sql.select(" * ", " a_diary ", " userId= " + id + " and classid=" + classID);
            rptHelper.dataBound(ref rptAllDiaries, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void btnNewClass_Click(object sender, EventArgs e) //控制添加新分类的控件的显示状态
    {
        if (divNewClass.Visible)
        {
            btnNewClass.Text = "添加新分类";
            divNewClass.Visible = false;
        }
        else
        {
            btnNewClass.Text = "取消添加";
            divNewClass.Visible = true;
        }
    }

    protected void btnAddClass_Click(object sender, EventArgs e)
    {
        string className = txtAddClass.Text;
        string id = Session["NowUserId"].ToString();
        SQLOperation sql = new SQLOperation();
        if(className == null)
        {
            txtAddClass.ForeColor = System.Drawing.Color.Red;
            txtAddClass.Text = "分类名不可以为空！";
        }
        else
        {
            if (sql.add(" classes ", " " + id + " ,'" + className + "','日志',0,'T'"))
            {
                Response.Write("<script> alert('添加分类成功！');location=  'DiaryPage.aspx'</script> ");

            }

        }
    }

    protected void btnBackMe_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PersonalCenterPage.aspx");
    }

    protected void rptAllDiaries_ItemDataBound(object sender, RepeaterItemEventArgs e) //为了不让除用户之外的访问者进行操作
    {
        if(Request.QueryString["id"] != null)
        {
            int count = rptAllDiaries.Items.Count;
            for(int i = 0;i < count;i++)
            {
                rptAllDiaries.Items[i].FindControl("lkbtnEdit").Visible = false;
                rptAllDiaries.Items[i].FindControl("btnDel").Visible = false;
            }

            Button btn = (Button)e.Item.FindControl("btnDel");
            string combinedID = btn.CommandArgument.ToString();
            string visitorID = Session["NowUserId"].ToString();
            string visitedID = Request.QueryString["id"].ToString();
            SQLOperation sql = new SQLOperation();
            DataTable dt = sql.select(" whocansee ", " a_diary ", " id=" + combinedID);
            string limit = dt.Rows[0][0].ToString();
            if (limit == "仅好友可见")
            {
                if (!helper.onlyFriendsJudge(visitorID, visitedID))
                {
                    e.Item.Visible = false;
                }
            }
            else if (limit == "部分好友不可见")
            {
                if (!helper.someFriendsCantSee(visitorID, visitedID, combinedID, "日志"))
                {
                    e.Item.Visible = false;
                }
            }
            else if (limit == "仅自己可见")
            {
                e.Item.Visible = false;
            }
        }
    }
}