using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Teacher_Teacher_delete_question : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["slider"] = null;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        SqlDataAdapter da = new SqlDataAdapter("delete from question where id=" + Request.QueryString["id"], con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        Response.Redirect("Teacher_question.aspx");
    }
}