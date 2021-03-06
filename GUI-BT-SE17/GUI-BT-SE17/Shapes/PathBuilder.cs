﻿using GUI_BT_SE17.ViewModels;
using Svg.Path.Operations;
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

        private static int stage = 0;

        private static void Init(this List<SvgCommand> self)
        {
            stage = 0;
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
            stage++;

            path.Clear();
            path.AddMoveCommand(mouseclick);

            drawModel.Add(path.XamlPath());
        }

        private static void AddLineCommand(this List<SvgCommand> self, Point mouseclick)
        {
            if (stage > 0) 
                self.Add(factory.LCmd(mouseclick.X, mouseclick.Y));
        }

        public static void SetPathPoint (Point mouseclick)
        {
            // start insert
            if (stage > 0)
            {
                if (path.Last().type != PointType.M)
                {
                    path.RemoveLast();
                }
                var previous = path.Last();
                var current = factory.LCmd(mouseclick.X, mouseclick.Y);
                var template = new List<SvgCommand>
                {
                    factory.MCmd(0, 0),
                    factory.LCmd(30, -30),
                    factory.LCmd(60, 0),
                    factory.LCmd(30, 30),
                    factory.LCmd(0, 0),
                    factory.MCmd(15, -15),
                    factory.LCmd(45, 15),
                    factory.MCmd(15, 15),
                    factory.LCmd(45, -15),
                    factory.MCmd(60, 0),

                };
                //var template = new List<SvgCommand>
                //{
                //    factory.MCmd(0, 0),
                //    factory.LCmd(10, -10),
                //    factory.LCmd(0, -30),
                //    factory.LCmd(-10, -10),
                //    factory.LCmd(10, 10),
                //    factory.LCmd(0, 30),
                //    factory.LCmd(-10, 10),
                //    factory.LCmd(0, 0),
                //    factory.MCmd(10, -10),
                //    factory.LCmd(30, 0),
                //    factory.LCmd(10, 10),
                //    factory.MCmd(-10, 10),
                //    factory.LCmd(-30, 0),
                //    factory.LCmd(-10, -10),
                //    factory.MCmd(0, 0),
                //};
                //var template = new List<SvgCommand>
                //{
                //    factory.MCmd(0, 0),
                //    factory.LCmd(20, -20),
                //    factory.LCmd(-20, -20),
                //    factory.LCmd(20, 20),
                //    factory.LCmd(-20, 20),
                //    factory.LCmd(0, 0),
                //};
                //var template = new List<SvgCommand> { factory.MCmd(0, 0), factory.LCmd(10, 10), factory.LCmd(20,0) };
                //var template = new List<SvgCommand> { factory.MCmd(0, 0), factory.QCmd(100,0,50, -200) };
                var svgs = drawModel.Svgs;
                var selfIndex = drawModel.SelectedIndex;

                bool inserted = TemplateInserter.TryApplyTemplate(previous, current, selfIndex, template, svgs, out List<SvgCommand> result);

                if (inserted)
                {
                    path.InsertRange(path.Count, result);
                }
                else
                {
                    path.AddLineCommand(mouseclick);
                }
                drawModel.AddLineToPath(path.XamlPath());
            }
            // end insert
            path.UpdatePath(mouseclick);
        }

        private static void UpdatePath(this List<SvgCommand> self, Point mouseclick)
        {
            if (stage > 0)
            {
                self.AddLineCommand(mouseclick);
                drawModel.AddLineToPath(path.XamlPath());
            }
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
            if (stage > 0)
            {
                if (path.Last().type != PointType.M)
                {
                    path.RemoveLast();
                }

                path.UpdatePath(mouseclick);
            }
        }
        private static void Close(this List<SvgCommand> self)
        {
            self.Add(factory.LCmd(self[0].x, self[0].y)); 

            //self.Add(factory.ZCmd());
        }

        public static void EndPath (bool close)
        {
            if (stage > 0)
            {
                if (close)
                {
                    path.RemoveLast();
                    path.Close();
                }

                drawModel.EndPath(path.XamlPath(), path);
                path.Init();
            }
        }
    }
}
