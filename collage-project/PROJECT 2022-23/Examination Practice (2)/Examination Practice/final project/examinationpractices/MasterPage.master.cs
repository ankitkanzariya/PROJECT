using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void login_Click(object sender, EventArgs e)
    {
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        //SqlCommand da = new SqlCommand("select * from admin where email='" + email.Text + "' and pass='" + pass.Text + "'", con);
        //SqlDataAdapter cmd = new SqlDataAdapter(da);
        //DataTable dt = new DataTable();
        //cmd.Fill(dt);
        //if (dt.Rows.Count != 0)
        //{
        //    Session["user"] = email.Text;
        //    Response.Redirect("admin/default.aspx");
        //}
        //email.Text = "";
        //pass.Text = "";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
}
