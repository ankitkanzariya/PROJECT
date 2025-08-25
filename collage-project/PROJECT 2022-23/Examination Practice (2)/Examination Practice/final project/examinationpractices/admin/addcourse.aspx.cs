using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_addcourse : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["login"] == null)
        {
            Response.Write("Success Failed");
            Response.Redirect("../login.aspx");
        }
    }
    protected void addcour_Click(object sender, EventArgs e)
    {
       
            SqlDataAdapter da = new SqlDataAdapter("insert into course (fname,sname,totsem) values ('" + fname.Text + "','" + sname.Text + "'," + totsem.Text + ") ", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            //Label1.Text = "SUCESSFULLY";
            fname.Text = "";
            sname.Text = "";
            totsem.Text = "";
            Response.Redirect("Default.aspx");
       
    }
}