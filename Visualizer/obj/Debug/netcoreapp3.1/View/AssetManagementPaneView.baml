   M S B A M L   `   `   `  ����E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  5      -    'Visualizer.View.AssetManagementPaneView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:Visualizer.View     5      f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	    	 
     5      .��5
      �   � RowDefinitions  5   
   �� 5      $��80 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   $
��Left=�6;   $��
16,60,0,15q�6W   $��2��6k   $��0��6|   .+�5      �� $��Showing��6   $��15(�6'   �� 5      $��5,0,0,0q�6_   $��15(�6p   ��6   �� *TagFilteredCount+ �� UpdateSourceTrigger$ PropertyChanged=�6p   �� 5      $��of��6   $��5,0,0,0q�6"   $��15(�63   �� 5      $��5,0,0,0q�6\   $��15(�6m   ��6   �� *TagTotalCount+$ PropertyChanged=�6m   �� 5      $
��Tags��6   $��5,0,0,0q�6$   $��15(�65   5      * 
 $Telerik.Windows.Controls.RadGridView  5   
   -   5          Name$ TagGridView��5      $��0��63   $��1��6C   ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl&   FrozenColumnsSplitterVisibility$ Hidden=�5      *  $Telerik.Windows.Controls.DataControl$   IsSynchronizedWithCurrentItem$ FalseY�6H      AutoGenerateColumns$ False��6n      RowIndicatorVisibility$ 	Collapsed=�5   7   $��10q�6Z   $��16(�6f   8 
 2Telerik.Windows.Controls.GridView.BaseItemsControl	   AlternationCount$	 2��6t   *
   #IsPropertyChangedAggregationEnabled$
 False��5      $
��Auto=�6J   $
��Auto=�6|      
IsReadOnly$
 True��5         SelectionMode$ Extended=�61   #   EnableLostFocusSelectedState$ False��6J      ItemsSource 5   P   �� *TagCollection+$ PropertyChanged=� �� NotifyOnSourceUpdated$
 True�� �� Mode$ OneWay=�   Columns 5      1 
 +Telerik.Windows.Controls.GridViewDataColumn  5       6 
 0Telerik.Windows.Controls.GridViewBoundColumnBase   Header$ Tag Name��6y   - 
 'Telerik.Windows.Controls.GridViewColumn   Width? 
 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$	 142 6�      DataMemberBinding 61   �� *Name+$ PropertyChanged=�6�     5!      $ Tag ID��6w   $	 175 6�    61   �� *Id+$ PropertyChanged=�6�     5"      $ Updated On Server��6�   $	 200 6�    61   �� *LastUpdatedOnServer+$ PropertyChanged=�6�     5#      $ Latitude��6}   $	 150 6�    61   �� *
Latitude+$ PropertyChanged=�6�     5$      $ 	Longitude��6z   $	 150 6�    6-   �� *	Longitude+$ PropertyChanged=�6�     5%      $  Current MZone��6w   $	 150 6�    6-   �� *MZone1+$ PropertyChanged=�6�     5&      $ Previous MZone��6{   $	 150 6�    61   �� *MZone2+$ PropertyChanged=�6�     5'      $ 	Reader ID��6}   $	 200 6�    61   �� *
ReaderId+$ PropertyChanged=�6�   5)      (	  "Telerik.Windows.Controls.RadButton	  5*   
   -   5        $��Generate Report��5*      $	��110��6`   $��30��6l   $��2��6x    ^� ShowDuration$ 12000��5+   7   $	��Top=�6[   $��Right=�6s   $��	0,55,10,0q�6�   ��5*   6   �� *GenerateReportCommand+ � ToolTip 5,      _� 5-      $7��1Export the current datagrid to selected location.��5.       _� 	StaysOpen$
 True��6Z   y�5-      �� *ShowToolTips+$ PropertyChanged=�    BooleanToVisibilityConverter �� 	Converter# [  5.   Z   50      �� 51   
   $ 
Horizontal=�6   $��0��6.   $
��Left=�6;   $��
16,60,0,15q�6W   $��2��6k   $��1��6|   .+�52      �� $��Showing��6   $��15(�6'   �� 53      $��5,0,0,0q�6a   $��15(�6r   ��6   �� *AssetFilteredCount+$ PropertyChanged=�6r   �� 54      $��of��6   $��5,0,0,0q�6"   $��15(�63   �� 55      $��5,0,0,0q�6^   $��15(�6o   ��6   �� *AssetTotalCount+$ PropertyChanged=�6o   �� 56      $��Assets��6   $��5,0,0,0q�6&   $��15(�67   57        58   
   -   5        $ AssetGridView��58      $��1��65   $��1��6E   $ Hidden=�59      $ FalseY�6H   $ False��6n   $ 	Collapsed=�5:   8   $��10q�6[   $��16(�6g   $	 2��6u   $
 False��5;      $
��Auto=�6J   $
��Auto=�6|   $ False��5<      $ Extended=�61   $ False��6J    58   R   �� *AssetCollection+$ PropertyChanged=�$
 True��$ OneWay=� � ContextMenu 5=      �� 5>      .@�5?      m� $��	Add Asset��6    m� Command 62   �� *AddAssetCommand+m� 5@      $��
Edit Asset��6    63   �� *EditAssetCommand+m� 5A      $��Delete Asset��6    65   �� *UnpairAssetCommand+5B       5D        5E      $
 Name��6u   $	 240 6�    6-   �� *Name+$ PropertyChanged=�6�     5F      $ 	Is Active��6y   $	 100 6�    6-   �� *
IsActive+$ PropertyChanged=�6�     5G      $ Asset Identifier��6�   $	 250 6�    6-   �� *AssetIdentifier+$ PropertyChanged=�6�   5I      5J      5K      