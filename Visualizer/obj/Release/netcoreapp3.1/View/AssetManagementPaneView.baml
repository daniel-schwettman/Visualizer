   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  -    'Visualizer.View.AssetManagementPaneView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:Visualizer.View     f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	    	 
     .���   � RowDefinitions  �� $��80 ��� $��* � � ColumnDefinitions �� $��2* ��� $��* �.+���  �� Orientation$ 
Horizontal=�$��0��$
��Left=�$��
16,60,0,15q�$��2��$��0��.+��� $��Showing��$��15(��� $��5,0,0,0q�$��15(����� *TagFilteredCount+ �� UpdateSourceTrigger$ PropertyChanged=��� $��of��$��5,0,0,0q�$��15(��� $��5,0,0,0q�$��15(����� *TagTotalCount+$ PropertyChanged=��� $
��Tags��$��5,0,0,0q�$��15(�* 
 $Telerik.Windows.Controls.RadGridView  -     Name$ TagGridView��$��0��$��1��; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl&   FrozenColumnsSplitterVisibility$ Hidden=�*  $Telerik.Windows.Controls.DataControl$   IsSynchronizedWithCurrentItem$ FalseY�   AutoGenerateColumns$ False��   RowIndicatorVisibility$ 	Collapsed=�$��10q�$��16(�8 
 2Telerik.Windows.Controls.GridView.BaseItemsControl	   AlternationCount$	 2��*
   #IsPropertyChangedAggregationEnabled$
 False��$
��Auto=�$
��Auto=�   
IsReadOnly$
 True��   SelectionMode$ Extended=�#   EnableLostFocusSelectedState$ False��   ItemsSource �� *TagCollection+$ PropertyChanged=� �� NotifyOnSourceUpdated$
 True�� �� Mode$ OneWay=�   Columns 1 
 +Telerik.Windows.Controls.GridViewDataColumn  6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Tag Name��- 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 142    DataMemberBinding �� *Name+$ PropertyChanged=�  $ Tag ID��$	 175  �� *Id+$ PropertyChanged=�  $ Updated On Server��$	 200  �� *LastUpdatedOnServer+$ PropertyChanged=�  $ Latitude��$	 150  �� *
Latitude+$ PropertyChanged=�  $ 	Longitude��$	 150  �� *	Longitude+$ PropertyChanged=�  $  Current MZone��$	 150  �� *MZone1+$ PropertyChanged=�  $ Previous MZone��$	 150  �� *MZone2+$ PropertyChanged=�  $ 	Reader ID��$	 200  �� *
ReaderId+$ PropertyChanged=�(	  "Telerik.Windows.Controls.RadButton	  -   $��Generate Report��$	��110��$��30��$��2�� ^� ShowDuration$ 12000��$	��Top=�$��Right=�$��	0,55,10,0q����� *GenerateReportCommand+ � ToolTip _� $7��1Export the current datagrid to selected location.�� _� 	StaysOpen$
 True��y��� *ShowToolTips+$ PropertyChanged=�    BooleanToVisibilityConverter �� 	Converter# [  �� $ 
Horizontal=�$��0��$
��Left=�$��
16,60,0,15q�$��2��$��1��.+��� $��Showing��$��15(��� $��5,0,0,0q�$��15(����� *AssetFilteredCount+$ PropertyChanged=��� $��of��$��5,0,0,0q�$��15(��� $��5,0,0,0q�$��15(����� *AssetTotalCount+$ PropertyChanged=��� $��Assets��$��5,0,0,0q�$��15(�  -   $ AssetGridView��$��1��$��1��$ Hidden=�$ FalseY�$ False��$ 	Collapsed=�$��10q�$��16(�$	 2��$
 False��$
��Auto=�$
��Auto=�$ False��$ Extended=�$ False�� �� *AssetCollection+$ PropertyChanged=�$
 True��$ OneWay=� � ContextMenu �� .@�m� $��	Add Asset�� m� Command �� *AddAssetCommand+m� $��
Edit Asset�� �� *EditAssetCommand+m� $��Delete Asset�� �� *UnpairAssetCommand+   $
 Name��$	 240  �� *Name+$ PropertyChanged=�  $ 	Is Active��$	 100  �� *
IsActive+$ PropertyChanged=�  $ Asset Identifier��$	 250  �� *AssetIdentifier+$ PropertyChanged=�