﻿#pragma checksum "..\..\AddText.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "995B948928E4D19C949CF019ADDBA94E4468E5A3DB35E9B8C460399C900D4771"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
using System.Windows.Forms.Integration;
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
using WpfApp1;


namespace WpfApp1 {
    
    
    /// <summary>
    /// AddText
    /// </summary>
    public partial class AddText : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\AddText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtUnos;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\AddText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbColor;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\AddText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbFontSize;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\AddText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTekstGreska;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\AddText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblcmbBojaGreska;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\AddText.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblcmbVelicinaGreska;
        
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
            System.Uri resourceLocater = new System.Uri("/RGrafikaPZ1;component/addtext.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddText.xaml"
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
            this.txtUnos = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.cmbColor = ((System.Windows.Controls.ComboBox)(target));
            
            #line 15 "..\..\AddText.xaml"
            this.cmbColor.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CmbColor_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmbFontSize = ((System.Windows.Controls.ComboBox)(target));
            
            #line 25 "..\..\AddText.xaml"
            this.cmbFontSize.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new System.Windows.Controls.TextChangedEventHandler(this.CmbFontSize_Changed));
            
            #line default
            #line hidden
            return;
            case 4:
            this.lblTekstGreska = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lblcmbBojaGreska = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.lblcmbVelicinaGreska = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            
            #line 30 "..\..\AddText.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EnterText);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

