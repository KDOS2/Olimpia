<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormFactura.aspx.cs" Inherits="Aplicacion_Factura.FormFactura" %>

<!DOCTYPE html>
<link href="Css/Styles.css" rel="stylesheet" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div id="contenedor">
            <asp:HiddenField ID="hdId" runat="server" />
            <div class="contenedor2">
                <asp:Label runat="server" Text="N° Factura" ID="LblFactura"></asp:Label>
                <asp:TextBox ID="TxtNFactura" runat="server"></asp:TextBox>
            </div>
            <div class="contenedor2">
                <asp:Label runat="server" Text="Nit" ID="LblNit"></asp:Label>
                <asp:TextBox ID="TxtNit" runat="server"></asp:TextBox>
            </div>
            <div class="contenedor2">
                <asp:Label runat="server" Text="Descripción" ID="Label1"></asp:Label>
                <asp:TextBox ID="TxtDescripcion" runat="server"></asp:TextBox>
            </div>
            <div class="contenedor2">
                <asp:Label runat="server" Text="Valor total" ID="LblValor"></asp:Label>
                <asp:TextBox ID="TxtValor" runat="server"></asp:TextBox>
            </div>
            <div class="contenedor2">
                <asp:Label runat="server" Text="Iva" ID="LblIva"></asp:Label>
                <asp:TextBox ID="TxtIva" runat="server"></asp:TextBox>
            </div>

            <div class="contenedor2">
                <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="Editar" OnClick="btnActualizar_Click"/>
                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="Nuevo" OnClick="btnNuevo_Click"/>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>

                <div class="contenedor2">
                    <asp:Button ID="BtnEnviar" runat="server" Width="400px" Text="Enviar Facturas" CssClass="Nuevo" OnClick="BtnEnviar_Click"/>
                </div>

                <div class="contenedor2">
                    <asp:Label runat="server" Text="" ID="lbError" CssClass="ValidacionErronea"></asp:Label>
                </div>

                <div class="resultado">

                    <asp:GridView ID="GgView" DataKeyNames="id" runat="server" AutoGenerateColumns="False" OnRowDataBound="GgView_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="N° Factura" DataField="NFactura" />
                            <asp:BoundField HeaderText="Nit" DataField="Nit" />
                            <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                            <asp:BoundField HeaderText="Valor" DataField="valor" />
                            <asp:BoundField HeaderText="Iva" DataField="iva" />

                            <asp:TemplateField HeaderText="Seleccionar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkbSelect" runat="server" ClientIDMode="Static"/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Editar">
                                <ItemTemplate>
                                    <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="Editar" CommandName="Editar"/>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
<script src="Scripts/JsFile.js"></script>
</html>
