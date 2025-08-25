using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class admin_paperstyle : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["login"] == null)
        {
            Response.Write("Success Failed");
            Response.Redirect("../login.aspx");
        }
        if (!IsPostBack)
        {   
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select id,sname from course", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            coursenameDropDownList.DataSource = ds;
            coursenameDropDownList.DataValueField = "id";
            coursenameDropDownList.DataTextField = "sname";
            coursenameDropDownList.DataBind();
            coursenameDropDownList.Items.Insert(0, new ListItem("-Select-", "0"));
            con.Close();
        }
    }
 
    protected void Button2_Click(object sender, EventArgs e)
    {
        Session["mainno"] = mainno.Text;
        disque.Text = "";
        attque.Text = "";
        eachque.Text = "";
        totmark.Text = "";
        set.Visible = true;
        Session["course"] = coursenameDropDownList.SelectedItem.Value.ToString();
       // Session["sem"] = DropDownListsem.SelectedItem.Value.ToString();
       // Session["sub"] = DropDownListsub.SelectedItem.Value.ToString();
       // Session["sem"] = DropDownListsem.SelectedItem.Value.ToString();
       // lop.Text = Convert.ToString(Session["sub"]);
        disque.Text = "";
        attque.Text = "";
        eachque.Text = "";
        totmark.Text = "";
      
    }

    protected void set_Click(object sender, EventArgs e)
    {   
        Session["course"] = coursenameDropDownList.SelectedItem.Value.ToString();
        
        Session["disque"] = disque.Text.ToString();
        Session["attque"] = attque.Text.ToString();
        Session["eachque"] = eachque.Text.ToString();
        Session["totmark"] = totmark.Text.ToString();
        Session["mainno"] = mainno.Text.ToString();
        Session["time"] = timing.Text.ToString();

        string datas = "3,";
        string data4 = Convert.ToString(Session["totmark"]);
        string[] splitData4 = data4.Split(',');

        if (disque.Text == datas)
        {
            Response.Write("Please complete filed");
        }
        else
        {
            Response.Redirect("temp.aspx");
        }
    }   
}