<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
</head>
<body>
    <h1>Template Design</h1>
   <div style="border : solid 2px #0ff; padding : 4px; width : 1000px; height : 600px; overflow:auto;">
    <%=LoadSVG() %>
      </div>
 <input id="Button1" type="button" value="Zoom Out"/>
 <input id="Button2" type="button" value="Zoom In"/>
    <div id="scale">100%</div>
</body>

<script src="scripts/jquery-1.7.2.js"></script>
<script src="scripts/WebForm1Script.js"></script>


    <%--Inline Version: --%>
<%--
<script type="text/javascript">

    // Privileged method
    function Container(param)
    {
        this.holder = param;
        var hidden = param;
        var that = this;

        function getHidden()
        {
            return hidden;
        }
        this.Hidden = function () { return getHidden(); };
    }

    function usingFunctional(spec)
    {
        var that = {};
        that.get_name = function () { return spec.name; };
        that.says = function () { return spec.saying || ""; };
        return that;
    }

    var base = {
        name: "Base object",
        get_name: function ()
        {
            return this.name;
        }
    }

    //function quo (status){} is same thing
    var quo = function (status)
    {
        return {
            get_status: function ()
            {
                return status;
            }
        };
    };

    //Add method to String type
    String.prototype.AddName = (function ()
    {
        return function (theName)
        {
            return this + " " + theName;
        }
    }());


    function zoom_out()
    {

        var v = document.getElementById("ATest");

        t = v.transform.baseVal.getItem(0);
        if (t.matrix.a > 1) {
            var f = t.matrix.a / 10;
            t.matrix.a -= f; t.matrix.d -= f;
            var s = Math.round(t.matrix.a * 100) + "%";
          //  var v2 = $('#theDoc');
          //  v2.css("width",s);
          //  v2.css("height", s);

            $('#scale')[0].innerHTML = s;

        }
    

    }

    function zoom_in()
    {

        var v = document.getElementById("ATest");

        t = v.transform.baseVal.getItem(0);

        var f = t.matrix.a / 10;

        t.matrix.a += f; t.matrix.d += f;
        var s = Math.round(t.matrix.a * 100) + "%";
       // var v2 = $('#theDoc');
       // v2.css("width", (t.matrix.a * 100) + "%");
       // v2.css("height", (t.matrix.a * 100) + "%");
        
        $('#scale')[0].innerHTML = s;


        ////////////////////////////////////////////////////////
        var dog = {};
        Object.defineProperty(dog, "name", { get: function () { return name; }, set: function (theName) { name = theName; } });

        dog.name = "Bonzo";

        var d = Object.create(dog);
        d.name = "Rover";
    //    document.getElementById("blah").textContent = d.name;
        ///////////////////////////////////////////////////////

        var cat = {
            name: "Tiggy",
            get ItsName() { return this.name; },
            set ItsName(newName) { this.name = newName; }
        };

      //  document.getElementById('blah2').textContent = cat.name;
        //////////////////////////////////////////////////////////
        var c = new Container("Something");
        var xx = c.Hidden();

        var ll = c.TheValue;

        var o = { a: 7, get b() { return this.a; } };

        var ee = o.b;

        var myFunctional = usingFunctional({ name: "Viv", saying: "Hi" });

        var myBase = Object.create(base);
        var w = myBase.get_name();

        var inherited = Object.create(base);
        inherited.name = "Inherited";
        inherited.get_name = function () { return "My name is: " + this.name; };

        var ww = inherited.get_name();

        //Test quo
        var aQuo = quo("OK");
        var xx = aQuo.get_status();

        //Test AddName
        var v = "Hello";
        xx = v.AddName("Viv");
        xx = "What are you doing".AddName("Dummy");
    };

    function button_clicked(t)
    {
        //alert('CLICKED ' + t);
        var v = ARect.width.baseVal.value;
        if (v == 100) {
            ARect.width.baseVal.value = 200;
        }
        else {
            ARect.width.baseVal.value = 100;
        }
    };

    function rect_clicked(evt)
    {
        var xx = evt.target;
        if (xx.getAttribute("width") == 100) {
            xx.setAttribute("width", 200);
        }
        else {
            xx.setAttribute("width", 100);
        }

        var l2 = theTextPath.getBoundingClientRect();
        var length = theTextPath.getComputedTextLength();
        var words = theTextPath.textContent.split(" ");

        var lengthList = getWordLengths(theTextPath.textContent);

        var tf = new LLNet.TextFormatter(theTextPath.textContent);

        var c = parseInt(yellow2.getAttribute("x")) + 20;

        yellow2.setAttribute("x", c);
    };

    function text_clicked(evt)
    {
        var v = evt.target;
        if (v.textContent == "This text") {
            v.textContent = "This is a much longer piece of text";
            v.setAttribute("fill", "green");
        }
        else {
            v.textContent = "This text";
            v.setAttribute("fill", "red");
        }
        var length = v.getComputedTextLength();

        var currentSize = 80;
        v.setAttribute("font-size", currentSize);
        while (v.getComputedTextLength() > 200) {
            v.setAttribute("font-size", currentSize);
            currentSize--;
        }
        blah.textContent = length;
    };

    function createTextElement()
    {
        var d = document.createElementNS("http://www.w3.org/2000/svg", "text");
        return d;
    };

    function getWordLengths(theString, theFontSize)
    {

        //Maybe we could temporarily use an existing element but.....
        var tmpText = createTextElement();
        tmpText.setAttribute("font-size", theFontSize);
        theDoc.appendChild(tmpText);

        var words = theString.split(' ');
        var twoArray = new Array(words.length);
        for (i = 0 ; i < words.length; i++) {
            twoArray[i] = new Array(2);
            twoArray[i][0] = words[i] + ' ';
            tmpText.textContent = words[i];
            twoArray[i][1] = tmpText.getComputedTextLength();
        }
        theDoc.removeChild(tmpText);
        return twoArray;
    };

    //Simulate namespace.....
    var LLNet = {}

    LLNet.TextFormatter = function (theString)
    {
        this.theString = theString;
    };

    //Not implemented but could use this rather than doing it in constructor....
    LLNet.TextFormatter.prototype.GetWordLengths = getWordLengths;
    LLNet.TextFormatter.prototype.LineList = new Array();

    $(document).ready(function ()
    {
        $("#Button1").click(zoom_out);
        $("#Button2").click(zoom_in);
    });

</script>
--%>
</html>

