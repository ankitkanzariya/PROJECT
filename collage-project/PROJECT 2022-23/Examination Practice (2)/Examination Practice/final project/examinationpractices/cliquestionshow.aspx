<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="cliquestionshow.aspx.cs" Inherits="cliquestionshow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<%--<div class="col-md-12 w3-animate-zoom">--%><br />
        <% Response.Write("<a style='margin-left:80px;text-decoration:none;' href='cliquestionshow.aspx?&course=" + Request.QueryString["course"] + "&sem=" + Request.QueryString["sem"] + "&sub=" + Request.QueryString["sub"] + "' class='w3-btn w3-hover-khaki'>Refresh</a>"); %>
        <input id="Button2" type="button" class="w3-btn w3-hover-khaki" onclick="PrintDiv()" value="Print" style="margin-left:800px" /><br />
<div id="printarea">   
         	        <% System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConn"].ToString());
                       System.Data.SqlClient.SqlDataAdapter da;
                       System.Data.DataTable dt;
                       
                       da = new System.Data.SqlClient.SqlDataAdapter("select * from master_p_style where cid=" + Request.QueryString["course"], con);
                       dt = new System.Data.DataTable();
                       da.Fill(dt);
                      
                       if (dt.Rows.Count > 0)
                       {
                           
                       da = new System.Data.SqlClient.SqlDataAdapter("select * from course where id=" + Request.QueryString["course"], con);
                       dt = new System.Data.DataTable();
                       da.Fill(dt);
                       Response.Write("<h1 align='center'><b>" + dt.Rows[0][2].ToString() + "</b></h1>");
                       Response.Write("<h1 align='center'><b>Semester - " + Request.QueryString["sem"] + "</b></h1>");


                       da = new System.Data.SqlClient.SqlDataAdapter("select * from sub where cid=" + Request.QueryString["course"]+"and sem="+Request.QueryString["sem"]+"and id="+ Request.QueryString["sub"], con);
                       dt = new System.Data.DataTable();
                       da.Fill(dt);
                       Response.Write("<h1 align='center'><b>" + dt.Rows[0][3].ToString() + "</b></h1>");
                    %>
                      
                    <%  da = new System.Data.SqlClient.SqlDataAdapter("select top 1 datetime,mainque,examtime from master_p_style where cid=" + Request.QueryString["course"] + "order by datetime DESC", con);
                        dt = new System.Data.DataTable();
                        da.Fill(dt);
                        Response.Write("<a id='timings' style='text-decoration:none;margin-left:50px;font-size:20px;'><b>Time:-" + dt.Rows[0]["examtime"].ToString() + "  </b></a>");
                        da = new System.Data.SqlClient.SqlDataAdapter("select que,disque,attque,totmark,eachque,gt from child_p_style where datetime='" + dt.Rows[0]["datetime"] + "' and cid=" + Request.QueryString["course"], con);
                        dt = new System.Data.DataTable();
                        da.Fill(dt);

                        Response.Write("<a style='text-decoration: none;margin-left:650px;font-size:20px;'><b>Total Marks:-  " + dt.Rows[0]["gt"].ToString() + "</b></a><br /><br />");
                    %>
                       <%--<div align="left">--%>
                      <%
                       int totalrecords = dt.Rows.Count;
                       for (int i = 0; i < totalrecords; i++)
                       {   
                           Response.Write("<br />");
                           Response.Write("<p style='font-size:20px;margin-left:110px;'><b> Q" + dt.Rows[i][0] + "&nbsp;&nbsp;");
                           Response.Write("&nbsp;&nbsp;&nbsp; Attempt any &nbsp;&nbsp;&nbsp;" + dt.Rows[i][2] + "");
                           Response.Write("<a style='margin-left:600px;text-decoration:none;'>  " + dt.Rows[i][3] + "</a></p></b>");
                           //SELECT top 3 que FROM question where mark=2 order by NEWID()
                           int disp = Convert.ToInt32(dt.Rows[i][1]);
                           System.Data.SqlClient.SqlDataAdapter da1 = new System.Data.SqlClient.SqlDataAdapter("select top " + dt.Rows[i][1] + " que from question where hide=0 and cid=" + Request.QueryString["course"] + "and sid=" + Request.QueryString["sem"] + "and subid=" + Request.QueryString["sub"] + " and mark=" + dt.Rows[i][4] + " order by NEWID()", con);
                           System.Data.DataTable dt1 = new System.Data.DataTable();
                           da1.Fill(dt1);
                           int totalrecords1 = dt1.Rows.Count;
                           if (totalrecords1 != 0)
                           {
                               for (int g = 0; g < disp; g++)
                               {
                                   if (totalrecords1 >= disp)
                                   {
                                       int s = g + 1;
                                       Response.Write("<p style='font-size:18px;margin-left:140px;'>" + s + " &nbsp;&nbsp;&nbsp;" + dt1.Rows[g]["que"] + "</p>");
                                   }
                                  else
                                  {%>
                                      <script type="text/javascript">
                                          $(document).ready(function () {
                                              document.getElementById('id02').style.display = 'block'
                                          });
                                </script>
                                      <%
                                  }
                               }
                           }
                           else
                           {%>
                               <script type="text/javascript">
                                   $(document).ready(function () {
                                       document.getElementById('id02').style.display = 'block'
                                   });
                                </script>
                            <%
                           }
                           %>
                        
                        <% } %>
                        
                    <% } 
                    else
                    {%>
                        <script type="text/javascript">
                            $(document).ready(function () {
                                document.getElementById('id02').style.display = 'block'
                            });
                </script> 
                       <%}%>
                        
                          
                        
        <br /><br /><br />        
 </div>
    <%--</div>--%>



      <div class="row"><br/ >
              <div class="col-md-3" align="center">
                		  <div id="id02" class="w3-modal">
							<div class="w3-modal-content w3-animate-zoom w3-card-4" style="width:900px">
							  
							  <div class="w3-container" align="center"><br />
								  
                                  <div class="w3-center w3-animate-left"><br>
                                        <img src="admin/images/sorry.png" alt="Avatar" style="width:30%" class="w3-circle w3-margin-top">
                                      </div>
								  
                                      <div class="w3-container w3-animate-right" align="center">
                                  <br />
                                  <div class="w3-panel w3-khaki w3-card-24">
                                  
                                  <br />   <p style="font-size:40px;"><b>Sorry Not Proper Question Contact to Administrator</b></p><br />
                                  </div><br /><br />

                                  <a class="w3-btn w3-hover-khaki w3-animate-bottom"" href="course.aspx">Back to Home</a><br /><br />
                                  <a class="w3-btn w3-hover-khaki w3-animate-bottom"" href="http://gmail.com">examinationpractices@gmail.com</a><br /><br />

							  </div>
                                    
                              </div>
							</div>
						  </div>
			</div>
	<!-- w3 container-->
    </div>
   


    <script type="text/javascript">

        function PrintDiv() {
            var divToPrint = document.getElementById('printarea');
            var popupWin = window.open('', '_blank', 'width=1950,height=1000,location=no,left=200px');
            popupWin.document.open();
            popupWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</html>');
            popupWin.document.close();
        }
         </script>
</asp:Content>

