   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  f bTelerik.Windows.Controls, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�Zclr-namespace:Telerik.Windows.Controls.TransitionEffects;assembly=Telerik.Windows.Controls*Telerik.Windows.Controls.TransitionEffects 1    +Visualizer.View.ApplicationSettingsPaneView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:Visualizer.View     ytelerikTransitionsZclr-namespace:Telerik.Windows.Controls.TransitionEffects;assembly=Telerik.Windows.Controls     k gTelerik.Windows.Controls.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Docking, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r	 nTelerik.Windows.Controls.FileDialogs, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2022.1.222.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.PersistenceFramework, Version=2021.1.119.310, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Otelerik1http://schemas.telerik.com/2008/xaml/presentation	    	 
     .���   � ColumnDefinitions  �� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+�*  $Telerik.Windows.Controls.RadExpander  -     Name$ SettingsPanel��$��0��   ExpandDirection$
 Down=�$��
20,20,20,0q����� $��Application Settings��$��20��$
��Left=�$��14(���� $��	25,10,0,0q� �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��0�� �� Orientation$ Vertical=�$��0,10,0,0q�.+��� $ 
Horizontal=�.+��� $��Server:��$��Center=� ^� ShowDuration$ 12000�� � ToolTip _� $B��<The url for the Azure server where data will be loaded from.�� _� 	StaysOpen$
 True��y��� *ShowToolTips+ �� UpdateSourceTrigger$ PropertyChanged=�    BooleanToVisibilityConverter	 �� 	Converter#	 [  �� $��Center=�$	��220��$��20��$��96,0,0,0q����� 
 �� Path$
 	ServerUrl�$ PropertyChanged=��� $��1��$��0��$ Vertical=�$��0,10,0,0q�.+��� $ 
Horizontal=�.+��� $��Sync Interval (sec):��$��Center=�$ 12000�� _� $����The amount of time, in seconds, between pulling data from the server. Note: A more frequent sync interval will provide more accurate results at the cost of increased load times.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [  �� $��Center=�$��60��$��20��$��26,0,0,0q����� $
 SyncInterval�$ PropertyChanged=��� $��2��$��0��$ Vertical=�$��0,10,0,0q�.+��� $ 
Horizontal=�.+��� $��	Dark Mode��$��Center=�$ 12000�� _� $+��%Toggle for Dark/Light Mode UI themes.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [  4  .Telerik.Windows.Controls.RadToggleSwitchButton  $��Center=�$��80,0,0,0q� d� 	IsChecked �� $
 
IsDarkMode�$ PropertyChanged=��� $��3��$��0��$ Vertical=�$��0,10,0,0q�.+��� $ 
Horizontal=�.+��� $��Show Tooltips��$��Center=�$ 12000�� _� $A��;Show tool tips when hovering over items in the application.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [    $��Center=�$��61,0,0,0q� �� $
 ShowToolTips�$ PropertyChanged=��� $��4��$��0��$ Vertical=�$��0,10,0,0q�.+��� $ 
Horizontal=�.+��� $��Show Reporting��$��Center=�$ 12000�� _� $-��'Show Reporting Pane in Navigation Menu.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [    $��Center=�$��52,0,0,0q� �� $
 ShowReporting�$ PropertyChanged=��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Show Inventory��$��Center=�$ 12000�� _� $-��'Show Inventory Pane in Navigation Menu.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [    $��Center=�$��55,0,0,0q� �� $
 ShowInventory�$ PropertyChanged=��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Show Mapping��$��Center=�$ 12000�� _� $/��)Show GPS Mapping Pane in Navigation Menu.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [    $��Center=�$��57,0,0,0q� �� $
 ShowMapping�$ PropertyChanged=��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��Show Assets��$��Center=�$ 12000�� _� $4��.Show Asset Management Pane in Navigation Menu.��$
 True��y��� *ShowToolTips+$ PropertyChanged=�#	 [    $��Center=�$��69,0,0,0q� �� $
 	ShowAsset�$ PropertyChanged=�(  "Telerik.Windows.Controls.RadButton  $��5��$��30��$	��120��$��Save Settings��$��2��$��0,30,0,0q����� *SaveSettingsCommand+9  3Telerik.Windows.Controls.Animation.AnimationManager   AnimationSelector :  4Telerik.Windows.Controls.Animation.AnimationSelector     
Animations. H  BTelerik.Windows.Controls.Animation.ExpanderExpandCollapseAnimation  5  /Telerik.Windows.Controls.Animation.RadAnimation   AnimationName$ ExpandHorizontal��@  :Telerik.Windows.Controls.Animation.ExpandCollapseAnimation   	Direction$ In=�   
SpeedRatio$ 2Q�   TargetElementName$ Content��  $ CollapseHorizontal��$	 Out=�$ 2Q�$ Content��