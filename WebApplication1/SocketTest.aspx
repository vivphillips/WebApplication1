<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SocketTest.aspx.cs" Inherits="WebApplication1.SocketTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <textarea id="box" cols="20" rows="2"></textarea>

    </form>
</body>
<script src="scripts/jquery-1.7.2.js"></script>
<script type="text/javascript">

    var b = document.getElementById("box");

    $(document).ready(function() {  
        if (!("WebSocket" in window)) {
            b.innerHTML = "No Support";

        }else 
        {
            debugger;
            socket = new WebSocket("ws://localhost:49809/Handler1.ashx");
            socket.onmessage = function (msg)
            {
                b.innerHTML = msg;
            }
            socket.send("From Viv");
        }
              
        });  




</script>
</html>
