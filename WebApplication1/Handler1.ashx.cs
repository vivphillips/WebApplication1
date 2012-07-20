using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Net.WebSockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
  //  public class Handler1 : IHttpHandler
  //  {

        //public void ProcessRequest(HttpContext context)
        //{

        //    if (context.IsWebSocketRequest)
        //        context.AcceptWebSocketRequest(Handle);
        //}

        //public bool IsReusable
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        //async Task Handle(WebSocketContext wsContext)
        //{
        //    WebSocket socket = wsContext.WebSocket;
        //    while (true)
        //    {
        //        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);

        //        // Asynchronously wait for a message to arrive from a client 
        //        WebSocketReceiveResult result =
        //            await socket.ReceiveAsync(buffer, CancellationToken.None);

        //        // If the socket is still open, echo the message back to the client 
        //        if (socket.State == WebSocketState.Open)
        //        {
        //            string userMessage = Encoding.UTF8.GetString(buffer.Array, 0,
        //                result.Count);
        //            userMessage = "You sent: " + userMessage + " at " +
        //                DateTime.Now.ToLongTimeString();
        //            buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(userMessage));

        //            // Asynchronously send a message to the client 
        //            await socket.SendAsync(buffer, WebSocketMessageType.Text,
        //                true, CancellationToken.None);
        //        }
        //        else { break; }
        //    } 
        //}
  //  }

}