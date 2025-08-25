<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="addquestion.aspx.cs" Inherits="admin_addquestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   

 <div class="row">
     <div class="col-md-2"></div>
             <div class="col-md-9">
                <div class="w3-card-4">
                         <header class="w3-container w3-yellow">
		                        <h2 align="center">Question</h2>
		                 </header>
                           <div class="w3-container" align="center"><br />
                           <table>
                             <label>Course:</label><br />
                                <asp:DropDownList ID="coursenameDropDownList" class="btn btn-default dropdown-toggle" runat="server" OnSelectedIndexChanged="coursenameDropDownList_SelectedIndexChanged" AutoPostBack="True" Width="200px"  Height="40px" ></asp:DropDownList><br/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please choose a course" ControlToValidate="coursenameDropDownList" InitialValue="0"></asp:RequiredFieldValidator><br />
                            <label>Semester :  </label><br />
                                <asp:DropDownList ID="DropDownListsem" class="btn btn-default dropdown-toggle" runat="server" Width="200px"  Height="40px" onselectedindexchanged="DropDownListsem_SelectedIndexChanged" AutoPostBack="True" > <asp:ListItem Value="--- Select ---"></asp:ListItem></asp:DropDownList><br/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please choose a semester" ControlToValidate="DropDownListsem" InitialValue="0"></asp:RequiredFieldValidator><br/>
                            <label>Subjects :  </label><br />
                                <asp:DropDownList ID="DropDownListsub" class="btn btn-default dropdown-toggle" runat="server" Height="40px" Width="200px"><asp:ListItem Value="--- Select ---"></asp:ListItem></asp:DropDownList><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please choose a subject" ControlToValidate="DropDownListsub" InitialValue="0"></asp:RequiredFieldValidator><br/>
                            <label>Chapter No. :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="chno" runat="server" data-toggle="tooltip" title="Exmple correct = 1 2 3 and incorrect=1,2,3"  data-placement="right"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="chno"></asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="chno" ForeColor="Red" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" ></asp:RegularExpressionValidator><br />
                            <label>Question :  </label><br />
                                <textarea id="TextArea1" cols="80" rows="3" runat="server" data-toggle="tooltip" title="Exmple: What is array?" data-placement="top"></textarea><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="TextArea1"></asp:RequiredFieldValidator><br />
                            <label>Marks :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="mark" runat="server"  data-toggle="tooltip" title="Exmple: 2 " data-placement="right"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="mark"></asp:RequiredFieldValidator><br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="mark"  ForeColor="Red" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" ></asp:RegularExpressionValidator><br />
                            <label>Remark :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="remark" runat="server"  data-toggle="tooltip" title="Example sau,2016 "  data-placement="right"></asp:TextBox><br />
                              
                            <label>Link :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="link" runat="server"   data-toggle="tooltip" title="www.example.com"  data-placement="right"></asp:TextBox><br />
                              
                            </table>    
                            <hr />
                               <asp:Button ID="addque" runat="server" class="btn btn-warning btn-warng1" Text="ADD Question" onclick="addque_Click" />  
                               
                </div><br /> 
         </div> <br />         
    </div>   
</div>
   
</asp:Content>

