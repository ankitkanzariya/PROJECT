<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="showquestion.aspx.cs" Inherits="admin_showquestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    
    <div align="center" class="w3-panel w3-light-grey"><h1 class="w3-text-shadow">SHOW QUESTION</h1></div>  
  <div class="container">
    <div class="row">
    <div class="w3-container">
             <div class="col-md-4">
     	         <div class="w3-card-4">
                    <header class="w3-container w3-lime">
		                <h2 align="center">Choosing</h2>
		            </header>
                    <div class="w3-container" align="center"><br />
                                    <label>Course:</label><br />
                                         <asp:DropDownList ID="coursenameDropDownList" runat="server"  class="btn btn-default dropdown-toggle"
                                            OnSelectedIndexChanged="coursenameDropDownList_SelectedIndexChanged" 
                                            AutoPostBack="True" Width="200px"  Height="40px" ></asp:DropDownList>
                                            </asp:DropDownList>  <br />   

                                     <label>Semester:</label><br />
                                            <asp:DropDownList ID="DropDownListsem" runat="server" Width="180px"  Height="40px" class="btn btn-default dropdown-toggle"
                                             onselectedindexchanged="DropDownListsem_SelectedIndexChanged" 
                                            AutoPostBack="True" >
                                            <asp:ListItem Value="--- Select ---"></asp:ListItem></asp:DropDownList> </asp:DropDownList><br />
                                     <label>Subject:</label><br />
                                            <asp:DropDownList ID="DropDownListsub" runat="server"  class="btn btn-default dropdown-toggle" Height="40px" Width="150px">
                                            <asp:ListItem Value="--- Select ---"></asp:ListItem></asp:DropDownList>
                                            </asp:DropDownList><br />
                                    
                                    <hr>
                                        <asp:Button ID="show" runat="server" Text="show" class="btn btn-warning btn-warng1" onclick="show_Click" /><br/><br/>
                    
                  </div>
            </div>  
          </div>
         
        
   
   <% if (Session["course"] != null && Session["sem"] != null && Session["sub"] != null)
      {%>
        <div class="col-md-8">
         	         <div class="w3-card-4">
                    <div class="table">
	                    <table class="table">
                            <thead>
                                <th>Q. No.</th>
                                <th>Quesiton</th>
                                <th>Chapter</th>
                                <th>Marks</th>
                                <th>Edit</th>
                                <th>Delete</th>
                           </thead>
                           <tbody>
                    <% System.Data.SqlClient.SqlConnection con;
                       System.Data.SqlClient.SqlDataAdapter da;
                       System.Data.DataTable dt;

                       con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                       da = new System.Data.SqlClient.SqlDataAdapter("select id,que,chno,mark from question where cid=" + Session["course"] + " and sid=" + Session["sem"] + " and subid=" + Session["sub"], con);
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
                        <tr>
                            <td><% Response.Write(i + 1); %></td>
                            <td><% Response.Write(dt.Rows[i][1].ToString()); %></td>
                            <td><% Response.Write(dt.Rows[i][2].ToString()); %></td>
                            <td><% Response.Write(dt.Rows[i][3].ToString()); %></td>   
                            <td style="text-align:center"><a href="edit_question.aspx?id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/edit.png' height='25' /></a></td>
                            <td style="text-align:center"><a href="Delete.aspx?&tbl=question&id=<% Response.Write(dt.Rows[i][0].ToString()); %>"><img src='images/del.png' height='25' /></a></td> 
                        </tr>
                        <% } %>
                        
                            </tbody>
                    </table>
                </div>
                
            </div>
              

              <div align="center">
                    <%
                        if (totalrecords > 10)
                        {
                            Response.Write("<ul class='pagination pagination-sm'>");
                            Response.Write("<li ");
                            if (page == 1) { Response.Write("class='disabled'"); }
                            Response.Write("><a href='showquestion.aspx?page=1' ");
                            if (page == 1) { Response.Write("onclick='return false;'"); }
                            Response.Write("><<</a></li>");

                            Response.Write("<li ");
                            if (page == 1) { Response.Write("class='disabled'"); }
                            Response.Write("><a href='showquestion.aspx?page=" + (page - 1) + "' ");
                            if (page == 1) { Response.Write("onclick='return false;'"); }
                            Response.Write(">|<</a></li>");
                            for (int i = 1; i <= totalpages; i++)
                            {
                                //Response.Write "<a href='Category.aspx?page=".$i."'> Page ".$i."</a>&nbsp";
                                if (page == 1 || page == 2)
                                {
                                    Response.Write("<li"); if (page == 1) { Response.Write(" class='active'"); } Response.Write("><a href='showquestion.aspx?page=1'>1</a></li>");
                                    if (totalpages >= 2)
                                        Response.Write("<li"); if (page == 2) { Response.Write(" class='active'"); } Response.Write("><a href='showquestion.aspx?page=2'>2</a></li>");
                                    if (totalpages >= 3)
                                        Response.Write("<li><a href='showquestion.aspx?page=3'>3</a></li>");
                                    if (totalpages >= 4)
                                        Response.Write("<li><a href='showquestion.aspx?page=4'>4</a></li>");
                                    if (totalpages >= 5)
                                        Response.Write("<li><a href='showquestion.aspx?page=5'>5</a></li>");
                                    break;
                                }
                                else if ((i == page || i == page - 1 || i == page - 2 || i == page - 3 || i == page - 4) && (i == totalpages || i == totalpages - 1 || i == totalpages - 2 || i == totalpages - 3 || i == totalpages - 4))
                                {
                                    if (page == i)
                                        Response.Write("<li class='active'><a href='showquestion.aspx?page=" + i + "'>" + i + "</a></li>");
                                    else
                                        Response.Write("<li><a href='showquestion.aspx?page=" + i + "'>" + i + "</a></li>");
                                }
                                else if (i == page - 2 || i == page - 1 || i == page || i == page + 1 || i == page + 2)
                                {
                                    if (page == i)
                                        Response.Write("<li class='active'><a href='showquestion.aspx?page=" + i + "'>" + i + "</a></li>");
                                    else
                                        Response.Write("<li><a href='showquestion.aspx?page=" + i + "'>" + i + "</a></li>");
                                }
                            }
                            Response.Write("<li ");
                            if (page == totalpages) { Response.Write("class='disabled'"); }
                            Response.Write("><a href='showquestion.aspx?page=" + (page + 1) + "' ");
                            if (page == totalpages) { Response.Write("onclick='return false;'"); }
                            Response.Write(">>|</a></li>");

                            Response.Write("<li ");
                            if (page == totalpages) { Response.Write("class='disabled'"); }
                            Response.Write("><a href='showquestion.aspx?page=" + totalpages + "' ");
                            if (page == totalpages) { Response.Write("onclick='return false;'"); }
                            Response.Write(">>></a></li>");
                            Response.Write("</ul>");
        
                        }
                    %>
                    </div>

                    </div>



        
          <% } %>
      </div>
       
   </div>
 </div>
          <script>
          $(".btn btn-warning btn-warng1").click(function () {
              $('.table').show();
          });
          </script>
</asp:Content>

