using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // Response.Write(ds.Tables[0].Rows[0][0].ToString()); %>
    }
    public void dosi(object sender, EventArgs e)
    {
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(@"Data Source=PC146\SQLEXPRESS;Initial Catalog=exam_pra;Integrated Security=True");
        System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select sname form course", con);
        System.Data.DataSet ds = new System.Data.DataSet();
        da.Fill(ds);
        ds.Tables[0].Rows[0][0].ToString();
    }
}
