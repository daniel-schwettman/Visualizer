   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.UtilVisualizer.Util  1clr-namespace:Visualizer.UtilVisualizer.Util  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   BoolToVisConverter 
 FadeIn  g� 	BeginTime �� 
TargetName �� TargetProperty O� KeyTime O� Value  FadeOut  thumbStyleRotate �� 
TargetType!    Visualizer.Util.RotateThumb �� Property �� Value �� 
TargetType	 J� SnapsToDevicePixels
 J� Opacity �� CornerRadius �� Triggers P� Property P� Value N� EnterActions N� ExitActions  thumbStyleCorner!   Visualizer.Util.BoundsThumb  thumbStyleEdge  thumbStyleEdgeLeft  thumbStyleEdgeRight  thumbStyleEdgeTop 	 thumbStyleEdgeBottom   Visualizer.Util.Resizer � Cursor   	ViewModel   ViewContext �� Path    	ViewModel    ViewContext�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     /DesignerViewsclr-namespace:Visualizer.Util  ,
modelingUIclr-namespace:Visualizer.Util  %�  &	        &	      &	 t     &	 �     7  7  &	 �    7  7  &	 �    &	 7    7  7  &	 �    7  7  &	 �    7  7  &		 �    7  7  '  K    7  7  7  7 	 7  7  7  7  7  �� �� .��U�   00:00:00$ HoverBorder��$ (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$	 0.3Q��� .��U�   00:00:00$ HoverBorder��$ (UIElement.Opacity)�.\��� $ 00:00:00.1000000��$ 0Q��� "   .���  � �� �� "   .R�� $��20��$��20��$
	 True��.+��� $��HoverBorder��
���� ��$
 0Q�$ 8��A� 
���� ��$��8��$��8��$��Center=�$��Center=� P�  �@J�IsMouseOver$
 True�� �� 8��   �� 8�� �� "  .���  � �� �� "  .R�� $��20��$��20��$
	 True��.+��� $��HoverBorder��
���� ��$
 0Q�$ 8��A� 
���   �$��1��
����  �$��8��$��8��$��Center=�$��Center=� P�  �@J�IsMouseOver$
 True�� �� 8��   �� 8�� �� "  .���  � �� �� "  .R�� .+��� 
����  ��� "  .���  � �� �� "  .R�� $��7��$��
-4.5,0,0,0q�$
	 True��.+��� $��HoverBorder��
���� ��$
 0Q� P�  �@J�IsMouseOver$
 True�� �� 8��   �� 8�� �� "  .���  � �� �� "  .R�� $��7��$��
0,0,-3.5,0q�$
	 True��.+��� $��HoverBorder��
���� ��$
 0Q� P�  �@J�IsMouseOver$
 True�� �� 8��   �� 8�� �� "  .���  � �� �� "  .R�� $��7��$��
0,-4.5,0,0q�$
	 True��.+��� $��HoverBorder��
���� ��$
 0Q� P�  �@J�IsMouseOver$
 True�� �� 8��   �� 8�� �� "  .���  � �� �� "  .R�� $��7��$��
0,0,0,-3.5q�$
	 True��.+��� $��HoverBorder��
���� ��$
 0Q� P�  �@J�IsMouseOver$
 True�� �� 8��   �� 8�� �� "  .���  � �� �� "  .R��� 
����  �$��1q�$��-1q�._�� .+�  8��  $��Stretch=�$
��Left=�$ SizeWE�� ��  �� $) #DataContext.ViewManager.ViewContext�  8�� $��Stretch=�$��Right=�$ SizeWE�� ��  �� $+ %DataContext.ViewManager.ViewContextol�  8�� $	��Top=�$��Stretch=�$ SizeNS�� ��  �� $) #DataContext.ViewManager.ViewContext�  8�� $��Bottom=�$��Stretch=�$ SizeNS�� ��  �� $+ %DataContext.ViewManager.ViewContetrol�  8�� $	��Top=�$
��Left=�$ SizeAll��$��-10 -10 0 0q� ��  �� $) #DataContext.ViewManager.ViewContext�  8�� $	��Top=�$��Right=�$ SizeAll��$��0 -10 -10 0q� ��  �� $) #DataContext.ViewManager.ViewContext�  8�� $��Bottom=�$
��Left=�$ SizeAll��$��-10 0 0 -10q� ��  �� $) #DataContext.ViewManager.ViewContext�  8�� $��Bottom=�$��Right=�$ SizeAll��$��0 0 -10 -10q� ��  �� $) #DataContext.ViewManager.ViewContext�   8�� $	��Top=�$��Right=�$
 Hand��$��0 -20 -20 0q� ��  �� $) #DataContext.ViewManager.ViewContext�