<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Teacher_edit_question.aspx.cs" Inherits="Teacher_Teacher_edit_question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<br/>
<div class="row">
     <div class="col-md-2"></div>
             <div class="col-md-9">
                <div class="w3-card-4">
                         <header class="w3-container w3-purple"><h2 align="center">Edit Question</h2></header>
                           <div class="w3-container" align="center"><br />
                            <label>course:</label><br />
                                <asp:DropDownList ID="coursenameDropDownList" class="btn btn-default dropdown-toggle" runat="server" OnSelectedIndexChanged="coursenameDropDownList_SelectedIndexChanged" AutoPostBack="True" Height="50px" Width="200px" ></asp:DropDownList><br/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please choose a course" ControlToValidate="coursenameDropDownList" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator><br />
                            <label>Semester :  </label><br />
                                <asp:DropDownList ID="DropDownListsem" class="btn btn-default dropdown-toggle" runat="server" Width="200px"  Height="40px" onselectedindexchanged="DropDownListsem_SelectedIndexChanged" AutoPostBack="True" ></asp:DropDownList><br/>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please choose a semester" ControlToValidate="DropDownListsem" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator><br/>
                            <label>Subjects :  </label><br />
                                <asp:DropDownList ID="DropDownListsub" class="btn btn-default dropdown-toggle" runat="server" Height="40px" Width="200px"></asp:DropDownList><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please choose a subject" ControlToValidate="DropDownListsub" InitialValue="0" ForeColor="Red"></asp:RequiredFieldValidator><br/>
                            <label>Chapter No. :  </label><br />
                               <asp:TextBox class="form-control1 control3" ID="chno" runat="server"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="chno"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="chno" ForeColor="Red" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" ></asp:RegularExpressionValidator><br />
                            <label>Question :  </label><br />
                                <textarea id="TextArea1" cols="80" rows="3" runat="server"></textarea><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="TextArea1"></asp:RequiredFieldValidator><br />
                            <label>Marks :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="mark" runat="server"></asp:TextBox><br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="mark"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="mark"  ForeColor="Red" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+" ></asp:RegularExpressionValidator><br />
                            <label>Remark :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="remark" runat="server"></asp:TextBox><br />
                              
                            <label>Link :  </label><br />
                                <asp:TextBox class="form-control1 control3" ID="link" runat="server"></asp:TextBox><br />
                                
                            <hr>
                               <asp:Button ID="update" runat="server" class="btn btn-warning btn-warng1" Text="Update Question" onclick="update_Click" />  
                        
                </div><br /> 
         </div> <br />         
    </div>   
</div>

</asp:Content>

