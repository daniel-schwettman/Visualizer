﻿#pragma checksum "..\..\..\..\View\MappingPaneView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "06CAAD8C2C96ECD93DF0E863A0589B2A2B247095"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Maps.MapControl.WPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;
using Telerik.Windows.Controls.Behaviors;
using Telerik.Windows.Controls.Carousel;
using Telerik.Windows.Controls.Data.PropertyGrid;
using Telerik.Windows.Controls.Docking;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.LayoutControl;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Controls.MultiColumnComboBox;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.RadialMenu;
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Controls.Wizard;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Persistence;
using Telerik.Windows.Persistence.SerializationMetadata;
using Telerik.Windows.Shapes;
using Visualizer.View;


namespace Visualizer.View {
    
    
    /// <summary>
    /// MappingPaneView
    /// </summary>
    public partial class MappingPaneView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\..\View\MappingPaneView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView TagGridView;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\View\MappingPaneView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView TagGridView2;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\..\..\View\MappingPaneView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView TagHistoryGridView;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\..\View\MappingPaneView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView TagHistoryGridView2;
        
        #line default
        #line hidden
        
        
        #line 137 "..\..\..\..\View\MappingPaneView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView AssetGridView;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\..\..\View\MappingPaneView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Microsoft.Maps.MapControl.WPF.Map MapView;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Visualizer;component/view/mappingpaneview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\MappingPaneView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\..\View\MappingPaneView.xaml"
            ((Visualizer.View.MappingPaneView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Map_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TagGridView = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 50 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagGridView.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.TagGridView_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 50 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagGridView.Filtered += new System.EventHandler<Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs>(this.TagGridView_Filtered);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 55 "..\..\..\..\View\MappingPaneView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CurrentTagsReportButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TagGridView2 = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 74 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagGridView2.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.TagGridView2_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagGridView2.Filtered += new System.EventHandler<Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs>(this.TagGridView2_Filtered);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TagHistoryGridView = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 94 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagHistoryGridView.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.TagHistoryGridView_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 94 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagHistoryGridView.Filtered += new System.EventHandler<Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs>(this.TagHistoryGridView_Filtered);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 109 "..\..\..\..\View\MappingPaneView.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.TagHistoryReportButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.TagHistoryGridView2 = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 119 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagHistoryGridView2.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.TagHistoryGridView2_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 119 "..\..\..\..\View\MappingPaneView.xaml"
            this.TagHistoryGridView2.Filtered += new System.EventHandler<Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs>(this.TagHistoryGridView2_Filtered);
            
            #line default
            #line hidden
            return;
            case 8:
            this.AssetGridView = ((Telerik.Windows.Controls.RadGridView)(target));
            
            #line 140 "..\..\..\..\View\MappingPaneView.xaml"
            this.AssetGridView.SelectionChanged += new System.EventHandler<Telerik.Windows.Controls.SelectionChangeEventArgs>(this.AssetGridView_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 140 "..\..\..\..\View\MappingPaneView.xaml"
            this.AssetGridView.Filtered += new System.EventHandler<Telerik.Windows.Controls.GridView.GridViewFilteredEventArgs>(this.AssetGridView_Filtered);
            
            #line default
            #line hidden
            return;
            case 9:
            this.MapView = ((Microsoft.Maps.MapControl.WPF.Map)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

