using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_edit_course : System.Web.UI.Page
{
    SqlConnection con;
    SqlDataAdapter da;
    DataTable dt;
    //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["login"] == null)
        {
            Response.Write("Success Failed");
            Response.Redirect("../login.aspx");
        }
        if (!IsPostBack)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("select fname,sname,totsem from course where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            fname.Text = dt.Rows[0][0].ToString();
            sname.Text = dt.Rows[0][1].ToString();
            totsem.Text = dt.Rows[0][2].ToString();
            //ddlutype.Items.FindByValue(dt.Rows[0][3].ToString()).Selected = true;
        }
    }
    protected void update_Click(object sender, EventArgs e)
    {
        con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        da = new SqlDataAdapter("update course set fname='" + fname.Text + "',sname='" + sname.Text + "',totsem='" + totsem.Text + "' where id=" + Request.QueryString["id"], con);
        dt = new DataTable();
        da.Fill(dt);
        Response.Redirect("Default.aspx");
    }
}