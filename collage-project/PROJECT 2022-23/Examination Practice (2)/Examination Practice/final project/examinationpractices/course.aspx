<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="course.aspx.cs" Inherits="course" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<br />
<div align="center">
        <div style="max-width:1000px;margin:16px;border:1px solid #ccc" align="center" class="w3-animate-zoom">
          <header class="w3-container w3-card-4 w3-blue"><h1 align="center"> COURSE </h1></header>
             <div class="w3-container w3-section" align="center">
              
                <%
                    System.Data.SqlClient.SqlConnection con;
                    System.Data.SqlClient.SqlDataAdapter da;
                    System.Data.DataTable dt;
        
                    con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                    da = new System.Data.SqlClient.SqlDataAdapter("SELECT id,sname from course", con);
                    dt = new System.Data.DataTable(); 
                    da.Fill(dt);
                  %>
             
                  <%
                     int totalrecords2 = dt.Rows.Count;
                     for (int i = 0; i < totalrecords2; i++) 
                     { 
                           Response.Write("<br/><b><a style='text-decoration: none;' href='sem.aspx?&id=" + dt.Rows[i][0].ToString() + "'><header class='w3-container w3-card-4 w3-light-green'><h1 align='center'> " + dt.Rows[i][1].ToString() + " </b> </h1></header></a>"); 
                     } 
                  %>
           </div>
        </div>
</div>
    <br /><br />


</asp:Content>

