<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebApplication1.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <svg xmlns="http://www.w3.org/2000/svg"
            xmlns:xlink="http://www.w3.org/1999/xlink"
            version="1.2" id="theDoc" width="500" height="500" viewBox="0 0 250 250">
            <g>
                <defs>
                    <clipPath id="rectpath">
                        <path d="M 20 20 h 150 v 150 h -150 z" ></path>
                    </clipPath>
                </defs>
                <use xlink:href="#rectpath" id="clonepath"></use>
             <image x="0" y="0" width="100%" height="100%" xlink:href="SVGS/Penguins.jpg" clip-path="url(#rectpath)"></image>
            </g>
            <g transform="translate(200 200)">
                    <clipPath id="ClipPath1" transform="translate(50 0)  rotate(25)">
                        <path d="M 40 40 h 150 v 150 h -150 z"></path>
                    </clipPath>
             <image  width="100%" height="100%" xlink:href="SVGS/Penguins.jpg"  clip-path="url(#ClipPath1)"></image>
            </g>
        </svg>
    </div>
    </form>
</body>
    <script type="text/javascript" src="jquery.js"></script> 
</html>
