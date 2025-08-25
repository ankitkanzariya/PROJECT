using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class admin_addsubject : System.Web.UI.Page
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
            coursenameDropDownList.Items.Insert(0, new ListItem("--- Select ---", "0"));
            con.Close();
        }
    }
    protected void coursenameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListsem.Items.Clear();
        con.Open();
        //query = "select name from subcategory where  c_id=" + DropDownList1.SelectedIndex.ToString() + "";
        int countryId = int.Parse(coursenameDropDownList.SelectedItem.Value);
        if (countryId > 0)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from course where id=" + int.Parse(coursenameDropDownList.SelectedItem.Value), con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int s = Convert.ToInt32(ds.Tables[0].Rows[0][3]);
            lop.Text = ds.Tables[0].Rows[0][0].ToString();
            for (int i = 1; i < s + 1; i++)
            {
                DropDownListsem.Items.Add(i.ToString());
            }
            DropDownListsem.Items.Insert( 0, new ListItem("-Select-", "0"));
        }
        con.Close();
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
    
        SqlDataAdapter da = new SqlDataAdapter("insert into sub (cid,sem,fname,sname) values ('" + coursenameDropDownList.SelectedItem.Value + "'," + DropDownListsem.SelectedItem.Value + ",'" + fname.Text + "','" + sname.Text + "') ", con);
        DataSet ds = new DataSet();
        da.Fill(ds);
        fname.Text = "";
        sname.Text = "";
        Response.Redirect("subject.aspx");
    
    }
}