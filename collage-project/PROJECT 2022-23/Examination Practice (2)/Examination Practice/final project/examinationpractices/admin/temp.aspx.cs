using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admin_temp : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["login"] == null)
        {
            Response.Write("Success Failed");
            Response.Redirect("../login.aspx");
        }
    }
    protected void set_Click(object sender, EventArgs e)
    {
        Response.Redirect("show.aspx");
    }
}