<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="question.aspx.cs" Inherits="admin_question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="center"><a href="addquestion.aspx" class="btn btn-warning btn-warng1" >Add Question</a></div><br />
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
                                <th style="text-align:center">Edit</th>
                                <th style="text-align:center">Delete</th>
                           </thead>
                           <tbody>
                    <%  System.Data.SqlClient.SqlConnection con;
                        System.Data.SqlClient.SqlDataAdapter da;
                        System.Data.DataTable dt; 
                         con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                         da = new System.Data.SqlClient.SqlDataAdapter("SELECT q.id,c.sname,q.sid,s.sname,q.chno,q.que,q.mark,q.remark,q.link,q.uid FROM course c JOIN sub s ON c.id = s.cid JOIN question q ON s.id = q.subid ORDER BY q.id", con);
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
                            <td style="text-align:center"><a href="edit_question.aspx?id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/edit.png' height='25' /></a></td>
                            <td style="text-align:center"><a href="Delete.aspx?&tbl=question&id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/del.png' height='25' /></a></td> 
                        </tr>
                        <% } %>
                        
                            </tbody>
                    </table>
                    </div>          
                    </div>          
          </div>
            
              <div align="center">
    <%
        if (totalrecords > 10)
        {
            Response.Write("<ul class='pagination pagination-sm'>");
            Response.Write("<li ");
            if (page == 1) { Response.Write("class='disabled'"); }
            Response.Write("><a href='question.aspx?page=1' ");
            if (page == 1) { Response.Write("onclick='return false;'"); }
            Response.Write("><<</a></li>");

            Response.Write("<li ");
            if (page == 1) { Response.Write("class='disabled'"); }
            Response.Write("><a href='question.aspx?page=" + (page - 1) + "' ");
            if (page == 1) { Response.Write("onclick='return false;'"); }
            Response.Write(">|<</a></li>");
            for (int i = 1; i <= totalpages; i++)
            {
                //Response.Write "<a href='Category.aspx?page=".$i."'> Page ".$i."</a>&nbsp";
                if (page == 1 || page == 2)
                {
                    Response.Write("<li"); if (page == 1) { Response.Write(" class='active'"); } Response.Write("><a href='question.aspx?page=1'>1</a></li>");
                    if (totalpages >= 2)
                        Response.Write("<li"); if (page == 2) { Response.Write(" class='active'"); } Response.Write("><a href='question.aspx?page=2'>2</a></li>");
                    if (totalpages >= 3)
                        Response.Write("<li><a href='question.aspx?page=3'>3</a></li>");
                    if (totalpages >= 4)
                        Response.Write("<li><a href='question.aspx?page=4'>4</a></li>");
                    if (totalpages >= 5)
                        Response.Write("<li><a href='question.aspx?page=5'>5</a></li>");
                    break;
                }
                else if ((i == page || i == page - 1 || i == page - 2 || i == page - 3 || i == page - 4) && (i == totalpages || i == totalpages - 1 || i == totalpages - 2 || i == totalpages - 3 || i == totalpages - 4))
                {
                    if (page == i)
                        Response.Write("<li class='active'><a href='question.aspx?page=" + i + "'>" + i + "</a></li>");
                    else
                        Response.Write("<li><a href='question.aspx?page=" + i + "'>" + i + "</a></li>");
                }
                else if (i == page - 2 || i == page - 1 || i == page || i == page + 1 || i == page + 2)
                {
                    if (page == i)
                        Response.Write("<li class='active'><a href='question.aspx?page=" + i + "'>" + i + "</a></li>");
                    else
                        Response.Write("<li><a href='question.aspx?page=" + i + "'>" + i + "</a></li>");
                }
            }
            Response.Write("<li ");
            if (page == totalpages) { Response.Write("class='disabled'"); }
            Response.Write("><a href='question.aspx?page=" + (page + 1) + "' ");
            if (page == totalpages) { Response.Write("onclick='return false;'"); }
            Response.Write(">>|</a></li>");

            Response.Write("<li ");
            if (page == totalpages) { Response.Write("class='disabled'"); }
            Response.Write("><a href='question.aspx?page=" + totalpages + "' ");
            if (page == totalpages) { Response.Write("onclick='return false;'"); }
            Response.Write(">>></a></li>");
            Response.Write("</ul>");
        
        }
    %>
    </div>
	

</asp:Content>

