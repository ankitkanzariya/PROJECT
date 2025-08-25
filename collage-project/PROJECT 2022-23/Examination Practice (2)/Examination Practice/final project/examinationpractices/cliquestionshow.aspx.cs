using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cliquestionshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["slider"] = null;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("cliquestionshow.aspx?&course=" + Request.QueryString["course"] + "&sem=" + Request.QueryString["sem"] + "&sub=" + Request.QueryString["sub"] + " '");
    }
}