<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeFile="users_profile.aspx.cs" Inherits="admin_users_profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
    <br /><br />
    <div align="center">
    
            
                <%string MyDbConn = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;%>
                <%System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter("select u.* from Users u where u.id="+ Request.QueryString["id"], MyDbConn);%>
                <%System.Data.DataTable dt = new System.Data.DataTable(); %>
                <% da.Fill(dt); %>


                <div class="w3-card-4" style="width:900px">
                
                    <header class="w3-container w3-blue">
                        <h1 align="center"> <%Response.Write(dt.Rows[0]["acc_type"].ToString());%>  Deatils</h1>
                    </header>

                    <div class="w3-container">
                    <br />
                        <div class="col-md-6">    
                            <% String temp = dt.Rows[0]["proof"].ToString();  %>
                            <% Response.Write("<img  src='uploads/" + temp + "' class='img-thumbnail' style='height:200px;width:300px;' />");%>
                        </div>        
                
                        <div class="col-md-6" style="font-size:20px;" align="left">    
                            <table >
                            <tr>
                                <td><%Response.Write(dt.Rows[0]["acc_type"].ToString());%> Name</td>
                                <td>:- <%Response.Write(dt.Rows[0]["uname"].ToString());  %></td>
                            </tr>
                            <tr>
                                 <td>Email Address</td><td>:-
                                 <%Response.Write(dt.Rows[0]["email"].ToString());  %></td>
                            </tr>
                           
                            <tr>
                                    <td> Mobile-No</td><td>:-
                                    <%Response.Write(dt.Rows[0]["mobile"].ToString());  %></td>
                             </tr>
                        
                            <tr>
                                 <td>Gender</td><td>:-
                                <%Response.Write(dt.Rows[0]["gender"].ToString());  %></td>
                            <tr>
                           <tr>
                             <td>Birthdate </td><td>:-
                            <%Response.Write(Convert.ToDateTime(dt.Rows[0]["birthdate"]).ToString("d/MMM/yyyy")); %></td>
                            </tr>
                        
                            q
                        </table>
                     </div>
                     
                 </div>
                 <br />
                    <footer class="w3-container w3-blue">
                    <%
                  if (dt.Rows[0]["acc_type"].ToString()== "Teacher")
                  {
                      Response.Write("<br /><a class='w3-btn w3-hover-red' href='Users_question_view.aspx?id=" + dt.Rows[0]["id"].ToString() + "'>Show Question</a> <br /><br />");
                  }
          
              %>
                    </footer>

                </div>



        </div>

</div>


         <%--    <div class="w3-container">
                <div class="w3-card-4">
                    <header class="w3-container w3-blue">
                        <h1 align="center"> <%Response.Write(dt.Rows[0]["acc_type"].ToString());%>  Deatils</h1>
                    </header>
                
               <div class="col-md-6">    
                            <% String temp = dt.Rows[0]["proof"].ToString();  %>
                            <% Response.Write("<img  src='uploads/" + temp + "' class='img-thumbnail' style='height:200px;width:300px;' />");%>
                </div>        
                
                <div class="col-md-6">    
                        
                             <%Response.Write(dt.Rows[0]["acc_type"].ToString());%>  
                             Name :-
                        
                            <%Response.Write(dt.Rows[0]["uname"].ToString());  %>
                        
                            Email Address :-
                            <%Response.Write(dt.Rows[0]["email"].ToString());  %>
                        
                            Mobile-No :-
                        
                            <%Response.Write(dt.Rows[0]["mobile"].ToString());  %>
                        
                            Gender :-
                        
                            <%Response.Write(dt.Rows[0]["gender"].ToString());  %>
                        
                            Birthdate :-
                        
                            <%Response.Write(Convert.ToDateTime(dt.Rows[0]["birthdate"]).ToString("d/MMM/yyyy")); %>
                        
                        
                            Collage Name :-
                        
                            <%Response.Write(dt.Rows[0]["c"].ToString());  %>
                        
                </div>--%>
                
           <%--</div>
            </div>
            <footer class="w3-container" align='center'>
              <%
                  if (dt.Rows[0]["acc_type"].ToString()== "Teacher")
                  {
                      Response.Write("<a  class='w3-btn w3-hover-red' href='Users_question_view.aspx?id=" + dt.Rows[0]["id"].ToString() + "'>Show Question</a> ");
                  }
          
              %>
            </footer>
    
    </div>
    --%>


    <%--
<div class="w3-container">
	<div class="col-lg-12 w3-animate-zoom">
          <div class="w3-card-4">
              <div class="w3-responsive"> 
                    <table class="w3-table-all w3-hoverable" >
                           <thead >
                            <tr class="w3-light-blue">
                                <th style="text-align:center">No.</th>
                                <th style="text-align:center">Teacher Name</th>
                                <th style="text-align:center">Collage Name</th>
                                <th style="text-align:center">Email</th>
                                <th style="text-align:center">Gender</th>
                                <th style="text-align:center">BirthDate</th>
                                <th style="text-align:center">Mobile No.</th>
                            </tr>
                           </thead>
                           <tbody >
                    <%  System.Data.SqlClient.SqlConnection con;
                        System.Data.SqlClient.SqlDataAdapter da;
                        System.Data.DataTable dt;
                        con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                        da = new System.Data.SqlClient.SqlDataAdapter("select uname,collname,email,gender,birthdate,mobile from users where id="+ Request.QueryString["id"], con);
                         dt = new System.Data.DataTable(); 
                         da.Fill(dt); %>
                             
                      <%
                          int totalrecords = dt.Rows.Count;
                          for (int i = 0; i < totalrecords; i++) 
                        {
                               
                        %>
                        <tr  class="w3-hover-red" style="text-align:center">
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["uname"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["collname"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["email"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["gender"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["birthdate"].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i]["mobile"].ToString()); %></td>
                       </tr>
                        <% } %>
                        
                            </tbody>
                    </table>
              </div>
           </div>
        </div>
    --%>
</asp:Content>
