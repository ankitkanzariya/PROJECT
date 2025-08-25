using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_Teacher_question : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["slider"] = null;
        if (Session["login"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Session["u_type"].ToString() != "Teacher")
        {
            Response.Redirect("login.aspx");
        }
    }
}