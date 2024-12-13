using FamilyTree.DAL;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace FamilyTree.Presentation
{
    public class Paint
    {
        public FamilyTree.DAL.FamilyTree Tree { get; set; }

        private List<Human> _humans = new List<Human>();
        private double _mLeft = 5;
        private double _mTop = 5;

        public Paint()
        {

        }

        public Paint(DBHandler dBHandler, FamilyTree.DAL.FamilyTree tree)
        {
            Tree = tree;
            _humans = dBHandler.GetHumans().Where(x => x.FamilyTreeId == tree.Id).ToList();
        }

        public void PaintTreeBtn()
        {
            if (_humans.Count > 0)
            {
                Window window = new Window()
                {
                    Title = "awd",
                    Width = 700,
                    Height = 500,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    //Content = grid,
                };

                Grid grid = new Grid()
                {
                    Name = "grid"
                };

                paintBtn2(window, grid);
                paintTreeLink(grid);
                window.Content = grid;
                window.Show();
            }
            else
            {
                MessageBox.Show("В данном дереве нет людей", "Ошибка");
            }
        }

        public void PaintTxt()
        {
            var sortedHuman = _humans.OrderByDescending(x => x.BirthDay).ToList();
            var sb = new StringBuilder();
            sb.AppendLine($"Древо '{Tree.Title}'");
            var appendedHuman = new List<Human>();
            foreach (var human in sortedHuman)
            {
                if (!appendedHuman.Any(x => x.FIO == human.FIO))
                {
                    if (human.Childrens.Any())
                    {
                        appendedHuman.Add(human);
                        sb.AppendLine($"{human.ToString()} (сторона {human.Childrens.First().FIO})");
                    }
                    else
                    {
                        appendedHuman.Add(human);
                        sb.AppendLine(human.ToString());
                    }

                    if (human.Parents.Any())
                    {
                        sb.AppendLine("Родители:");
                        foreach (var parent in human.Parents)
                        {
                            sb.AppendLine(parent.ToString());
                            appendedHuman.Add(parent);
                        }
                        sb.AppendLine();
                    }
                }
            }

            File.WriteAllText($"Древо {Tree.Title}.txt", sb.ToString());
        }

        private void paintBtn(Window window, Grid grid)
        {
            var countBtnOnRow = 0;
            var countRow = 1;


            var sortHuman = _humans.OrderBy(x => x.BirthDay).ToList();
            var iterChild = 0;
            var tempParent = "";
            foreach (var human in sortHuman)
            {
                if (human.Parents.Any())
                {
                    if (tempParent != human.Parents.First().FIO)
                    {
                        countRow++;
                        tempParent = human.Parents.First().FIO;
                        iterChild = 0;
                    }

                    iterChild++;

                    countBtnOnRow = human.Parents.First().Childrens.Count + 1;
                }
                else
                {
                    countBtnOnRow++;
                }
                if (countBtnOnRow != 1)
                {
                    if (iterChild == 0)
                    {
                        _mLeft = window.Width - 155;
                    }
                    else
                    {
                        _mLeft = (window.Width / countBtnOnRow * iterChild) - 75;
                    }
                }
                if (countRow != 1)
                {
                    _mTop = 5 + (70 * (countRow - 1));
                    //_mLeft = (window.Width / 2) / human.Parents.Max(x => x.Childrens.Count) + (150*countBtnOnRow)-100;
                    //countBtnOnRow++;
                }


                var btn = new Button()
                {
                    Content = $"{human.FIO}\r\n{human.BirthDay.ToShortDateString()}",
                    Tag = human.FIO,
                    Width = 150,
                    Height = 35,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(_mLeft, _mTop, 0, 0),
                };

                grid.Children.Add(btn);
            }
        }

        private void paintBtn2(Window window, Grid grid)
        {
            var countBtnOnRow = 0;
            var countRow = 1;


            var sortHuman = _humans.OrderByDescending(x => x.BirthDay).ToList();

            _mLeft = window.Width / 2 - 75;
            _mTop = window.Height - 100;

            var paintedHuman = new List<Human>();
            var iter = 1;
            foreach (var human in sortHuman)
            {
                if (!paintedHuman.Any(x => x.FIO == human.FIO))
                {

                    if (human.Childrens.Any(x => x.FIO == paintedHuman.FirstOrDefault(c => c.FIO == x.FIO).FIO))
                    {
                        var my = getCoord(grid, human.Childrens.First(x => x.FIO == paintedHuman.First(c => c.FIO == x.FIO).FIO).FIO);
                        _mTop = my.Y - (100 * (countRow - 1));
                        _mLeft = iter % 2 == 0 ? my.X + 85 : my.X - 85;
                        iter++;
                    }

                    var btn = new Button()
                    {
                        Content = $"{human.FIO}\r\n{human.BirthDay.ToShortDateString()}",
                        Tag = human.FIO,
                        Width = 150,
                        Height = 35,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(_mLeft, _mTop, 0, 0),
                    };

                    grid.Children.Add(btn);
                    paintedHuman.Add(human);
                    if (human.Parents.Any())
                    {
                        var parents = human.Parents;
                        paintParents(window, grid, parents, countRow, human);
                        paintedHuman.AddRange(parents);
                        countRow++;
                        iter = 1;
                    }
                }
            }
        }

        private void paintParents(Window window, Grid grid, List<Human> parents, int countRow, Human buttonThis)
        {
            var my = getCoord(grid, buttonThis.FIO);
            _mTop = my.Y - (100 * countRow);
            var iter = 1;
            foreach (var human in parents)
            {
                _mLeft = iter % 2 == 0 ? my.X + 150 : my.X - 150;
                var btn = new Button()
                {
                    Content = $"{human.FIO}\r\n{human.BirthDay.ToShortDateString()}",
                    Tag = human.FIO,
                    Width = 150,
                    Height = 35,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(_mLeft, _mTop, 0, 0),
                };
                grid.Children.Add(btn);
                iter++;
            }
        }

        private System.Windows.Point getCoord(Grid grid, string fio)
        {
            foreach (var child in grid.Children)
            {
                if (child is Button)
                {
                    if ((child as Button).Tag.ToString() == fio)
                    {
                        System.Windows.Point p = new System.Windows.Point()
                        {
                            X = (child as Button).Margin.Left,
                            Y = (child as Button).Margin.Top,
                        };
                        return p;
                    }
                }
            }
            return new System.Windows.Point();
        }

        private void paintTreeLink(Grid grid)
        {
            var sortedHuman = _humans.OrderBy(x => x.BirthDay).ToList();
            System.Windows.Point my;
            List<System.Windows.Point> parents = new List<System.Windows.Point>();

            foreach (var human in sortedHuman)
            {
                if (human.Parents.Any())
                {
                    foreach (var child in grid.Children)
                    {
                        if (child is Button)
                        {
                            my = getCoord(grid, human.FIO);
                            if (human.Parents.Any(x => x.FIO == (child as Button).Tag.ToString()))
                            {
                                parents.Add(getCoord(grid, (child as Button).Tag.ToString()));
                            }
                        }
                    }
                    foreach (var parent in parents)
                    {
                        Line l = new Line()
                        {
                            X1 = my.X + 75,
                            X2 = parent.X + 75,
                            Y1 = my.Y,
                            Y2 = parent.Y + 35,
                            Stroke = System.Windows.Media.Brushes.Black,
                            StrokeThickness = 1,

                        };
                        grid.Children.Add(l);
                    }
                    parents.Clear();
                }
            }



        }
    }

}
