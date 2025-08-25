<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="users_question_view.aspx.cs" Inherits="admin_users_question_view" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<h1 align="center">Teacher Question</h1><br />
<div class="col-lg-12 w3-animate-zoom">
              <div class="w3-card-4">
              
                    <div class="w3-responsive"> 
                       <table class="w3-table-all w3-hoverable">
                           <thead>
                            <tr class="w3-light-blue">
                                <th style="text-align:center">No.</th>
                                <th style="text-align:center">Course Name</th>
                                <th style="text-align:center">Semesters</th>
                                <th style="text-align:center">Subject</th>
                                <th style="text-align:center">Chapter No.</th>
                                <th style="text-align:center">Question</th>
                                <th style="text-align:center">Mark</th>
                                <th style="text-align:center">Remark</th>
                                <th style="text-align:center">Link</th>
                                <th style="text-align:center">Delete</th>
                                </tr>
                           </thead>
                           <tbody>
                    <%  System.Data.SqlClient.SqlConnection con;
                        System.Data.SqlClient.SqlDataAdapter da;
                        System.Data.DataTable dt; 
                         con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                         da = new System.Data.SqlClient.SqlDataAdapter("SELECT q.id,c.sname,q.sid,s.sname,q.chno,q.que,q.mark,q.remark,q.link,q.uid FROM course c,sub s,question q,Users u where c.id=s.cid and s.id=q.subid and u.id=q.uid and q.uid="+Request.QueryString["id"], con);
                          dt = new System.Data.DataTable(); 
                         da.Fill(dt);

                       // int l = 0;  //int j =  1 * Convert.ToInt32(Session["mainno"]); //Response.Write(Convert.ToString(Session["mainno"])); %>
                             <%
                                int totalrecords = dt.Rows.Count;
                                int limit = 10;
                                int totalpages = (int)Math.Ceiling((double)totalrecords / (double)limit);
                                int page;
                                if (Request.QueryString["page"] == null) { page = 1; } else if (int.Parse(Request.QueryString["page"]) < 1) { page = 1; } else { page = int.Parse(Request.QueryString["page"]); }
                                int start = (page - 1) * limit;
                            %>
                              
                      <% 
                         
                          for (int i = start; i < totalrecords; i++)
                        {
                            if (i == start + limit) { break; } 
                        %>
                        <tr class="w3-hover-red">
                            <td style="text-align:center"><% Response.Write(i+1); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][1].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][2].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][3].ToString()); %></td>   
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][4].ToString()); %></td>   
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][5].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][6].ToString()); %></td>
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][7].ToString()); %></td>   
                            <td style="text-align:center"><% Response.Write(dt.Rows[i][8].ToString()); %></td>   
                            <td style="text-align:center"><a href="Delete.aspx?&tbl=user_question&id=<% Response.Write(dt.Rows[i][0].ToString()); %>&uid=<% Response.Write(dt.Rows[i][9].ToString());  %>"><img src='images/del.png' height='25' /></a></td> 
                        </tr>
                        <% } %>
                        
                            </tbody>
                    </table>
                    </div>          
                    </div>          
          </div>
            

</asp:Content>

