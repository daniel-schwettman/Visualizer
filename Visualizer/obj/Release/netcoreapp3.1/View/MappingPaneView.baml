   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  d `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF %    Visualizer.View.MappingPaneView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:Visualizer.View     f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r
 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	   	 
      jmRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF	   	 
      -     � 	Resources     BoolToVisConverter��%   &	        �� .���� .���  � ColumnDefinitions �� $��* ��� $
��1.5* �.+�� $��0�� � RowDefinitions �� $��60 ��� $��8* �.+��� $��0��$��2�� �� Orientation$ Vertical=�$��0,0,0,0q�.+��� $ 
Horizontal=�$��0,0,0,0q�.+��� $��17(�$��	11,30,0,0q� ^� ShowDuration$ 12000�� d� 	IsChecked �� *IsGeoMappingEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� � ToolTip _� $N��HWhen enabled, show all tags within the selected range of a selected tag.�� _� 	StaysOpen$
 True��y��� *ShowToolTips+$ PropertyChanged=�   BooleanToVisibilityConverter	 �� 	Converter#	 [ .���� $��Enable Geomapping��$��3,0,0,0q�*  $Telerik.Windows.Controls.RadComboBox  $��	20,30,0,0q�$��85��$��20��$ 12000��y��� *IsGeoMappingEnabled+#	 [  $ PropertyChanged=����� *GeoMappingDistances+
 �� SelectedItem
 �� *SelectedGeoMappingDistance+$ PropertyChanged=� _� $?��9The range used for geomapping tags near the selected tag.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [ ,  &Telerik.Windows.Controls.RadTabControl  $��1�� �� SelectedIndex �� *SelectedTabIndex+.@�)  #Telerik.Windows.Controls.RadTabItem  $��Current Tags��.���� $ Vertical=�.+�*  $Telerik.Windows.Controls.RadGridView  -     Name$ TagGridView��;  5Telerik.Windows.Controls.GridView.GridViewDataControl&   FrozenColumnsSplitterVisibility$ Hidden=�*  $Telerik.Windows.Controls.DataControl$   IsSynchronizedWithCurrentItem$ FalseY�   AutoGenerateColumns$ False��   RowIndicatorVisibility$ 	Collapsed=�$��10q�$��16(�8  2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��$	��500��   SelectionMode$ Extended=�*   #IsPropertyChangedAggregationEnabled$ False��$
��Auto=�$
��Auto=�   
IsReadOnly$
 True��$��1��4  .Telerik.Windows.Persistence.PersistenceManager   	StorageId$ currentTagGrid��   ItemsSource �� *Tags+$ PropertyChanged=�   SelectedItem �� *SelectedTag+ � ContextMenu �� .@�m� $��Reports��$��10,0,0,0q�
���   �.@�m� -   $��Filtered Report��$��10,0,0,0q�
���   �m� $��Selected Report��$��10,0,0,0q�
���   � m� Command �� *!GenerateCurrentTagReportCommand+   Columns 1	  +Telerik.Windows.Controls.GridViewDataColumn	  6
  0Telerik.Windows.Controls.GridViewBoundColumnBase 
  Header$ ID��-  'Telerik.Windows.Controls.GridViewColumn   Width?  9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 150  
  DataMemberBinding �� *Id+$ PropertyChanged=�	  $
 Name��$	 150  �� *Name+$ PropertyChanged=�	  $ Updated On Server��$	 200  �� *LastUpdatedOnServer+$ PropertyChanged=�	  $ 	Longitude��$	 120  �� *	Longitude+$ PropertyChanged=�	  $ Latitude��$	 120  �� *
Latitude+$ PropertyChanged=�	  $
 Rssi��$ 70  �� *
MZone1Rssi+$ PropertyChanged=�	  $ 	Reader Id��$	 140  �� *
ReaderId+$ PropertyChanged=�  -   $ TagGridView2��$
 True��$��2��$ Hidden=�$ FalseY�$ False��$ 	Collapsed=�$��10q�$��16(�$ 2��$	��320��$ False��$
��Auto=�$
��Auto=� �� *GeoMappedTags+ �� *SelectedMappedTag+y��� *IsGeoMappingEnabled+#	 [  $ PropertyChanged=� 	  $ ID��$	 150  �� *Id+$ PropertyChanged=�	  $
 Name��$	 150  �� *Name+$ PropertyChanged=�	  $ Updated On Server��$	 200  �� *LastUpdatedOnServer+$ PropertyChanged=�	  $ 	Longitude��$	 120  �� *	Longitude+$ PropertyChanged=�	  $ Latitude��$	 120  �� *
Latitude+$ PropertyChanged=�	  $
 Rssi��$ 70  �� *
MZone1Rssi+$ PropertyChanged=�	  $ 	Reader Id��$	 140  �� *
ReaderId+$ PropertyChanged=�  $��Tag History��.���� $ Vertical=�.+�  -   $ TagHistoryGridView��$ Hidden=�$ FalseY�$ False��$ 	Collapsed=�$��10q�$��16(�$ 2��$	��500��$ tagHistoryGrid��$ Extended=�$ False��$
��Auto=�$
��Auto=�$
 True��$��1�� �� *HistoryTags+$ PropertyChanged=� �� *SelectedHistoryTag+ 	  $ ID��$	 150  �� *Id+$ PropertyChanged=�	  $
 Name��$	 150  �� *Name+$ PropertyChanged=�	  $ Updated On Server��$	 200  �� *LastUpdatedOnServer+$ PropertyChanged=�	  $ 	Longitude��$	 120  �� *	Longitude+$ PropertyChanged=�	  $ Latitude��$	 120  �� *
Latitude+$ PropertyChanged=�	  $
 Rssi��$ 70  �� *
MZone1Rssi+$ PropertyChanged=�	  $ 	Reader Id��$	 140  �� *
ReaderId+$ PropertyChanged=�	  $ Current MicroZone��$	 140  �� *MZone1+$ PropertyChanged=�	  $ Previous MicroZone��$	 140  �� *MZone2+$ PropertyChanged=� �� .@�m� $��Reports��$��10,0,0,0q�
���   �.@�m� -   $��Filtered Report��$��10,0,0,0q�
���   �m� $��Selected Report��$��10,0,0,0q�
���   � �� *!GenerateTagHistoryReportCommand+  -   $ TagHistoryGridView2��$
 True��$��2��$ Hidden=�$ FalseY�$ False��$	��320��$ 	Collapsed=�$��10q�$��16(�$ 2��$ False��$
��Auto=�$
��Auto=� �� *GeoMappedHistoryTags+ �� *SelectedMappedHistoryTag+y��� *IsGeoMappingEnabled+#	 [  $ PropertyChanged=� 	  $ ID��$	 150  �� *Id+$ PropertyChanged=�	  $
 Name��$	 150  �� *Name+$ PropertyChanged=�	  $ Updated On Server��$	 200  �� *LastUpdatedOnServer+$ PropertyChanged=�	  $ 	Longitude��$	 120  �� *	Longitude+$ PropertyChanged=�	  $ Latitude��$	 120  �� *
Latitude+$ PropertyChanged=�	  $
 Rssi��$ 70  �� *
MZone1Rssi+$ PropertyChanged=�	  $ 	Reader Id��$	 140  �� *
ReaderId+$ PropertyChanged=�	  $ Current MicroZone��$	 140  �� *
MZone1Name+$ PropertyChanged=�	  $ Previous MicroZone��$	 140  �� *
MZone2Name+$ PropertyChanged=�  $��Assets��.���� $ Vertical=�.+�  -   $ AssetGridView��$ Hidden=�$ FalseY�$ False��$ 	Collapsed=�$��10q�$��16(�$ 2��$	��500��$ Extended=�$ False��$
��Auto=�$
��Auto=�$
 True��$��1�� �� *Assets+$ PropertyChanged=� �� *SelectedAsset+ 	  $ ID��$	 150  �� *	AssetId+$ PropertyChanged=�	  $
 Name��$	 200  �� *Name+$ PropertyChanged=�	  $ 	Is Active��$	 100  �� *
IsActive+$ PropertyChanged=�	  $ Asset Identifier��$	 200  �� *AssetIdentifier+$ PropertyChanged=�� $��1�� �� .+�� $��0��$��Center=�   �� 
TargetType �� Property  �� Value��%f   '��       �� " �����  � ��$  Center=���  � ��$  16(���  � ��  �	#FF000000�� $��0��$��
0,70,10,10q�
���   �$��1q�._�'  !Microsoft.Maps.MapControl.WPF.Map  -	   $��MapView��+  %Microsoft.Maps.MapControl.WPF.MapCore!   CredentialsProviderT  NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F! @AsSN9EZQPu2jmToMf1e-nu691OHhxEzcdG7zaVEB8tQthLtvOTpXV-MNYUwprjWQ "   	ZoomLevel" �� *	ZoomLevel+# �� Mode$# TwoWay=�