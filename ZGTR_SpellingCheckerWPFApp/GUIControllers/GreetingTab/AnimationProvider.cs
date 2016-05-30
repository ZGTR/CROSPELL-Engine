using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.GreetingTab
{
    public class AnimationProvider
    {
        public static void CreateViewportScaleLikeAnimation(Viewport2DVisual3D viewport2DVisual3D, double from, double to)
        {
            var translateTransform3D = new TranslateTransform3D();
            var rotateAnimation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                IsCumulative = false
            };

            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, rotateAnimation);
            viewport2DVisual3D.Transform = translateTransform3D;
        }

        public static void CreateViewportConstantRotationAnimationAroundX(Viewport2DVisual3D viewport2DVisual3D)
        {
            // Create Animation Around X Axis
            var rotationAnimationAroundXAxis = new Rotation3DAnimation();
            rotationAnimationAroundXAxis.From = new AxisAngleRotation3D
            {
                Angle = 0,
                Axis = new Vector3D(1, 0, 0)    // X Axis
            };
            rotationAnimationAroundXAxis.To = new AxisAngleRotation3D
            {
                Angle = 340,
                Axis = new Vector3D(1, 0, 0)    // X Axis
            };
            rotationAnimationAroundXAxis.Duration = new Duration(TimeSpan.FromSeconds(2));
            rotationAnimationAroundXAxis.AutoReverse = true;
            rotationAnimationAroundXAxis.RepeatBehavior = RepeatBehavior.Forever;

            // Define Property to animate
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.BeginAnimation(RotateTransform3D.RotationProperty, rotationAnimationAroundXAxis);
            viewport2DVisual3D.Transform = rotateTransform3D;
        }

        public static void CreateViewportFlipAnimation(Viewport2DVisual3D viewport2DVisual3D)
        {
            // Create Animation
            Rotation3DAnimation FlipAnimation = new Rotation3DAnimation();
            FlipAnimation.From = new AxisAngleRotation3D
            {
                Angle = 0,
                Axis = new Vector3D(1, 0, 0)    // X Axis
            };
            FlipAnimation.To = new AxisAngleRotation3D
            {
                Angle = 180,
                Axis = new Vector3D(1, 0, 0)    // X Axis
            };
            FlipAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            FlipAnimation.AutoReverse = true;
            //myAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Define Property to animate
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.CenterZ = 0.3;
            rotateTransform3D.CenterY = 0.3;
            rotateTransform3D.CenterX = 0.3;
            rotateTransform3D.BeginAnimation(RotateTransform3D.RotationProperty, FlipAnimation);
            viewport2DVisual3D.Transform = rotateTransform3D;
        }

        public static void CreateViewportMovementAnimation(Viewport2DVisual3D viewport2DVisual3D)
        {
            DoubleAnimation movementAnimationXAxis = new DoubleAnimation(20, new Duration(TimeSpan.FromSeconds(3)));
            DoubleAnimation movementAnimationZAxis = new DoubleAnimation(20, new Duration(TimeSpan.FromSeconds(3)));

            TranslateTransform3D translateTransform3D = new TranslateTransform3D();
            viewport2DVisual3D.Transform = translateTransform3D;
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, movementAnimationXAxis);
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, movementAnimationZAxis);
        }

        public static void CreateViewportMovementAnimationAlongY(Viewport2DVisual3D viewport2DVisual3D)
        {
            DoubleAnimation movementAnimationYAxis = new DoubleAnimation(7, 3, new Duration(TimeSpan.FromSeconds(1)));
            movementAnimationYAxis.RepeatBehavior = RepeatBehavior.Forever;
            movementAnimationYAxis.AutoReverse = true;

            TranslateTransform3D translateTransform3D = new TranslateTransform3D();
            viewport2DVisual3D.Transform = translateTransform3D;
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetYProperty, movementAnimationYAxis);
        }

        public static void CreateCanvasScaleAnimation(Label label)
        {
            DoubleAnimation animation = new DoubleAnimation(0, 200, new Duration(TimeSpan.FromSeconds(3)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            label.BeginAnimation(Label.WidthProperty, animation);
        }

        public static void CreateViewportMovementAnimation(Viewport2DVisual3D viewport2DVisual3D, Point3D initialPosition, Point3D targetPosition)
        {
            DoubleAnimation movementAnimationXAxis = new DoubleAnimation(initialPosition.X, targetPosition.X,
                                                                         new Duration(TimeSpan.FromSeconds(2)));

            TranslateTransform3D translateTransform3D = new TranslateTransform3D(initialPosition.X, initialPosition.Y, initialPosition.Z);
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, movementAnimationXAxis);
            viewport2DVisual3D.Transform = translateTransform3D;
        }

        public static void CreateCameraMovementAlongXAxisAnimation(Point3D targetCameraPosition, PerspectiveCamera camera)
        {
            Point3DAnimation movementAnimationXAxis = new Point3DAnimation(
                camera.Position,
                targetCameraPosition,
                new Duration(TimeSpan.FromSeconds(1)));

            camera.BeginAnimation(ProjectionCamera.PositionProperty, movementAnimationXAxis);
        }

        public static void CreateTextBlockMovementin2D(TextBlock textBlock)
        {
            DoubleAnimation myAnimationX = new DoubleAnimation(500, 0, new Duration(TimeSpan.FromSeconds(7)));
            myAnimationX.RepeatBehavior = RepeatBehavior.Forever;
            textBlock.RenderTransform = new TranslateTransform();
            textBlock.RenderTransform.BeginAnimation(TranslateTransform.XProperty, myAnimationX);
        }

        public static void CreateBorderMovementAlongYAxis(Border borderStudentAbsent, Point source, Point destination)
        {
            DoubleAnimation myAnimationX = new DoubleAnimation(source.Y - 20, destination.Y - 20, new Duration(TimeSpan.FromSeconds(0.5)));
            borderStudentAbsent.RenderTransform = new TranslateTransform();
            borderStudentAbsent.RenderTransform.BeginAnimation(TranslateTransform.YProperty, myAnimationX);
        }

        public static void CreateCanvasColorAnimation(Canvas canvas)
        {
            SolidColorBrush myAnimatedBrush = new SolidColorBrush();
            myAnimatedBrush.Color = Colors.Transparent;
            canvas.Background = myAnimatedBrush;

            ColorAnimation animation = new ColorAnimation
            {
                From = Colors.Red,
                To = Colors.Transparent,
                Duration = new Duration(TimeSpan.FromSeconds(1.5))
            };
            animation.RepeatBehavior = new RepeatBehavior(3);
            myAnimatedBrush.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        public static void CreateConstantViewportRotationAnimationAroundPoint(Viewport2DVisual3D viewport2DVisual3D)
        {
            // Create Animation
            Rotation3DAnimation FlipAnimationX = new Rotation3DAnimation();
            FlipAnimationX.From = new AxisAngleRotation3D
            {
                Angle = 17,
                Axis = new Vector3D(0, 1, 0)    // X Axis
            };
            FlipAnimationX.To = new AxisAngleRotation3D
            {
                Angle = -17,
                Axis = new Vector3D(0, 1, 0)    // X Axis
            };
            FlipAnimationX.Duration = new Duration(TimeSpan.FromSeconds(7));
            FlipAnimationX.AutoReverse = true;
            //FlipAnimationX.IsAdditive = true;
            FlipAnimationX.AccelerationRatio = 0.01;
            //FlipAnimationX.DecelerationRatio = 0.0001;
            FlipAnimationX.RepeatBehavior = RepeatBehavior.Forever;
            // Define Property to animate
            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            rotateTransform3D.CenterX = 0.5;
            rotateTransform3D.CenterY = 0.5;
            rotateTransform3D.CenterZ = 0.5;
            rotateTransform3D.BeginAnimation(RotateTransform3D.RotationProperty, FlipAnimationX);
            viewport2DVisual3D.Transform = rotateTransform3D;
        }
        
        public static void AnimateCROSPELLENGINETextBox(TextBlock tbCROSPELL)
        {
            ColorAnimation cCrospellAnim = new ColorAnimation(Colors.Black, 
                Colors.Yellow, new Duration(TimeSpan.FromSeconds(6)));
            cCrospellAnim.AutoReverse = true;
            cCrospellAnim.RepeatBehavior = RepeatBehavior.Forever;
            cCrospellAnim.AccelerationRatio = 0.00001;
            SolidColorBrush sCrospellBrush = new SolidColorBrush();
            sCrospellBrush.BeginAnimation(SolidColorBrush.ColorProperty, cCrospellAnim);
            tbCROSPELL.Foreground = sCrospellBrush;
        }

        public static void AnimateCROSPELLENGINEStackPanel(StackPanel sp)
        {
            ColorAnimation cCrospellAnim = new ColorAnimation(Colors.Yellow,
                Colors.Black, new Duration(TimeSpan.FromSeconds(6)));
            //cCrospellAnim.AutoReverse = true;
            //cCrospellAnim.RepeatBehavior = RepeatBehavior.Forever;
            //cCrospellAnim.AccelerationRatio = 0.00001;
            SolidColorBrush sCrospellBrush = new SolidColorBrush();
            sCrospellBrush.BeginAnimation(SolidColorBrush.ColorProperty, cCrospellAnim);
            sp.Background = sCrospellBrush;
        }
    }
}
