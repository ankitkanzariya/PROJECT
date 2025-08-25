<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="row"><br/ >
              <div class="col-md-3" align="center">
                		  <div id="id01" class="w3-modal">
							<div class="w3-modal-content w3-card-8 w3-animate-top">
                                  <header class="w3-container w3-teal"> 
                                    <a href="login.aspx"><span onclick="document.getElementById('id01').style.display='none'" 
                                    class="w3-closebtn">&times;</span></a>
                                    <h2>Sing up</h2>
                                  </header>
                                  <div class="w3-container w3-center">

            <table class="table" >
               <tbody>
                         <tr align="center">
                            <td>Enter Username</td>
                            <td><asp:TextBox ID="username" class="w3-input w3-border" name="user-name" runat="server"></asp:TextBox></td>
                            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="username" ErrorMessage="Enter User Name" ForeColor="Red"></asp:RequiredFieldValidator></td>
                         </tr>
                         <tr align="center">
                              <td>Enter Email</td>
                              <td><asp:TextBox ID="email" class="w3-input w3-border" name="email"  runat="server"></asp:TextBox></td><td>
                                  <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                      ErrorMessage="Enter Valid EMail address" ForeColor="Red" 
                                      ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                     ControlToValidate="email"></asp:RegularExpressionValidator></td>
                         </tr>
                         <tr align="center"><td>Enter Password</td>
                              <td><asp:TextBox ID="password" type="password" class="w3-input w3-border" name="password" 
                                      runat="server"></asp:TextBox></td>
                             <td></td>
                         </tr>
                         <tr align="center">
                            <td>Re-enter Password</td>
                            <td>
                                <asp:TextBox ID="Rpassword"  type="password" class="w3-input w3-border" 
                                    name="password" runat="server"></asp:TextBox></td>
                                <td><asp:CompareValidator ID="CompareValidator1" runat="server" 
                                     ErrorMessage="password dont' match" ControlToCompare="password" 
                                     ControlToValidate="Rpassword" ForeColor="Red" ></asp:CompareValidator>
                             </td>
                          </tr>
                         <tr align="center">
                             <td>Enter Mobile-No</td>
                             <td>
                                 <asp:TextBox ID="mobile" class="w3-input w3-border" name="mobile" runat="server" 
                                     MaxLength="10"></asp:TextBox></td>
                                 <td>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                         ErrorMessage="Enter valid Mobile-No" ControlToValidate="mobile" ForeColor="Red" 
                                         ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
                             </td>
                         </tr>
                        <tr align="center">
                            <td>Gender</td>
                            <td> 
                                <asp:RadioButtonList ID="gen" runat="server">
                                 <asp:listitem ID="male" runat="server" Text="Male" GroupName="Gender" />
                                <asp:listitem ID="female" runat="server" Text="Female" GroupName="Gender" />
                                </asp:RadioButtonList></td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="gen" ErrorMessage="Choose Gender" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                        </tr>
                          
                          <tr>
                            <td>Birthdate</td>
                            <td> 
                                <asp:Calendar ID="birthdate" runat="server"></asp:Calendar>
                            </td>
                          </tr>

                           <tr align="center">
                            <td>Account Type</td>
                            <td>
                             <asp:RadioButtonList ID="Account" runat="server">
                                 <asp:listitem ID="teacher" runat="server" Text="Teacher" GroupName="acc" />
                                <asp:listitem ID="student" runat="server" Text="Student" GroupName="acc" />
                                </asp:RadioButtonList></td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="Account" ErrorMessage="Select Your Account Type" 
                                        ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                          </tr>
                           
                          <tr align="center">
                            <td>Add ID</td>
                            <td> 
                                <asp:FileUpload ID="FileUpload1" runat="server" /></td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Add Your Id" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                          </tr>
                          <tr align="center">
                            <td colspan="3" align="center"> 
                                <asp:Button ID="signup" class="w3-btn-block w3-green w3-section w3-padding" runat="server" Text="Sign Up" 
                                     onclick="signup_Click" />
                                <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                             </td>
                          </tr>
               </tbody>
            </table>
                
               
                  </div>           
                
            </div><br /><br />
            </div>

            
            
            <style type="text/css">
                .table th, .table td { 
                    border-top: none !important;
                    border-left: none !important;
                }

            

            </style>
            <script type="text/javascript">
                $(document).ready(function () {
                    document.getElementById('id01').style.display = 'block'
                });
            
            </script>
  


</asp:Content>

