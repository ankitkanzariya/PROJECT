using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

public partial class edit_profile : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["login"] == null)
        {
            Response.Write("Success Failed");
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            SqlDataAdapter da = new SqlDataAdapter(" select *  from Users where id='" + (Session["uid"]) + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            nname.Text = dt.Rows[0]["uname"].ToString();
            nmobile.Text = dt.Rows[0]["mobile"].ToString();
            email.Text = dt.Rows[0]["email"].ToString();
            password.Text = dt.Rows[0]["password"].ToString();
        }
    }

    protected void save_Click(object sender, EventArgs e)
    {
       
        string path = Server.MapPath("admin/uploads/");
        string ext = Path.GetExtension(nid.FileName);
        con.Open();
            nid.SaveAs(path + nid.FileName);
            string fname = nid.FileName;
            SqlCommand cmd = new SqlCommand("update Users set uname='" + nname.Text + "',mobile='" + nmobile.Text + "',proof='" + fname + "',birthdate=" + birthdate.SelectedDate.ToString("yyyy/MM/dd") + ",email='" + email.Text + "',password='" + password.Text + "' where id='" + (Session["uid"]) + "'", con);
            cmd.ExecuteNonQuery();

            con.Close();
    }

}