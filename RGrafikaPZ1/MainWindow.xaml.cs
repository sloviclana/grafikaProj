using RGrafikaPZ1;
using RGrafikaPZ1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using WpfApp1.Model;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Drawing.Pen;
//using Point = WpfApp1.Model.Point;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Drawing.Size;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Point> podeoci = new List<Point>();
        public List<PowerEntity> XMLelementi = new List<PowerEntity>();
        public List<LineEntity> vodovi = new List<LineEntity>();
        public List<LineEntity> vodDuplikat = new List<LineEntity>();
        public List<Polyline> nacrtaniVodovi = new List<Polyline>();
        public List<LineEntity> undergroundVodovi = new List<LineEntity>();
        public List<LineEntity> iznadZemljeVodovi = new List<LineEntity>();
        public List<Polyline> undergroundNacrtani = new List<Polyline>();
        public List<Polyline> iznadZemljeNacrtani = new List<Polyline>();

        public static List<Shape> nacrtaniElementi = new List<Shape>();
        public static List<TextBlock> dodatTekst = new List<TextBlock>();


        //pokupi poziciju klika
        public double klikX, klikY;
        //za poligon
        public List<double> koordinatePoX = new List<double>();
        public List<double> koordinatePoY = new List<double>();
        //undo redo clear
        List<UIElement> obrisaniListaZaBrojanje = new List<UIElement>();
        List<UIElement> ponovoIscrtaj = new List<UIElement>();
        public int numberChildren = 0;
        //za clear
        List<UIElement> mapa = new List<UIElement>();

        //tacke
        public Dictionary<Point, PowerEntity> dictTackaElement = new Dictionary<Point, PowerEntity>();
        public int checkMinMax = 1;
        public double noviX, noviY, praviX, praviY, praviXmin, praviXmax, praviYmin, praviYmax;
        public double razlikaMinMaxX, razlikaMinMaxY;
        //za presecanje vodova
        public List<Polyline> listaPresekaVodova = new List<Polyline>();
        public List<Rectangle> listaPresecnihTacaka = new List<Rectangle>();

        public MainWindow()
        {
            InitializeComponent();

            ZamisljeniPodeoci();
            UcitajElemente();
            RazvrstajVodove();
        }

        private static void ZamisljeniPodeoci()
        {
            //Pravim matricu
            Point rt;

            for (int i = 0; i < 700; i++) 
            {
                for (int j = 0; j < 900; j++)
                {
                    rt = new Point(i, j);
                    podeoci.Add(rt);
                }
            }
        }

        private void UcitajElemente()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            XmlNodeList nodeList;

            //substations
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                sub.ToolTip = "Substation\nID: " + sub.Id + "  Name: " + sub.Name;
                XMLelementi.Add(sub);

                ToLatLon(sub.X, sub.Y, 34, out noviX, out noviY);
                Provera(noviX, noviY);
            }

            //switches
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                SwitchEntity switch1 = new SwitchEntity();
                switch1.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                switch1.Name = node.SelectSingleNode("Name").InnerText;
                switch1.X = double.Parse(node.SelectSingleNode("X").InnerText);
                switch1.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                switch1.Status = node.SelectSingleNode("Status").InnerText;
                switch1.ToolTip = "Switch\nID: " + switch1.Id + "  Name: " + switch1.Name + " Status: " + switch1.Status;
                XMLelementi.Add(switch1);

                ToLatLon(switch1.X, switch1.Y, 34, out noviX, out noviY);
                Provera(noviX, noviY);
            }

            //nodes
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                NodeEntity node1 = new NodeEntity();
                node1.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                node1.Name = node.SelectSingleNode("Name").InnerText;
                node1.X = double.Parse(node.SelectSingleNode("X").InnerText);
                node1.Y = double.Parse(node.SelectSingleNode("Y").InnerText);
                node1.ToolTip = "Node\nID: " + node1.Id + "  Name: " + node1.Name;
                XMLelementi.Add(node1);

                ToLatLon(node1.X, node1.Y, 34, out noviX, out noviY);
                Provera(noviX, noviY);
            }

            //lines (vodovi) ->  first end - second end
            nodeList = xmlDoc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                LineEntity l = new LineEntity();
                l.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                l.Name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("true"))
                {
                    l.IsUnderground = true;
                }
                else
                {
                    l.IsUnderground = false;
                }
                l.R = float.Parse(node.SelectSingleNode("R").InnerText);
                l.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                l.LineType = node.SelectSingleNode("LineType").InnerText;
                l.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);
                l.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                l.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);

                // if there are are first end and seckond end in elements show line, else ignore line
                if (XMLelementi.Any(x => x.Id == l.FirstEnd))
                {
                    if (XMLelementi.Any(x => x.Id == l.SecondEnd))
                    {
                        vodovi.Add(l);
                    }
                }

                //delete duplicates of lines
                while (vodovi.Any(x => x.Id != l.Id && x.FirstEnd == l.FirstEnd && x.SecondEnd == l.SecondEnd))
                {
                    vodDuplikat = vodovi.FindAll(x => x.Id != l.Id && x.FirstEnd == l.FirstEnd && x.SecondEnd == l.SecondEnd);
                    foreach (LineEntity dupli in vodDuplikat)
                    {
                        vodovi.Remove(dupli);
                    }
                    vodDuplikat.Clear();
                }
            }
        }

        private void RazvrstajVodove()
        {

            foreach (LineEntity vod in vodovi)
            {
                if (vod.IsUnderground == true)
                {
                    undergroundVodovi.Add(vod);
                }
                else
                {
                    iznadZemljeVodovi.Add(vod);
                }
            }

            //RazvrstajNacrtane();

        }

        private void RazvrstajNacrtane()
        {
            foreach(LineEntity ln in undergroundVodovi)
            {
                PowerEntity l1start = XMLelementi.Find(x => x.Id == ln.FirstEnd);
                PowerEntity l1end = XMLelementi.Find(x => x.Id == ln.SecondEnd);
                Point startPoint1 = new Point(l1start.X, l1start.Y);
                Point endPoint1 = new Point(l1end.X, l1end.Y);
                Polyline nacrtano = nacrtaniVodovi.Find(p => p.Points.Contains(startPoint1) && p.Points.Contains(endPoint1));

                if (nacrtano != null)
                    undergroundNacrtani.Add(nacrtano);
            }

            foreach (LineEntity ln in iznadZemljeVodovi)
            {
                PowerEntity l1start = XMLelementi.Find(x => x.Id == ln.FirstEnd);
                PowerEntity l1end = XMLelementi.Find(x => x.Id == ln.SecondEnd);
                Point startPoint1 = new Point(l1start.X, l1start.Y);
                Point endPoint1 = new Point(l1end.X, l1end.Y);
                Polyline nacrtano = nacrtaniVodovi.Find(p => p.Points.Contains(startPoint1) && p.Points.Contains(endPoint1));

                if (nacrtano != null)
                    iznadZemljeNacrtani.Add(nacrtano);
            }
        }

        private void LoadGraph_Click(object sender, RoutedEventArgs e)
        {
            //Crtanje elemenata koji treba da se nalaze u mrezi
            foreach (var element in XMLelementi)
            {
                ToLatLon(element.X, element.Y, 34, out noviX, out noviY);
                KoordinateNaCanvasu(noviX, noviY, out praviX, out praviY);

                Rectangle rect = new Rectangle();
                rect.Fill = element.Boja;
                rect.ToolTip = element.ToolTip;
                rect.Width = 2;
                rect.Height = 2;

                Point mojaTacka = podeoci.Find(pomocnaTacka => pomocnaTacka.X == praviX && pomocnaTacka.Y == praviY);

                int brojac = 1;
                if (!dictTackaElement.ContainsKey(mojaTacka))
                {
                    dictTackaElement.Add(mojaTacka, element);
                }
                else
                {
                    bool flag = false;
                    while (true)
                    {
                        for (double iksevi = praviX - brojac * 3; iksevi <= praviX + brojac * 3; iksevi += 3) //opet na oba 3 da se ne bi preklapali
                        {
                            if (iksevi < 0)
                                continue;
                            for (double ipsiloni = praviY - brojac * 3; ipsiloni <= praviY + brojac * 3; ipsiloni += 3)
                            {
                                if (ipsiloni < 0)
                                    continue;
                                mojaTacka = podeoci.Find(t => t.X == iksevi && t.Y == ipsiloni);
                                if (!dictTackaElement.ContainsKey(mojaTacka))
                                {
                                    dictTackaElement.Add(mojaTacka, element);
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                                break;
                        }
                        if (flag)
                            break;

                        brojac++;
                    }
                }

                
                //ovde sam nesto smuljao jer se slika rotira cudno, ali je proslo
                Canvas.SetBottom(rect, mojaTacka.X);
                Canvas.SetLeft(rect, mojaTacka.Y);
                canvas.Children.Add(rect);
            }

            //crtanje vodova
            foreach (LineEntity line in vodovi)
            {
                Point start, end;
                krajeviVoda(line, out start, out end);

                if (start.X != end.X)
                {
                    
                    Polyline polyline = new Polyline();
                    polyline.Stroke = Brushes.Black;
                    polyline.StrokeThickness = 0.5;

                    //pod pravim uglom
                    Point p1 = new Point(1 + start.Y, 600 - 1 - start.X);
                    Point p2 = new Point(1 + end.Y, 600 - 1 - start.X);
                    Point p3 = new Point(1 + end.Y, 600 - 1 - end.X);
                    polyline.Points.Add(p1);
                    polyline.Points.Add(p2);
                    polyline.Points.Add(p3);
                    
                    polyline.ToolTip = "Line\nID: " + line.Id + " Name: " + line.Name;

                    polyline.MouseRightButtonDown += promenaBojeAnim_MouseRightButtonDown;

                    //ako ima presek nacrtaj
                    //NadjiPreseke();
                    //nacrtaniVodovi.Add(polyline);
                    canvas.Children.Add(polyline);
                    if(line.IsUnderground == true)
                    {
                        undergroundNacrtani.Add(polyline);
                    } else
                    {
                        iznadZemljeNacrtani.Add(polyline);
                    } 
                    /*
                    Line newLine = new Line();
                    newLine.X1 = start.X;
                    newLine.X2 = end.X;
                    newLine.Y1 = start.Y;
                    newLine.Y2 = end.Y;
                    double centerX = (newLine.X1 + newLine.X2) / 2;
                    double centerY = (newLine.Y1 + newLine.Y2) / 2;
                    newLine.Stroke = Brushes.Black;
                    newLine.StrokeThickness = 1;
                    RotateTransform rotateTransform = new RotateTransform(90, end.Y, start.X);
                    newLine.RenderTransform = rotateTransform;
                    canvas.Children.Add(newLine);
                    */
                }
            }

            //obrisi duplikate tj. vodove koji se preklapaju sa nekim drugim vodom
            foreach (Polyline pl in listaPresekaVodova)
            {
                if (!canvas.Children.Contains(pl))
                {
                    canvas.Children.Remove(pl);
                }
            }

            // ovo mi treba za undo i redo da ne bih clearovao pocetnu mapu
            //RazvrstajNacrtane();
            NadjiPreseke();
            NacrtajPreseke();
            numberChildren = canvas.Children.Count;
        }

        public List<Point?> preseciUn = new List<Point?>();
        public List<Point?> preseciIznZem = new List<Point?>();

        public void NacrtajPreseke()
        {
            foreach(Point p1 in preseciUn)
            {
                Circle circle = new Circle
                {
                    Width = 1,
                    Height = 1,
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.3,
                    Fill = Brushes.Red
                };

                // Set the position of the Circle on the Canvas
                Canvas.SetLeft(circle, p1.X-0.3);
                Canvas.SetTop(circle, p1.Y-0.3);

                // Add the Circle to the Canvas
                canvas.Children.Add(circle);
            }

            foreach (Point p1 in preseciIznZem)
            {
                Circle circle = new Circle
                {
                    Width = 1,
                    Height = 1,
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.3,
                    Fill = Brushes.Orange
                };

                // Set the position of the Circle on the Canvas
                Canvas.SetLeft(circle, p1.X-0.3);
                Canvas.SetTop(circle, p1.Y - 0.3);

                // Add the Circle to the Canvas
                canvas.Children.Add(circle);
            }
        }

        public Point? GetIntersectionPoint(Line line1, Line line2)
        {
            double x1 = line1.X1;
            double y1 = line1.Y1;
            double x2 = line1.X2;
            double y2 = line1.Y2;
            double x3 = line2.X1;
            double y3 = line2.Y1;
            double x4 = line2.X2;
            double y4 = line2.Y2;

            double denominator = ((y4 - y3) * (x2 - x1)) - ((x4 - x3) * (y2 - y1));

            if (denominator == 0)
            {
                return null; // lines are parallel
            }

            double ua = (((x4 - x3) * (y1 - y3)) - ((y4 - y3) * (x1 - x3))) / denominator;
            double ub = (((x2 - x1) * (y1 - y3)) - ((y2 - y1) * (x1 - x3))) / denominator;

            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                // intersection point lies within both line segments
                double intersectionX = x1 + (ua * (x2 - x1));
                double intersectionY = y1 + (ua * (y2 - y1));

                return new Point(intersectionX, intersectionY);
            }
            else
            {
                return null; // lines do not intersect
            }
        }

        public List<Point?> GetIntersectionPoints(Polyline polyline1, Polyline polyline2)
        {
            List<Point?> intersectionPoints = new List<Point?>();

            List<Point> polyline1Points = polyline1.Points.ToList();
            List<Point> polyline2Points = polyline2.Points.ToList();

            List<Line> polyline1Lines = new List<Line>();
            List<Line> polyline2Lines = new List<Line>();

            // create a list of all the line segments that make up each polyline
            for (int i = 0; i < polyline1Points.Count - 1; i++)
            {
                Line newLine = new Line();
                newLine.X1 = polyline1Points[i].X;
                newLine.Y1 = polyline1Points[i].Y;
                newLine.X2 = polyline1Points[i+1].X;
                newLine.Y2 = polyline1Points[i+1].Y;

                polyline1Lines.Add(newLine);
            }

            for (int i = 0; i < polyline2Points.Count - 1; i++)
            {
                Line newLine = new Line();
                newLine.X1 = polyline2Points[i].X;
                newLine.Y1 = polyline2Points[i].Y;
                newLine.X2 = polyline2Points[i + 1].X;
                newLine.Y2 = polyline2Points[i + 1].Y;

                polyline2Lines.Add(newLine);
                //polyline2Lines.Add(new Line(polyline2Points[i], polyline2Points[i + 1]));
            }

            // iterate through each line segment in the first polyline
            foreach (Line line1 in polyline1Lines)
            {
                // check if it intersects any line segment in the second polyline
                foreach (Line line2 in polyline2Lines)
                {
                    Point? intersection = GetIntersectionPoint(line1, line2);

                    if (intersection != null)
                    {
                        // add the intersection point to the list
                        intersectionPoints.Add(intersection);
                    }
                }
            }

            // repeat the above steps for each line segment in the second polyline
            foreach (Line line2 in polyline2Lines)
            {
                foreach (Line line1 in polyline1Lines)
                {
                    Point? intersection = GetIntersectionPoint(line2, line1);

                    if (intersection != null)
                    {
                        intersectionPoints.Add(intersection);
                    }
                }
            }

            return intersectionPoints;
        }

        private void NadjiPreseke()
        {
            
            for (int i = 0; i < undergroundNacrtani.Count - 1; i++)
            {
                for (int j = i + 1; j < undergroundNacrtani.Count; j++)
                {
                    preseciUn.AddRange(GetIntersectionPoints(undergroundNacrtani[i], undergroundNacrtani[j]));
                }
            }

            for (int i = 0; i < iznadZemljeNacrtani.Count - 1; i++)
            {
                for (int j = i + 1; j < iznadZemljeNacrtani.Count; j++)
                {
                    preseciIznZem.AddRange(GetIntersectionPoints(iznadZemljeNacrtani[i], iznadZemljeNacrtani[j]));
                }
            }
        }


        public Rectangle r, r2;
        //public Rectangle r3, r4;
        private async void promenaBojeAnim_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //na novi klik se prethodna dva gase
            canvas.Children.Remove(r);
            canvas.Children.Remove(r2);

            Polyline mojVod = (Polyline)sender;
            Point p1 = new Point();
            Point p2 = new Point();
            p1 = mojVod.Points.First();
            p2 = mojVod.Points.ElementAt(mojVod.Points.Count - 1);

            PowerEntity pe1 = XMLelementi.Find(k => k.X == p1.X && k.Y == p1.Y);
            PowerEntity pe2 = XMLelementi.Find(k => k.X == p2.X && k.Y == p2.Y);

            r = new Rectangle();
            r.Name = "FirstNode";
            r.Fill = Brushes.Red;
            r.Width = 4;
            r.Height = 4;
            Canvas.SetBottom(r, 600 - 2 - p1.Y);
            Canvas.SetLeft(r, -2 + p1.X);
            canvas.Children.Add(r);

            r2 = new Rectangle();
            r2.Fill = Brushes.Red;
            r2.Width = 4;
            r2.Height = 4;
            Canvas.SetBottom(r2, 600 - 2 - p2.Y);
            Canvas.SetLeft(r2, -2 + p2.X);
            canvas.Children.Add(r2);


            await Task.Delay(1000);
            canvas.Children.Remove(r);
            canvas.Children.Remove(r2);

            r.Width = 2;
            r.Height = 2;
            Canvas.SetBottom(r, 600 - 1 - p1.Y);
            Canvas.SetLeft(r, -1 + p1.X);

            r2.Width = 2;
            r2.Height = 2;
            Canvas.SetBottom(r2, 600 - 1 - p2.Y);
            Canvas.SetLeft(r2, -1 + p2.X);

            canvas.Children.Add(r);
            canvas.Children.Add(r2);
            /*
            r3 = new Rectangle();
            r3.Name = "FirstNode";
            r3.Fill = Brushes.Red;
            r3.Width = 2;
            r3.Height = 2;
            Canvas.SetBottom(r3, 600 - 1 - p1.Y);
            Canvas.SetLeft(r3, -1 + p1.X);

            r4 = new Rectangle();
            r4.Name = "SecondNode";
            r4.Fill = Brushes.Red;
            r4.Width = 1;
            r4.Height = 1;
            Canvas.SetBottom(r4, 600 - 1 - p2.Y);
            Canvas.SetLeft(r4, -1 + p2.X);
            canvas.Children.Add(r3);
            canvas.Children.Add(r4);
            */
        }

        //trazim tacke za vod
        private void krajeviVoda(LineEntity le, out Point start, out Point end)
        {
            PowerEntity elem;

            elem = XMLelementi.Find(x => x.Id == le.FirstEnd);
            start = dictTackaElement.Where(x => x.Value == elem).First().Key;

            elem = XMLelementi.Find(x => x.Id == le.SecondEnd);
            end = dictTackaElement.Where(x => x.Value == elem).First().Key;
        }

        private void Provera(double noviX, double noviY)
        {
            //treba mi zbog skaliranja
            if (checkMinMax == 1)
            {
                praviXmax = noviX;
                praviYmax = noviY;
                praviXmin = noviX;
                praviYmin = noviY;

                checkMinMax++;
            }
            else
            {
                //proveraMAX
                if (noviX > praviXmax)
                {
                    praviXmax = noviX;
                }

                if (noviY > praviYmax)
                {
                    praviYmax = noviY;
                }

                //proveraMIN
                if (noviX < praviXmin)
                {
                    praviXmin = noviX;
                }

                if (noviY < praviYmin)
                {
                    praviYmin = noviY;
                }
            }

            razlikaMinMaxX = (praviXmax - praviXmin) * 100;
            razlikaMinMaxY = (praviYmax - praviYmin) * 100;
        }

        // daje mi koordinate za canvas
        private void KoordinateNaCanvasu(double noviX, double noviY, out double praviX, out double praviY)
        {
            //razvlacim sliku preko canvasa
            double odstojanjeX = 200 / razlikaMinMaxX;
            double odstojanjeY = 200 / razlikaMinMaxY;

            praviX = Math.Round((noviX - praviXmin) * 100 * odstojanjeX); //preklapaju se
            praviY = Math.Round((noviY - praviYmin) * 100 * odstojanjeY);

            //na kraju da bi se se tacke toliko pomerile po datoj osi, tj. onaj razmak
            // pa se nece preklapati
            praviX = praviX * 3; //---ovo ce mi biti po visini
            praviY = praviY * 3; //--- ovo ce mi biti po sirini
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void editObj_Click(object sender, MouseButtonEventArgs e)
        {
            
            //Update za kliknut objekat
            if (e.OriginalSource is Ellipse)
            {
                Ellipse ClickedRectangle = (Ellipse)e.OriginalSource;

                //otvori ovu elipsu
                canvas.Children.Remove(ClickedRectangle);

                //za textBlock
                string bojaTeksta = "Black", samTekst = "nekiTekst";
                foreach (FrameworkElement item in canvas.Children)
                {
                    if (item.Name == ClickedRectangle.Name + "eltb") //treba mi item.Name, a Name(F12) vodi na FrameworkElement
                    {
                        canvas.Children.Remove(item);
                        bojaTeksta = ((TextBlock)item).Foreground.ToString();
                        samTekst = ((TextBlock)item).Text;
                        break;
                    }
                }

                EditElipsa editElipsa = new EditElipsa(ClickedRectangle.Height, ClickedRectangle.Width, ClickedRectangle.StrokeThickness, ClickedRectangle.Fill, bojaTeksta, samTekst);
                editElipsa.Show();

            }
            else if (e.OriginalSource is Polygon)
            {
                Polygon ClickedRectangle = (Polygon)e.OriginalSource;

                canvas.Children.Remove(ClickedRectangle);

                //za textBlock
                string bojaTeksta = "Black", samTekst = "nekiTekst";
                foreach (FrameworkElement item in canvas.Children)
                {
                    if (item.Name == ClickedRectangle.Name + "pgtb") //treba mi item.Name, a Name(F12) vodi na FrameworkElement
                    {
                        canvas.Children.Remove(item);
                        bojaTeksta = ((TextBlock)item).Foreground.ToString();
                        samTekst = ((TextBlock)item).Text;
                        break;
                    }
                }

                EditPolygon editPoligon = new EditPolygon(ClickedRectangle.StrokeThickness, ClickedRectangle.Fill.ToString(), bojaTeksta, samTekst, ClickedRectangle.Points);
                editPoligon.Show();
            }
            else if (e.OriginalSource is TextBlock)
            {
                TextBlock ClickedRectangle = (TextBlock)e.OriginalSource;

                string slova = ClickedRectangle.Name;
                slova = slova.Substring(8, slova.Length - 8);
                if (slova != "pgtb" && slova != "eltb")
                {
                    //otvori ovaj tekst
                    canvas.Children.Remove(ClickedRectangle);
                    EditText editTekst = new EditText(ClickedRectangle.FontSize, ClickedRectangle.Foreground, ClickedRectangle.Text);
                    editTekst.Show();
                }
            }
            
        }

        
        private void LeviKlikPoligon(object sender, MouseButtonEventArgs e)
        {
            
            Poligon poligonCrtez = new Poligon();

            //grupisem da samo 1 moze da se izabere
            int i = 1;

            if (EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true || EllipseChecked.IsChecked == true && TextChecked.IsChecked == true ||
                EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true && TextChecked.IsChecked == true ||
                PolygonChecked.IsChecked == true && TextChecked.IsChecked == true)
            {
                i = 2;
                MessageBox.Show("Select only one option!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (i == 1 && PolygonChecked.IsChecked == true && koordinatePoX.Count >= 3)
            {
                poligonCrtez.Show();
            }
            else if (PolygonChecked.IsChecked == true)
            {
                MessageBox.Show("You have to click on right mouse click at least trhee times to create new polygon!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Information);
                koordinatePoX.Clear();
                koordinatePoY.Clear();
            }
            
        }

        private void DesniKlik(object sender, MouseButtonEventArgs e)
        {
            
            //grupisem da samo 1 moze da se izabere
            int i = 1;

            if (EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true || EllipseChecked.IsChecked == true && TextChecked.IsChecked == true ||
                EllipseChecked.IsChecked == true && PolygonChecked.IsChecked == true && TextChecked.IsChecked == true ||
                PolygonChecked.IsChecked == true && TextChecked.IsChecked == true)
            {
                i = 2;
                MessageBox.Show("Select only one option!", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (i == 1)
            {
                if (EllipseChecked.IsChecked == true)
                {
                    Elipsa elipsaCrtez = new Elipsa();

                    //Pokupio ih je
                    klikX = Mouse.GetPosition(canvas).X;
                    klikY = Mouse.GetPosition(canvas).Y;

                    elipsaCrtez.Show();
                }
                else if (PolygonChecked.IsChecked == true)
                {
                    klikX = Mouse.GetPosition(canvas).X;
                    klikY = Mouse.GetPosition(canvas).Y;

                    koordinatePoX.Add(klikX);
                    koordinatePoY.Add(klikY);
                }
                else if (TextChecked.IsChecked == true)
                {
                    AddText dodajTekstCrtez = new AddText();

                    klikX = Mouse.GetPosition(canvas).X;
                    klikY = Mouse.GetPosition(canvas).Y;

                    dodajTekstCrtez.Show();
                }
            }
            
        }


        List<Shape> obrisanoUPoslednjojIter = new List<Shape>();
        List<TextBlock> obrisanTextUPoslednjojIter = new List<TextBlock>();

        //da bi se obrisao tekst unutar elementa mora 2 puta undo
        private void Undo_Click(object sender, RoutedEventArgs e)
        {

            if (canvas.Children.Count > 0)
            {
                obrisaniListaZaBrojanje.Add(canvas.Children[canvas.Children.Count - 1]);
                canvas.Children.Remove(canvas.Children[canvas.Children.Count - 1]);
            }

            if (canvas.Children.Count != numberChildren)
            {
                for (int i = 0; i < ponovoIscrtaj.Count; i++)
                {
                    if (ponovoIscrtaj[i] != null)
                        canvas.Children.Add(ponovoIscrtaj[i]);
                }
            }

            for (int i = 0; i < ponovoIscrtaj.Count; i++)
            {
                ponovoIscrtaj[i] = null;
            }
        }

        //da bi se vratio tekst unutar elementa mora 2 puta redo
        private void RedO_Click(object sender, RoutedEventArgs e)
        {

            if (obrisaniListaZaBrojanje.Count > 0)
            {
                //vraca na prethodnu i onda je brise sa liste                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                canvas.Children.Add(obrisaniListaZaBrojanje[obrisaniListaZaBrojanje.Count - 1]);
                obrisaniListaZaBrojanje.RemoveAt(obrisaniListaZaBrojanje.Count - 1);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            
            //brise samo one iscrtane objekte, a ne celu mapu
            if (canvas.Children.Count > 0)
            {
                // da se ne bu obrisala mapa
                foreach (UIElement jedanOdElemenata in canvas.Children)
                {
                    mapa.Add(jedanOdElemenata);
                }

                // cuvam one koje zelim da crtam ponovo
                if (canvas.Children.Count > numberChildren)
                {
                    for (int i = numberChildren; i < canvas.Children.Count; i++)
                    {
                        ponovoIscrtaj.Add(canvas.Children[i]);
                    }
                }
                canvas.Children.Clear();

                //ponovo crtam pocetnu mapu
                for (int i = 0; i < numberChildren; i++)
                {
                    canvas.Children.Add(mapa[i]);
                }
                mapa.Clear();

                numberChildren = canvas.Children.Count;
            }
            

            /*
            foreach(Shape shape in nacrtaniElementi)
            {
                canvas.Children.Remove(shape);
            }

            foreach(TextBlock tb in dodatTekst)
            {
                canvas.Children.Remove(tb);
            }
            */
            
        }

        //From UTM to Latitude and longitude in decimal
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
    }
}
