using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_user_approve : System.Web.UI.Page
{
    SqlConnection con;
    SqlDataAdapter da;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        da = new SqlDataAdapter("update Users set approve=" + 0 + " where id=" + Request.QueryString["id"], con);
        dt = new DataTable();
        da.Fill(dt);

        da = new SqlDataAdapter("update question set hide=" + 0 + " where uid=" + Request.QueryString["id"], con);
        dt = new DataTable();
        da.Fill(dt);
        Response.Redirect("users.aspx");
    }
}