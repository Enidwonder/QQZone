using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PersonalInfoPage : System.Web.UI.Page
{
    SpecialOperations helper = new SpecialOperations();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if(Session["NowuserId"] != null)
            {
                string id;
                /*权限设置的选项初始化*/
                dplstLimit.Items.Add("所有人可见");
                dplstLimit.Items.Add("仅好友可见");
                dplstLimit.Items.Add("仅自己可见");
                dplstLimit.Items.Add("部分好友不可见");
                if (Request.QueryString["id"] == null) //not visiting
                {
                    divChanges.Visible = true;//if not visiting , you can change informations
                    id = Session["NowUserId"].ToString();
                    calBirthday.SelectedDate = DateTime.Now.Date;
                }
                else //visiting
                {
                    id = Request.QueryString["id"].ToString();
                    divChanges.Visible = false;
                    divChangeZone.Visible = false;
                }
                SQLOperation sql = new SQLOperation();
                DataTable dt = sql.select(" * ", " users ", " id =" + id);
                txtNickName.Text = dt.Rows[0][1].ToString();
                txtName.Text = dt.Rows[0][5].ToString();
                txtSex.Text= dt.Rows[0][6].ToString();
                txtAge.Text= dt.Rows[0][7].ToString();
                txtBirthday.Text= dt.Rows[0][10].ToString();
                txtEmail.Text= dt.Rows[0][9].ToString();
            }
            else
            {
                Response.Write("<script> alert('请先登录！');location=  'MainPage.aspx'</script> ");
            }
        }
        
    }

    protected void btnChangePhoto_Click(object sender, EventArgs e)
    {
        divUpload.Visible = true;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string id = Session["NowUserId"].ToString();
        //SpecialOperations operate = new SpecialOperations();
        //operate.PictureUpload(fileUpHead,1024);
        // 取得上传的文件对象

        
        HttpPostedFile hpf = Request.Files[0];
        string fileType = hpf.FileName.Substring(hpf.FileName.LastIndexOf(".") + 1).ToString(); //get the type
        if(fileType != "jpg" && fileType != "png")
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
            string fileName = id + "HeadPic." + fileType;
            // 取得服务器站点根目录的绝对路径
            string serverPath = Server.MapPath("~/images/");
            // 保存文件
            hpf.SaveAs(serverPath + fileName);
            SQLOperation sql = new SQLOperation();
            if (sql.update(" users ", " headPicture=' ~/images/" + fileName + "' ", " id=" + id))
                Response.Write("<srcipt>alert('csuccessful');location.href='PersonalInfoPage.aspx';</script>");
            else
                Response.Write("<srcipt>alert('unsuccessful');</script>");



        }

    }

    protected void btnChangeInfo_Click(object sender, EventArgs e)
    {
        txtName.ReadOnly = false;
        txtAge.ReadOnly = false;
        //txtBirthday.ReadOnly = false;
        calBirthday.Visible = true;
        txtEmail.ReadOnly = false;
        txtNickName.ReadOnly = false;
        txtSex.ReadOnly = false;

        divSaveChange.Visible = true;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if(calBirthday.SelectedDate != null)
        {
            txtBirthday.Text = calBirthday.SelectedDate.ToShortDateString();
        }
        string nickname = txtNickName.Text;
        string name = txtName.Text;
        string birthday = txtBirthday.Text;
        string email = txtEmail.Text;
        string sex = txtSex.Text;
        string age = txtAge.Text;

        if(txtNickName==null || txtSex == null ||txtEmail == null)
        {
            Response.Write("<script> alert('请填写完整！');</script> ");
        }
        else
        {
            SQLOperation sql = new SQLOperation();
            string id = Session["NowUserId"].ToString();
            bool updateResult = sql.update(" users ", " nickname=N'" + nickname + "',name=N'" + name + "',birthday='" + birthday + "',email='" + email + "',sex ='" + sex + "',age=" + age + " ", " id=" + id);
            if (updateResult)
            {
                Response.Write("<script> alert('修改信息成功！');location=  'PersonalInfoPage.aspx'</script> ");
            }
            else
            {
                Response.Write("<script> alert('修改信息失败！');location=  'PersonalInfoPage.aspx'</script> ");
            }
        }

        
    }

    protected void btnZoneChange_Click(object sender, EventArgs e)
    {
        if (txtZoneTitle.Visible == true) //点击为了取消修改
        {
            btnZoneChange.Text = "空间设置修改";
            txtZoneTitle.Visible = false;
            dplstLimit.Visible = false;
            btnZoneSave.Visible = false;
        }
        else
        {
            btnZoneChange.Text = "取消修改";
            txtZoneTitle.Visible = true;
            dplstLimit.Visible = true;
            btnZoneSave.Visible = true;
        }
    }

    protected void btnZoneSave_Click(object sender, EventArgs e)
    {
        try
        {
            SQLOperation sql = new SQLOperation();
            string userID = Session["NowUserId"].ToString();
            if(txtZoneTitle == null)
            {
                helper.alertHelper(this, "空间标题不可以为空！");
            }
            else
            {
                string limit = dplstLimit.SelectedValue.ToString();
                sql.update(" zoneInfo ", " title='" + txtZoneTitle.Text + "',WhoCanSee='" + limit + "' ", " userid= " + userID);
                btnZoneChange.Text = "空间设置修改";
                txtZoneTitle.Visible = false;
                dplstLimit.Visible = false;
                btnZoneSave.Visible = false;
                helper.alertHelper(this, "空间修改成功！", "PersonalInfoPage.aspx");
            }
        }
        catch(Exception exception)
        {
            helper.alertHelper(this, "空间修改失败！");
        }
    }

    protected void calBirthday_SelectionChanged(object sender, EventArgs e)
    {
        txtBirthday.Text = calBirthday.SelectedDate.ToShortDateString();
    }
}