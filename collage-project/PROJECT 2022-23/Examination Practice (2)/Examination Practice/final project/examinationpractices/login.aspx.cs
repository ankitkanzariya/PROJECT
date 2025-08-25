using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["slider"] = true;
    }
    protected void logins_Click(object sender, EventArgs e)
    {
        
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            SqlDataAdapter da = new SqlDataAdapter("select id,uname,email,acc_type,approve from Users where email='" + email.Text + "' and password='" + pass.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count >= 1)
            {
                Session["login"] = "yes";
                Session["uid"] = dt.Rows[0]["id"].ToString();
                Session["uname"] = dt.Rows[0]["uname"].ToString();
                Session["u_type"] = dt.Rows[0]["acc_type"].ToString();
                Session["email"] = dt.Rows[0]["email"].ToString();
               
                Session["acc"] = dt.Rows[0]["approve"].ToString();
                
                if (Session["u_type"].ToString() == "Admin")
                {
                    Response.Redirect("Admin/Default.aspx");
                }
                if (Session["u_type"].ToString() == "Student")
                {
                    Response.Redirect("course.aspx");
                }
                if (Session["u_type"].ToString() == "Teacher")
                {
                    if (Session["acc"].ToString() == "1")
                    {
                        Label1.Visible = true;
                        HyperLink1.Visible = true;
                        Label1.Text = "It looks like your account has been blocked please contact Administrator at ";
                        Session.Clear();
                    }
                    else
                    {
                        Response.Redirect("Teacher_question.aspx");
                    }
                }
            }
            else
            {
                Label1.Visible = true;
                Label1.Text="Invalid E-mail";
                //Response.Redirect("login.aspx");
            }

    }
}