   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.UtilVisualizer.Util  q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�^clr-namespace:Telerik.Windows.Controls.Navigation;assembly=Telerik.Windows.Controls.Navigation#Telerik.Windows.Controls.Navigation     Visualizer.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     .Utilclr-namespace:Visualizer.Util     f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r
 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	   	 
      
navigation^clr-namespace:Telerik.Windows.Controls.Navigation;assembly=Telerik.Windows.Controls.Navigation	   	 
      @  :Telerik.Windows.Controls.Navigation.RadWindowInteropHelper    Title$  LocOn��   ShowInTaskbar$
 True�� � 	Resources    BoolToVisConverter��%   &	        �� ���� $��LocOn��$��Italic$�$��Medium"�$��Center=�$��18(�
�������.���  � RowDefinitions �� $��* �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�0  *Telerik.Windows.Controls.RadNavigationView  -     Name$ C��$��1��   AutoChangeDisplayMode$ False��   DisplayMode$ Compact=�$��LeftToRight=�   
PaneFooter 4  .Telerik.Windows.Controls.RadNavigationViewItem  -   $��Application Settings��$��18(�$��Segoe UI)�$��	10,-3,0,0q����� *$"ShowApplicationSettingsPaneCommand+	   Icon	 '  !Telerik.Windows.Controls.RadGlyph  
   Glyph$	
 ��   FontSize$ 25(�@�  -   $��	Dashboard��$��18(�$��Segoe UI)�$��	10,-1,0,0q�$��0,5,0,0q����� * ShowSystemReportingPaneCommand+y��� *IsReportingEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� �� 	Converter# [  	   $	
 ��$ 25(�  -   $��Inventory Management��$��18(�$��Segoe UI)�$��10,2,0,0q�$��0,5,0,0q����� *$"ShowInventoryManagementPaneCommand+y��� *IsInventoryEnabled+$ PropertyChanged=�# [  	   $	
 ��$ 23(�$��2,0,0,0q�  -   $��Asset Management��$��18(�$��Segoe UI)�$��10,2,0,0q�$��0,5,0,0q����� * ShowAssetManagementPaneCommand+y��� *IsAssetEnabled+$ PropertyChanged=�# [  	   $	
 ��$ 23(�$��2,0,0,0q�  -   $��Mapping��$��18(�$��Segoe UI)�$��10,2,0,0q�$��0,5,0,0q����� *ShowMappingPaneCommand+y��� *IsMappingEnabled+$ PropertyChanged=�# [  	   $	
 ��$ 25(�$��2,0,0,0q�   Content. ) 	 #Telerik.Windows.Controls.RadDocking  -     Name$ 
radDocking��   SerializationTag$ 
RadDocking��   AllowUnsafeMode$
 True��   HasDocumentHost$ False��   PanesSource �� *Panes+$ PropertyChanged=� �� NotifyOnSourceUpdated$
 True�� �� Mode$ TwoWay=� � DataContext ��    DockingPanesFactory /   )Visualizer.Util.CustomDockingPanesFactory  .@�0 	 *Telerik.Windows.Controls.RadSplitContainer     InitialPosition$ 
DockedLeft=�+ 	 %Telerik.Windows.Controls.DockingPanel   InitialSize$ 300,600��.@�+	 	 %Telerik.Windows.Controls.RadPaneGroup	  -   $��	leftGroup��$ 	leftGroup��;
  5Telerik.Windows.Controls.TabControl.RadTabControlBase 
  IsContentPreserved$
 True�� ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType"   �� Path$ DataContext�  $ DockedRight=�.@�	  -	    	 Name$ 
rightGroup��$ 
rightGroup��$
 True�� ��  �� $ FindAncestor=�"  $ DataContext�  $ DockedBottom=�.@�	  -
   $ bottomGroup��$ bottomGroup��$
 True�� ��  �� $ FindAncestor=�"  $ DataContext�