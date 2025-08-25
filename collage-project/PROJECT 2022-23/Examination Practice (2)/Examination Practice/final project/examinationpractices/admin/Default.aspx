<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>

    $(document).ready(function () {

        //paging
        $(document).on("click", '.hide1', function (event) {
            alert("sdfd");
            var id = $(this).attr('data');
            /*alert(page)*/

            $.ajax({
                url: "hide.aspx?id="+ id,
                method: "POST",
                data: { id: id },

                success: function (data) {
                    $('.cateringdish').html(data);
                }
            });
        });

    });
  </script>

  <div align="center"><a href="addcourse.aspx" class="btn btn-warning btn-warng1" >Add Course</a></div><br />
  <div class="w3-container">

	<div class="col-lg-12 w3-animate-zoom">
          <div class="w3-card-4">
              <div class="w3-responsive"> 
                    <table class="w3-table-all w3-hoverable" >
                           <thead >
                            <tr class="w3-light-blue">
                                <th style="text-align:center">No.</th>
                                <th style="text-align:center">Full Name</th>
                                <th style="text-align:center">Short Name</th>
                                <th style="text-align:center">Semester</th>
                                <th style="text-align:center">Edit</th>
                                <th style="text-align:center">Delete</th>
                            </tr>
                           </thead>
                           <tbody >
                    <%  System.Data.SqlClient.SqlConnection con;
                        System.Data.SqlClient.SqlDataAdapter da;
                        System.Data.DataTable dt;
                        con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                        da = new System.Data.SqlClient.SqlDataAdapter("select * from course", con);
                         dt = new System.Data.DataTable(); 
                         da.Fill(dt);

                       // int l = 0;  //int j =  1 * Convert.ToInt32(Session["mainno"]); //Response.Write(Convert.ToString(Session["mainno"])); %>
                             
                      <%
                          int totalrecords = dt.Rows.Count;
                          for (int i = 0; i < totalrecords; i++) 
                        {
                               
                        %>
                        <tr  class="w3-hover-red" style="text-align:center">
                            <td style="text-align:center"><% Response.Write(i+1); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][1].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][2].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][3].ToString()); %></td>        
                            <td style="text-align:center"><a href="edit_course.aspx?id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/edit.png' height='25' /></a></td>
                            <td style="text-align:center"><a href="Delete.aspx?&tbl=course&id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/del.png' height='25' /></a></td>
                       </tr>
                        <% } %>
                        
                            </tbody>
                    </table>
              </div>
           </div>
        </div>
   </div>
</asp:Content>

