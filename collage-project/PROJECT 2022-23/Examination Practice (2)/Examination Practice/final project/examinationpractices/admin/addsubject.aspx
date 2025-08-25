<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeFile="addsubject.aspx.cs" Inherits="admin_addsubject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div class="col-md-4" >
    </div>

    <div class="col-md-4">
        	    <div class="w3-card-4">
                         <header class="w3-container w3-purple">
		                        <h2 align="center">Subject</h2>
		                 </header>
                           <div class="w3-container" align="center"><br />
                                <label>course:</label><br />
                                     <asp:DropDownList ID="coursenameDropDownList" runat="server" class="btn btn-default dropdown-toggle" OnSelectedIndexChanged="coursenameDropDownList_SelectedIndexChanged" 
                                     AutoPostBack="True" Height="40px" Width="200px">
                                     </asp:DropDownList><br />
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" ErrorMessage="Please choose a course" ControlToValidate="coursenameDropDownList" InitialValue="0"></asp:RequiredFieldValidator><br />
                                <label>Semester :  </label><br />
                                     <asp:DropDownList ID="DropDownListsem" class="btn btn-default dropdown-toggle" runat="server" Width="200px" Height="40px" >  <asp:ListItem Value="--- Select ---"></asp:ListItem> </asp:DropDownList><br/> 
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" ErrorMessage="Please choose a semester" ControlToValidate="DropDownListsem" InitialValue="0"></asp:RequiredFieldValidator><br/>
                                <label>Full Name :  </label><br />
                                     <asp:TextBox class="form-control1 control3" ID="fname" runat="server" data-toggle="tooltip" title="Example: bachelor of computer application"  data-placement="right"></asp:TextBox><br />
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="fname"></asp:RequiredFieldValidator><br />
                                <label>Short Name :  </label><br />
                                     <asp:TextBox class="form-control1 control3" ID="sname" runat="server" AutoCompleteType="Disabled" data-toggle="tooltip" title="B.C.A"  data-placement="right"></asp:TextBox><br />
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Please" ForeColor="Red" ControlToValidate="sname"></asp:RequiredFieldValidator><br /><br/>
                                <hr>
                                   <div align="center">
                                        <asp:Button ID="Button1" runat="server" class="btn btn-warning btn-warng1" Text="Add Subject" onclick="Button1_Click1" /><br /><br/>
                                  </div>
                        </div>
              </div>
      </div>
      <div class="col-md-4"><asp:Label ID="lop" runat="server" Text=""></asp:Label></div>
      <div class="col-md-12"><br/><br/></div>
      
</asp:Content>

