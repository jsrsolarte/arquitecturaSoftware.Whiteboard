using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArquSoft.Whiteboard.Back
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Length { get; set; }
        public double Angle { get; set; }
        public double AngleInRadians { get; set; }
    }

    public interface IWhiteboardHub
    {
        Task ReceiveInitPoint(string guid, Point point, string colour);
        Task ReceiveMiddlePoint(string guid, Point point);
        Task ReceiveEndPoint(string guid, Point point);

        Task ReceiveHistory(IEnumerable<Point> points);
    }

    public class WhiteboardHub : Hub<IWhiteboardHub>
    {
        private static List<Point> _points = new();

        public override Task OnConnectedAsync()
        {
            Console.WriteLine("board connected");
            Clients.Caller.ReceiveHistory(_points);
            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("board disconnected");
            return base.OnDisconnectedAsync(exception);
        }

        public Task SendInitPoint(string guid, Point point, string colour)
        {
            _points.Add(point);
            return Clients.Others.ReceiveInitPoint(guid, point, colour);
        }

        public Task SendMiddlePoint(string guid, Point point)
        {
            _points.Add(point);
            return Clients.Others.ReceiveMiddlePoint(guid, point);
        }

        public Task SendEndPoint(string guid, Point point)
        {
            _points.Add(point);
            return Clients.Others.ReceiveEndPoint(guid, point);
        }
    }
}