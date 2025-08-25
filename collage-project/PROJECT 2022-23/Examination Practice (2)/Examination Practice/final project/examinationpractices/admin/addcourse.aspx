<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="addcourse.aspx.cs" Inherits="admin_addcourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <div class="row">
         <div class="col-md-4"></div>
            <div class="col-md-4"><br />
     	             <div class="w3-card-4">
                         <header class="w3-container w3-purple">
		                        <h2 align="center">Course</h2>
		                 </header>
                           <div class="w3-container" align="center"><br />
                            
                                    <label>Full Name : </label><br />
                                    <asp:TextBox  ID="fname" runat="server" data-toggle="tooltip" title="Exmple: Bachelor of Business Administration"  data-placement="right"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server" ErrorMessage="Please Enter name" ControlToValidate="fname" ForeColor="Red"></asp:RequiredFieldValidator><br />
                                    <label>Short Name :  </label><br />
                        
                                    <asp:TextBox ID="sname" runat="server" data-toggle="tooltip" title="Exmple:B.B.A"  data-placement="right"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter name" ControlToValidate="sname" ForeColor="Red"></asp:RequiredFieldValidator><br />

                                    <label>Total Sem :</label><br />
                        
                                    <asp:TextBox ID="totsem" runat="server" data-toggle="tooltip" title="Exmple: 6"  data-placement="right"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter sem" ControlToValidate="totsem" ForeColor="Red"></asp:RequiredFieldValidator><br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="totsem" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" MaximumValue="12" ForeColor="Red"></asp:RegularExpressionValidator>
                                    
                                    <div align="center">
                                            <asp:Button ID="addcour"  runat="server" Text="Add Course" class="btn btn-warning btn-warng1" onclick="addcour_Click" />
                                    </div>
                                    <br/>
                         </div>
                  </div>
              </div>
        <div class="col-md-4"></div>     
        <br/><br/><br/><br/><br/>
   </div>
    
</asp:Content>

