   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  &     Visualizer.View.AssetUserControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:Visualizer.View     .���� 
������ $��0.0001q�$��1.5, 1.5, 1.5, 1.5q�.��F� -     F�Name$  Asset��
���   �$��Center=�$��Center=�����  �� 
TargetType" F� �� Triggers ��  �� Binding ��  �� Path$ 
IsInMotion� �� UpdateSourceTrigger$ PropertyChanged=� �� Value True.a���  �� Property � �� �� Value �� *SystemSettingsInMotionColor+��  �� $ IsStationary�$ PropertyChanged=� True.a���  � �� �� *SystemSettingsStationaryColor+��  �� $ IsDoubleTap�$ PropertyChanged=� True.a���  � ��
 �������  �� $ 
IsSelected�$ PropertyChanged=� True.a���  � ��
 ���� ��  �� *
IsEmployee+ True.a���  � �� �� 	 �� GeometryCombineMode$		 Xor=�
 �� 	Geometry1
 @�  @� RadiusX$ 10Q� @� RadiusY$ 10Q� @� Center$ 10,10&� �� 	Geometry2 @� $ 5Q�$ 5Q�$ 10,10&� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��[��� .���� .��W�  �� 
TargetName$ Asset�� �� TargetProperty$ Opacity� W� From 0.3	 W� To 1 g� RepeatBehavior$ Forever�� g� AutoReverse$
 True�� J� BitmapEffect ��  �� Radius$ 1Q� �� 
KernelType$	 Box=� � ContextMenu �� .@�m� ����  �� Orientation$ 
Horizontal=�.+��� $
��ID: ���� ���� *	AssetId+�� m� ���� $ 
Horizontal=�.+��� $��Name: ���� ���� *Name+