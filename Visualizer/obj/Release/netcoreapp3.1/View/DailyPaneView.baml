   M S B A M L   `   `   `  ���� E  AVisualizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Visualizer.ViewVisualizer.View  #    Visualizer.View.DailyPaneView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:Visualizer.View     .���   � 	Resources     AlternatingWithTriggers �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%Z  &	        �� " �� P�  �@��AlternationIndex$ 0��. ���  � ��
 �������  � ��
 �   ���  � ��$ 1q���  � ��
 �   ���  � ��$ 0,3,0,3q�P�  �@��AlternationIndex$ 1��. ���  � ��
 �������  � ��
 �   ���  � ��$ 0q���  � ��
 �   ���  � ��$ 0,3,0,3q�.+�� $��30q� � ColumnDefinitions �� $��8* ��� $��2* �.+���  �� Orientation$ Vertical=�.+��  �� $��5* ��� $��5* �.+��� $��0��$��
Daily List��$��20(�$
��Left=�$��Bottom=��� $��1��$��14(�$��Right=�$��Bottom=����� *DailyListTagCount+�� 
���   �$��2q�._��� $
��Auto=�$
��Auto=�.���� $	��500������ *	DailyTags+	 �� UpdateSourceTrigger$	 PropertyChanged=�
 �� SelectedItem
 �� *SelectedTag+  ��%M   '��       �� " ��.���  � ��$ Stretch=���  � ��$ 2,0,0,0q� �� View �� .P���  �� CellTemplate �� .R���  d� 	IsChecked �� *	IsChecked+�� $��Tag ID�� �� Width$	 200�� �� .R��� $��18(����� *TagResult.RawId+�� $��Tag Name��$	 200�� �� .R��� $��18(����� *TagResult.Name+�� $
��Type��$	 200�� �� .R��� $��18(����� *TagResult.AssignedType+�� $��Category��$	 200�� �� .R��� $��18(����� *TagResult.Category+�� $��1��$��Print Daily List��$��Center=�$��Center=�$	��150��$��	0,0,0,100q����� *PrintDailyListCommand+�� $��1��$ ��Delete Tag from Daily List��$��Center=�$��Center=�$	��150��$��	0,100,0,0q����� *DeleteTagFromDailyListCommand+