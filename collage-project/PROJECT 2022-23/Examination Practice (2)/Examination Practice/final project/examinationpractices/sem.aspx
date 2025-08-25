<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="sem.aspx.cs" Inherits="sem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<br />
<div align="center">
        <div style="max-width:1000px;margin:16px;border:1px solid #ccc" align="center" class="w3-animate-zoom">
           <header class="w3-container w3-card-4 w3-blue"><h1 align="center"> Semesters </h1></header>
            <div class="w3-container w3-section" align="center">
              <%
                    System.Data.SqlClient.SqlConnection con;
                    System.Data.SqlClient.SqlDataAdapter da;
                    System.Data.DataTable dt;
                    con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                    da = new System.Data.SqlClient.SqlDataAdapter("SELECT * from course where id="+Request.QueryString["id"], con);
                    dt = new System.Data.DataTable(); 
                    da.Fill(dt);
              %>
              <% 
                   int s = Convert.ToInt32(dt.Rows[0][3]);
                   for (int i = 1; i < s + 1; i++)
                   {
                       Response.Write("<br/><a style='text-decoration:none;' href='subject.aspx?&course=" + Request.QueryString["id"] + "&sem=" + i.ToString() + "'><header class='w3-container w3-card-2 w3-khaki'><h1 align='center'> Semester - " + i.ToString() + " </b> </h1></header></a>");
                   } 
              %>
           </div>
       </div>
</div>

<br /><br /><br /><br />
</asp:Content>

