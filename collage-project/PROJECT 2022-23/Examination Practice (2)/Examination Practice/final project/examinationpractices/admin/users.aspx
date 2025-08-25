<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="users.aspx.cs" Inherits="admin_users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <h1 align="center">Users</h1>


 <div class="w3-container">
	<div class="col-lg-12 w3-animate-zoom">
          <div class="w3-card-4">
              <div class="w3-responsive"> 
                    <table class="w3-table-all w3-hoverable" >
                           <thead >
                            <tr class="w3-light-blue">
                                <th style="text-align:center">No.</th>
                                <th style="text-align:center">Name</th>
                                <th style="text-align:center">Email</th>
                                <th style="text-align:center">Account Type</th>
                                <th style="text-align:center">Profile</th>
                                <th style="text-align:center">approve</th>
                            </tr>
                           </thead>
                           <tbody >
                    <%  System.Data.SqlClient.SqlConnection con;
                        System.Data.SqlClient.SqlDataAdapter da;
                        System.Data.DataTable dt;
                        con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                        da = new System.Data.SqlClient.SqlDataAdapter("select id,uname,email,acc_type from Users where approve=0 order by acc_type DESC", con);
                         dt = new System.Data.DataTable(); 
                         da.Fill(dt);%>
                             
                      <%
                          int totalrecords = dt.Rows.Count;
                          for (int i = 0; i < totalrecords; i++) 
                        {
                               
                        %>
                        <tr  class="w3-hover-red" style="text-align:center">
                            <td style="text-align:center"><% Response.Write(i+1); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["uname"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["email"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["acc_type"].ToString()); %></td>        
                            <td style="text-align:center"><a href="users_profile.aspx?id=<% Response.Write(dt.Rows[i]["id"].ToString()); %>"><img src='images/profile.png' height='25' /></a></td>
                            <td style="text-align:center"><a href="user_unapprove.aspx?id=<% Response.Write(dt.Rows[i]["id"].ToString()); %>"><img src='images/approve.jpg' height='25' /></a></td>
                        </tr>
                        <% } %>
                        
                       <% 
                         con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                         da = new System.Data.SqlClient.SqlDataAdapter("select id,uname,email,acc_type from Users where approve=1 order by acc_type DESC", con);
                         dt = new System.Data.DataTable(); 
                         da.Fill(dt);%>
                             
                      <%
                          int totalrecords1 = dt.Rows.Count;
                          for (int j = 0; j < totalrecords1; j++) 
                        {
                               
                        %>
                         <tr  class="w3-hover-red" style="text-align:center">
                            <td style="text-align:center"><% Response.Write(j+1); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[j]["uname"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[j]["email"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[j]["acc_type"].ToString()); %></td>        
                            <td style="text-align:center"><a href="users_profile.aspx?id=<% Response.Write(dt.Rows[j]["id"].ToString()); %>"><img src='images/profile.png' height='25' /></a></td>
                            <td style="text-align:center"><a href="user_approve.aspx?id=<% Response.Write(dt.Rows[j]["id"].ToString()); %>"><img src='images/unapprove.jpg' height='25' /></a></td>
                       </tr>
                        <% } %>


                            </tbody>
                    </table>
              </div>
           </div>
        </div>
            
</asp:Content>

