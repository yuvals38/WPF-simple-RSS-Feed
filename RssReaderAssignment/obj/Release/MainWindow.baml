   M S B A M L   `   `   `  ���� N  JRssReaderAssignment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:RssReaderAssignmentRssReaderAssignment  M+clr-namespace:RssReaderAssignment.ViewModelRssReaderAssignment.ViewModel  C&clr-namespace:RssReaderAssignment.ViewRssReaderAssignment.View  $    RssReaderAssignment.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     3local!clr-namespace:RssReaderAssignment     A	ViewModel+clr-namespace:RssReaderAssignment.ViewModel     ;controls&clr-namespace:RssReaderAssignment.View       � WindowStartupLocation$  CenterScreen=� � Title$ 
MainWindow��$	��450��$	��800�� � DataContext 0   *RssReaderAssignment.ViewModel.RssViewModel  .���  � RowDefinitions �� $
��Auto ��� $��* �.+�� $��2�� � ColumnDefinitions �� $��1* �$	��100���� $��5 ��� $��3* �$	��100��.+�&    RssReaderAssignment.View.RssView  $��2��$��0���� -    ��Name$ 
TextSearch��$��2��$��Right=�$��
178,30,0,0q�$	��Top=�$	��200��$��20������ *SearchString+ �� UpdateSourceTrigger$ PropertyChanged=��� -    ��Name$ ListBoxItems��$��2��$��1q���.  $
��Left=�$��40.5,68,0,0q�$	��Top=�$	��656��$	��480�����	#FFE4E3E4���� *
RssItems+ �� SelectedItem �� *SelectedRss+	 �� Mode$	 TwoWay=����� 
 �� 
TargetType"
 �� �� Triggers P�  P� Property �@��
IsSelected P� Value . . ���  �� Property � �� �� Value$
 Bold"���  � ��
 ���� ��  � ��
 �   � �� 	Resources  �� Color��%E   (��       * SystemColors.HighlightBrushKey+)�� $ Transparent������ .R���  �� Orientation$ 
Horizontal=�$	��740��$	��227��.+�� $	��430��$
��Left=�.+��� $��1q�
���   �$��0��$��0��$��1���� $��0,0,0,0q�$
��Left=�$	��Top=�$��WrapWithOverflow=�$��15(�$��40������ *Title+�� $��	0,42,0,20q�$	��160��$
��Left=�$	��Top=�$
��Wrap=����� *Description+�� $
��Left=�$��Bottom=�$��0,0,0,0q��� -   ���� *Link+.�
To article�� $
��Left=�$��Bottom=�$��0,0,0,0q���  ��Name$ 
btnAddLink��$	��Add��$��
388,30,0,0q�$��50��$��20��$��Right=�$	��Top=�{���  �� ElementName$ ListBoxItems�� �� Path$ SelectedItems.Count����� *DataContext.AddLink+ �� RelativeSource �� *FindAncestor+ �� AncestorType" ��� $	��310��$��Right=�$	��220��$	��Top=�.+��� ���� *	ImagePath+