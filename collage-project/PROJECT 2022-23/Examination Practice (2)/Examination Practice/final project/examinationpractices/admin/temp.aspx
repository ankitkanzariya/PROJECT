<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="temp.aspx.cs" Inherits="admin_temp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div class="w3-container">
    <div class="row">
  <div class="col-md-2">
      
        </div>
  <div class="col-md-9">
		<div class="w3-card-4">
		   <header class="w3-container w3-blue"><h1 align="center">Your Question Paper Style Preview</h1></header>
				  <table> 
						<%  int gt = 0;
							int f = Convert.ToInt32(Session["mainno"]);
                            Session["time"].ToString();
							for (int i = 0; i < f; i++)
							{
								int qu = i + 1;
                                Response.Write("<tr>");
                                Response.Write("<td style='font-size:20px; color:red'>"); Response.Write("<br/><b>");
                                Response.Write("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Q." + qu);
								Response.Write("&nbsp;&nbsp;Attempt any:");
						
								string dataA = Convert.ToString(Session["attque"]);
								string[] splitDataA = dataA.Split(',');
                                Response.Write("&nbsp;<a style='text-dector:none;color:green'>" + splitDataA[i] + "</a>");

								for (int k = 0; k < 60; k++)
								{ 
									Response.Write("&nbsp;");
								}
								string data4 = Convert.ToString(Session["totmark"]);
								string[] splitData4 = data4.Split(',');
                                Response.Write(splitData4[i] + "<br />");
									gt =gt + Convert.ToInt32(splitData4[i]);
									Response.Write("</td>");
									Response.Write("</tr>");
									Response.Write("<tr>");
                                    Response.Write("<td style='font-size:20px;'></b>");

								string data1 = Convert.ToString(Session["disque"]);
								string[] splitData1 = data1.Split(',');

									 int dito = Convert.ToInt32(splitData1[i]);
									 Response.Write("&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ");
									for (int j = 0+1; j < dito+1; j++)
									{

                                        Response.Write("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
										Response.Write(j + "&nbsp; &nbsp;");
                                        Response.Write("what is array?");
								 
									}
                                    Response.Write("<br/><br/>");
                                    Response.Write("</td>");
									Response.Write("</tr>");
									  
						   }
                            Response.Write("<h3><a align='left'><b>Time: - &nbsp;" + Session["time"].ToString() + "</b>&nbsp;&nbsp;</a>");
                            Response.Write("<a align='right'><b>Total Marks: - &nbsp;" + gt + "</b>&nbsp;&nbsp;</a></h3>");
                            Session["gt"] = gt.ToString();
							%>
				 </table>
		</div>
	</div>
</div>
      <div class="row"><br/ >
			<div class="col-md-12" align="center">
                	<a  onclick="document.getElementById('id01').style.display='block'" class="w3-btn w3-hover-khaki">Set Paper style</a>
						  <div id="id01" class="w3-modal">
							<div class="w3-modal-content w3-animate-zoom w3-card-8">
							  <header class="w3-container w3-purple"> 
								<span onclick="document.getElementById('id01').style.display='none'" 
								class="w3-closebtn">&times;</span>
								<h2 align="center">Conform</h2>
							  </header>
							  <div class="w3-container" align="center">
                                  <br />
                                  <div class="w3-panel w3-khaki w3-card-24">
                                  <br />   <p style="font-size:40px;"><b>Do you want to Save Paper Style?</b></p><br />
                                  </div>
							  </div>
							  <footer class="w3-container ">
								<br /><br /><div align="center"><asp:Button ID="set" runat="server" class="w3-btn w3-black w3-round-xxlarge w3-text-shadow" Text="Ok" onclick="set_Click"/><br /><br /></div>
							  </footer>
							</div>
						  </div>
			</div>
	<!-- w3 container-->
    </div>
<!-- row -->
</div>
          
    
 <br />
</asp:Content>

