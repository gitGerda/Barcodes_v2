   M S B A M L   `   `   `  ����F  BBarcodesAPP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:NiceLabel.SDKNiceLabel.SDK  5   	   "    NiceLabel.SDK.WindowSettings   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:NiceLabel.SDK     5   	   -   5          � Title$&   BarcodesApp # Настройки��5   	   $	��656��6)   $	��750��66    � Topmost .  � Icon$ NiceLabel.ico��6h   $	��750��6�    � SizeToContent$ Height=�6�   $	��797��6�    � 
ResizeMode$ NoResize=�6�    � WindowStartupLocation$ CenterOwner=�6�   .��5	      � $��0,0,0,-8q�6    � ColumnDefinitions 5
   
   �� 5      $	��15* �6   6-   �� 5      $
��109* �6   6.    � RowDefinitions 5   
   �� 5      �� 5      $��53 �6   $��53��6(   .+�5   
   ]� $��0��6   $��2��6$   ���	#FF535474.+�5      �� $2��,flower_plant_flower_pot_vase_icon_181460.png��6   $��Center=�6J   $��Center=�6g   $	��140��6�   $��0,214,0,281q�6�   $��78��6�   6�   5      �� 5   
   $��1��6   $��2��6   ���	#FF424242._�5      �� -   5        $��Закрыть��5      $
��Left=�6'   $��
634,10,0,0q�6B   $	��Top=�6V   $��90��6n   $��29��6�   ���	#FFC7AB30$
��Bold"�6�   $��1��6�   5      �� 5   
   $��1��6   $
��Auto=�6'   .��5      �� $��0��6   .+�5      9� $��Шаблоны��6   $��15(�6,    9� 
IsExpanded . .��5       � $	��497��6   $��	24,0,26,0q�6)   $	��Top=�6<   ���	#FFECE0E3 5!      �� 5"      $��27 �6,   6;   �� 5#      $��3 �6,   �� 5$      $��36 �6,   �� 5%      $	��62* �6,   �� 5&      $��35 �6,   �� 5'      $��35 �6,   �� 5(      $	��75* �6,   6;    5*      �� 5+      $	��89* �6/   �� 5,      $
��193* �6/   �� 5-      $	��52* �6/   .+�5/      �� $��0��6!   $��0��6.   $��3��6>   
����+��._�50      �� $P��JАвтозаполняемые (1С:Предприятие) шаблоны��6$   $��
159,0,84,0q�6W   ���	#FFFFFBFB$��14(�6�   51      �� 52      $��4��6!   $��0��6.   $��3��6>   
����+��$��0,4,0,3q�6j   ._�53      �� $;��5Шаблоны ручного ввода данных��6$   $��186,0,139,0q�6K   ���	#FFFFFBFB$��14(�6w   54      &   System.Windows.Controls.DataGrid  56      -   5        	  Name$	 dg_autoModels��56   #   $��3��6:   $��0��6H   $��3��6X   
   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 * 6l      
IsReadOnly . 
�������   HorizontalGridLinesBrush �	#FF895252   VerticalGridLinesBrush �	#FF895252   RowBackground
 �����   HeadersVisibility$ Column=�61  $��Italic$�6L     CanUserReorderColumns .     CanUserSortColumns .  58        5:      -   5        $	 dg_manualModels��5:   #   $��6��6<   $��3��6I   $
 * 6]    . 
�������
 �����$ Column=�6�    �	#FF975353 �	#FF975353$��Italic$�6?   .   .  6p  �� 5;      -   5         ��Name$ btn_manualModel��5;   !   $��Добавить��6:   $��1��6M   $
��Left=�6]   $��	124,4,0,0q�6x   $	��Top=�6�   $��90��6�   $��26��6�   $��5��6�   ���	#FFC4ECA86�   �� 5<      -   5        $ btn_ManualDelete��5<   !   $��Удалить��6;   $��1��6M   $
��Left=�6]   $��	325,4,0,0q�6x   $	��Top=�6�   $��90��6�   $��26��6�   $��5��6�   ���	#FFF39696$��2��6�   6�   �� 5=      -   5        $ btn_AutoModelAdd��5=   !   $��Добавить��6;   $��1��6N   $
��Left=�6^   $��	124,3,0,0q�6y   $	��Top=�6�   $��90��6�   $��26��6�   $��2��6�   ���	#FFC4ECA86�   �� 5>      -   5        $ btn_AutoModelDel��5>   !   $��Удалить��6;   $��1��6M   $
��Left=�6]   $��	325,3,0,0q�6x   $	��Top=�6�   $��90��6�   $��26��6�   $��2��6�   ���	#FFF39696$��2��6�   6�   �� 5?      -	   5        $ btn_AutoModelChange��5?   !   $��Изменить��6>   $��1��6Q   $
��Left=�6a   $��	226,3,0,0q�6|   $	��Top=�6�   $��90��6�   $��26��6�   $��2��6�   ���	#FFFFEA896�   �� 5@      -
   5        $ btn_ManualModelChange��5@   !   $��Изменить��6@   $��1��6S   $
��Left=�6c   $��	226,4,0,0q�6~   $	��Top=�6�   $��90��6�   $��26��6�   $��5��6�   ���	#FFFFEA896�   5A      5B      9� 5D      $��Сканер��6   $��15(�6,   .��5E      �� ���	#FFECE0E3$��91��64   $��0,0,0,7q�6@    � UseLayoutRounding . $��	22,0,38,0q�6s   $	��584��6�   ._�5F      � $
��Left=�6   $��6,7,0,3q�6:   $	��465��6K    5G      �� 5H   "   $	��33* �63   �� 5I   "   $
��122* �63    5K      �� 5L   "   $	��35* �60   �� 5M   "   $	��41* �60   6?   .+�5O      �� $��COM порт :��6$   $��8,0,5,0q�69   $	��Top=�6J   $��27��6b   $��0��6n   �� 5P      -   5         ��Name$ 
tb_ComPort��5P   &   $��0��6:   $��1��6G   $
��Left=�6W   $��35��6r   $��8,2,0,0q�6~    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��348��6�   $��2��6�   �� 5Q      -   5         ��Name$ cb_scanOnOff��5Q   '   $��1��6=   $)��#Включить/Выключить��6J   $��10,11,125,9q�6g   $��1��6|   6�   5R      5S      5T      9� 5U      $��База данных��6   $��15(�61   $	��244��6?   .��5V      �� ���	#FFECE0E3$��	21,0,37,0q�64   $	��224��6G   ._�5X      � $
��Left=�6   $��6,0,0,0q�6:   $	��497��6K    5Y      �� 5Z   "   $	��42* �60   �� 5[   "   $	��41* �60   �� 5\   "   $	��40* �60   �� 5]   "   $	��42* �60   �� 5^   "   $	��59* �60   6?    5`      �� 5a   "   $
��112* �63   �� 5b   "   $
��467* �63   .+�5d      �� $,��&Сервер :                        ��6$   $
��Left=�6O   $��8,2,0,0q�6j   $	��Top=�6{   $��27��6�   $��62��6�   �� 5e      -   5        $ 	tb_Server��5e   &   $��1��69   $
��Left=�6I   $��35��6d   $��4,5,0,0q�6p   $
 Wrap=�6�   $	��Top=�6�   $	��387��6�   6�   �� 5f      $5��/База данных :                        ��6$   $
��Left=�6T   $��7,0,0,0q�6o   $	��Top=�6�   $��27��6�   $��96��6�   $��1��6�   $��2��6�   �� 5g      $8��2Пользователь :                        ��6$   $
��Left=�6U   $��5,0,0,0q�6p   $	��Top=�6�   $��27��6�   $	��102��6�   $��2��6�   $��2��6�   �� 5h      $,��&Пароль :                        ��6$   $
��Left=�6O   $��5,0,0,0q�6j   $	��Top=�6{   $��27��6�   $��61��6�   $��3��6�   �� 5i      -   5        $ tb_Db��5i   &   $��1��65   $
��Left=�6E   $��35��6`   $��4,2,0,0q�6l   $
 Wrap=�6}   $	��Top=�6�   $	��387��6�   $��1��6�   6�   �� 5j      -   5        $ tb_User��5j   &   $��1��67   $
��Left=�6G   $��35��6b   $��4,2,0,0q�6n   $
 Wrap=�6   $	��Top=�6�   $	��387��6�   $��2��6�   6�   G� 5k      -   5         G�Name$ tb_Pwd��5k   *   $
��Left=�6:   $��4,4,0,0q�6U   $��3��6f   $	��Top=�6s   $��1��6�   $	��387��6�   $��35��6�   �� 5l      -   5        $-��'Проверить соединение��5l   %   $
��Auto��6D   ���	#FFB1E079$��4��6�   $��1��6�   $��
5,5,219,20q�6�   5m      5o      5p      � 5r       5s      �� 5t      $��20 �6(   66   5v      5x      5y      5{      5|      