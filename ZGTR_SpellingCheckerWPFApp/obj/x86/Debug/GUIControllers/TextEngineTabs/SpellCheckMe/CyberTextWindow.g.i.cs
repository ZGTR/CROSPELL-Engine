﻿#pragma checksum "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EAE47489CF08BD82C21A909625EBCD98"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ZGTR_CROSPELLSpellingCheckerApp.GUIControllers.TextEngineTabs.SpellCheckMe {
    
    
    /// <summary>
    /// CyberTextWindow
    /// </summary>
    public partial class CyberTextWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblCTWTextArea;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bCTWCorrect;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bCTWDoneEditing;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ZGTR_CROSPELLSpellingCheckerApp;component/guicontrollers/textenginetabs/spellche" +
                    "ckme/cybertextwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tblCTWTextArea = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.bCTWCorrect = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml"
            this.bCTWCorrect.Click += new System.Windows.RoutedEventHandler(this.bCTWCorrect_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.bCTWDoneEditing = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\..\..\GUIControllers\TextEngineTabs\SpellCheckMe\CyberTextWindow.xaml"
            this.bCTWDoneEditing.Click += new System.Windows.RoutedEventHandler(this.bCTWDoneEditing_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

