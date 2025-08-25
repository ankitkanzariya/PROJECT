<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="subject.aspx.cs" Inherits="admin_subject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            
<div align="center"><a href="addsubject.aspx" class="btn btn-warning btn-warng1" >Add Subject</a></div><br />
	<div class="col-lg-12 w3-animate-zoom">
        <div class="w3-card-4">
          <div class="w3-responsive"> 
             <table class="w3-table-all w3-hoverable">
                 <thead >
                    <tr class="w3-light-blue">
                        <th style="text-align:center">No.</th>
                        <th style="text-align:center">Course Name</th>
                        <th style="text-align:center">Semester</th>
                        <th style="text-align:center">Full Name</th>
                        <th style="text-align:center">Short Name</th>
                        <th style="text-align:center">Edit</th>
                        <th style="text-align:center">Delete</th></tr>
                  </thead>
                  <tbody>
                        <%  System.Data.SqlClient.SqlConnection con;
                            System.Data.SqlClient.SqlDataAdapter da;
                            System.Data.DataTable dt;
                            con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                            da = new System.Data.SqlClient.SqlDataAdapter("SELECT s.id,c.sname,s.sem,s.fname,s.sname from sub s,course c where s.cid=c.id", con);
                            dt = new System.Data.DataTable(); 
                            da.Fill(dt);
                         %>
                             
                         <%
                            int totalrecords2 = dt.Rows.Count;
                            for (int i = 0; i < totalrecords2; i++) 
                            {
                               
                            %>
                            <tr class="w3-hover-red">
                                <td style="text-align:center"><% Response.Write(i+1); %></td>
                                <td style="text-align:center"><% Response.Write(dt.Rows[i][1].ToString()); %></td>
                                <td style="text-align:center"><% Response.Write(dt.Rows[i][2].ToString()); %></td>
                                <td style="text-align:center"><% Response.Write(dt.Rows[i][3].ToString()); %></td>   
                                <td style="text-align:center"><% Response.Write(dt.Rows[i][4].ToString()); %></td>   
                                <td style="text-align:center"><a href="edit_subject.aspx?id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/edit.png' height='25' /></a></td>
                                <td style="text-align:center"><a href="Delete.aspx?&tbl=subject&id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/del.png' height='25' /></a></td>
                            </tr>
                            <% } %>
                   </tbody>
            </table>
        </div>
      </div><br /><br />
    </div>
          
</asp:Content>

