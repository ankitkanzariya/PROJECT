using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class admin_showquestion : System.Web.UI.Page
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
            coursenameDropDownList.Items.Insert(0, new ListItem("---Select---", "0"));
            con.Close();
        }
    }
    protected void coursenameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListsem.Items.Clear();        
        //connnectDb();
        con.Open();
        //query = "select name from subcategory where  c_id=" + DropDownList1.SelectedIndex.ToString() + "";
        int countryId = int.Parse(coursenameDropDownList.SelectedItem.Value);
        if (countryId > 0)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from course where id=" + int.Parse(coursenameDropDownList.SelectedItem.Value), con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int s = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
            

            for (int i = 1; i < s + 1; i++)
            {
                DropDownListsem.Items.Add(i.ToString());
            }
            DropDownListsem.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        con.Close();
    }
    protected void DropDownListsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        con.Open();
        //query = "select name from subcategory where  c_id=" + DropDownList1.SelectedIndex.ToString() + "";
        int countryId = int.Parse(coursenameDropDownList.SelectedItem.Value);
        if (countryId > 0)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from sub where sem=" + DropDownListsem.SelectedItem.Value.ToString() + "and cid=" + coursenameDropDownList.SelectedItem.Value.ToString(), con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DropDownListsub.DataSource = ds;
            DropDownListsub.DataValueField = "id";
            DropDownListsub.DataTextField = "sname";
            DropDownListsub.DataBind();
            DropDownListsub.Items.Insert(0, new ListItem("-Select-", "0"));
            con.Close();
        }
        con.Close();
    }
    protected void show_Click(object sender, EventArgs e)
    {
                      

        try
        {
            Session["course"] = coursenameDropDownList.SelectedItem.Value.ToString();
            Session["sem"] = DropDownListsem.SelectedItem.Value.ToString();
            Session["sub"] = DropDownListsub.SelectedItem.Value.ToString();
        }
        catch(Exception ) {
            
        }
        
       
    }
}