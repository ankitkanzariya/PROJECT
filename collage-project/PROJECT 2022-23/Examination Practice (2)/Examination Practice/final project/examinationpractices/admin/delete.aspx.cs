using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class admin_delete : System.Web.UI.Page
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
        if (Request.QueryString["tbl"] == "course")
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from course where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from sub where cid=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from question where cid=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            Response.Redirect("default.aspx");
        }
        if (Request.QueryString["tbl"] == "subject")
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from sub where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            da = new SqlDataAdapter("delete from question where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            Response.Redirect("subject.aspx");
        }
        if (Request.QueryString["tbl"] == "question")
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from question where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            Response.Redirect("question.aspx");
        }
        if (Request.QueryString["tbl"] == "collage")
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from collage where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            Response.Redirect("collage.aspx");
        }
        if (Request.QueryString["tbl"] == "user_question")
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
            da = new SqlDataAdapter("delete from question where id=" + Request.QueryString["id"], con);
            dt = new DataTable();
            da.Fill(dt);
            Response.Redirect("Users_question_view.aspx?id=" + Request.QueryString["uid"] + "");
        }
    }
}