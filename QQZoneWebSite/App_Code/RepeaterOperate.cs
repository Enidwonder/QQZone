using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// RepeaterOperate 的摘要说明
/// </summary>
public class RepeaterOperate
{
    public RepeaterOperate()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    

    public bool dataBound(ref Repeater rpt, ref  DataTable  dt,int currentPage,ref Label lbl,int pageSize)
    {
        PagedDataSource pds = new PagedDataSource();

        pds.AllowPaging = true;

        pds.PageSize = pageSize;

        pds.DataSource = dt.DefaultView;

        lbl.Text = pds.PageCount.ToString();

        pds.CurrentPageIndex = currentPage - 1;//当前页数从零开始，故把接受的数减一

        rpt.DataSource = pds;

        rpt.DataBind();


        
        return true;
    }
}