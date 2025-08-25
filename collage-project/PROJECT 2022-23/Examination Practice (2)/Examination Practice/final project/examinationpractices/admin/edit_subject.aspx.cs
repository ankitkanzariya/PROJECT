using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_editsubject : System.Web.UI.Page
{
    SqlConnection con;
    SqlDataAdapter da;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["login"] == null)
        {
            Response.Write("Success Failed");
            Response.Redirect("../login.aspx");
        }
        if (!IsPostBack)
        {
            dropdown1();
            dropdown2();
            
        }
    }
    protected void coursenameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListsem.Items.Clear();    
        dropdown2();
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("update sub set fname='" + fname.Text + "',sname='" + sname.Text + "',cid='" + coursenameDropDownList.SelectedItem.Value + "',sem='" + DropDownListsem.SelectedItem.Value + "' where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            Response.Redirect("subject.aspx");
    }
    public void dropdown2()
    {
        con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        da = new SqlDataAdapter("select * from course where id=" + int.Parse(coursenameDropDownList.SelectedItem.Value), con);
        dt = new DataTable();
        da.Fill(dt);
        DropDownListsem.Items.Insert(0, new ListItem("--- Select ---", "0"));
        
        int s = Convert.ToInt32(dt.Rows[0][3]);
        
        for (int i = 1; i < s + 1; i++)
        {
            DropDownListsem.Items.Add(i.ToString());
        }
    }
    public void dropdown1()
    {
        con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        
        da = new SqlDataAdapter("select id,sname from course", con);
        dt = new DataTable();
        da.Fill(dt);
        coursenameDropDownList.Items.Clear();

        coursenameDropDownList.Items.Insert(0, new ListItem("--- Select ---", "0"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            coursenameDropDownList.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
        }

        da = new SqlDataAdapter("select cid,sem,fname,sname from sub where id=" + Request.QueryString["id"], con);
        dt = new DataTable();
        da.Fill(dt);
        fname.Text = dt.Rows[0][2].ToString();
        sname.Text = dt.Rows[0][3].ToString();
        coursenameDropDownList.SelectedValue = dt.Rows[0][0].ToString();
        DropDownListsem.SelectedValue = dt.Rows[0][1].ToString();
    }
}