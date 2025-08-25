<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="paperstyle.aspx.cs" Inherits="admin_paperstyle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div align="center" class="w3-panel w3-light-grey"><h1 class="w3-text-shadow">SET PAPER STYLE</h1></div>  

<div class="container">
    <div class="row">
             <div class="col-md-4 ">
     	        <div class="w3-card-4">
                    <header class="w3-container w3-red">
		                <h2 align="center">Choosing</h2>
		            </header>
                    <div class="w3-container" align="center"><br />
                            <label>Course:</label><br />
                                    <asp:DropDownList ID="coursenameDropDownList" runat="server"  class="btn btn-default dropdown-toggle" AutoPostBack="True" Height="40px" Width="200px" ></asp:DropDownList>  <br />   
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please choose a course" ControlToValidate="coursenameDropDownList" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator><br />
                            <label> How many main Question :  </label><br/>
                                    <asp:TextBox class="form-control1 control3" ID="mainno" runat="server" data-toggle="tooltip" title="For Exmples: Q1 ,Q2 , Q3"  data-placement="right"></asp:TextBox><br/>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="mainno"  ForeColor="Red" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" ></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter" ForeColor="Red" ControlToValidate="mainno"></asp:RequiredFieldValidator><br />
                            <hr />
                                <asp:Button ID="Button2" runat="server" Text="SET" class="btn btn-warning btn-warng1" onclick="Button2_Click" />
                                <br/><br/>
                     </div>
                  </div>
              </div>
               
          <% if (Session["course"] != null)
             {%>
        <div class="paper">
            <div class="col-md-8">
                    <div class="table">
	                    <table class="table">
                            <thead >
                            <tr class="w3-khaki">
                                <th style="text-align:center">Q no</th>
                                <th style="text-align:center">Display Question</th>
                                <th style="text-align:center">Attempt Question</th>
                                <th style="text-align:center">Mark of Each Question</th>
                                <th style="text-align:center">Total Marks of Question</th>
                            </tr>
                           </thead>
                           <tbody>
                            <% int j =  1 * Convert.ToInt32(Session["mainno"]); %>
                            <%if (Session["mainno"] != null)
                              {
                                for (int i = 0; i < j; i++)
                                {%>
                                <tr style="text-align:center" class="w3-white">
                                    <td class="w3-light-grey"><% Response.Write(i+1); %></td>
                                    <td><asp:TextBox ID="disque" runat="server" class="disque" MaxLength="5" size="3" required></asp:TextBox></td>
                                    <td><asp:TextBox ID="attque" runat="server" class="attque" MaxLength="5" size="3" required></asp:TextBox></td>
                                    <td><asp:TextBox ID="eachque" runat="server" class="eachque" MaxLength="5" size="3" required></asp:TextBox></td>
                                    <td><asp:TextBox ID="totmark" runat="server" class="totmark" MaxLength="5" size="3" required></asp:TextBox></td>
                               </tr>
                            <% }
                             }  %>
                             <div class="validation"> 
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Enter All Display Question" ControlToValidate="disque" ForeColor="Red"></asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="disque" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" MaximumValue="12" ForeColor="Red"></asp:RegularExpressionValidator><br/>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Enter Attempt Question" ControlToValidate="attque" ForeColor="Red"></asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="attque" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" MaximumValue="12" ForeColor="Red"></asp:RegularExpressionValidator>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Enter Mark of each Question" ControlToValidate="eachque" ForeColor="Red"></asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="eachque" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" MaximumValue="12" ForeColor="Red"></asp:RegularExpressionValidator></div>
                           </tbody>
                            
                    </table>
                    <div align="center"><b>Timing Set :- <asp:TextBox ID="timing" runat="server" type="time"></asp:TextBox></b></div>
                    </div>
                   
                    <div align="center">
                        <asp:Button ID="set" runat="server" class="btn btn-warning btn-warng1 vali" Text="Preview" onclick="set_Click" /><br /><br />
                    </div>
                   </div>  
                   <% 
      } Session["course"] = null; %>
	            </div>
        </div>
</div>
        <br /><br />

<script type="text/javascript">
    $(".validation").hide();
   
    $(".btn btn-warning btn-warng1").click(function() {
        $('.paper').show();
        $(".validation").show();
    });
     
    $(".vali").click(function () {
        $(".validation").show();
    });

    



</script> 

</asp:Content>

