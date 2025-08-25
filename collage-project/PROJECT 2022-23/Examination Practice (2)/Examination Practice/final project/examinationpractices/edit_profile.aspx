<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="edit_profile.aspx.cs" Inherits="edit_profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<%string con = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;%>
      
       <%System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(" select *  from Users where id='" + (Session["uid"]) + "'", con);%>
        <%System.Data.DataTable dt = new System.Data.DataTable(); %>
        <% da.Fill(dt); %>
        
        <br />
        <div align="center">
         <div style="max-width:1000px;margin:16px;border:1px solid #ccc" align="center" class="w3-animate-zoom">

        <div class="w3-card-4">

            <header class="w3-container w3-blue">
              <h1>Edit Profile</h1>
            </header>

            <div class="w3-container">
             
                    
                    <table class="table">
                         <tbody>
                                 <tr>
                                    <td>Enter  New Username</td>
                                     <%//nname.Text = dt.Rows[0]["uname"].ToString(); %>
                                    <td><asp:TextBox ID="nname" class="form-control" runat="server"></asp:TextBox></td>
                                 </tr>
                                 <tr>
                                     <td>Enter New email</td>
                                     <%//email.Text = dt.Rows[0]["email"].ToString(); %>
                                     <td><asp:TextBox ID="email" class="form-control" runat="server"></asp:TextBox></td>
                                 </tr>
                                 <tr>
                                     <td>Enter New password</td>
                                     <%//password.Text = dt.Rows[0]["password"].ToString(); %>
                                     <td><asp:TextBox ID="password" class="form-control" runat="server"></asp:TextBox></td>
                                 </tr>
                                 <tr>
                                     <td>Enter New Mobile-No</td>
                                     <%//nmobile.Text = dt.Rows[0]["mobile"].ToString(); %>
                                     <td><asp:TextBox ID="nmobile" class="form-control" runat="server"></asp:TextBox></td>
                                 </tr>
                                   <tr>
                                    <td>Birthdate</td>
                                    <td > 
                                        <asp:Calendar ID="birthdate" runat="server"></asp:Calendar>
                                        <%Label1.Text = Convert.ToDateTime(dt.Rows[0]["birthdate"]).ToString("d/MMM/yyyy"); %>
                                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                                    </td>
                                  </tr>
                                  <tr>
                                      <td>Add New ID</td>
                                      <td><asp:FileUpload ID="nid" runat="server" />
                                      <% String temp = dt.Rows[0]["proof"].ToString();  %>
                                 <% Response.Write("<img  src='admin/uploads/" + temp + "' style='height:200px;width:200px;margin-right:0px;margin-left:250px;margin-top:-10px;' />");%>
                  
                                      </td>
                                  </tr>
                                  
                                        
                                  
                       </tbody>
                    </table>


            </div>

            <footer class="w3-container w3-blue">
             <h5> <asp:Button ID="save" class="w3-btn w3-hover-khaki" runat="server" Text="Save" 
                                            Width="100px" onclick="save_Click" /></h5>
            </footer>

       </div>

       </div>
</div>
<br />






</asp:Content>

