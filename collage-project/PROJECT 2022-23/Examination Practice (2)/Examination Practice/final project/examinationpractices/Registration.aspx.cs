using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Registration : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
    SqlDataAdapter da;
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["slider"] = true;
    }
    protected void signup_Click(object sender, EventArgs e)
    {

        string path = Server.MapPath("admin/uploads/");

            da = new SqlDataAdapter("select * from Users where email='" + email.Text + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count != 1)
            {
                //response.write(path);
                if (FileUpload1.HasFile)
                {
                    FileUpload1.SaveAs(path + FileUpload1.FileName);
                    string name = FileUpload1.FileName;
                    
                     da = new SqlDataAdapter("insert into Users (uname,email,password,mobile,gender,birthdate,acc_type,proof,modified,approve) values ('" + username.Text + "','" + email.Text + "','" + password.Text + "'," + mobile.Text + ",'" + gen.SelectedItem.Text + "'," + birthdate.SelectedDate.ToString("yyyy/MM/dd") + ",'" + Account.SelectedItem.Text + "','" + name + "','" + Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) + "'," + 0 + ")", con);
                     ds = new DataSet();
                    da.Fill(dt);
                    Label1.Text = "sucess upload";
                    Response.Redirect("login.aspx");
                }
                else
                {
                    Response.Write("Please Select File");
                }
            }
            else
            {
                Label1.Text = "Email already Registered ";
            }
    }
}