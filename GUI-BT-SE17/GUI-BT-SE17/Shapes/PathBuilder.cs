using GUI_BT_SE17.ViewModels;
using Svg.Wrapper;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GUI_BT_SE17
{
    public static class PathBuilder
    {
        private static DrawingModel drawModel = DrawingModel.GetInstance();
        private static List<SvgCommand> path = new List<SvgCommand>();
        private static SvgCommandFactory factory = new SvgCommandFactory();

        private static void Clear(this List<SvgCommand> self)
        {
            self = null;
            self = new List<SvgCommand>();
        }

        private static void AddMoveCommand(this List<SvgCommand> self, Point mouseclick)
        {
            self.Add(factory.MCmd(mouseclick.X, mouseclick.Y));
        }

        private static String XamlPath(this List<SvgCommand> self)
        {
            string path = "F1";

            foreach (var cmd in self)
            {
                path += $" {cmd.ToString()}";
            }
            return path;
        }

        public static void StartPath(Point mouseclick)
        {
            path.Clear();
            path.AddMoveCommand(mouseclick);

            drawModel.Add(path.XamlPath());
        }

        private static void AddLineCommand(this List<SvgCommand> self, Point mouseclick)
        {
            self.Add(factory.LCmd(mouseclick.X, mouseclick.Y));
        }

        public static void SetPathPoint (Point mouseclick)
        {
            path.UpdatePath(mouseclick);
        }

        private static void UpdatePath(this List<SvgCommand> self, Point mouseclick)
        {
            self.AddLineCommand(mouseclick);
            drawModel.AddLineToPath(path.XamlPath());
        }

        private static SvgCommand Last (this List<SvgCommand> self)
        {
            var count = self.Count;
            if (count < 1)
                throw new IndexOutOfRangeException("PathBuilder: count was < 1 in Last(self)");
            return self[count - 1];
        }

        private static void RemoveLast(this List<SvgCommand> self)
        {
            var count = self.Count;
            if (count < 1)
                throw new IndexOutOfRangeException("PathBuilder: count was < 1 in RemoveLast(self)");
            self.RemoveAt(count - 1);
        }

        public static void UpdatePath(Point mouseclick)
        {
            if (path.Last().type != PointType.M)
            {
                path.RemoveLast();
            }

            path.UpdatePath(mouseclick);
        }
        private static void Close(this List<SvgCommand> self)
        {
            self.Add(factory.ZCmd());
        }

        public static void EndPath (bool close)
        {
            if (close)
            {
                path.RemoveLast();
                path.Close();
            }

            drawModel.EndPath(path.XamlPath(), path);


            path.Clear();
        }
    }
}
