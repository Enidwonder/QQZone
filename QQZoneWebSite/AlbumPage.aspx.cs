using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AlbumPage : System.Web.UI.Page
{
    SpecialOperations helper = new SpecialOperations();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if(Session["NowUserId"] != null)
            {
                string id;
                SQLOperation sql = new SQLOperation();
                DataTable dt = new DataTable();
                divNewAlbum.Visible = false;
                /*权限设置的选项初始化*/
                dplstLimit.Items.Add("所有人可见");
                dplstLimit.Items.Add("仅好友可见");
                dplstLimit.Items.Add("仅自己可见");
                dplstLimit.Items.Add("部分好友不可见");
                  
                if (Request.QueryString["id"] != null) //visiting
                {
                    id = Request.QueryString["id"].ToString();
               
                    rptAllAlbum.FindControl("btnDelAlbum").Visible = false;//访问者不可以删除相册
                    rptPhotos.FindControl("btnDelPhoto").Visible = false; //访问者不可以删除照片 
                    btnNewAlbum.Visible = false; //访问者不可以添加相册
                    divAddPhoto.Visible = false;//访问者不可以上传照片

                    /*绑定数据使显示所有相册*/
                    dt = sql.select(" id,AlbumName ", " a_album ", " userId = " + id);
                    rptAllAlbum.DataSource = dt;
                    rptAllAlbum.DataBind();
                }
                else //not visiting 
                {
                    id = Session["NowUserId"].ToString();

                    /*给相册分类的下拉框绑定数据并且赋给默认值*/
                    dt = sql.select(" * ", " a_album ", " userId = " + id);
                    dplstAlbums.DataSource = dt;
                    dplstAlbums.DataBind();
                    if (Request.QueryString["albumId"] != null)
                    {
                        dplstAlbums.SelectedValue = Request.QueryString["albumId"].ToString();
                    }

                    /*绑定数据使显示所有相册*/
                    dt = sql.select(" id,AlbumName ", " a_album ", " userId = " + id);
                    RepeaterOperate rptHelper = new RepeaterOperate();
                    rptHelper.dataBound(ref rptAllAlbum, ref dt, 1, ref lblTotal, 5);
                    lblNow.Text = "1";
                }
            }
            else
            {
                Response.Write("<script> alert('请先登录！');location=  'MainPage.aspx'</script> ");
            }
            
        }
    }

    protected void rptAllAlbum_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if(Request.QueryString["id"] != null)
        {
            string visitedID = Request.QueryString["id"].ToString();
            string visitorID = Session["NowUserId"].ToString();
            Button btn = (Button)e.Item.FindControl("btnDelAlbum");
            string combinedID = btn.CommandArgument.ToString();
            SQLOperation sql = new SQLOperation();
            DataTable dt = sql.select(" whocansee ", " a_album ", " id=" + combinedID);
            string limit = dt.Rows[0][0].ToString();
            if (limit == "仅好友可见")
            {
                if(!helper.onlyFriendsJudge(visitorID, visitedID))
                {
                    e.Item.Visible = false;
                }
            }
            else if(limit == "部分好友不可见")
            {
                if(!helper.someFriendsCantSee(visitorID, visitedID, combinedID, "相册"))
                {
                    e.Item.Visible = false;
                }
            }
            else if(limit == "仅自己可见")
            {
                e.Item.Visible = false;
            }
        }
        
    }

    protected void rptAllAlbum_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        SQLOperation sql = new SQLOperation();
        RepeaterOperate rptHelper = new RepeaterOperate();
        DataTable dt = new DataTable();
        if (e.CommandName == "btnDelAlbum")
        {
            string albumId = e.CommandArgument.ToString();
            if(sql.delete(" a_album "," id =" + albumId))
            {
                Response.Write("<script> alert('删除相册成功！');location=  'AlbumPage.aspx'</script> ");
            }
            else
            {
                Response.Write("<script> alert('删除相册失败！');</script> ");
            }
        }
        else if (e.CommandName == "imgBtnAlbum")
        {
            rptAllAlbum.Visible = false;
            divShowPhotos.Visible = true;
            divPagesForAll.Visible = false;
            divPagesForPhoto.Visible = true;

            /*绑定数据是显示当前相册下的所有照片*/
            dt = sql.select(" * ", " a_photo ", " albumID=" + Request.QueryString["albumId"].ToString());
            rptHelper.dataBound(ref rptPhotos, ref dt, 1, ref lblTotalPhoto, 30);
            lblNowPhoto.Text = "1";
        }
    } //所有相册的显示--按钮判断

    protected void btnNewAlbum_Click(object sender, EventArgs e)
    {
        divNewAlbum.Visible = true;
    } //创建相册按钮--点开之后使与创建有关的输入、按钮等内容可见

    protected void btnAddNew_Click(object sender, EventArgs e) //立刻创建相册按钮
    {
        if(divNewAlbum.Visible = true)
        {
            btnAddNew.Text = "立刻创建";
            
            string newName = txtNewName.Text;
            string newDes = txtNewDescipt.Text;
            SQLOperation sql = new SQLOperation();
            //id,userid,albumname,amount,des,null
            string id = Session["NowUserId"].ToString();
            string limit = dplstLimit.SelectedValue.ToString();
            if (sql.add(" a_album ", " " + id + ",'" + newName + "',0,'" + newDes + "','"+limit+"'"))
            {
                divNewAlbum.Visible = false;
                btnNewAlbum.Visible = true;
                Response.Write("<script> alert('添加相册成功！');location=  'AlbumPage.aspx'</script> ");
            }
        }
        else
        {
            btnAddNew.Text = "取消创建";
            btnNewAlbum.Visible = false;
            divNewAlbum.Visible = true;
        }
    }

    protected void rptPhotos_ItemCommand(object source, RepeaterCommandEventArgs e) //显示当前相册下所有照片 --- 按钮判断
    {
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = new DataTable();
        string rootPath = "E:/enid1/Documents/003IT/程序/QQZoneWebSite";
        if (e.CommandName== "btnDelPhoto")//删除图片
        {
            string picID = e.CommandArgument.ToString(); //得到这张图片在数据库中的ID
            dt = sql.select(" picture ", " a_photo ", " id=" + picID);
            string path = rootPath + dt.Rows[0][0].ToString().Substring(1);
            dt = sql.select(" amount ", " a_album ", " id =" + Request.QueryString["albumId"].ToString());
            string amount = (int.Parse(dt.Rows[0][0].ToString()) - 1).ToString();
            if (File.Exists(path))
            {
                File.Delete(path);
                if (sql.delete(" a_photo ", " id=" + picID) && sql.update(" a_album ", " amount =" + amount, " id= " + Request.QueryString["albumId"].ToString()))
                    Response.Write("<script> alert('删除照片成功！');location=  'AlbumPage.aspx'</script> ");
            }
        }
        if(e.CommandName == "btnLikePhoto") //点赞
        {
            string time = DateTime.Now.ToString();
            string picID = e.CommandArgument.ToString(); //得到这张图片在数据库中的ID
            Button btn = (Button)e.Item.FindControl("btnLikePhoto");
            if (Request.QueryString["id"] != null)
            {
                id = Request.QueryString["id"].ToString();
            }

            if (btn.Text.StartsWith("赞"))
            {
                dt = sql.select(" likeamount ", " a_photo ", " id=" + picID);
                int midAmount = int.Parse(dt.Rows[0][0].ToString()) + 1;
                string amount = midAmount.ToString();
                //userid,likedid,likekind,time
                if (sql.add(" a_like ", " " + id + "," + picID + ",'图片','" + time + "'") && sql.update(" a_photo ", " likeamount =" + amount + " ", " id=" + picID))
                {
                    btn.Text = "取消赞(" + amount + ")";
                }             
            }
            else
            {
                 dt = sql.select(" likeamount ", " a_photo ", " id=" + picID);
                int midAmount = int.Parse(dt.Rows[0][0].ToString()) - 1;
                string amount = midAmount.ToString();
                if (sql.delete(" a_like ", " likedId=" + picID + " and userId=" + id + " and likekind='图片'") && sql.update(" a_photo ", " likeamount =" + amount + " ", " id=" + picID))
                {
                    btn.Text = "赞（" + amount + ")";
                }
            }
            
        }
    }

    protected void btnAddPhoto_Click(object sender, EventArgs e) //点击上传图片
    {
        if (fupAddPhoto.Visible)
        {
            btnAddPhoto.Text = "上传照片";
            fupAddPhoto.Visible = false;
            btnUploading.Visible = false;
            dplstAlbums.Visible = false;
        }
        else
        {
            btnAddPhoto.Text = "取消上传";
            fupAddPhoto.Visible = true;
            btnUploading.Visible = true;
            dplstAlbums.Visible = true;
        }
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        if(Request.QueryString["id"] != null)
        {
            id = Request.QueryString["id"].ToString();
        }
        
        DataTable dt = sql.select(" * ", " a_album ", " userid=" + id);
        dplstAlbums.DataSource = dt;
        dplstAlbums.DataTextField = "AlbumName";
        dplstAlbums.DataValueField = "id";
        dplstAlbums.DataBind();
        /*初始化相册下拉框选中值*/
        if (Request.QueryString["albumId"] != null)
        {
            dplstAlbums.SelectedValue = Request.QueryString["albumId"].ToString();
        }
        /*接下来是文件上传的工作*/

    }
    protected void btnUploading_Click(object sender, EventArgs e) //上传图片在文件夹和数据库中的操作
    {
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        HttpPostedFile hpf = Request.Files[0];
        string fileType = hpf.FileName.Substring(hpf.FileName.LastIndexOf(".") + 1).ToString(); //get the type
        string albumID = dplstAlbums.SelectedValue.ToString(); //获得储存照片的相册ID
        DataTable dt = sql.select(" amount ", " a_album ", " id=" + albumID);
        int count = int.Parse(dt.Rows[0][0].ToString()) + 1;

        dt = sql.select(" id ", " a_album ", " albumName='所有照片' ");// 获得默认相册的ID
        string albumDefaultID = dt.Rows[0][0].ToString();

        string time = DateTime.Now.ToShortDateString();
        if (fileType != "jpg" && fileType != "png")
        {
            Response.Write("<script>alert('上传图片类型错误!')</script>");
        }
        else if (hpf.ContentLength < 1024 || hpf.ContentLength > 1048576)
        {
            Response.Write("<script>alert('上传图片不得小于1K或不得大于1M!')</script>");
        }
        else
        {
            // 取得文件路径
            //string filePath = hpf.FileName;
            // 从路径中取出文件名用来作为保存的文件名
            string fileName = id + count.ToString()+"Pic." + fileType;
            // 取得服务器站点根目录的绝对路径
            string serverPath = Server.MapPath("~/images/");
            // 保存文件
            hpf.SaveAs(serverPath + fileName);
            //userid,picture,albumid,time,whocansee,likeAmount
            //这里添加的时候要在相册和所有照片中各加一张
            if (sql.add(" a_photo ", " "+ id +",'~/images/" + fileName + "',"+albumID+",'"+time+"',null,0") && sql.add(" a_photo ", " " + id + ",'~/images/" + fileName + "'," + albumDefaultID + ",'" + time + "',null,0"))
                Response.Write("<srcipt>alert('上传照片成功');location='AlbumPage.aspx';</script>");
            else
                Response.Write("<srcipt>alert('上传失败！');</script>");
        }
    }

    //----------------------------------------------------下面均为分页按钮-----------------------------------------//
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
            DataTable dt = sql.select(" * ", " a_album ", " userId= " + id );
            rptHelper.dataBound(ref rptAllAlbum, ref dt, toPage, ref lblTotal, 5);

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
            DataTable dt = sql.select(" * ", " a_album ", " userId= " + id);
            rptHelper.dataBound(ref rptAllAlbum, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        int toPage = 1;

        lblNow.Text = Convert.ToString(toPage);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" * ", " a_album ", " userId= " + id);
        rptHelper.dataBound(ref rptAllAlbum, ref dt, toPage, ref lblTotal, 5);
    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        string nowPage = lblTotal.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        lblNow.Text = Convert.ToString(toPage);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowUserId"].ToString();
        DataTable dt = sql.select(" * ", " a_album ", " userId= " + id);
        rptHelper.dataBound(ref rptAllAlbum, ref dt, toPage, ref lblTotal, 5);
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
            DataTable dt = sql.select(" * ", " a_album ", " userId= " + id);
            rptHelper.dataBound(ref rptAllAlbum, ref dt, toPage, ref lblTotal, 5);

        }
    }

    protected void btnUpPhoto_Click(object sender, EventArgs e)
    {
        string nowPage = lblNowPhoto.Text;
        int toPage = Convert.ToInt32(nowPage) + 1;

        if (toPage <= Convert.ToInt32(lblTotalPhoto.Text))
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowAlbumId"].ToString();
            DataTable dt = sql.select(" * ", " a_photo ", " albumId= " + id);
            rptHelper.dataBound(ref rptPhotos, ref dt, toPage, ref lblTotalPhoto, 30);

        }
    }

    protected void btnDownPhoto_Click(object sender, EventArgs e)
    {
        string nowPage = lblNowPhoto.Text;
        int toPage = Convert.ToInt32(nowPage) - 1;

        if (toPage >= 1)
        {
            lblNow.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowAlbumId"].ToString();
            DataTable dt = sql.select(" * ", " a_photo ", " albumId= " + id);
            rptHelper.dataBound(ref rptPhotos, ref dt, toPage, ref lblTotalPhoto, 30);

        }
    }

    protected void btnFirstPhoto_Click(object sender, EventArgs e)
    {
        int toPage = 1;

        lblNowPhoto.Text = Convert.ToString(toPage);
        RepeaterOperate rptHelper = new RepeaterOperate();
        SQLOperation sql = new SQLOperation();
        string id = Session["NowAlbumId"].ToString();
        DataTable dt = sql.select(" * ", " a_photo ", " albumId= " + id);
        rptHelper.dataBound(ref rptPhotos, ref dt, toPage, ref lblTotalPhoto, 30);
    }

    protected void btnLastPhoto_Click(object sender, EventArgs e)
{
    string nowPage = lblTotalPhoto.Text;
    int toPage = Convert.ToInt32(nowPage) + 1;

    lblNowPhoto.Text = Convert.ToString(toPage);
    RepeaterOperate rptHelper = new RepeaterOperate();
    SQLOperation sql = new SQLOperation();
    string id = Session["NowAlbumId"].ToString();
    DataTable dt = sql.select(" * ", " a_photo ", " albumId= " + id);
    rptHelper.dataBound(ref rptPhotos, ref dt, toPage, ref lblTotalPhoto, 30);
}

    protected void btnJumpPhoto_Click(object sender, EventArgs e)
    {
        int toPage = Convert.ToInt32(txtJumpPhoto.Text);

        if (toPage <= Convert.ToInt32(lblTotalPhoto.Text) && toPage >= 1)
        {
            lblNowPhoto.Text = Convert.ToString(toPage);
            RepeaterOperate rptHelper = new RepeaterOperate();
            SQLOperation sql = new SQLOperation();
            string id = Session["NowAlbumId"].ToString();
            DataTable dt = sql.select(" * ", " a_photo ", " albumId= " + id);
            rptHelper.dataBound(ref rptPhotos, ref dt, toPage, ref lblTotalPhoto, 30);

        }
    }


    protected void dplstLimit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(e.ToString() == "部分好友不可见")
        {
            ckbxLimit.Visible = true;
            SQLOperation sql = new SQLOperation();
            string userId = Session["NowUserId"].ToString();
            DataTable dt1 = sql.select(" users.username ", " friends,users ", " (friends.userid = " + userId + " and users.id= friends.friendId) or (friends.friendId=" + userId + " and users.id = friends.userId)");
            ckbxLimit.DataSource = dt1;
            ckbxLimit.DataBind();
        }
    }
}