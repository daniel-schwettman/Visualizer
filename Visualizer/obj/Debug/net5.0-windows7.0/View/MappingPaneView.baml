   M S B A M L   `   `   `  ����E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  5      d `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF 5      %    Visualizer.View.MappingPaneView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:Visualizer.View     5      f bTelerik.Windows.Controls, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Mtelerik1http://schemas.telerik.com/2008/xaml/presentation   	 
     5      hmRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF   	 
     5      -   5          � 	Resources  5         BoolToVisConverter��%   &	        �� 5   
   6'   5      .��5      �� .��5   
   �  � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��1.5* �6#   .+�5      � $��0��6    � RowDefinitions 5      �� 5      $��60 �6$   �� 5      $��8* �6$   .+�5      �� $��0��6   $��2��6*    �� Orientation$ Vertical=�6>   $��0,0,0,0q�6U   .+�5      �� $ 
Horizontal=�6!   $��0,0,0,0q�6:   .+�5      �� $��17(�6#   $��	11,30,0,0q�61    ^� ShowDuration$ 12000��6�    d� 	IsChecked 6D   �� *IsGeoMappingEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� � ToolTip 5      _� 5   "   $N��HWhen enabled, show all tags within the selected range of a selected tag.��5       _� 	StaysOpen$
 True��6q   y�5   *   �� *ShowToolTips+$ PropertyChanged=�   BooleanToVisibilityConverter	 �� 	Converter#	 [ 5   q   .��5!      �� $��Enable Geomapping��6(   $��3,0,0,0q�6A   5"      *  $Telerik.Windows.Controls.RadComboBox  5#      $��	20,30,0,0q�6.   $��85��6A   $��20��6L   $ 12000��5$   �   y�5#   X   �� *IsGeoMappingEnabled+#	 [  $ PropertyChanged=���5$      �� *GeoMappingDistances+
 �� SelectedItem
 6K   �� *SelectedGeoMappingDistance+$ PropertyChanged=� 5%      _� 5&   "   $?��9The range used for geomapping tags near the selected tag.��5'      $
 True��6b   y�5&   *   �� *ShowToolTips+$ PropertyChanged=�#	 [ 5'   b   5)      5*      5+      ,  &Telerik.Windows.Controls.RadTabControl  5,      $��1��6(    �� SelectedIndex 65   �� *SelectedTabIndex+.@�5-      )  #Telerik.Windows.Controls.RadTabItem  $��Current Tags��6)   .��5.      �� $ Vertical=�6%   .+�5/      * 
 $Telerik.Windows.Controls.RadGridView  -   5          Name$ TagGridView��5/   2   ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl&   FrozenColumnsSplitterVisibility$ Hidden=�6�   *  $Telerik.Windows.Controls.DataControl$   IsSynchronizedWithCurrentItem$ FalseY�6�      AutoGenerateColumns$ False��6�      RowIndicatorVisibility$ 	Collapsed=�50      $��10q�6A   $��16(�6M   8 
 2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��6[   $	��500��6p      SelectionMode$ Extended=�51      *   #IsPropertyChangedAggregationEnabled$ False��67   $
��Auto=�6c   $
��Auto=�6�      
IsReadOnly$
 True��52      $��1��60   4  .Telerik.Windows.Persistence.PersistenceManager   	StorageId$ currentTagGrid��53         ItemsSource 5/   G   �� *Tags+$ PropertyChanged=�   SelectedItem 6�   �� *SelectedTag+ � ContextMenu 54   "   �� 55   &   .@�56   *   m� $��Reports��63   $��10,0,0,0q�6D   
���   �.@�57   .   m� -   5        $��Filtered Report��57   7   $��10,0,0,0q�6v   
���   �6�   m� 58   .   $��Selected Report��67   $��10,0,0,0q�6�   
���   � m� Command 6P   �� *!GenerateCurrentTagReportCommand+6�   59   +   5:   '      Columns 5<   "   1	 
 +Telerik.Windows.Controls.GridViewDataColumn	  5=   &   6
 
 0Telerik.Windows.Controls.GridViewBoundColumnBase 
  Header$ ID��6�   - 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 150 6�    
  DataMemberBinding 6A   �� *Id+$ PropertyChanged=�6�   	  5>   &   $
 Name��6�   $	 150 6�    6A   �� *Name+$ PropertyChanged=�6�   	  5?   &   $ Updated On Server��6�   $	 200 6�    6A   �� *LastUpdatedOnServer+$ PropertyChanged=�6�   	  5@   &   $ 	Longitude��6�   $	 120 6�    6A   �� *	Longitude+$ PropertyChanged=�6�   	  5A   &   $ Latitude��6�   $	 120 6�    6A   �� *
Latitude+$ PropertyChanged=�6�   	  5B   &   $
 Rssi��6�   $ 70 6�    6A   �� *
MZone1Rssi+$ PropertyChanged=�6�   	  5C   &   $ 	Reader Id��6�   $	 140 6�    6A   �� *
ReaderId+$ PropertyChanged=�6�   5E        5F      -   5        $ TagGridView2��5F   2   $
 True��6�   $��2��6�   $ Hidden=�5H   '   $ FalseY�6Q   $ False��6w   $ 	Collapsed=�5I   '   $��10q�6J   $��16(�6V   $ 2��6d   $	��320��6y   $ False��5K      $
��Auto=�6J   $
��Auto=�6|    5F   H   �� *GeoMappedTags+ 6n   �� *SelectedMappedTag+y�5G   '   �� *IsGeoMappingEnabled+#	 [  $ PropertyChanged=� 5L   "   	  5M   &   $ ID��6�   $	 150 6�    6A   �� *Id+$ PropertyChanged=�6�   	  5N   &   $
 Name��6�   $	 150 6�    6A   �� *Name+$ PropertyChanged=�6�   	  5O   &   $ Updated On Server��6�   $	 200 6�    6A   �� *LastUpdatedOnServer+$ PropertyChanged=�6�   	  5P   &   $ 	Longitude��6�   $	 120 6�    6A   �� *	Longitude+$ PropertyChanged=�6�   	  5Q   &   $ Latitude��6�   $	 120 6�    6A   �� *
Latitude+$ PropertyChanged=�6�   	  5R   &   $
 Rssi��6�   $ 70 6�    6A   �� *
MZone1Rssi+$ PropertyChanged=�6�   	  5S   &   $ 	Reader Id��6�   $	 140 6�    6A   �� *
ReaderId+$ PropertyChanged=�6�   5U      5V      5W        5X      $��Tag History��6)   .��5Y      �� $ Vertical=�6%   .+�5Z        -   5        $ TagHistoryGridView��5Z   2   $ Hidden=�6�   $ FalseY�6�   $ False��6  $ 	Collapsed=�5[      $��10q�6A   $��16(�6M   $ 2��6[   $	��500��6p   $ tagHistoryGrid��5\   .   $ Extended=�5]      $ False��67   $
��Auto=�6c   $
��Auto=�6�   $
 True��5^      $��1��60    5Z   N   �� *HistoryTags+$ PropertyChanged=� 6�   �� *SelectedHistoryTag+ 5_   "   	  5`   &   $ ID��6�   $	 150 6�    6A   �� *Id+$ PropertyChanged=�6�   	  5a   &   $
 Name��6�   $	 150 6�    6A   �� *Name+$ PropertyChanged=�6�   	  5b   &   $ Updated On Server��6�   $	 200 6�    6A   �� *LastUpdatedOnServer+$ PropertyChanged=�6�   	  5c   &   $ 	Longitude��6�   $	 120 6�    6A   �� *	Longitude+$ PropertyChanged=�6�   	  5d   &   $ Latitude��6�   $	 120 6�    6A   �� *
Latitude+$ PropertyChanged=�6�   	  5e   &   $
 Rssi��6�   $ 70 6�    6A   �� *
MZone1Rssi+$ PropertyChanged=�6�   	  5f   &   $ 	Reader Id��6�   $	 140 6�    6A   �� *
ReaderId+$ PropertyChanged=�6�   	  5g   &   $ Current MicroZone��6�   $	 140 6�    6A   �� *MZone1+$ PropertyChanged=�6�   	  5h   &   $ Previous MicroZone��6�   $	 140 6�    6A   �� *MZone2+$ PropertyChanged=�6�    5j   "   �� 5k   &   .@�5l   *   m� $��Reports��63   $��10,0,0,0q�6D   
���   �.@�5m   .   m� -   5        $��Filtered Report��5m   7   $��10,0,0,0q�6u   
���   �6�   m� 5n   .   $��Selected Report��67   $��10,0,0,0q�6�   
���   � 6P   �� *!GenerateTagHistoryReportCommand+6�   5o   +   5p   '   5r        5s      -   5        $ TagHistoryGridView2��5s   2   $
 True��6�   $��2��6�   $ Hidden=�5u   '   $ FalseY�6Q   $ False��6w   $	��320��6�   $ 	Collapsed=�5v   '   $��10q�6J   $��16(�6V   $ 2��6d   $ False��5x      $
��Auto=�6J   $
��Auto=�6|    5s   O   �� *GeoMappedHistoryTags+ 6|   �� *SelectedMappedHistoryTag+y�5t   '   �� *IsGeoMappingEnabled+#	 [  $ PropertyChanged=� 5y   "   	  5z   &   $ ID��6�   $	 150 6�    6A   �� *Id+$ PropertyChanged=�6�   	  5{   &   $
 Name��6�   $	 150 6�    6A   �� *Name+$ PropertyChanged=�6�   	  5|   &   $ Updated On Server��6�   $	 200 6�    6A   �� *LastUpdatedOnServer+$ PropertyChanged=�6�   	  5}   &   $ 	Longitude��6�   $	 120 6�    6A   �� *	Longitude+$ PropertyChanged=�6�   	  5~   &   $ Latitude��6�   $	 120 6�    6A   �� *
Latitude+$ PropertyChanged=�6�   	  5   &   $
 Rssi��6�   $ 70 6�    6A   �� *
MZone1Rssi+$ PropertyChanged=�6�   	  5�   &   $ 	Reader Id��6�   $	 140 6�    6A   �� *
ReaderId+$ PropertyChanged=�6�   	  5�   &   $ Current MicroZone��6�   $	 140 6�    6A   �� *
MZone1Name+$ PropertyChanged=�6�   	  5�   &   $ Previous MicroZone��6�   $	 140 6�    6A   �� *
MZone2Name+$ PropertyChanged=�6�   5�      5�      5�        5�      $��Assets��6)   .��5�      �� $ Vertical=�6%   .+�5�        -   5        $ AssetGridView��5�   2   $ Hidden=�6�   $ FalseY�6�   $ False��6  $ 	Collapsed=�5�      $��10q�6A   $��16(�6M   $ 2��6[   $	��500��6p   $ Extended=�5�      $ False��67   $
��Auto=�6c   $
��Auto=�6�   $
 True��5�      $��1��60    5�   I   �� *Assets+$ PropertyChanged=� 6�   �� *SelectedAsset+ 5�   "   	  5�   &   $ ID��6�   $	 150 6�    6A   �� *	AssetId+$ PropertyChanged=�6�   	  5�   &   $
 Name��6�   $	 200 6�    6A   �� *Name+$ PropertyChanged=�6�   	  5�   &   $ 	Is Active��6�   $	 100 6�    6A   �� *
IsActive+$ PropertyChanged=�6�   	  5�   &   $ Asset Identifier��6�   $	 200 6�    6A   �� *AssetIdentifier+$ PropertyChanged=�6�   5�      5�      5�      5�      5�      � 5�      $��1��6    5�      �� 5�      .+�5�      � $��0��6   $��Center=�6$     5�       �� 
TargetType �� Property  �� Value��%�   '��       �� 5�      " ��6    �5�      �� 5�   "    � ��$  Center=�6F   �� 5�   "    � ��$  16(�6=   �� 5�   "    � ��  �	#FF0000006?   5�      5�      5�      �� 5�      $��0��6   $��
0,70,10,10q�6&   
���   �$��1q�6N   ._�5�      '  !Microsoft.Maps.MapControl.WPF.Map  -	   5        $��MapView��5�      +  %Microsoft.Maps.MapControl.WPF.MapCore!   CredentialsProviderT  NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F! @AsSN9EZQPu2jmToMf1e-nu691OHhxEzcdG7zaVEB8tQthLtvOTpXV-MNYUwprjWQ 6+   "   	ZoomLevel" 6�   �� *	ZoomLevel+# �� Mode$# TwoWay=�5�      5�      5�      5�      5�      