using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.GreetingTab
{
    public class GreetingContainer
    {
        private Point3D _position;
        private static MainWindow _mainWindow;
        private static Viewport3D _viewport;
        private static Viewport2DVisual3D _viewport2DVisual3D;
        private static PerspectiveCamera _camera;

        #region Properties
        public Viewport2DVisual3D ContainerViewport2DVisual3D
        {
            get
            {
                return _viewport2DVisual3D;
            }
            set
            {
                _viewport2DVisual3D = value;
            }
        }
        #endregion

        public GreetingContainer(MainWindow mainWindow, Viewport3D viewport,
            Point3D position, PerspectiveCamera camera)
        {
            // Initialize data-members
            _mainWindow = mainWindow;
            _viewport = viewport;
            _camera = camera;
            this._position = position;

            // Create Lighted Area
            CreateModelVisual3DArea();

            // Create Face
            CreateFace();
        }

        private void CreateModelVisual3DArea()
        {
            _viewport.Children.Add(new ModelVisual3D
            {
                Content = new DirectionalLight
                {
                    Color = Colors.White,
                    Direction = new Vector3D(0, 0, -1)
                }
            });
        }

        public void CreateFace()
        {
            _viewport2DVisual3D = new Viewport2DVisual3D();

            // Creating Meshes
            CreateMeshes();

            // Adding All container to viewport3D
            _viewport.Children.Add(_viewport2DVisual3D);
        }

        private void CreateMeshes()
        {
            CreateMeshGeometry(_position);
            CreateComponents();
        }

        private void CreateMeshGeometry(Point3D initialPoint)
        {
            double pointParam = 1.3;
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection 
            { 
               HelperGeometry.GetSummedPoint(new Point3D(-pointParam, pointParam, 0),initialPoint), 
               HelperGeometry.GetSummedPoint(new Point3D(-pointParam, -(pointParam), 0),initialPoint),
               HelperGeometry.GetSummedPoint(new Point3D(2*pointParam, -(pointParam), 0), initialPoint),
               HelperGeometry.GetSummedPoint(new Point3D(2*pointParam, (pointParam), 0) ,initialPoint)
            };
            mesh.TriangleIndices = new Int32Collection(new int[] { 0, 1, 2, 0, 2, 3 });
            mesh.TextureCoordinates = new PointCollection(new Point[] 
            { 
                new Point(0, 0), 
                new Point(0, 1), 
                new Point(1, 1), 
                new Point(1, 0) 
            });
            _viewport2DVisual3D.Geometry = mesh;
            var material = new DiffuseMaterial
            {
                Brush = Brushes.White
            };
            Viewport2DVisual3D.SetIsVisualHostMaterial(material, true);
            _viewport2DVisual3D.Material = material;
        }

        private static void CreateComponents()
        {
            _viewport2DVisual3D.Visual = InitializeStackPanel();
            AnimationProvider.CreateConstantViewportRotationAnimationAroundPoint(_viewport2DVisual3D);
        }

        private static Border InitializeStackPanel()
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(4.5);

            // TextBlock Header
            TextBlock tbCROSPELL = new TextBlock();
            tbCROSPELL.FontFamily = new FontFamily("Agency FB");
            tbCROSPELL.Margin = new Thickness(0.2);
            tbCROSPELL.TextAlignment = TextAlignment.Center;
            tbCROSPELL.Text = "CROSPELL" + Environment.NewLine + "E N G I N E";
            tbCROSPELL.FontSize = 80;
            //tbCROSPELL.FontWeight = FontWeights.Bold;
            tbCROSPELL.Margin = new Thickness(40);

            //TextBlock tbEngine = new TextBlock();
            //tbEngine.FontFamily = new FontFamily("Agency FB");
            //tbEngine.Margin = new Thickness(0.2);
            //tbEngine.TextAlignment = TextAlignment.Center;
            //tbEngine.Text = "E N G I N E";
            //tbEngine.FontSize = 80;

            AnimationProvider.AnimateCROSPELLENGINETextBox(tbCROSPELL);

            stackPanel.Children.Add(tbCROSPELL);
            //stackPanel.Children.Add(tbEngine);
            stackPanel.Background = new SolidColorBrush(Colors.Gold);

            AnimationProvider.AnimateCROSPELLENGINEStackPanel(stackPanel);

            Border border = new Border();
            border.BorderThickness = new Thickness(1);

            // Set BackGround
            //LinearGradientBrush myBrush = new LinearGradientBrush();
            //myBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
            //myBrush.GradientStops.Add(new GradientStop(Colors.Gray, 1.0));
            //border.Background = myBrush;
            border.Child = stackPanel;
            border.Padding = new Thickness(7);
            border.BorderBrush = new SolidColorBrush(Colors.DarkGray);
            border.CornerRadius = new CornerRadius(0);
            
            return border;
        }
    }
}
