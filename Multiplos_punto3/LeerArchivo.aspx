<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeerArchivo.aspx.cs" Inherits="Multiplos_punto3.LeerArchivo" %>

<!DOCTYPE html>
<link href="Css/Styles.css" rel="stylesheet"/>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="LbUno" runat="server" Text="Seleccione el archivo a leer..." CssClass="label"></asp:Label>
        </div>
        <div id="cargue">            
            <asp:FileUpload ID="FileLoad" runat="server"  />
        </div>
        <div id="Procesar">
            <asp:Button ID="BtnProcesar" runat="server" Text="Procesar" OnClick="BtnProcesar_Click" />
        </div>
    </form>
</body>
</html>
