<%@ Page Language="C#" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="true" CodeBehind="contest.aspx.cs" Inherits="Khaaaantest.umbraco.plugins.Khaaaantest.Contest" %>

<%@ Register Namespace="umbraco.uicontrols" Assembly="controls" TagPrefix="umb" %>

<asp:Content ID="Content" ContentPlaceHolderID="body" runat="server">
    <umb:umbracopanel runat="server" text="Contests">
        <umb:Pane runat="server" Text="Détails du concours">
            <umb:PropertyPanel runat="server" Text="Nombre de gagnants: ">
                <asp:Label ID="lblNumberOfWinners" runat="server" MaxLength="4"></asp:Label>
            </umb:PropertyPanel>
            <umb:PropertyPanel runat="server" Text="Nom du concours: ">
                <asp:Label ID="lblContestName" runat="server" MaxLength="50"></asp:Label>
            </umb:PropertyPanel>
            <umb:PropertyPanel runat="server" Text="Date de début: ">
                <asp:Label ID="lblStartDate" runat="server" MaxLength="50"></asp:Label>
            </umb:PropertyPanel>
            <umb:PropertyPanel runat="server" Text="Date de fin: ">
                <asp:Label ID="lblEndDate" runat="server" MaxLength="50"></asp:Label>
            </umb:PropertyPanel>
            <umb:PropertyPanel runat="server" Text="Nombre de participants: ">
                <asp:Label ID="lblContestantCount" runat="server" MaxLength="50"></asp:Label>
            </umb:PropertyPanel>
            <umb:PropertyPanel runat="server" Text="Téléchargez">
                <a href="ContestDownload.aspx?id=<%= ContestId %>" target="_blank">Téléchargez la liste des participants en format .csv</a>
            </umb:PropertyPanel>
        </umb:Pane>
        <umb:Pane runat="server" Text="Gagnants du concours">
            <asp:Button runat="server" Text="Choisir le gagnant" ID="btnWinner" OnClick="Winner_OnClick"/> <br/>
            <asp:Repeater ID="rptWinners" runat="server">
                <ItemTemplate>
                    <%# (int.Parse(DataBinder.Eval(Container, "ItemIndex", "")) +1).ToString() %>. <%# DataBinder.Eval(Container.DataItem,"FirstName") %> <%# DataBinder.Eval(Container.DataItem,"LastName") %> <%# DataBinder.Eval(Container.DataItem,"Email") %> <br/>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <%# (int.Parse(DataBinder.Eval(Container, "ItemIndex", "")) +1).ToString() %>. <%# DataBinder.Eval(Container.DataItem,"FirstName") %> <%# DataBinder.Eval(Container.DataItem,"LastName") %> <%# DataBinder.Eval(Container.DataItem,"Email") %> <br/>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </umb:Pane>
        <umb:Pane runat="server" Text="Contestants">
            
            <div style="overflow-x:auto;width:100%">
                <!-- Label for error here -->
                <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                <!-- Label for error here -->
                <asp:GridView ID="GridView1" OnRowCommand="GridView1_RowCommand" runat="server" AllowPaging="True"  AutoGenerateColumns="False" DataKeyNames="Id" ShowFooter="True" DataSourceID="ContestDS">
                    <PagerSettings Position="TopAndBottom"></PagerSettings>
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" ShowHeader="False" SortExpression="Id" Visible="False" />
                        <asp:TemplateField InsertVisible="true" HeaderText="Nom" SortExpression="LastName"><InsertItemTemplate><asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("LastName") %>'></asp:TextBox></InsertItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Width="60px" Text='<%# Bind("LastName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtLastName" Width="60px" runat="server"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Prénom" SortExpression="FirstName">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("FirstName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtFirstName" Width="60px" runat="server"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Courriel" SortExpression="Email">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtEmail" Width="60px" runat="server"/>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContestId" HeaderText="ContestId" SortExpression="ContestId" InsertVisible="False" Visible="False" />
                        <asp:TemplateField HeaderText="Gagnant" SortExpression="WinnerNumber">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox8" runat="server" Width="20px" Text='<%# Bind("WinnerNumber") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("WinnerNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn" InsertVisible="False" Visible="False" />
                        <asp:TemplateField ShowHeader="False">
                            <EditItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Rafraîchir"></asp:LinkButton>
                                <br/><asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Canceller"></asp:LinkButton>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Edit" Text="Éditer"></asp:LinkButton>
                                <br/><asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Delete" Text="Supprimer" OnClientClick="return confirm('Êtes-vous certain(e) de vouloir supprimer cette entrée?');"></asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton ID="btnInsert" runat="server" Text="Insérer" CommandName="Add" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <asp:SqlDataSource ID="ContestDS" runat="server"
                DeleteCommand="DELETE FROM [khaaaantest_Contestant] WHERE [Id] = @Id" 
                InsertCommand="INSERT INTO [khaaaantest_Contestant] ([FirstName], [LastName], [Email], [ContestId], [WinnerNumber]) VALUES (@FirstName, @LastName, @Email, @ContestId, -1)" 
                SelectCommand="SELECT * FROM [khaaaantest_Contestant] WHERE ContestId=@contestId ORDER BY [WinnerNumber] DESC, [LastName]" 
                UpdateCommand="UPDATE [khaaaantest_Contestant] SET [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email, [WinnerNumber] = @WinnerNumber WHERE [Id] = @Id"
                OnSelecting="GridView1_OnSelecting">
                <DeleteParameters>
                    <asp:Parameter Name="Id" Type="Int32" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:Parameter Name="ContestId" Type="String" />
                </SelectParameters>
                <InsertParameters>
                    <asp:Parameter Name="FirstName" Type="String" />
                    <asp:Parameter Name="LastName" Type="String" />
                    <asp:Parameter Name="Email" Type="String" />
                    <asp:Parameter Name="ContestId" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="FirstName" Type="String" />
                    <asp:Parameter Name="LastName" Type="String" />
                    <asp:Parameter Name="Email" Type="String" />
                    <asp:Parameter Name="Id" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>

        </umb:Pane>
    </umb:umbracopanel>

</asp:Content>