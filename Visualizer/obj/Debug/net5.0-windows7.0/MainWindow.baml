   M S B A M L   `   `   `  ����E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.UtilVisualizer.Util  5   	   q mTelerik.Windows.Controls.Navigation, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�^clr-namespace:Telerik.Windows.Controls.Navigation;assembly=Telerik.Windows.Controls.Navigation#Telerik.Windows.Controls.Navigation 5   	       Visualizer.MainWindow   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   .Utilclr-namespace:Visualizer.Util     5   	   f bTelerik.Windows.Controls, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Mtelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
     5   	   }
navigation^clr-namespace:Telerik.Windows.Controls.Navigation;assembly=Telerik.Windows.Controls.Navigation   	 
     5   	   @  :Telerik.Windows.Controls.Navigation.RadWindowInteropHelper    Title$  LocOn��5   	      ShowInTaskbar$
 True��5   	    � 	Resources 5	         BoolToVisConverter��%   &	        �� 5
   
   6'   5      ��5      �� 5   
   $��LocOn��6   $��Italic$�6!   $��Medium"�64   $��Center=�6H   $��18(�6e   
�������6s   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��Center=�6X   5      0  *Telerik.Windows.Controls.RadNavigationView  5   
   -   5          Name$ C��5   1   $��1��6$      AutoChangeDisplayMode$ False��6<      DisplayMode$ Compact=�6Z   $��LeftToRight=�6p      
PaneFooter 5      4  .Telerik.Windows.Controls.RadNavigationViewItem  5      -   5        $��Application Settings��5   0   $��18(�6O   $��Segoe UI)�6^   $��	10,-3,0,0q�6t   ��6�   �� *$"ShowApplicationSettingsPaneCommand+	   Icon	 5      '  !Telerik.Windows.Controls.RadGlyph  5      
   Glyph$	
 ��6+      FontSize$ 25(�6<   5      @�5        5$      -   5        $��	Dashboard��5$   0   $��18(�6D   $��Segoe UI)�6R   $��	10,-1,0,0q�6h   $��0,5,0,0q�5%   1   ��6B   �� * ShowSystemReportingPaneCommand+y�6�   �� *IsReportingEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� �� 	Converter# [  	 5&        5'      $	
 ��6+   $ 25(�6<   5)        5*      -   5        $��Inventory Management��5*   0   $��18(�6�   $��Segoe UI)�6�   $��10,2,0,0q�6�   $��0,5,0,0q�5+   0   ��5*   O   �� *$"ShowInventoryManagementPaneCommand+y�5+   f   �� *IsInventoryEnabled+$ PropertyChanged=�# [  	 5,        5-      $	
 ��6+   $ 23(�6<   $��2,0,0,0q�6J   5/        50      -   5        $��Asset Management��50   0   $��18(�6~   $��Segoe UI)�6�   $��10,2,0,0q�6�   $��0,5,0,0q�51   0   ��50   K   �� * ShowAssetManagementPaneCommand+y�51   f   �� *IsAssetEnabled+$ PropertyChanged=�# [  	 52        53      $	
 ��6+   $ 23(�6<   $��2,0,0,0q�6J   55        56      -   5        $��Mapping��56   0   $��18(�6m   $��Segoe UI)�6{   $��10,2,0,0q�6�   $��0,5,0,0q�57   1   ��56   B   �� *ShowMappingPaneCommand+y�57   f   �� *IsMappingEnabled+$ PropertyChanged=�# [  	 58        59      $	
 ��6+   $ 25(�6<   $��2,0,0,0q�6J   5;         Content. 5=      )  #Telerik.Windows.Controls.RadDocking  -   5          Name$ 
radDocking��5=   !      SerializationTag$ 
RadDocking��5>   !      AllowUnsafeMode$
 True��5@   !      HasDocumentHost$ False��5A   !      PanesSource 5B   !   �� *Panes+$ PropertyChanged=� �� NotifyOnSourceUpdated$
 True�� �� Mode$ TwoWay=� � DataContext 5C   !   ��    DockingPanesFactory 5D      /   )Visualizer.Util.CustomDockingPanesFactory  5E      .@�5G      0  *Telerik.Windows.Controls.RadSplitContainer     InitialPosition$ 
DockedLeft=�6,   +  %Telerik.Windows.Controls.DockingPanel   InitialSize$ 300,600��6I   .@�5H      +	  %Telerik.Windows.Controls.RadPaneGroup	  -   5        $��	leftGroup��5H   +   $ 	leftGroup��6<   ;
  5Telerik.Windows.Controls.TabControl.RadTabControlBase 
  IsContentPreserved$
 True��6l    5I   '   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType"   �� Path$ DataContext�5J      5K        5L      $ DockedRight=�6,   .@�5M      	  -	   5         	 Name$ 
rightGroup��5M   +   $ 
rightGroup��6?   $
 True��6p    5N   +   ��  �� $ FindAncestor=�"  $ DataContext�5O      5P        5Q      $ DockedBottom=�6,   .@�5R      	  -
   5        $ bottomGroup��5R   +   $ bottomGroup��6@   $
 True��6r    5S   +   ��  �� $ FindAncestor=�"  $ DataContext�5T      5U      5V      5W      5X      5Y      