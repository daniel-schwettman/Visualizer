   M S B A M L   `   `   `  ����E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  5      &     Visualizer.View.AssetUserControl   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:Visualizer.View     5      .��5	      �� 
������ $��0.0001q�6&   $��1.5, 1.5, 1.5, 1.5q�6?   .��5
   
   F� -   5          F�Name$  Asset��5
      
���   �$��Center=�6-   $��Center=�6J   ��5      �� 5       �� 
TargetType" F�6    �� Triggers 5      �� 5       �� Binding 6&   ��  �� Path$ 
IsInMotion� �� UpdateSourceTrigger$ PropertyChanged=� �� Value True6o   .a�5      ��  �� Property � �� �� Value 65   �� *SystemSettingsInMotionColor+5      �� 5       6&   �� $ IsStationary�$ PropertyChanged=� True6q   .a�5      ��  � �� 65   �� *SystemSettingsStationaryColor+5      �� 5       6&   �� $ IsDoubleTap�$ PropertyChanged=� True6p   .a�5      ��  � ��
 �����65   5      �� 5       6&   �� $ 
IsSelected�$ PropertyChanged=� True6o   .a�5      ��  � ��
 ���� 65   5      �� 5       6&   �� *
IsEmployee+ True6E   .a�5      ��  � �� 5   "   �� 5   &   	 �� GeometryCombineMode$		 Xor=�67   
 �� 	Geometry1
 5   *   @� 5   .    @� RadiusX$ 10Q�6>    @� RadiusY$ 10Q�6K    @� Center$ 10,10&�6X    �� 	Geometry2 5!   *   @� 5"   .   $ 5Q�6>   $ 5Q�6J   $ 10,10&�6V   5$   '   5&      5'      5)       � Triggers 5,      :� 5-       :� RoutedEvent$ FrameworkElement.Loaded��6   [�5.      �� 5/      .��50      �� .��51   "   W�  �� 
TargetName$ Asset��62    �� TargetProperty$ Opacity�52   "    W� From 0.353   "   	 W� To 154   "    g� RepeatBehavior$ Forever��55   "    g� AutoReverse$
 True��56   "   57      58      5:       J� BitmapEffect 5=      �� 5>       �� Radius$ 1Q�6#    �� 
KernelType$	 Box=�6.   5@       � ToolTip 5B   
   _� 5C      
���   �$��1q�6*   .��5D      �  � RowDefinitions 5E      �� 5F      �� 5G       � ColumnDefinitions 5I      �� 5J      �� 5K      .+�5M      �� $
��ID: ��6    $��0��6,   $��0��69   $
��Left=�6I   �� 5N      $��0��69   $��1��6F   $��Right=�6V   $��20,0,0,0q�6r   ��6    �� *	AssetId+6r   �� 5O      $��Name: ��6    $��1��6.   $��0��6;   $
��Left=�6K   �� 5P      $��1��66   $��1��6C   $��Right=�6S   $��20,0,0,0q�6o   ��6    �� *Name+6o   5Q      5R      5T      5U      