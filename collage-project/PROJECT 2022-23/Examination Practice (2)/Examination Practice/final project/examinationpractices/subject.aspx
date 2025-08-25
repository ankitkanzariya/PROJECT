<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="subject.aspx.cs" Inherits="subject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<br />
<div align="center">
      <div style="max-width:1000px;margin:16px;border:1px solid #ccc" align="center" class="w3-animate-zoom">
             <header class="w3-container w3-card-4 w3-blue"><h1 align="center"> Subject </h1></header>
           <div class="w3-container w3-section" align="center">
                <% 
                    System.Data.SqlClient.SqlConnection con;
                    System.Data.SqlClient.SqlDataAdapter da;
                    System.Data.DataTable dt;
                    con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                    da = new System.Data.SqlClient.SqlDataAdapter("SELECT id,sname from sub where cid=" + Request.QueryString["course"] + "and sem=" + Request.QueryString["sem"], con);
                    dt = new System.Data.DataTable(); 
                    da.Fill(dt);
                %>
                <%
                    int totalrecords2 = dt.Rows.Count;
                    for (int i = 0; i < totalrecords2; i++) 
                    {
                        Response.Write("<br/><a style='text-decoration: none;' href='cliquestionshow.aspx?&course=" + Request.QueryString["course"] + "&sem=" + Request.QueryString["sem"] + "&sub=" + dt.Rows[i]["id"].ToString() + " '><header class='w3-container w3-card-4 w3-lime'><h1 align='center'>" + dt.Rows[i][1].ToString() + "</b> </h1></header></a>");
                    }
                %>
          </div>
      </div>
</div>

<br /><br />

</asp:Content>

