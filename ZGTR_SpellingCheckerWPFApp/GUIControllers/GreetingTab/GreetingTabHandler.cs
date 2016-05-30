using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.GreetingTab
{
    public class GreetingTabHandler
    {
        private GreetingContainer _GreetingContainer;
        public MainWindow MainWindow { get; set; }

        public GreetingTabHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.Position = new Point3D(0.5, 0, 6.5);
            camera.LookDirection = new Vector3D(0, 0, -10);
            this.MainWindow.VPGreeting.Camera = camera;

            // Create the init Hall
            _GreetingContainer = new GreetingContainer(this.MainWindow,
                this.MainWindow.VPGreeting, new Point3D(), camera);
        }
    }
}
