   M S B A M L   `   `   `  ����E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  5      f bTelerik.Windows.Controls, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�Zclr-namespace:Telerik.Windows.Controls.TransitionEffects;assembly=Telerik.Windows.Controls*Telerik.Windows.Controls.TransitionEffects 5      1    +Visualizer.View.ApplicationSettingsPaneView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:Visualizer.View     5      ytelerikTransitionsZclr-namespace:Telerik.Windows.Controls.TransitionEffects;assembly=Telerik.Windows.Controls     5      n jTelerik.Windows.Controls.Docking, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.Controls.FileDialogs, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o	 kTelerik.Windows.Controls.GridView, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l
 hTelerik.Windows.Controls.Input, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2025.1.211.462, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Mtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
    5      .��5
      �   � ColumnDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   *  $Telerik.Windows.Controls.RadExpander  -   5          Name$ SettingsPanel��5   B   $��0��6      ExpandDirection$
 Down=�6+   $��
20,20,20,0q�6Y   ��5      �� 5      $��Application Settings��6   $��20��68   $
��Left=�6D   $��14(�6_   ��5      � 5      $��	25,10,0,0q�6    5      �� 5      $��* �6(   �� 5      $��* �6(   �� 5      $��* �6(   �� 5       $��* �6(   �� 5!      $��* �6(   �� 5"      $��* �6(   �� 5#      $��* �6(   �� 5$      $��* �6(   .+�5&      �� $��0��6!   $��0��6.    �� Orientation$ Vertical=�6>   $��0,10,0,0q�6U   .+�5'      �� $ 
Horizontal=�6%   .+�5(      �� $��Server:��6(   $��Center=�67    ^� ShowDuration$ 12000��6R    � ToolTip 5)   "   _� 5*   &   $B��<The url for the Azure server where data will be loaded from.��5+       _� 	StaysOpen$
 True��6e   y�5*   .   �� *ShowToolTips+ �� UpdateSourceTrigger$ PropertyChanged=�    BooleanToVisibilityConverter	 �� 	Converter#	 [  5+   e   5-      �� 5.      $��Center=�6k   $	��220��6�   $��20��6�   $��96,0,0,0q�6�   ��6&   �� 
 �� Path$
 	ServerUrl�$ PropertyChanged=�6�   5/      50      �� 51      $��1��6!   $��0��6.   $ Vertical=�6>   $��0,10,0,0q�6U   .+�52      �� $ 
Horizontal=�6%   .+�53      �� $��Sync Interval (sec):��6(   $��Center=�6D   $ 12000��6_    54   "   _� 55   &   $����The amount of time, in seconds, between pulling data from the server. Note: A more frequent sync interval will provide more accurate results at the cost of increased load times.��56      $
 True��6�   y�55   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  56   �   58      �� 59      $��Center=�6n   $��60��6�   $��20��6�   $��26,0,0,0q�6�   ��6&   �� $
 SyncInterval�$ PropertyChanged=�6�   5:      5;      �� 5<      $��2��6!   $��0��6.   $ Vertical=�6>   $��0,10,0,0q�6U   .+�5=      �� $ 
Horizontal=�6%   .+�5>      �� $��	Dark Mode��6(   $��Center=�69   $ 12000��6T    5?   "   _� 5@   &   $+��%Toggle for Dark/Light Mode UI themes.��5A      $
 True��6N   y�5@   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  5A   N   5C      4  .Telerik.Windows.Controls.RadToggleSwitchButton  5D      $��Center=�6�   $��80,0,0,0q�6�    d� 	IsChecked 6<   �� $
 
IsDarkMode�$ PropertyChanged=�6�   5E      5F      �� 5G      $��3��6!   $��0��6.   $ Vertical=�6>   $��0,10,0,0q�6U   .+�5H      �� $ 
Horizontal=�6%   .+�5I      �� $��Show Tooltips��6(   $��Center=�6=   $ 12000��6X    5J   "   _� 5K   &   $A��;Show tool tips when hovering over items in the application.��5L      $
 True��6d   y�5K   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  5L   d   5N        5O      $��Center=�6�   $��61,0,0,0q�6�    6<   �� $
 ShowToolTips�$ PropertyChanged=�6�   5P      5Q      �� 5R      $��4��6!   $��0��6.   $ Vertical=�6>   $��0,10,0,0q�6U   .+�5S      �� $ 
Horizontal=�6%   .+�5T      �� $��Show Reporting��6(   $��Center=�6>   $ 12000��6Y    5U   "   _� 5V   &   $-��'Show Reporting Pane in Navigation Menu.��5W      $
 True��6P   y�5V   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  5W   P   5Y        5Z      $��Center=�6�   $��52,0,0,0q�6�    6<   �� $
 ShowReporting�$ PropertyChanged=�6�   5[      �� 5\      $ 
Horizontal=�6%   $��0,10,0,0q�6>   .+�5]      �� $��Show Inventory��6(   $��Center=�6>   $ 12000��6Y    5^   "   _� 5_   &   $-��'Show Inventory Pane in Navigation Menu.��5`      $
 True��6P   y�5_   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  5`   P   5b        5c      $��Center=�6�   $��55,0,0,0q�6�    6<   �� $
 ShowInventory�$ PropertyChanged=�6�   5d      �� 5e      $ 
Horizontal=�6%   $��0,10,0,0q�6>   .+�5f      �� $��Show Mapping��6(   $��Center=�6<   $ 12000��6W    5g   "   _� 5h   &   $/��)Show GPS Mapping Pane in Navigation Menu.��5i      $
 True��6R   y�5h   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  5i   R   5k        5l      $��Center=�6�   $��57,0,0,0q�6�    6<   �� $
 ShowMapping�$ PropertyChanged=�6�   5m      �� 5n      $ 
Horizontal=�6%   $��0,10,0,0q�6>   .+�5o      �� $��Show Assets��6(   $��Center=�6;   $ 12000��6V    5p   "   _� 5q   &   $4��.Show Asset Management Pane in Navigation Menu.��5r      $
 True��6W   y�5q   .   �� *ShowToolTips+$ PropertyChanged=�#	 [  5r   W   5t        5u      $��Center=�6�   $��69,0,0,0q�6�    6<   �� $
 	ShowAsset�$ PropertyChanged=�6�   5v      5w      (  "Telerik.Windows.Controls.RadButton  5x      $��5��6(   $��30��65   $	��120��6A   $��Save Settings��6M   $��2��6e   $��0,30,0,0q�6�   ��6y   �� *SaveSettingsCommand+6�   5y      9  3Telerik.Windows.Controls.Animation.AnimationManager   AnimationSelector 5{      :  4Telerik.Windows.Controls.Animation.AnimationSelector  5|         
Animations. 5}      H  BTelerik.Windows.Controls.Animation.ExpanderExpandCollapseAnimation  5  /Telerik.Windows.Controls.Animation.RadAnimation   AnimationName$ ExpandHorizontal��6>   @  :Telerik.Windows.Controls.Animation.ExpandCollapseAnimation   	Direction$ In=�5~   6      
SpeedRatio$ 2Q�5   6      TargetElementName$ Content��5�   6     5�      $ CollapseHorizontal��6>   $	 Out=�5�   6   $ 2Q�5�   6   $ Content��5�   6   5�      5�      5�      5�      