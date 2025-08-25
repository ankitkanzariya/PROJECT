using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class Teacher_Teacher_edit_question : System.Web.UI.Page
{
    SqlConnection con;
    SqlDataAdapter da;
    DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["slider"] = null;
        if (!IsPostBack)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            //da = new SqlDataAdapter("select fname,sname,totsem from course where id=" + Request.QueryString["id"], con);
            da = new SqlDataAdapter("select id,sname from course", con);
            dt = new DataTable();
            da.Fill(dt);
            coursenameDropDownList.Items.Clear();
            coursenameDropDownList.Items.Add(new ListItem("--------Main--------", "0"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                coursenameDropDownList.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
            }

            da = new SqlDataAdapter("select chno,que,mark,remark,link from question where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            chno.Text = dt.Rows[0][0].ToString();
            TextArea1.Value = dt.Rows[0][1].ToString();
            mark.Text = dt.Rows[0][2].ToString();
            remark.Text = dt.Rows[0][3].ToString();
            link.Text = dt.Rows[0][4].ToString();
        }
    }
    protected void coursenameDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownListsem.Items.Clear();
        con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        da = new SqlDataAdapter("select * from course where id=" + int.Parse(coursenameDropDownList.SelectedItem.Value), con);
        dt = new DataTable();
        da.Fill(dt);
        DropDownListsem.Items.Add(new ListItem("--------Main--------", "0"));
        int s = Convert.ToInt32(dt.Rows[0][3]);
        //lop.Text = ds.Tables[0].Rows[0][0].ToString();
        for (int i = 1; i < s + 1; i++)
        {
            DropDownListsem.Items.Add(i.ToString());
        }
    }
    protected void DropDownListsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        //query = "select name from subcategory where  c_id=" + DropDownList1.SelectedIndex.ToString() + "";
        int countryId = int.Parse(coursenameDropDownList.SelectedItem.Value);
        if (countryId > 0)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("select * from sub where sem=" + DropDownListsem.SelectedItem.Value.ToString() + "and cid=" + coursenameDropDownList.SelectedItem.Value.ToString(), con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DropDownListsub.DataSource = ds;
            DropDownListsub.DataValueField = "id";
            DropDownListsub.DataTextField = "sname";
            DropDownListsub.DataBind();
            con.Close();
        }

    }
    protected void update_Click(object sender, EventArgs e)
    {
        con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
        da = new SqlDataAdapter("update question set chno='" + chno.Text + "',que='" + TextArea1.Value + "',mark='" + mark.Text + "',remark='" + remark.Text + "',cid='" + coursenameDropDownList.SelectedItem.Value + "',sid='" + DropDownListsem.SelectedItem.Value + "',subid='" + DropDownListsub.SelectedItem.Value + "' where id=" + Request.QueryString["id"], con);
        dt = new DataTable();
        da.Fill(dt);
        Response.Redirect("Teacher_question.aspx");
    }
}