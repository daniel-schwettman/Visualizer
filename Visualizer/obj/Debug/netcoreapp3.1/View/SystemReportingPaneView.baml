   M S B A M L   `   `   `  ����E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Visualizer.ViewModelVisualizer.ViewModel  5      1clr-namespace:Visualizer.ViewVisualizer.View  5      -    'Visualizer.View.SystemReportingPaneView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1vm"clr-namespace:Visualizer.ViewModel     5      f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	    	 
     5      9localclr-namespace:Visualizer.View	    	 
     5      .��5
      �   � RowDefinitions  5   
   �� 5      $��80 �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5      (  "Telerik.Windows.Controls.RadButton  $��0��6    $��	Sync Data��6-   $��80��6A   $��0, 0, 10, 0q�6L   $��30��6�    ^� ShowDuration$ 12000��6�   ��6a   �� *SyncFeedCommand+ � ToolTip 5      _� 5      $&�� Force sync data with the server.��5       _� 	StaysOpen$
 True��6I   y�5      �� *ShowToolTips+ �� UpdateSourceTrigger$ PropertyChanged=�    BooleanToVisibilityConverter �� 	Converter# [  5   I   5        5      $��0��6    $��Delete All Data��6-   $	��100��6G   $��0, 0, 10, 0q�6S   $��30��6�   $ 12000��6�   ��6h   �� *DeleteDataCommand+ 5      _� 5      $$��Delete all data on the server.��5      $
 True��6G   y�5      �� *ShowToolTips+$ PropertyChanged=�# [  5   G   5      5      �� 5   
   $ 
Horizontal=�6   $��0��6.   $
��Left=�6;   $��
16,60,0,15q�6W   $��2��6k   .+�5      �� $��Showing��6   $��15(�6'   �� 5      $��5,0,0,0q�6\   $��15(�6m   ��6   �� *FilteredCount+$ PropertyChanged=�6m   �� 5       $��of��6   $��5,0,0,0q�6"   $��15(�63   �� 5!      $��5,0,0,0q�6Y   $��15(�6j   ��6   �� *
TotalCount+$ PropertyChanged=�6j   �� 5"      $
��Tags��6   $��5,0,0,0q�6$   $��15(�65   5#      * 
 $Telerik.Windows.Controls.RadGridView  5$   
   -   5          Name$ TagsGrid��5$      $��1��60   ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl&   FrozenColumnsSplitterVisibility$ Hidden=�5%      *  $Telerik.Windows.Controls.DataControl$	   IsSynchronizedWithCurrentItem$	 FalseY�6H   
   AutoGenerateColumns$
 False��6n      RowIndicatorVisibility$ 	Collapsed=�5&   7   $��10q�6Z   $��16(�6f   8 
 2Telerik.Windows.Controls.GridView.BaseItemsControl   AlternationCount$ 2��6t      SelectionMode$ Extended=�6�   *   #IsPropertyChangedAggregationEnabled$ False��5'      $
��Auto=�6J   $
��Auto=�6|      
IsReadOnly$
 True��5(         GroupRenderMode$
 Flat=�6]      ShowGroupPanel$ False��6t      SelectionUnit$ FullRow=�6�   4  .Telerik.Windows.Persistence.PersistenceManager   	StorageId$ tagGrid��5)         ItemsSource 5$   =   �� *Tags+$ PropertyChanged=� �� NotifyOnSourceUpdated$
 True�� �� Mode$ OneWay=� � ContextMenu 5*      �� 5+      .@�5,      m� $��Reports��6   $��10,0,0,0q�60   
���   �.@�5-      m� -   5        $��Filtered Report��5-   #   $��10,0,0,0q�6W   
���   �6i   m� 5.      $��Selected Report��6#   $��10,0,0,0q�6n   
���   � m� Command 6<   �� *GenerateSelectedReportCommand+6�   5/      50         Columns 52      1 
 +Telerik.Windows.Controls.GridViewDataColumn  53      6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Tag Name��6u   -	 
 'Telerik.Windows.Controls.GridViewColumn 	  Width?
 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 142
 6�      DataMemberBinding 6-   �� *Name+$ PropertyChanged=�6�     54      $ Tag ID��6s   $	 175
 6�    6-   �� *Id+$ PropertyChanged=�6�     55      $ Updated On Server��6�   $	 200
 6�    6-   �� *LastUpdatedOnServer+$ PropertyChanged=�6�     56      $ Latitude��6y   $	 150
 6�    6-   �� *
Latitude+$ PropertyChanged=�6�     57      $ 	Longitude��6z   $	 150
 6�    6-   �� *	Longitude+$ PropertyChanged=�6�     58      $ Temperature��6�   $	 150
 6�    6-   �� *Temperature+$ PropertyChanged=� �� StringFormat$  ��6�     59      $ Humidity��6y   $	 150
 6�    6-   �� *
Humidity+$ PropertyChanged=�6�     5:      $ 	Operation��6z   $	 150
 6�    6-   �� *	Operation+$ PropertyChanged=�6�     5<      $	 120
 6{   $ 12000��6�    6-   �� *
StatusCode+$ PropertyChanged=� 5=      �� 5>      $��Status Code��6$   $ 12000��67    5?      �� 5@   "   $��10,0,0,0q�6,   0 - Stationary no mzone�� 5B      )'10 - Stationary no mzone Button Pressed�� 5D      20 - Stationary with mzone�� 5F      +)30 - Stationary with mzone Button Pressed�� 5H      40 - In Motion no mzone�� 5J      (&50 - In Motion no mzone Button Pressed�� 5L      60 - In Motion with mzone�� 5N      *(70 - In Motion with mzone Button Pressed�� 5P      80 - Double Tap no mzone�� 5R      !90 - Double Tap no mzone Button�� 5T      A0 - Double Tap with mzone�� 5V      )'B0 - Double Tap with mzone Button Press�� 5X      5Y   #   5[      5]        5^      $ Battery��6x   $	 120
 6�    6-   �� *	Battery+$ PropertyChanged=�6�     5_      $
 Rssi��6{   $	 120
 6�    6-   �� *
MZone1Rssi+$ PropertyChanged=�6�     5g      $  Current MZone��6w   $	 150
 6�    6-   �� *MZone1+$ PropertyChanged=�6�     5h      $ Previous MZone��6w   $	 150
 6�    6-   �� *MZone2+$ PropertyChanged=�6�     5i      $ Sequence Number��6   $	 150
 6�    6-   �� *SequenceNumber+$ PropertyChanged=�6�     5j      $ 	Reader ID��6y   $	 200
 6�    6-   �� *
ReaderId+$ PropertyChanged=�6�   5l      5m      5n      