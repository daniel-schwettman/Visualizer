   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Visualizer.ViewModelVisualizer.ViewModel  1clr-namespace:Visualizer.ViewVisualizer.View  -    'Visualizer.View.SystemReportingPaneView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     1vm"clr-namespace:Visualizer.ViewModel     f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	    	 
     9localclr-namespace:Visualizer.View	    	 
     .���   � RowDefinitions  �� $��80 ��� $��* �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Right=�.+�(  "Telerik.Windows.Controls.RadButton  $��0��$��	Sync Data��$��80��$��0, 0, 10, 0q�$��30�� ^� ShowDuration$ 12000������ *SyncFeedCommand+ � ToolTip _� $&�� Force sync data with the server.�� _� 	StaysOpen$
 True��y��� *ShowToolTips+ �� UpdateSourceTrigger$ PropertyChanged=�    BooleanToVisibilityConverter �� 	Converter# [    $��0��$��Delete All Data��$	��100��$��0, 0, 10, 0q�$��30��$ 12000������ *DeleteDataCommand+ _� $$��Delete all data on the server.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�# [  �� $ 
Horizontal=�$��0��$
��Left=�$��
16,60,0,15q�$��2��.+��� $��Showing��$��15(��� $��5,0,0,0q�$��15(����� *FilteredCount+$ PropertyChanged=��� $��of��$��5,0,0,0q�$��15(��� $��5,0,0,0q�$��15(����� *
TotalCount+$ PropertyChanged=��� $
��Tags��$��5,0,0,0q�$��15(�* 
 $Telerik.Windows.Controls.RadGridView  -     Name$ TagsGrid��$��1��; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl&   FrozenColumnsSplitterVisibility$ Hidden=�*  $Telerik.Windows.Controls.DataControl$	   IsSynchronizedWithCurrentItem$	 FalseY�
   AutoGenerateColumns$
 False��   RowIndicatorVisibility$ 	Collapsed=�$��10q�$��16(�8 
 2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��   SelectionMode$ Extended=�*   #IsPropertyChangedAggregationEnabled$ False��$
��Auto=�$
��Auto=�   
IsReadOnly$
 True��   GroupRenderMode$
 Flat=�   ShowGroupPanel$ False��   SelectionUnit$ FullRow=�4  .Telerik.Windows.Persistence.PersistenceManager   	StorageId$ tagGrid��   ItemsSource �� *Tags+$ PropertyChanged=� �� NotifyOnSourceUpdated$
 True�� �� Mode$ OneWay=� � ContextMenu �� .@�m� $��Reports��$��10,0,0,0q�
���   �.@�m� -   $��Filtered Report��$��10,0,0,0q�
���   �m� $��Selected Report��$��10,0,0,0q�
���   � m� Command �� *GenerateSelectedReportCommand+   Columns 1 
 +Telerik.Windows.Controls.GridViewDataColumn  6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Tag Name��-	 
 'Telerik.Windows.Controls.GridViewColumn 	  Width?
 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 142
    DataMemberBinding �� *Name+$ PropertyChanged=�  $ Tag ID��$	 175
  �� *Id+$ PropertyChanged=�  $ Updated On Server��$	 200
  �� *LastUpdatedOnServer+$ PropertyChanged=�  $ Latitude��$	 150
  �� *
Latitude+$ PropertyChanged=�  $ 	Longitude��$	 150
  �� *	Longitude+$ PropertyChanged=�  $ Temperature��$	 150
  �� *Temperature+$ PropertyChanged=� �� StringFormat$  ��  $ Humidity��$	 150
  �� *
Humidity+$ PropertyChanged=�  $ 	Operation��$	 150
  �� *	Operation+$ PropertyChanged=�  $	 120
 $ 12000�� �� *
StatusCode+$ PropertyChanged=� �� $��Status Code��$ 12000�� �� $��10,0,0,0q�0 - Stationary no mzone�� )'10 - Stationary no mzone Button Pressed�� 20 - Stationary with mzone�� +)30 - Stationary with mzone Button Pressed�� 40 - In Motion no mzone�� (&50 - In Motion no mzone Button Pressed�� 60 - In Motion with mzone�� *(70 - In Motion with mzone Button Pressed�� 80 - Double Tap no mzone�� !90 - Double Tap no mzone Button�� A0 - Double Tap with mzone�� )'B0 - Double Tap with mzone Button Press��   $ Battery��$	 120
  �� *	Battery+$ PropertyChanged=�  $
 Rssi��$	 120
  �� *
MZone1Rssi+$ PropertyChanged=�  $  Current MZone��$	 150
  �� *MZone1+$ PropertyChanged=�  $ Previous MZone��$	 150
  �� *MZone2+$ PropertyChanged=�  $ Sequence Number��$	 150
  �� *SequenceNumber+$ PropertyChanged=�  $ 	Reader ID��$	 200
  �� *
ReaderId+$ PropertyChanged=�