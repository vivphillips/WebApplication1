<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxTest.aspx.cs" Inherits="WebApplication1.AjaxTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="scripts/jquery-1.7.2.js"></script>

    <form id="form1" runat="server">
    <div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        <Services>
            <asp:ServiceReference Path="TestService.svc" />
        </Services>
    </asp:ScriptManager>
    <p>
        <input id="Button1" type="button" value="button" onclick="run();" />
    </p>
        <iframe id="PDFContent"></iframe>
        <iframe id="PDF2"  width="1000" height="400" ></iframe>
    </form>

    <script type="text/javascript">
        "use strict";
        function run()
        {
            var service = new WebApplication1.TestService();
          //  service.SayHello("Viv", onSuccess, null, null);

         //   service.GetCar(received, null, null);
        //    service.SayGoodbye("Viv", onSuccess, null, null);
        //    var v = new WebApplication1.Car();
        //    v.Make = "Subaru";
       //     v.Model = "Legacy";
       //     service.UpgradeCar(v, onUpgraded, null, null);

             var result = service.GetDocument(10,docReceived,null,null);

             var xx = document.getElementById("PDF2");
             xx.setAttribute("src","GetPdf.aspx?TextExample.pdf");
        }


        function docReceived(result)
        {
            //alert("Doc received");
          //  PDFContent.Data = result;
            var obj = document.getElementById("PDFContent");
            obj.Data = result;
        }

        function onSuccess(result)
        {
            alert(result);
        }

        function received(result)
        {
            alert(result.Make + " " + result.Model);
        }

        function onUpgraded(result)
        {
            alert(result.Model);
        }

        $(document).ready(function ()
        {
        });

        function bang()
        {
            alert("IFrame loaded");
        }

    </script>
</body>
</html>
